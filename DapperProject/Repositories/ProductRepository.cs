using Dapper;
using Microsoft.Data.Sqlite;
using SharedModels.Models;

namespace DapperProject.Repositories;

public class ProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        const string sql = @"
            SELECT Id, Name, Description, Price, StockQuantity, CreatedAt, UpdatedAt, IsActive 
            FROM Products 
            WHERE IsActive = 1 
            ORDER BY Name";
        
        return await connection.QueryAsync<Product>(sql);
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        const string sql = @"
            SELECT Id, Name, Description, Price, StockQuantity, CreatedAt, UpdatedAt, IsActive 
            FROM Products 
            WHERE Id = @Id AND IsActive = 1";
        
        return await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });
    }

    public async Task<Product> CreateAsync(Product product)
    {
        using var connection = new SqliteConnection(_connectionString);
        const string sql = @"
            INSERT INTO Products (Name, Description, Price, StockQuantity, CreatedAt, IsActive) 
            VALUES (@Name, @Description, @Price, @StockQuantity, @CreatedAt, @IsActive);
            SELECT last_insert_rowid()";
        
        product.CreatedAt = DateTime.UtcNow;
        product.Id = await connection.QuerySingleAsync<int>(sql, product);
        return product;
    }

    public async Task<Product?> UpdateAsync(int id, Product product)
    {
        using var connection = new SqliteConnection(_connectionString);
        const string sql = @"
            UPDATE Products 
            SET Name = @Name, Description = @Description, Price = @Price, 
                StockQuantity = @StockQuantity, UpdatedAt = @UpdatedAt 
            WHERE Id = @Id AND IsActive = 1";
        
        product.UpdatedAt = DateTime.UtcNow;
        var rowsAffected = await connection.ExecuteAsync(sql, new { 
            product.Name, 
            product.Description, 
            product.Price, 
            product.StockQuantity, 
            product.UpdatedAt, 
            Id = id 
        });
        
        if (rowsAffected == 0)
            return null;
        
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        const string sql = @"
            UPDATE Products 
            SET IsActive = 0, UpdatedAt = @UpdatedAt 
            WHERE Id = @Id AND IsActive = 1";
        
        var rowsAffected = await connection.ExecuteAsync(sql, new { 
            UpdatedAt = DateTime.UtcNow, 
            Id = id 
        });
        
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm)
    {
        using var connection = new SqliteConnection(_connectionString);
        const string sql = @"
            SELECT Id, Name, Description, Price, StockQuantity, CreatedAt, UpdatedAt, IsActive 
            FROM Products 
            WHERE IsActive = 1 AND Name LIKE @SearchTerm 
            ORDER BY Name";
        
        return await connection.QueryAsync<Product>(sql, new { SearchTerm = $"%{searchTerm}%" });
    }

    public async Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        using var connection = new SqliteConnection(_connectionString);
        const string sql = @"
            SELECT Id, Name, Description, Price, StockQuantity, CreatedAt, UpdatedAt, IsActive 
            FROM Products 
            WHERE IsActive = 1 AND Price >= @MinPrice AND Price <= @MaxPrice 
            ORDER BY Price";
        
        return await connection.QueryAsync<Product>(sql, new { MinPrice = minPrice, MaxPrice = maxPrice });
    }
} 