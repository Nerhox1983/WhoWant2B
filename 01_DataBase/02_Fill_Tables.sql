USE [WhoWant2B];
GO

SET XACT_ABORT ON;
SET NOCOUNT ON;

-- ==========================================================================
-- 0. LIMPIEZA DE CORTESÍA (Por si acaso quedaron datos duplicados previos)
-- Descomenta las siguientes líneas si quieres vaciar las tablas antes de iniciar:
-- ==========================================================================
-- TRUNCATE TABLE [dbo].[Usuarios];
-- TRUNCATE TABLE [dbo].[EstadosJuego];
-- TRUNCATE TABLE [dbo].[Categorias];
-- TRUNCATE TABLE [dbo].[Complejidades];
-- GO

-- ==========================================================================
-- 1. TRANSACCIÓN GLOBAL DE CARGA SEGURA (IDEMPOTENTE)
-- ==========================================================================
BEGIN TRANSACTION;
BEGIN TRY

    DECLARE @ComplejidadesInsertadas INT = 0;
    DECLARE @CategoriasInsertadas INT = 0;
    DECLARE @EstadosInsertados INT = 0;
    DECLARE @RolesInsertados INT = 0;
    DECLARE @UsuariosInsertados INT = 0;
    

    -- ----------------------------------------------------------------------
    -- A. Carga de Complejidades (Evitando duplicar por Nombre)
    -- ----------------------------------------------------------------------
    INSERT INTO [dbo].[Complejidades] ([Nombre], [Descripcion])
    SELECT v.[Nombre], v.[Descripcion]
    FROM (VALUES     
        ('Novato', 'Cultura general, datos que todos saben.'),    
        ('Aprendiz', 'Geografía, historia básica, literatura.'),    
        ('Conocedor', 'Preguntas que requieren haber estudiado el tema.'),    
        ('Maestro', 'Datos curiosos y complejos que pocos conocen.'),    
        ('Omnisciente', 'Preguntas técnicas o de nicho extremo.')
    ) AS v([Nombre], [Descripcion])
    WHERE NOT EXISTS (
        SELECT 1 FROM [dbo].[Complejidades] c WHERE c.[Nombre] = v.[Nombre]
    );
    SET @ComplejidadesInsertadas = @@ROWCOUNT;


    -- ----------------------------------------------------------------------
    -- B. Carga de Categorías (Evitando duplicar por Nombre)
    -- ----------------------------------------------------------------------
    INSERT INTO [dbo].[Categorias] ([Nombre], [Descripcion]) 
    SELECT v.[Nombre], v.[Descripcion]
    FROM (VALUES           
        -- Ciencias y Academia    
        ('Matemáticas', 'Números, operaciones, álgebra, geometría y acertijos lógicos.'),    
        ('Biología y Naturaleza', 'El cuerpo humano, animales, plantas, ecosistemas y la vida en el planeta.'),    
        ('Historia Universal', 'Grandes civilizaciones, guerras, personajes históricos y eventos del pasado.'),    
        ('Geografía', 'Países, capitales, banderas, ríos, montañas y mapas del mundo.'),    
        ('Física y Química', 'Leyes del universo, materia, elementos químicos, energía y experimentos.'),    
        ('Literatura y Lengua', 'Obras clásicas, escritores, gramática, ortografía y mitología literaria.'),    
        ('Tecnología e Informática', 'Historia de la computación, internet, software, hardware y gadgets.'),    
        -- Entretenimiento y Cultura Pop    
        ('Cine y Séptimo Arte', 'Películas de culto, directores, premios Óscar, sagas famosas y actores.'),    
        ('Series y Televisión', 'Producciones de streaming, comedias clásicas, dibujos animados y shows de TV.'),    
        ('Música del Mundo', 'Géneros musicales, bandas icónicas, cantantes, álbumes y premios Grammy.'),    
        ('Videojuegos y Cultura Geek', 'Consolas, juegos clásicos y modernos, eSports, anime, manga y cómics.'),    
        ('Farándula y Espectáculos', 'Celebridades, alfombras rojas, chismes históricos y la vida de los famosos.'),    
        -- Deportes y Estilo de Vida    
        ('Fútbol Internacional', 'Mundiales, Champions League, ligas del mundo, jugadores y estadísticas.'),    
        ('Deportes y Olimpíadas', 'Baloncesto, Fórmula 1, tenis, ciclismo, atletismo y récords olímpicos.'),    
        ('Gastronomía y Bebidas', 'Platos típicos del mundo, alta cocina, ingredientes, licores y cafés.'),    
        ('Marcas y Negocios', 'Empresas multinacionales, logos famosos, eslóganes e historia comercial.'),    
        -- Curiosidades y Generalidades    
        ('Mitos y Mitología', 'Dioses griegos, romanos, nórdicos y leyendas urbanas populares.'),    
        ('Inventos y Descubrimientos', 'Grandes hallazgos de la humanidad, científicos, inventores y avances.'),    
        ('Récords y Curiosidades', 'Datos insólitos, Récords Guinness y hechos extraños pero reales.'),    
        ('Cultura General', 'Preguntas variadas que mezclan datos cotidianos de todo un poco.')
    ) AS v([Nombre], [Descripcion])
    WHERE NOT EXISTS (
        SELECT 1 FROM [dbo].[Categorias] cat WHERE cat.[Nombre] = v.[Nombre]
    );
    SET @CategoriasInsertadas = @@ROWCOUNT;


    -- ----------------------------------------------------------------------
    -- C. Carga de Estados de Juego (Evitando duplicar por Nombre)
    -- ----------------------------------------------------------------------
    INSERT INTO [dbo].[EstadosJuego] ([Nombre], [Descripcion])
    SELECT v.[Nombre], v.[Descripcion]
    FROM (VALUES    
        ('En curso', 'La partida está activa y el jugador aún no ha terminado de responder'),    
        ('Ganado', 'El jugador respondió correctamente las preguntas de los 5 niveles de complejidad'),    
        ('Retirado', 'El jugador optó por el Retiro Voluntario, llevándose el acumulado obtenido hasta la ronda anterior'),    
        ('Perdido', 'El jugador tuvo una Derrota al responder mal, lo que implica que pierde todo el acumulado')
    ) AS v([Nombre], [Descripcion])
    WHERE NOT EXISTS (
        SELECT 1 FROM [dbo].[EstadosJuego] e WHERE e.[Nombre] = v.[Nombre]
    );
    SET @EstadosInsertados = @@ROWCOUNT;

        -- ----------------------------------------------------------------------
    -- D. Carga de Roles (Evitando duplicar por Nombre)
    -- ----------------------------------------------------------------------

    INSERT INTO [dbo].[Roles] ([Nombre])
    SELECT v.[Nombre]
        FROM (VALUES                
        ('Administrador'),                
        ('Jugador')
    ) AS v([Nombre])
    WHERE NOT EXISTS (
    SELECT 1 
    FROM [dbo].[Roles] r 
    WHERE r.[Nombre] = v.[Nombre]
    );
    SET @RolesInsertados = @@ROWCOUNT;

    -- ----------------------------------------------------------------------
    -- E. Carga de Usuarios (Evitando duplicar Login o Doble Administrador)
    -- ----------------------------------------------------------------------
    INSERT INTO [dbo].[Usuarios] ([Login], [Password], [IdRol], [NombreReal])
    SELECT v.[Login], v.[Password], v.[IdRol], v.[NombreReal]
    FROM (VALUES        
        ('AWN2BM', HASHBYTES('SHA2_512', '1983Feb12#'), 1, 'Administrador'),        
        ('(NN)',   HASHBYTES('SHA2_512', '2026May15&'), 2, 'Ningun Nombre')
    ) AS v([Login], [Password], [IdRol], [NombreReal])
    WHERE NOT EXISTS (
        SELECT 1 FROM [dbo].[Usuarios] u 
        WHERE u.[Login] = v.[Login]
           OR (v.[IdRol] = 1 AND u.[IdRol] = 1)
    );
    SET @UsuariosInsertados = @@ROWCOUNT;

    -- Si todo salió bien hasta aquí, guardamos los cambios definitivamente
    COMMIT TRANSACTION;

    -- ==========================================================================
    -- 2. REPORTE DE RESULTADOS DE EJECUCIÓN
    -- ==========================================================================
    PRINT '==================================================';
    PRINT ' REPORTE DE CARGA DE DATOS';
    PRINT '==================================================';
    PRINT '-> Complejidades: ' + CASE WHEN @ComplejidadesInsertadas > 0 THEN 'ÉXITO (' + CAST(@ComplejidadesInsertadas AS VARCHAR) + ' agregadas).' ELSE 'OMITIDO (Ya existían).' END;
    PRINT '-> Categorías:    ' + CASE WHEN @CategoriasInsertadas > 0 THEN 'ÉXITO (' + CAST(@CategoriasInsertadas AS VARCHAR) + ' agregadas).' ELSE 'OMITIDO (Ya existían).' END;
    PRINT '-> Estados Juego: ' + CASE WHEN @EstadosInsertados > 0 THEN 'ÉXITO (' + CAST(@EstadosInsertados AS VARCHAR) + ' agregados).' ELSE 'OMITIDO (Ya existían).' END;
    PRINT '-> Roles:         ' + CASE WHEN @EstadosInsertados > 0 THEN 'ÉXITO (' + CAST(@RolesInsertados AS VARCHAR) + ' agregados).' ELSE 'OMITIDO (Ya existían).' END;
    PRINT '-> Usuarios:      ' + CASE WHEN @UsuariosInsertados > 0 THEN 'ÉXITO (' + CAST(@UsuariosInsertados AS VARCHAR) + ' agregados).' ELSE 'OMITIDO (Login o Admin ya existen).' END;
    PRINT '==================================================';

END TRY
BEGIN CATCH
    -- Si ALGO falla en cualquiera de las tablas, no se guarda ABSOLUTAMENTE NADA
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
        
    PRINT 'ERROR CRÍTICO: Se detectó un problema técnico. Toda la transacción fue cancelada (ROLLBACK).';
    THROW; 
END CATCH;
GO