#!/bin/bash
# Bash script to create SQLite database for ORM Comparison

DATABASE_PATH="ORMComparison.db"
SQL_SCRIPT_PATH="CreateDatabase.sql"

echo "Creating SQLite database for ORM Comparison..."

# Check if sqlite3 is available
if command -v sqlite3 &> /dev/null; then
    echo "SQLite3 found, creating database..."
    
    # Create database and run SQL script
    sqlite3 "$DATABASE_PATH" < "$SQL_SCRIPT_PATH"
    
    if [ -f "$DATABASE_PATH" ]; then
        echo "Database created successfully at: $(pwd)/$DATABASE_PATH"
        
        # Show database info
        echo ""
        echo "Database contents:"
        sqlite3 "$DATABASE_PATH" "SELECT 'Products' as TableName, COUNT(*) as Count FROM Products UNION ALL SELECT 'Customers', COUNT(*) FROM Customers;"
        
    else
        echo "Failed to create database!"
        exit 1
    fi
else
    echo "SQLite3 not found. Please install SQLite3:"
    echo "  - macOS: brew install sqlite3"
    echo "  - Ubuntu: sudo apt-get install sqlite3"
    echo "  - CentOS/RHEL: sudo yum install sqlite3"
    echo ""
    echo "Or use the .NET tool:"
    echo "  dotnet tool install --global dotnet-sqlite"
    echo "  dotnet sqlite $DATABASE_PATH < $SQL_SCRIPT_PATH"
    exit 1
fi 