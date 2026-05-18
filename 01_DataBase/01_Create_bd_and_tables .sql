CREATE DATABASE WhoWant2B;
GO
USE WhoWant2B;
GO

-- 1. Complejidades
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Complejidades]') AND type in (N'U'))
BEGIN
    CREATE TABLE Complejidades (
        IdComplejidad INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(100) NOT NULL        
    );
    PRINT 'Tabla Complejidades creada exitosamente.';
END
GO

-- 2. Categorias
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categorias]') AND type in (N'U'))
BEGIN
    CREATE TABLE Categorias (
        IdCategoria INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(100) NOT NULL        
    );
    PRINT 'Tabla Categorias creada exitosamente.';
END
GO

-- 3. Preguntas
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Preguntas]') AND type in (N'U'))
BEGIN
    CREATE TABLE Preguntas (
        IdPregunta INT IDENTITY(1,1) PRIMARY KEY, 
        Texto NVARCHAR(500) NOT NULL, 
        IdCategoria INT NOT NULL,
        IdComplejidad INT NOT NULL,
        
        CONSTRAINT FK_Preguntas_Categorias FOREIGN KEY (IdCategoria) REFERENCES Categorias (IdCategoria),
        CONSTRAINT FK_Preguntas_Complejidades FOREIGN KEY (IdComplejidad) REFERENCES Complejidades (IdComplejidad)
    );
    PRINT 'Tabla Preguntas creada exitosamente.';
END
GO

-- 4. Opciones
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Opciones]') AND type in (N'U'))
BEGIN
    CREATE TABLE Opciones (
        IdOpcion INT IDENTITY(1,1) PRIMARY KEY,
        Texto NVARCHAR(255) NOT NULL, -- Aumentado un poco el rango
        IdPregunta INT NOT NULL,        
        Valida BIT DEFAULT 0,

        CONSTRAINT FK_Opciones_Preguntas FOREIGN KEY (IdPregunta) REFERENCES Preguntas(IdPregunta)        
    );
    PRINT 'Tabla Opciones creada exitosamente.';
END
GO

-- 5. EstadoJuego
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EstadosJuego]') AND type in (N'U'))
BEGIN
    CREATE TABLE EstadosJuego (
        IdEstadoJuego INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(100) NOT NULL        
    );
    PRINT 'Tabla EstadosJuego creada exitosamente.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Roles](
	    IdRol int IDENTITY(1,1) NOT NULL PRIMARY KEY,
	    Nombre NVARCHAR(50) NOT NULL
    );
    PRINT 'Tabla Roles creada exitosamente.';
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Usuarios]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Usuarios](
    
        IdUsuario INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Login NVARCHAR(50) NOT NULL,
        Password VARBINARY(MAX) NOT NULL,
        IdRol INT NOT NULL,
        NombreReal VARCHAR(100) NOT NULL,
    
        CONSTRAINT FK_Usuarios_Roles FOREIGN KEY (IdRol) REFERENCES Roles(IdRol)        
    );
    PRINT 'Tabla Usuarios creada exitosamente.';
    END
GO

-- 6. Historicos (Corregido)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Historicos]') AND type in (N'U'))
BEGIN
    CREATE TABLE Historicos (
        IdHistorico INT IDENTITY(1,1) PRIMARY KEY,        
        PuntosAcumulados INT NOT NULL,        
        DineroAcumulado MONEY NOT NULL, -- Se agregó tipo de dato MONEY
        IdJugador INT NOT NULL,
        IdEstadoJuego INT NOT NULL,     -- Se quitaron paréntesis innecesarios
        Fecha DATETIME DEFAULT GETDATE(), -- Se agregó valor por defecto y coma faltante
        
        CONSTRAINT FK_Historicos_EstadosJuego FOREIGN KEY (IdEstadoJuego) REFERENCES EstadosJuego(IdEstadoJuego),
        CONSTRAINT FK_Historicos_Usuarios FOREIGN KEY (IdJugador) REFERENCES Usuarios(IdUsuario)
    );
    PRINT 'Tabla Historicos creada exitosamente.';
END
GO