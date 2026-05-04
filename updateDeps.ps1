
Write-Host "CARGO UPDATE"
Write-Host "Starting dependency update process..."
Set-Location '.\t-src-modules\eam_commons'
Write-Host "Updating eam_commons dependencies..."
cargo update

Set-Location '..\eam_plus_lib_mock'
Write-Host "Updating eam_plus_lib_mock dependencies..."
cargo update

Set-Location '..\eam_background_sync'
Write-Host "Updating eam_background_sync dependencies..."
cargo update

Set-Location '..\..\src-tauri'
Write-Host "Updating exalt account manager dependencies..."
cargo update

Set-Location '..'
Write-Host "All cargo dependencies updated successfully!"