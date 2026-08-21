<#
.SYNOPSIS
    Refreshes the game-data snapshot shipped with the installer.

.DESCRIPTION
    EAM seeds an empty cache from this snapshot, so a fresh install stays usable
    when its first contact with the game-data service fails. The snapshot only
    has to be recent enough to be useful; the service supersedes it on the first
    successful refresh, including by a diff against it.

    Run this before cutting a release. Both files are committed.

.PARAMETER ServiceUrl
    Base URL of the game-data service. Defaults to production.

.EXAMPLE
    ./update-snapshot.ps1
    ./update-snapshot.ps1 -ServiceUrl http://localhost:3700
#>
[CmdletBinding()]
param(
    [string]$ServiceUrl = 'https://game-assets.api.exaltaccountmanager.com'
)

$ErrorActionPreference = 'Stop'

function Compress-File {
    param([string]$Source, [string]$Destination)
    $reader = $null; $writer = $null; $gzip = $null
    try {
        $reader = [System.IO.File]::OpenRead($Source)
        $writer = [System.IO.File]::Create($Destination)
        $gzip = New-Object System.IO.Compression.GzipStream($writer, [System.IO.Compression.CompressionLevel]::Optimal)
        $reader.CopyTo($gzip)
    }
    finally {
        if ($gzip) { $gzip.Dispose() }
        if ($writer) { $writer.Dispose() }
        if ($reader) { $reader.Dispose() }
    }
}
 
$targetDirectory = $PSScriptRoot
$base = $ServiceUrl.TrimEnd('/')

Write-Host "Reading the current build from $base"
$latest = Invoke-RestMethod -Uri "$base/api/v1/builds/latest" -Method Get
Write-Host "  build      $($latest.buildId)"
Write-Host "  realm hash $($latest.realmBuildHash)"

# The manifest is verified against the hash the service publishes for it, so a
# truncated or corrupted download cannot be committed.
$manifestPath = Join-Path $targetDirectory 'manifest.json'
Invoke-WebRequest -Uri "$base/api/v1/builds/$($latest.buildId)/manifest" -OutFile $manifestPath
$manifestHash = (Get-FileHash -Path $manifestPath -Algorithm SHA256).Hash.ToLowerInvariant()
if ($manifestHash -ne $latest.manifestSha256) {
    Remove-Item $manifestPath -Force
    throw "Manifest SHA-256 mismatch. Expected $($latest.manifestSha256), got $manifestHash."
}
Write-Host "  manifest   $([math]::Round((Get-Item $manifestPath).Length / 1MB, 2)) MB, hash verified"

# Stored compressed: as JSON it is mostly repeated keys and deflates to about a
# sixth of its size.
Compress-File -Source $manifestPath -Destination (Join-Path $targetDirectory 'manifest.json.gz')
Remove-Item $manifestPath -Force

# Sprites are stored compressed: the archive pads every entry to a 512-byte
# boundary, which roughly doubles it uncompressed for no benefit on disk.
$tarPath = Join-Path $targetDirectory 'sprites.tar'
$gzPath = Join-Path $targetDirectory 'sprites.tar.gz'
Invoke-WebRequest -Uri "$base/api/v1/builds/$($latest.buildId)/sprites" -OutFile $tarPath
Compress-File -Source $tarPath -Destination $gzPath
Remove-Item $tarPath -Force

Write-Host "  sprites    $([math]::Round((Get-Item $gzPath).Length / 1MB, 2)) MB compressed"
Write-Host ''
Write-Host 'Snapshot updated. Commit manifest.json.gz and sprites.tar.gz.'
