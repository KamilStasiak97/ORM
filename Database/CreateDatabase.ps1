#!/usr/bin/env pwsh
# PowerShell script to create SQLite database for ORM Comparison

$DatabasePath = "ORMComparison.db"
$SqlScriptPath = "CreateDatabase.sql"

Write-Host "Creating SQLite database for ORM Comparison..." -ForegroundColor Green

# Check if sqlite3 is available
try {
    $null = Get-Command sqlite3 -ErrorAction Stop
    Write-Host "SQLite3 found, creating database..." -ForegroundColor Yellow
    
    # Create database and run SQL script
    sqlite3 $DatabasePath < $SqlScriptPath
    
    if (Test-Path $DatabasePath) {
        Write-Host "Database created successfully at: $((Get-Location).Path)\$DatabasePath" -ForegroundColor Green
        
        # Show database info
        Write-Host "`nDatabase contents:" -ForegroundColor Cyan
        sqlite3 $DatabasePath "SELECT 'Products' as TableName, COUNT(*) as Count FROM Products UNION ALL SELECT 'Customers', COUNT(*) FROM Customers;"
        
    } else {
        Write-Host "Failed to create database!" -ForegroundColor Red
    }
}
catch {
    Write-Host "SQLite3 not found. Please install SQLite3:" -ForegroundColor Red
    Write-Host "  - Windows: Download from https://www.sqlite.org/download.html" -ForegroundColor Yellow
    Write-Host "  - macOS: brew install sqlite3" -ForegroundColor Yellow
    Write-Host "  - Ubuntu: sudo apt-get install sqlite3" -ForegroundColor Yellow
    Write-Host "`nOr use the .NET tool:" -ForegroundColor Yellow
    Write-Host "  dotnet tool install --global dotnet-sqlite" -ForegroundColor Yellow
    Write-Host "  dotnet sqlite $DatabasePath < $SqlScriptPath" -ForegroundColor Yellow
} 