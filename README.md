# .NET 8 ORM Comparison Project

This solution demonstrates three different approaches to data access in .NET 8:
- **Entity Framework Core** - Full-featured ORM with change tracking, migrations, and LINQ support
- **Dapper** - Micro-ORM with high performance and SQL control
- **ADO.NET** - Raw data access with maximum control and performance

## Project Structure

```
ORM/
├── ORMComparison.sln                    # Main solution file
├── SharedModels/                        # Shared entity models
│   └── Models/
│       ├── Product.cs
│       ├── Customer.cs
│       ├── Order.cs
│       └── OrderItem.cs
├── EntityFrameworkProject/              # Entity Framework implementation
│   ├── Controllers/
│   ├── Data/
│   ├── Repositories/
│   └── Program.cs
├── DapperProject/                       # Dapper implementation
│   ├── Controllers/
│   ├── Repositories/
│   └── Program.cs
├── AdoNetProject/                       # ADO.NET implementation
│   ├── Controllers/
│   ├── Repositories/
│   └── Program.cs
└── Database/                            # Shared SQLite database
    ├── ORMComparison.db                 # SQLite database file
    ├── CreateDatabase.sql               # SQL script for database creation
    ├── CreateDatabase.sh                # Bash script for database creation
    └── CreateDatabase.ps1               # PowerShell script for database creation
```

## Prerequisites

- .NET 8 SDK
- SQLite3 (optional, for database creation)
- Visual Studio 2022 or VS Code

## Setup Instructions

### 1. Clone and Build

```bash
git clone <repository-url>
cd ORM
dotnet restore
dotnet build
```

### 2. Create Database

Create the shared SQLite database:

**Option 1: Using SQLite3 (recommended)**
```bash
cd Database
# On macOS/Linux:
./CreateDatabase.sh
# On Windows:
./CreateDatabase.ps1
```

**Option 2: Using .NET tool**
```bash
dotnet tool install --global dotnet-sqlite
cd Database
dotnet sqlite ORMComparison.db < CreateDatabase.sql
```

**Option 3: Manual creation**
```bash
cd Database
sqlite3 ORMComparison.db < CreateDatabase.sql
```

### 3. Run the Applications

**Option 1: Run all projects at once (recommended)**
```bash
# On macOS/Linux:
./run-all.sh

# On Windows:
.\run-all.ps1
```

**Option 2: Run projects individually**
```bash
# Entity Framework Project (Port 5001)
cd EntityFrameworkProject
dotnet run --urls "https://localhost:5001"

# Dapper Project (Port 5002)
cd ../DapperProject
dotnet run --urls "https://localhost:5002"

# ADO.NET Project (Port 5003)
cd ../AdoNetProject
dotnet run --urls "https://localhost:5003"
```

## API Endpoints

All three projects expose the same REST API endpoints:

- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product (soft delete)
- `GET /api/products/search?name={searchTerm}` - Search products by name
- `GET /api/products/price-range?minPrice={min}&maxPrice={max}` - Get products by price range

## Comparison Overview

### Entity Framework Core
**Pros:**
- Full ORM with change tracking
- LINQ support for type-safe queries
- Automatic migrations
- Rich configuration options
- Built-in connection pooling
- Lazy loading support

**Cons:**
- Higher memory usage
- Potential N+1 query issues
- Less control over generated SQL
- Steeper learning curve

**Best for:**
- Complex domain models
- Rapid development
- Teams new to .NET
- Applications requiring change tracking

### Dapper
**Pros:**
- High performance
- Full SQL control
- Lightweight
- Easy to learn
- Great for stored procedures
- Minimal memory overhead

**Cons:**
- Manual SQL writing
- No change tracking
- No automatic migrations
- More boilerplate code

**Best for:**
- Performance-critical applications
- Existing database schemas
- Complex queries
- Microservices

### ADO.NET
**Pros:**
- Maximum performance
- Complete control over SQL
- No abstraction overhead
- Direct database access
- Minimal dependencies

**Cons:**
- Most verbose code
- Manual parameter handling
- No type safety
- Higher maintenance cost
- SQL injection risks if not careful

**Best for:**
- Legacy applications
- Maximum performance requirements
- Direct database operations
- Custom data access patterns

## Performance Considerations

### Memory Usage
- **Entity Framework**: Highest (change tracking, object materialization)
- **Dapper**: Low (minimal overhead)
- **ADO.NET**: Lowest (direct data access)

### Development Speed
- **Entity Framework**: Fastest (LINQ, migrations, scaffolding)
- **Dapper**: Medium (SQL but with object mapping)
- **ADO.NET**: Slowest (manual everything)

### Runtime Performance
- **Entity Framework**: Good (with proper configuration)
- **Dapper**: Excellent (minimal overhead)
- **ADO.NET**: Best (no abstraction)


Example curl commands:

```bash
# Get all products
curl -X GET "https://localhost:5001/api/products"

# Create a product
curl -X POST "https://localhost:5001/api/products" \
  -H "Content-Type: application/json" \
  -d '{"name":"Test Product","description":"Test Description","price":99.99,"stockQuantity":10}'

# Search products
curl -X GET "https://localhost:5001/api/products/search?name=laptop"
```

## Configuration

All projects share the same SQLite database file located at `Database/ORMComparison.db`. Each project has its own `appsettings.json` with the same connection string pointing to the shared database.

## Quick Start

1. **Clone and build:**
   ```bash
   git clone <repository-url>
   cd ORM
   dotnet build
   ```

2. **Create database:**
   ```bash
   cd Database
   ./CreateDatabase.sh  # macOS/Linux
   # or
   .\CreateDatabase.ps1 # Windows
   ```

3. **Run all projects:**
   ```bash
   ./run-all.sh  # macOS/Linux
   # or
   .\run-all.ps1 # Windows
   ```

4. **Test APIs:**
   - Entity Framework: https://localhost:5001/swagger
   - Dapper: https://localhost:5002/swagger
   - ADO.NET: https://localhost:5003/swagger

