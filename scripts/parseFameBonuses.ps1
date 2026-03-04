<#
.SYNOPSIS
    Parses a FameBonus.xml file into a JavaScript object literal.

.DESCRIPTION
    Reads the ArrayOfFameBonus XML and outputs a nested JS const structured as:
      DisplayGroup -> DisplayCategory -> array of bonus entries.
    Each bonus entry includes its fields and a conditions array.

.PARAMETER XmlPath
    (Required) Path to the FameBonus.xml file.

.PARAMETER OutFile
    (Optional) Path to write the JS output to. If omitted, prints to console.

.EXAMPLE
    .\parseFameBonuses.ps1 -XmlPath "path\to\FameBonus.xml"

.EXAMPLE
    .\parseFameBonuses.ps1 -XmlPath "path\to\FameBonus.xml" -OutFile "src\assets\fameBonuses.js"
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$XmlPath,
    [string]$OutFile = ""
)

[xml]$xml = Get-Content -Path $XmlPath -Encoding UTF8

# --- Group: DisplayGroup -> DisplayCategory -> list of entries ---
$grouped = New-Object System.Collections.Specialized.OrderedDictionary

foreach ($bonus in $xml.ArrayOfFameBonus.FameBonus) {
    $group    = $bonus.DisplayGroup
    $category = $bonus.DisplayCategory

    if (-not $grouped.Contains($group)) {
        $grouped[$group] = New-Object System.Collections.Specialized.OrderedDictionary
    }
    if (-not $grouped[$group].Contains($category)) {
        $grouped[$group][$category] = [System.Collections.Generic.List[object]]::new()
    }

    # Build conditions array
    $conditions = @()
    $rawConditions = @($bonus.Condition.Condition)
    foreach ($cond in $rawConditions) {
        # The text content of the node is the type (e.g. StatValue, MaxedStat)
        $type      = $cond.'#text'
        $threshold = $cond.threshold
        $stat      = $cond.stat  # may be empty

        $condObj = [ordered]@{
            type      = $type
            threshold = $threshold
        }
        if (-not [string]::IsNullOrWhiteSpace($stat)) {
            $condObj['stat'] = $stat
        }
        $conditions += $condObj
    }

    $entry = [ordered]@{
        id             = $bonus.id
        displayName    = $bonus.DisplayName
        absoluteBonus  = [double]$bonus.AbsoluteBonus
        relativeBonus  = [double]$bonus.RelativeBonus
        maxRepeatCount = [int]$bonus.MaxRepeatCount
        repeatable     = ($bonus.Repeatable -eq 'true')
        conditions     = $conditions
    }

    $grouped[$group][$category].Add($entry)
}

# --- Helpers ---
function Escape-JS($str) {
    return $str -replace "'", "\'"
}

function Render-Condition($cond) {
    $parts = @("type: '$(Escape-JS $cond.type)'", "threshold: $($cond.threshold)")
    if ($cond.Contains('stat')) {
        $parts += "stat: '$(Escape-JS $cond.stat)'"
    }
    return "{ $($parts -join ', ') }"
}

function Render-Entry($entry, $indent) {
    $i  = ' ' * $indent
    $i2 = ' ' * ($indent + 4)
    $i3 = ' ' * ($indent + 8)

    $lines = @()
    $lines += "${i}{"
    $lines += "${i2}id: '$(Escape-JS $entry.id)',"
    $lines += "${i2}displayName: '$(Escape-JS $entry.displayName)',"
    $lines += "${i2}absoluteBonus: $($entry.absoluteBonus),"
    $lines += "${i2}relativeBonus: $($entry.relativeBonus),"
    $lines += "${i2}maxRepeatCount: $($entry.maxRepeatCount),"
    $lines += "${i2}repeatable: $($entry.repeatable.ToString().ToLower()),"

    if ($entry.conditions.Count -eq 0) {
        $lines += "${i2}conditions: [],"
    } elseif ($entry.conditions.Count -eq 1) {
        $lines += "${i2}conditions: [$(Render-Condition $entry.conditions[0])],"
    } else {
        $lines += "${i2}conditions: ["
        foreach ($cond in $entry.conditions) {
            $lines += "${i3}$(Render-Condition $cond),"
        }
        $lines += "${i2}],"
    }

    $lines += "${i}}"
    return $lines -join "`n"
}

# --- Build JS output ---
$out = @("const fameBonuses = {")

$groupKeys = $grouped.Keys | Sort-Object
foreach ($group in $groupKeys) {
    $out += "    '$(Escape-JS $group)': {"

    $catKeys = $grouped[$group].Keys  # already ordered (insertion order)
    foreach ($cat in $catKeys) {
        $entries = $grouped[$group][$cat]
        $out += "        '$(Escape-JS $cat)': ["

        foreach ($entry in $entries) {
            $out += (Render-Entry $entry 12) + ","
        }

        $out += "        ],"
    }

    $out += "    },"
}

$out += "};"
$out += ""
$out += "export default fameBonuses;"

$result = $out -join "`n"

if ($OutFile) {
    $result | Set-Content -Path $OutFile -Encoding UTF8
    Write-Host "Written to $OutFile"
} else {
    Write-Output $result
}
