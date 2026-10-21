CREATE DATABASE SistemaGestion
GO

USE SistemaGestion
GO

CREATE TABLE Suplidores(
idSuplidor INT IDENTITY(1,1) PRIMARY KEY,
nombreEmpresa NVARCHAR(150) NOT NULL UNIQUE,
nombreContacto NVARCHAR(100) NOT NULL,
telefono  VARCHAR(15) NOT NULL, 
email VARCHAR(100) NOT NULL UNIQUE,
fechaCreacion DATETIME NOT NULL,
fechaModificacion DATETIME,
estado  BIT NOT NULL
)

CREATE TABLE Imagenes(
idImagen INT IDENTITY(1,1) PRIMARY KEY,
fechaCreacion DATETIME NOT NULL,
fechaModificacion DATETIME,
estado BIT NOT NULL
)

CREATE TABLE Categorias(
idCategoria INT IDENTITY(1,1) PRIMARY KEY,
nombre NVARCHAR(100) NOT NULL,
descripcion NVARCHAR(500) NOT NULL,
idImagen INT FOREIGN KEY REFERENCES Imagenes(idImagen),
fechaCreacion DATETIME NOT NULL,
fechaModificacion  DATETIME,
estado BIT NOT NULL
)


CREATE TABLE Productos(
idProducto  INT IDENTITY(1,1) PRIMARY KEY,
nombre VARCHAR(100) NOT NULL,
precio  DECIMAL(18,2) NOT NULL,
idCategoria INT FOREIGN KEY REFERENCES Categorias(idCategoria),
idSuplidor INT FOREIGN KEY REFERENCES Suplidores(idSuplidor),
fechaCreacion  DATETIME NOT NULL,
fechaModificacion DATETIME,
estado BIT NOT NULL
)
GO
------------------------------------------
--Procedimientos almacenados
-------------------------------------------
-- Crear Categoría
CREATE or ALTER PROCEDURE sp_Categoria_Crear
    @nombre NVARCHAR(100),
    @descirpcion NVARCHAR(500),
    @idImagen INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Categorias (nombre, descripcion, idImagen, fechaCreacion, estado)
    VALUES (@nombre, @descirpcion, @idImagen, GETDATE(), 1);
END;
GO


-- Editar Categoría
CREATE OR ALTER PROCEDURE sp_Categoria_Editar
    @idCategoria INT,
    @nombre NVARCHAR(100),
    @descripcion NVARCHAR(500),
    @idImagen INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Categorias
    SET nombre = @nombre,
        descripcion = @descripcion,
        idImagen = @idImagen,
        fechaModificacion = GETDATE()
    WHERE idCategoria = @idCategoria;
END;
GO

-- Eliminar Lógicamente Categoría (Validando productos activos)
CREATE OR ALTER PROCEDURE sp_Categoria_Eliminar
    @idCategoria INT
AS
BEGIN
    SET NOCOUNT ON;

   
    IF EXISTS (SELECT 1 FROM Productos WHERE idCategoria = @idCategoria AND estado = 1)
    BEGIN
        RAISERROR('No se puede eliminar la categoría porque tiene productos activos asociados.', 16, 1);
        RETURN;
    END

    UPDATE Categorias
    SET estado = 0,
        fechaModificacion = GETDATE()
    WHERE idCategoria = @idCategoria;
END;
GO

-- Restaurar Categoría desde la Papelera
CREATE OR ALTER PROCEDURE sp_Categoria_Restaurar
    @idCategoria INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Categorias
    SET estado = 1,
        fechaModificacion = GETDATE()
    WHERE idCategoria = @idCategoria;
END;
GO

-- Listar Categorías en Papelera
CREATE OR ALTER PROCEDURE sp_Categoria_ObtenerPapelera
AS
BEGIN
    SET NOCOUNT ON;
    SELECT idCategoria, nombre, descripcion, idImagen, fechaCreacion, fechaModificacion
    FROM Categorias
    WHERE estado = 0;
END;
GO


-- Crear Suplidor
CREATE OR ALTER PROCEDURE sp_Suplidor_Crear
    @nombreEmpresa NVARCHAR(150),
    @nombreContacto NVARCHAR(100),
    @telefono VARCHAR(15),
    @email VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Suplidores (nombreEmpresa, nombreContacto, telefono, email, fechaCreacion, estado)
    VALUES (@nombreEmpresa, @nombreContacto, @telefono, @email, GETDATE(), 1);
END;
GO

