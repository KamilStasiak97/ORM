using Microsoft.Data.Sqlite;
using SharedModels.Models;

namespace AdoNetProject.Repositories;

public class ProductRepository
{
    private readonly string _connectionString;

    public ProductRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var products = new List<Product>();
        
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqliteCommand(@"
            SELECT Id, Name, Description, Price, StockQuantity, CreatedAt, UpdatedAt, IsActive 
            FROM Products 
            WHERE IsActive = 1 
            ORDER BY Name", connection);
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            products.Add(new Product
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            });
        }
        
        return products;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqliteCommand(@"
            SELECT Id, Name, Description, Price, StockQuantity, CreatedAt, UpdatedAt, IsActive 
            FROM Products 
            WHERE Id = @Id AND IsActive = 1", connection);
        
        command.Parameters.AddWithValue("@Id", id);
        
        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Product
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }
        
        return null;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqliteCommand(@"
            INSERT INTO Products (Name, Description, Price, StockQuantity, CreatedAt, IsActive) 
            VALUES (@Name, @Description, @Price, @StockQuantity, @CreatedAt, @IsActive);
            SELECT last_insert_rowid()", connection);
        
        product.CreatedAt = DateTime.UtcNow;
        
        command.Parameters.AddWithValue("@Name", product.Name);
        command.Parameters.AddWithValue("@Description", product.Description);
        command.Parameters.AddWithValue("@Price", product.Price);
        command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
        command.Parameters.AddWithValue("@CreatedAt", product.CreatedAt);
        command.Parameters.AddWithValue("@IsActive", product.IsActive);
        
        product.Id = Convert.ToInt32(await command.ExecuteScalarAsync());
        return product;
    }

    public async Task<Product?> UpdateAsync(int id, Product product)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqliteCommand(@"
            UPDATE Products 
            SET Name = @Name, Description = @Description, Price = @Price, 
                StockQuantity = @StockQuantity, UpdatedAt = @UpdatedAt 
            WHERE Id = @Id AND IsActive = 1", connection);
        
        product.UpdatedAt = DateTime.UtcNow;
        
        command.Parameters.AddWithValue("@Name", product.Name);
        command.Parameters.AddWithValue("@Description", product.Description);
        command.Parameters.AddWithValue("@Price", product.Price);
        command.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
        command.Parameters.AddWithValue("@UpdatedAt", product.UpdatedAt);
        command.Parameters.AddWithValue("@Id", id);
        
        var rowsAffected = await command.ExecuteNonQueryAsync();
        if (rowsAffected == 0)
            return null;
        
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqliteCommand(@"
            UPDATE Products 
            SET IsActive = 0, UpdatedAt = @UpdatedAt 
            WHERE Id = @Id AND IsActive = 1", connection);
        
        command.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);
        command.Parameters.AddWithValue("@Id", id);
        
        var rowsAffected = await command.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Product>> SearchByNameAsync(string searchTerm)
    {
        var products = new List<Product>();
        
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqliteCommand(@"
            SELECT Id, Name, Description, Price, StockQuantity, CreatedAt, UpdatedAt, IsActive 
            FROM Products 
            WHERE IsActive = 1 AND Name LIKE @SearchTerm 
            ORDER BY Name", connection);
        
        command.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            products.Add(new Product
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            });
        }
        
        return products;
    }

    public async Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        var products = new List<Product>();
        
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync();
        
        using var command = new SqliteCommand(@"
            SELECT Id, Name, Description, Price, StockQuantity, CreatedAt, UpdatedAt, IsActive 
            FROM Products 
            WHERE IsActive = 1 AND Price >= @MinPrice AND Price <= @MaxPrice 
            ORDER BY Price", connection);
        
        command.Parameters.AddWithValue("@MinPrice", minPrice);
        command.Parameters.AddWithValue("@MaxPrice", maxPrice);
        
        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            products.Add(new Product
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.GetString(reader.GetOrdinal("Description")),
                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            });
        }
        
        return products;
    }
} 