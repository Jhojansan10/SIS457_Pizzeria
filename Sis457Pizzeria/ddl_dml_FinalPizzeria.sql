-- Crear base de datos
CREATE DATABASE FinalPizzeria;
GO

USE FinalPizzeria;
GO

-- Cliente
CREATE TABLE Cliente (
  id INT PRIMARY KEY IDENTITY(1,1),
  cedulaIdentidad VARCHAR(20) NOT NULL,
  nombres VARCHAR(100) NOT NULL,
  primerApellido VARCHAR(50),
  segundoApellido VARCHAR(50),
  celular VARCHAR(20),
  direccion VARCHAR(250),
  fechaRegistro DATETIME DEFAULT GETDATE(),
  estado INT DEFAULT 1
);
GO

-- UsuarioCliente
CREATE TABLE UsuarioCliente (
  id INT PRIMARY KEY IDENTITY(1,1),
  idCliente INT NOT NULL,
  usuarioLogin VARCHAR(50) NOT NULL UNIQUE,
  clave VARCHAR(100) NOT NULL,
  fechaRegistro DATETIME DEFAULT GETDATE(),
  estado INT DEFAULT 1,
  FOREIGN KEY (idCliente) REFERENCES Cliente(id)
);
GO

-- Empleado
CREATE TABLE Empleado (
  id INT PRIMARY KEY IDENTITY(1,1),
  cedulaIdentidad VARCHAR(20) NOT NULL,
  nombres VARCHAR(100) NOT NULL,
  primerApellido VARCHAR(50),
  segundoApellido VARCHAR(50),
  direccion VARCHAR(250),
  celular VARCHAR(20),
  cargo VARCHAR(20) NOT NULL, -- 'Administrador', 'Empleado', 'Repartidor'
  fechaRegistro DATETIME DEFAULT GETDATE(),
  estado INT DEFAULT 1
);
GO

-- Usuario (Empleado)
CREATE TABLE Usuario (
  id INT PRIMARY KEY IDENTITY(1,1),
  idEmpleado INT NOT NULL,
  usuarioLogin VARCHAR(50) NOT NULL UNIQUE,
  clave VARCHAR(100) NOT NULL,
  fechaRegistro DATETIME DEFAULT GETDATE(),
  estado INT DEFAULT 1,
  FOREIGN KEY (idEmpleado) REFERENCES Empleado(id)
);
GO

-- CategoriaProducto
CREATE TABLE CategoriaProducto (
  id INT PRIMARY KEY IDENTITY(1,1),
  nombre VARCHAR(50) NOT NULL UNIQUE,
  descripcion VARCHAR(250),
  fechaRegistro DATETIME DEFAULT GETDATE(),
  estado INT DEFAULT 1
);
GO

-- Producto
CREATE TABLE Producto (
  id INT PRIMARY KEY IDENTITY(1,1),
  codigo VARCHAR(20) NOT NULL UNIQUE,
  descripcion VARCHAR(100) NOT NULL,
  precioVenta DECIMAL(10,2) NOT NULL CHECK (precioVenta > 0),
  idCategoria INT NOT NULL,
  stock INT DEFAULT 0,
  usuarioRegistro INT NULL, -- FK a Usuario que registró el producto
  fechaRegistro DATETIME DEFAULT GETDATE(),
  estado INT DEFAULT 1,
  FOREIGN KEY (idCategoria) REFERENCES CategoriaProducto(id)
);
GO

-- Direccion
CREATE TABLE Direccion (
  id INT PRIMARY KEY IDENTITY(1,1),
  idCliente INT NOT NULL,
  calle VARCHAR(250) NOT NULL,
  indicaciones VARCHAR(250),
  fechaRegistro DATETIME DEFAULT GETDATE(),
  estado INT DEFAULT 1,
  FOREIGN KEY (idCliente) REFERENCES Cliente(id)
);
GO

-- Pedido
CREATE TABLE Pedido (
  id INT PRIMARY KEY IDENTITY(1,1),
  idCliente INT NOT NULL,
  idRepartidor INT NULL,
  modoEntrega VARCHAR(20) NOT NULL, -- 'Local' o 'Delivery'
  estadoEntrega VARCHAR(30) NOT NULL, -- 'Pendiente', 'En camino', 'Entregado', 'Cancelado'
  total DECIMAL(10,2) NOT NULL,
  fechaPedido DATETIME DEFAULT GETDATE(),
  estado INT DEFAULT 1,
  FOREIGN KEY (idCliente) REFERENCES Cliente(id),
  FOREIGN KEY (idRepartidor) REFERENCES Empleado(id)
);
GO

-- DetallePedido
CREATE TABLE DetallePedido (
  id INT PRIMARY KEY IDENTITY(1,1),
  idPedido INT NOT NULL,
  idProducto INT NOT NULL,
  cantidad INT NOT NULL CHECK (cantidad > 0),
  precioUnitario DECIMAL(10,2) NOT NULL,
  total DECIMAL(10,2) NOT NULL,
  estado INT DEFAULT 1,
  FOREIGN KEY (idPedido) REFERENCES Pedido(id),
  FOREIGN KEY (idProducto) REFERENCES Producto(id)
);
GO

-- Resena
CREATE TABLE Resena (
  id INT PRIMARY KEY IDENTITY(1,1),
  idCliente INT NOT NULL,
  idPedido INT NOT NULL UNIQUE,
  calificacion INT NOT NULL CHECK (calificacion BETWEEN 1 AND 5),
  comentario VARCHAR(500),
  fecha DATETIME DEFAULT GETDATE(),
  estado INT DEFAULT 1,
  FOREIGN KEY (idCliente) REFERENCES Cliente(id),
  FOREIGN KEY (idPedido) REFERENCES Pedido(id)
);
GO

-- Pago
CREATE TABLE Pago (
  id INT PRIMARY KEY IDENTITY(1,1),
  idPedido INT NOT NULL UNIQUE,
  metodoPago VARCHAR(20) NOT NULL CHECK (metodoPago IN ('Efectivo', 'QR')),
  estado INT DEFAULT 1,
  FOREIGN KEY (idPedido) REFERENCES Pedido(id)
);
GO

INSERT INTO CategoriaProducto (nombre, descripcion, estado)
VALUES 
    ('Pizza Clásica', 'Pizzas con ingredientes tradicionales', 1),
    ('Pizza Especial', 'Pizzas con ingredientes premium', 1),
    ('Bebidas', 'Gaseosas, jugos y aguas', 1),
    ('Postres', 'Tiramisú, helados, tortas', 1);


ALTER TABLE Pedido
ADD CONSTRAINT FK_Pedido_Direccion
FOREIGN KEY (idDireccion) REFERENCES Direccion(id);
