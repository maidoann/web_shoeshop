-- Tạo cơ sở dữ liệu
CREATE DATABASE WEB_MQDD_SHOESHOP;
GO

USE WEB_MQDD_SHOESHOP;
GO

-- Bảng users
CREATE TABLE users (
    id INT PRIMARY KEY IDENTITY,
    name NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) UNIQUE NOT NULL,
    password NVARCHAR(255) NOT NULL,
    role NVARCHAR(10) NOT NULL DEFAULT 'Customer',
    avatar NVARCHAR(255) NULL,
    deleted BIT NOT NULL DEFAULT 0, -- Thêm cột deleted
    CONSTRAINT CHK_Role CHECK (role IN ('Customer', 'Admin'))
);

-- Bảng orders
CREATE TABLE orders (
    id INT PRIMARY KEY IDENTITY,
    user_id INT NOT NULL,
    first_name NVARCHAR(255) NOT NULL,
    last_name NVARCHAR(255) NOT NULL,
    street_address NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) NULL,
    phone NVARCHAR(20) NOT NULL,
    CONSTRAINT FK_orders_users FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);

-- Bảng brands
CREATE TABLE brands (
    id INT PRIMARY KEY IDENTITY,
    name NVARCHAR(255) NOT NULL,
    deleted BIT NOT NULL DEFAULT 0, -- Thêm cột deleted

);
INSERT INTO brands(name)
VALUES (N'Puma');

-- Bảng product_category
CREATE TABLE product_category (
    id INT PRIMARY KEY IDENTITY,
    name NVARCHAR(255) NOT NULL,
    deleted BIT NOT NULL DEFAULT 0, -- Thêm cột deleted

);
INSERT INTO product_category(name)
VALUES (N'Teen');

-- Bảng products
CREATE TABLE products (
    id INT PRIMARY KEY IDENTITY,
    brand_id INT NOT NULL,
    product_category_id INT NOT NULL,
    name NVARCHAR(255) NOT NULL,
    description NVARCHAR(MAX) NOT NULL,
    content NVARCHAR(MAX) NOT NULL,
    tag NVARCHAR(255) NULL,
    deleted BIT NOT NULL DEFAULT 0, -- Thêm cột deleted
    CONSTRAINT FK_products_brands FOREIGN KEY (brand_id) REFERENCES brands(id),
    CONSTRAINT FK_products_category FOREIGN KEY (product_category_id) REFERENCES product_category(id)
);

-- Bảng product_details
CREATE TABLE product_details (
    id INT PRIMARY KEY IDENTITY,
    product_id INT NOT NULL,
    color NVARCHAR(50) NOT NULL,
    size NVARCHAR(50) NOT NULL,
    price DECIMAL(18, 2) NOT NULL,
    qty INT,
    CONSTRAINT FK_product_details_products FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE
);

-- Bảng product_images
CREATE TABLE product_images (
    id INT PRIMARY KEY IDENTITY,
    product_id INT NOT NULL,
    path NVARCHAR(255) NULL,
    CONSTRAINT FK_product_images_products FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE
);

-- Bảng product_comments
CREATE TABLE product_comments (
    id INT PRIMARY KEY IDENTITY,
    product_id INT NOT NULL,
    user_id INT NOT NULL,
    name NVARCHAR(255) NOT NULL,
    messages NVARCHAR(MAX),
    rating DECIMAL(3, 2) NOT NULL,
    CONSTRAINT FK_product_comments_products FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE,
    CONSTRAINT FK_product_comments_users FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
);

-- Bảng wishlist
CREATE TABLE wishlist (
    id INT PRIMARY KEY IDENTITY,
    user_id INT NOT NULL,
    product_id INT NOT NULL,
    CONSTRAINT FK_wishlist_users FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    CONSTRAINT FK_wishlist_products FOREIGN KEY (product_id) REFERENCES products(id) ON DELETE CASCADE
);

-- Bảng order_details
CREATE TABLE order_details (
    id INT PRIMARY KEY IDENTITY,
    order_id INT NOT NULL,
    product_detail_id INT NOT NULL,
    qty INT NOT NULL,
    total DECIMAL(18, 2) NOT NULL,
    CONSTRAINT FK_order_details_orders FOREIGN KEY (order_id) REFERENCES orders(id) ON DELETE CASCADE,
    CONSTRAINT FK_order_details_product_details FOREIGN KEY (product_detail_id) REFERENCES product_details(id) ON DELETE CASCADE
);

GO

-- Ví dụ INSERT dữ liệu vào bảng products
INSERT INTO products (brand_id, product_category_id, name, description, content, tag)
VALUES (1, 1, 'TENSANPHAM', 'MOTA', 'CHITIETSANPHAM', 'TUKHOATAG');