-- 1) Crear la base si no existe
IF DB_ID('LabPizzeria') IS NULL
  CREATE DATABASE LabPizzeria;
GO

-- 2) Usar la base
USE LabPizzeria;
GO

-- 3) Borrar tablas viejas si existen (solo desarrollo)
IF OBJECT_ID('dbo.DETALLE_PEDIDO','U') IS NOT NULL DROP TABLE dbo.DETALLE_PEDIDO;
IF OBJECT_ID('dbo.PAGO','U') IS NOT NULL DROP TABLE dbo.PAGO;
IF OBJECT_ID('dbo.RESENA','U') IS NOT NULL DROP TABLE dbo.RESENA;
IF OBJECT_ID('dbo.REPARTIDOR','U') IS NOT NULL DROP TABLE dbo.REPARTIDOR;
IF OBJECT_ID('dbo.PEDIDO','U') IS NOT NULL DROP TABLE dbo.PEDIDO;
IF OBJECT_ID('dbo.DIRECCION','U') IS NOT NULL DROP TABLE dbo.DIRECCION;
IF OBJECT_ID('dbo.PLATILLO','U') IS NOT NULL DROP TABLE dbo.PLATILLO;
IF OBJECT_ID('dbo.CATEGORIA','U') IS NOT NULL DROP TABLE dbo.CATEGORIA;
IF OBJECT_ID('dbo.USUARIO','U') IS NOT NULL DROP TABLE dbo.USUARIO;
GO

-- 4) Crear tablas principales
CREATE TABLE USUARIO (
    usuario_id      INT IDENTITY(1,1) PRIMARY KEY,
    nombre          NVARCHAR(100) NOT NULL,
    email           NVARCHAR(100) NOT NULL UNIQUE,
    contraseña      NVARCHAR(255) NOT NULL,
    rol             NVARCHAR(20) NOT NULL
                    CONSTRAINT CK_Usuario_Rol CHECK (rol IN ('cliente','repartidor','admin')),
    fecha_registro  DATETIME NOT NULL DEFAULT(GETDATE()),
    estado          BIT      NOT NULL DEFAULT(1)
);
GO

CREATE TABLE CATEGORIA (
    categoria_id INT IDENTITY(1,1) PRIMARY KEY,
    nombre       NVARCHAR(100) NOT NULL,
    descripcion  NVARCHAR(255) NULL,
    estado       BIT         NOT NULL DEFAULT(1)
);
GO

CREATE TABLE PLATILLO (
    platillo_id        INT IDENTITY(1,1) PRIMARY KEY,
    nombre             NVARCHAR(100) NOT NULL,
    descripcion        NVARCHAR(255) NULL,
    precio             DECIMAL(10,2) NOT NULL,
    tiempo_preparacion INT           NOT NULL,
    imagen_url         NVARCHAR(255) NULL,
    categoria_id       INT           NOT NULL,
    estado             BIT           NOT NULL DEFAULT(1),
    CONSTRAINT FK_Platillo_Categoria FOREIGN KEY(categoria_id)
        REFERENCES CATEGORIA(categoria_id)
);
GO

CREATE TABLE DIRECCION (
    direccion_id  INT IDENTITY(1,1) PRIMARY KEY,
    usuario_id    INT           NOT NULL,
    calle         NVARCHAR(200) NOT NULL,
    ciudad        NVARCHAR(100) NOT NULL,
    codigo_postal NVARCHAR(20)  NULL,
    indicaciones  NVARCHAR(255) NULL,
    fecha_registro DATETIME     NOT NULL DEFAULT(GETDATE()),
    estado        BIT           NOT NULL DEFAULT(1),
    CONSTRAINT FK_Direccion_Usuario FOREIGN KEY(usuario_id)
        REFERENCES USUARIO(usuario_id)
);
GO

CREATE TABLE PEDIDO (
    pedido_id    INT IDENTITY(1,1) PRIMARY KEY,
    usuario_id   INT      NOT NULL,
    fecha        DATETIME NOT NULL DEFAULT(GETDATE()),
    estado       NVARCHAR(20) NOT NULL
                 CONSTRAINT CK_Pedido_Estado CHECK (estado IN (
                   'pendiente','en_preparacion','en_reparto','entregado','cancelado')),
    total        DECIMAL(10,2) NOT NULL,
    direccion_id INT NOT NULL,
    estado_registro BIT NOT NULL DEFAULT(1),
    CONSTRAINT FK_Pedido_Usuario   FOREIGN KEY(usuario_id)
        REFERENCES USUARIO(usuario_id),
    CONSTRAINT FK_Pedido_Direccion FOREIGN KEY(direccion_id)
        REFERENCES DIRECCION(direccion_id)
);
GO

CREATE TABLE DETALLE_PEDIDO (
    detalle_id      INT IDENTITY(1,1) PRIMARY KEY,
    pedido_id       INT NOT NULL,
    platillo_id     INT NOT NULL,
    cantidad        INT NOT NULL,
    precio_unitario DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_DetallePedido_Pedido   FOREIGN KEY(pedido_id)
        REFERENCES PEDIDO(pedido_id),
    CONSTRAINT FK_DetallePedido_Platillo FOREIGN KEY(platillo_id)
        REFERENCES PLATILLO(platillo_id)
);
GO

CREATE TABLE PAGO (
    pago_id      INT IDENTITY(1,1) PRIMARY KEY,
    pedido_id    INT NOT NULL,
    monto_pagado DECIMAL(10,2) NOT NULL,
    metodo       NVARCHAR(20) NOT NULL
                 CONSTRAINT CK_Pago_Metodo CHECK (metodo IN ('efectivo','tarjeta','paypal')),
    fecha_pago   DATETIME NOT NULL DEFAULT(GETDATE()),
    CONSTRAINT FK_Pago_Pedido FOREIGN KEY(pedido_id)
        REFERENCES PEDIDO(pedido_id)
);
GO

CREATE TABLE RESENA (
    resena_id    INT IDENTITY(1,1) PRIMARY KEY,
    usuario_id   INT NOT NULL,
    pedido_id    INT NOT NULL,
    calificacion TINYINT NOT NULL
                 CONSTRAINT CK_Resena_Calificacion CHECK (calificacion BETWEEN 1 AND 5),
    comentario   NVARCHAR(500) NULL,
    fecha        DATETIME NOT NULL DEFAULT(GETDATE()),
    estado_registro BIT NOT NULL DEFAULT(1),
    CONSTRAINT FK_Resena_Usuario FOREIGN KEY(usuario_id)
        REFERENCES USUARIO(usuario_id),
    CONSTRAINT FK_Resena_Pedido  FOREIGN KEY(pedido_id)
        REFERENCES PEDIDO(pedido_id)
);
GO

CREATE TABLE REPARTIDOR (
    usuario_id    INT PRIMARY KEY,
    nro_licencia  NVARCHAR(50) NOT NULL,
    fecha_ingreso DATETIME NOT NULL DEFAULT(GETDATE()),
    estado_registro BIT NOT NULL DEFAULT(1),
    CONSTRAINT FK_Repartidor_Usuario FOREIGN KEY(usuario_id)
        REFERENCES USUARIO(usuario_id)
);
GO
