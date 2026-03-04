<#
.SYNOPSIS
    Parses a PlayerStat.xml file into a JavaScript object literal.

.DESCRIPTION
    Reads the ArrayOfPlayerStat XML structure and outputs a JS const where each
    entry is keyed by the stat's index. The 'short' field maps to the XML <id>,
    and 'name' maps to <dungeonId> if non-empty, otherwise <displayName>.

.PARAMETER XmlPath
    (Required) Path to the PlayerStat.xml file.

.PARAMETER OutFile
    (Optional) Path to write the JS output to. If omitted, prints to console.

.EXAMPLE
    # Print to console
    .\parsePlayerStats.ps1 -XmlPath "path\to\PlayerStat.xml"

.EXAMPLE
    # Write to a JS file
    .\parsePlayerStats.ps1 -XmlPath "path\to\PlayerStat.xml" -OutFile "src\assets\playerStats.js"
#>

param(
    [Parameter(Mandatory = $true)]
    [string]$XmlPath,
    [string]$OutFile = ""
)

[xml]$xml = Get-Content -Path $XmlPath -Encoding UTF8

$entries = @{}

foreach ($stat in $xml.ArrayOfPlayerStat.PlayerStat) {
    $index = [int]$stat.index
    $short = $stat.id

    $displayName = $stat.displayName
    $dungeonId   = $stat.dungeonId

    $isDungeon = -not [string]::IsNullOrWhiteSpace($dungeonId)

    if ($isDungeon) {
        $name = $dungeonId
    } else {
        $name = $displayName
    }

    $entries[$index] = @{ short = $short; name = $name; isDungeon = $isDungeon }
}

$lines = @("const playerStats = {")

foreach ($key in ($entries.Keys | Sort-Object)) {
    $short     = $entries[$key].short -replace "'", "\'"
    $name      = $entries[$key].name  -replace "'", "\'"
    $isDungeon = if ($entries[$key].isDungeon) { 'true' } else { 'false' }
    $lines += "    $key`: { short: '$short', name: '$name', isDungeon: $isDungeon },"
}

$lines += "};"

$output = $lines -join "`n"

if ($OutFile) {
    $output | Set-Content -Path $OutFile -Encoding UTF8
    Write-Host "Written to $OutFile"
} else {
    Write-Output $output
}
