-- Create database for Entity Framework project
USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'ORMComparisonEF')
BEGIN
    ALTER DATABASE ORMComparisonEF SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE ORMComparisonEF;
END
GO

CREATE DATABASE ORMComparisonEF;
GO

USE ORMComparisonEF;
GO

-- Create Products table
CREATE TABLE Products (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(18,2) NOT NULL,
    StockQuantity INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

-- Create Customers table
CREATE TABLE Customers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Phone NVARCHAR(20),
    Address NVARCHAR(500),
    City NVARCHAR(100),
    PostalCode NVARCHAR(20),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL,
    IsActive BIT NOT NULL DEFAULT 1
);

-- Create Orders table
CREATE TABLE Orders (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL,
    OrderDate DATETIME2 NOT NULL DEFAULT GETDATE(),
    TotalAmount DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',
    Notes NVARCHAR(MAX),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME2 NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);

-- Create OrderItems table
CREATE TABLE OrderItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    TotalPrice AS (Quantity * UnitPrice) PERSISTED,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

-- Create indexes
CREATE INDEX IX_Products_Name ON Products(Name);
CREATE INDEX IX_Products_Price ON Products(Price);
CREATE INDEX IX_Customers_Email ON Customers(Email);
CREATE INDEX IX_Orders_OrderDate ON Orders(OrderDate);
CREATE INDEX IX_Orders_CustomerId ON Orders(CustomerId);
CREATE INDEX IX_OrderItems_OrderId ON OrderItems(OrderId);
CREATE INDEX IX_OrderItems_ProductId ON OrderItems(ProductId);

-- Insert sample data
INSERT INTO Products (Name, Description, Price, StockQuantity) VALUES
('Laptop', 'High-performance laptop for professionals', 1299.99, 50),
('Smartphone', 'Latest smartphone with advanced features', 799.99, 100),
('Tablet', 'Portable tablet for entertainment and work', 499.99, 75),
('Headphones', 'Wireless noise-canceling headphones', 199.99, 200),
('Mouse', 'Ergonomic wireless mouse', 49.99, 150);

INSERT INTO Customers (FirstName, LastName, Email, Phone, Address, City, PostalCode) VALUES
('John', 'Doe', 'john.doe@email.com', '555-0101', '123 Main St', 'New York', '10001'),
('Jane', 'Smith', 'jane.smith@email.com', '555-0102', '456 Oak Ave', 'Los Angeles', '90210'),
('Bob', 'Johnson', 'bob.johnson@email.com', '555-0103', '789 Pine Rd', 'Chicago', '60601'),
('Alice', 'Brown', 'alice.brown@email.com', '555-0104', '321 Elm St', 'Houston', '77001'),
('Charlie', 'Wilson', 'charlie.wilson@email.com', '555-0105', '654 Maple Dr', 'Phoenix', '85001');

GO 