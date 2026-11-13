

USE master
GO

-- Si la base ya existe, se borra para poder correr el script de cero
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'LaboratorioKits')
BEGIN
    DROP DATABASE LaboratorioKits
END
GO

CREATE DATABASE LaboratorioKits
GO

USE LaboratorioKits
GO

-- ===== TABLAS =====
-- Las tablas "padre" van primero porque las otras las referencian

CREATE TABLE Categorias (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL
)

CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario VARCHAR(50) NOT NULL UNIQUE,
    Contrasena VARCHAR(50) NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    Apellido VARCHAR(50) NOT NULL
)

CREATE TABLE Bloques (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Numero INT NOT NULL,
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL
)

CREATE TABLE Kits (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(500) NOT NULL,
    Imagen VARCHAR(200) NOT NULL,
    IdCategoria INT NOT NULL REFERENCES Categorias(Id),
    Activo BIT NOT NULL DEFAULT 1
)

CREATE TABLE Prestamos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdKit INT NOT NULL REFERENCES Kits(Id),
    IdUsuario INT NOT NULL REFERENCES Usuarios(Id),
    Fecha DATE NOT NULL,
    IdBloque INT NOT NULL REFERENCES Bloques(Id),
    Codigo VARCHAR(6) NOT NULL UNIQUE,
    Cancelado BIT NOT NULL DEFAULT 0,
    FechaCancelacion DATETIME NULL
)
GO