-- Editar Suplidor
CREATE OR ALTER PROCEDURE sp_Suplidor_Editar
    @idSuplidor INT,
    @nombreEmpresa NVARCHAR(150),
    @nombreContacto NVARCHAR(100),
    @telefono VARCHAR(15),
    @email VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Suplidores
    SET nombreEmpresa = @nombreEmpresa,
        nombreContacto = @nombreContacto,
        telefono = @telefono,
        email = @email,
        fechaModificacion = GETDATE()
    WHERE idSuplidor = @idSuplidor;
END;
GO

-- Eliminar Lógicamente Suplidor (Validando productos activos)
CREATE OR ALTER PROCEDURE sp_Suplidor_Eliminar
    @idSuplidor INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Productos WHERE idSuplidor = @idSuplidor AND estado = 1)
    BEGIN
        RAISERROR('No se puede eliminar el suplidor porque tiene productos activos asociados.', 16, 1);
        RETURN;
    END

    UPDATE Suplidores
    SET estado = 0,
        fechaModificacion = GETDATE()
    WHERE idSuplidor = @idSuplidor;
END;
GO



-- Restaurar Suplidor desde la Papelera
CREATE OR ALTER PROCEDURE sp_Suplidor_Restaurar
    @idSuplidor INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Suplidores
    SET estado = 1,
        fechaModificacion = GETDATE()
    WHERE idSuplidor = @idSuplidor;
END;
GO


-- Crear Producto
CREATE OR ALTER PROCEDURE sp_Producto_Crear
    @nombre VARCHAR(100),
    @precio DECIMAL(18,2),
    @idCategoria INT,
    @idSuplidor INT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Productos (nombre, precio, idCategoria, idSuplidor, fechaCreacion, estado)
    VALUES (@nombre, @precio, @idCategoria, @idSuplidor, GETDATE(), 1);
END;
GO

-- Editar Producto
CREATE OR ALTER PROCEDURE sp_Producto_Editar
    @idProducto INT,
    @nombre VARCHAR(100),
    @precio DECIMAL(18,2),
    @idCategoria INT,
    @idSuplidor INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Productos
    SET nombre = @nombre,
        precio = @precio,
        idCategoria = @idCategoria,
        idSuplidor = @idSuplidor,
        fechaModificacion = GETDATE()
    WHERE idProducto = @idProducto;
END;
GO

-- Eliminar Lógicamente Producto
CREATE OR ALTER PROCEDURE sp_Producto_Eliminar
    @idProducto INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Productos
    SET estado = 0,
        fechaModificacion = GETDATE()
    WHERE idProducto = @idProducto;
END;
GO

-- Restaurar Producto desde la Papelera
CREATE OR ALTER PROCEDURE sp_Producto_Restaurar
    @idProducto INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Productos
    SET estado = 1,
        fechaModificacion = GETDATE()
    WHERE idProducto = @idProducto;
END;
GO

-- Listar Productos en Papelera
CREATE OR ALTER VIEW vw_Producto_ObtenerPapelera
AS
   
    SELECT p.idProducto, p.nombre, p.precio, c.nombre AS Categoria, s.nombreEmpresa AS Suplidor, p.fechaCreacion, p.fechaModificacion
    FROM Productos p
    INNER JOIN Categorias c ON p.idCategoria = c.idCategoria
    INNER JOIN Suplidores s ON p.idSuplidor = s.idSuplidor
    WHERE p.estado = 0;
GO



--Vista para las categorias
CREATE OR ALTER VIEW dbo.vw_Categorias AS
SELECT 
    idCategoria,
    nombre,
    descripcion,
    idImagen,
    fechaCreacion,
    fechaModificacion,
    estado
FROM dbo.Categorias;
GO

--Vista para Suplidores

CREATE OR ALTER VIEW dbo.vw_Suplidores AS
SELECT 
    idSuplidor,
    nombreEmpresa,
    nombreContacto,
    telefono,
    email,
    fechaCreacion,
    fechaModificacion,
    estado
FROM dbo.Suplidores;
GO

--Vista para Productos 

CREATE or ALTER VIEW dbo.vw_Productos AS
SELECT 
    p.idProducto,
    p.nombre,
    p.precio,
    p.idCategoria,
    c.nombre AS CategoriaNombre,
    p.idSuplidor,
    s.nombreEmpresa AS SuplidorNombre,
    p.fechaCreacion,
    p.fechaModificacion,
    p.estado
FROM dbo.Productos p
INNER JOIN dbo.Categorias c ON p.idCategoria = c.idCategoria
INNER JOIN dbo.Suplidores s ON p.idSuplidor = s.idSuplidor;
GO

TRUNCATE TABLE Productos
TRUNCATE TABLE Categorias
TRUNCATE TABLE Suplidores
TRUNCATE TABLE Imagenes

