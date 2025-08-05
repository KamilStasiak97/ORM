# PowerShell script to run all three ORM comparison projects

Write-Host "Starting ORM Comparison Projects..." -ForegroundColor Green
Write-Host "==================================" -ForegroundColor Green

# Check if database exists
if (-not (Test-Path "Database/ORMComparison.db")) {
    Write-Host "Database not found! Creating database..." -ForegroundColor Yellow
    Set-Location Database
    & .\CreateDatabase.ps1
    Set-Location ..
}

Write-Host "Starting Entity Framework Project on port 5001..." -ForegroundColor Cyan
Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run", "--urls", "https://localhost:5001" -WorkingDirectory "EntityFrameworkProject"

Write-Host "Starting Dapper Project on port 5002..." -ForegroundColor Cyan
Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run", "--urls", "https://localhost:5002" -WorkingDirectory "DapperProject"

Write-Host "Starting ADO.NET Project on port 5003..." -ForegroundColor Cyan
Start-Process -NoNewWindow -FilePath "dotnet" -ArgumentList "run", "--urls", "https://localhost:5003" -WorkingDirectory "AdoNetProject"

Write-Host ""
Write-Host "All projects are starting..." -ForegroundColor Green
Write-Host "Entity Framework: https://localhost:5001" -ForegroundColor Yellow
Write-Host "Dapper:           https://localhost:5002" -ForegroundColor Yellow
Write-Host "ADO.NET:          https://localhost:5003" -ForegroundColor Yellow
Write-Host ""
Write-Host "Press any key to stop all projects..." -ForegroundColor Red

$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")

Write-Host "Stopping all projects..." -ForegroundColor Red
Get-Process -Name "dotnet" | Where-Object { $_.ProcessName -eq "dotnet" } | Stop-Process -Force 