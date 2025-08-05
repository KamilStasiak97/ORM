#!/bin/bash
# Script to run all three ORM comparison projects

echo "Starting ORM Comparison Projects..."
echo "=================================="

# Check if database exists
if [ ! -f "Database/ORMComparison.db" ]; then
    echo "Database not found! Creating database..."
    cd Database
    ./CreateDatabase.sh
    cd ..
fi

echo "Starting Entity Framework Project on port 5001..."
cd EntityFrameworkProject
dotnet run --urls "https://localhost:5001" &
EF_PID=$!
cd ..

echo "Starting Dapper Project on port 5002..."
cd DapperProject
dotnet run --urls "https://localhost:5002" &
DAPPER_PID=$!
cd ..

echo "Starting ADO.NET Project on port 5003..."
cd AdoNetProject
dotnet run --urls "https://localhost:5003" &
ADONET_PID=$!
cd ..

echo ""
echo "All projects are starting..."
echo "Entity Framework: https://localhost:5001"
echo "Dapper:           https://localhost:5002"
echo "ADO.NET:          https://localhost:5003"
echo ""
echo "Press Ctrl+C to stop all projects"

# Wait for user to stop
trap "echo 'Stopping all projects...'; kill $EF_PID $DAPPER_PID $ADONET_PID; exit" INT
wait 