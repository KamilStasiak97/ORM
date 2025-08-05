using Microsoft.EntityFrameworkCore;
using SharedModels.Models;

namespace EntityFrameworkProject.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure indexes
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.Name);

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.OrderDate);

        // Configure default values for SQLite
        modelBuilder.Entity<Product>()
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("datetime('now')");

        modelBuilder.Entity<Customer>()
            .Property(c => c.CreatedAt)
            .HasDefaultValueSql("datetime('now')");

        modelBuilder.Entity<Order>()
            .Property(o => o.CreatedAt)
            .HasDefaultValueSql("datetime('now')");

        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.TotalPrice)
            .HasComputedColumnSql("Quantity * UnitPrice");
    }
} 