-- Create SQLite database for ORM Comparison
-- This database will be shared by all three projects (EF, Dapper, ADO.NET)

-- Create Products table
CREATE TABLE IF NOT EXISTS Products (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    Description TEXT,
    Price REAL NOT NULL,
    StockQuantity INTEGER NOT NULL DEFAULT 0,
    CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
    UpdatedAt TEXT,
    IsActive INTEGER NOT NULL DEFAULT 1
);

-- Create Customers table
CREATE TABLE IF NOT EXISTS Customers (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    Email TEXT NOT NULL UNIQUE,
    Phone TEXT,
    Address TEXT,
    City TEXT,
    PostalCode TEXT,
    CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
    UpdatedAt TEXT,
    IsActive INTEGER NOT NULL DEFAULT 1
);

-- Create Orders table
CREATE TABLE IF NOT EXISTS Orders (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    CustomerId INTEGER NOT NULL,
    OrderDate TEXT NOT NULL DEFAULT (datetime('now')),
    TotalAmount REAL NOT NULL,
    Status TEXT NOT NULL DEFAULT 'Pending',
    Notes TEXT,
    CreatedAt TEXT NOT NULL DEFAULT (datetime('now')),
    UpdatedAt TEXT,
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);

-- Create OrderItems table
CREATE TABLE IF NOT EXISTS OrderItems (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    OrderId INTEGER NOT NULL,
    ProductId INTEGER NOT NULL,
    Quantity INTEGER NOT NULL,
    UnitPrice REAL NOT NULL,
    TotalPrice REAL GENERATED ALWAYS AS (Quantity * UnitPrice) STORED,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Products(Id)
);

-- Create indexes
CREATE INDEX IF NOT EXISTS IX_Products_Name ON Products(Name);
CREATE INDEX IF NOT EXISTS IX_Products_Price ON Products(Price);
CREATE INDEX IF NOT EXISTS IX_Customers_Email ON Customers(Email);
CREATE INDEX IF NOT EXISTS IX_Orders_OrderDate ON Orders(OrderDate);
CREATE INDEX IF NOT EXISTS IX_Orders_CustomerId ON Orders(CustomerId);
CREATE INDEX IF NOT EXISTS IX_OrderItems_OrderId ON OrderItems(OrderId);
CREATE INDEX IF NOT EXISTS IX_OrderItems_ProductId ON OrderItems(ProductId);

-- Insert sample data
INSERT OR IGNORE INTO Products (Name, Description, Price, StockQuantity) VALUES
('Laptop', 'High-performance laptop for professionals', 1299.99, 50),
('Smartphone', 'Latest smartphone with advanced features', 799.99, 100),
('Tablet', 'Portable tablet for entertainment and work', 499.99, 75),
('Headphones', 'Wireless noise-canceling headphones', 199.99, 200),
('Mouse', 'Ergonomic wireless mouse', 49.99, 150);

INSERT OR IGNORE INTO Customers (FirstName, LastName, Email, Phone, Address, City, PostalCode) VALUES
('John', 'Doe', 'john.doe@email.com', '555-0101', '123 Main St', 'New York', '10001'),
('Jane', 'Smith', 'jane.smith@email.com', '555-0102', '456 Oak Ave', 'Los Angeles', '90210'),
('Bob', 'Johnson', 'bob.johnson@email.com', '555-0103', '789 Pine Rd', 'Chicago', '60601'),
('Alice', 'Brown', 'alice.brown@email.com', '555-0104', '321 Elm St', 'Houston', '77001'),
('Charlie', 'Wilson', 'charlie.wilson@email.com', '555-0105', '654 Maple Dr', 'Phoenix', '85001'); 