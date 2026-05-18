USE [WhoWant2B];
GO

-- ==============================================================================
-- SCRIPT COMPLETO DE POBLACIÓN: 5 PREGUNTAS POR CATEGORÍA (1 POR NIVEL)
-- PROTEGIDO CONTRA DUPLICADOS (IF NOT EXISTS)
-- ==============================================================================

DECLARE @IdPregunta INT;
DECLARE @IdCategoria INT;
DECLARE @IdComplejidad INT;

-- ==============================================================================
-- 1. CATEGORÍA: MATEMÁTICAS
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Matemáticas';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuánto es 7 x 8?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuánto es 7 x 8?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('54', 0, @IdPregunta), ('56', 1, @IdPregunta), ('64', 0, @IdPregunta), ('48', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es la raíz cuadrada de 144?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es la raíz cuadrada de 144?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('10', 0, @IdPregunta), ('11', 0, @IdPregunta), ('12', 1, @IdPregunta), ('14', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué número romano representa al 50?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué número romano representa al 50?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('V', 0, @IdPregunta), ('X', 0, @IdPregunta), ('L', 1, @IdPregunta), ('C', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el valor aproximado del número de Euler (e)?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el valor aproximado del número de Euler (e)?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('3.1416', 0, @IdPregunta), ('1.6180', 0, @IdPregunta), ('2.7182', 1, @IdPregunta), ('1.4142', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué área de las matemáticas estudia las propiedades de los espacios que no cambian bajo transformaciones continuas?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué área de las matemáticas estudia las propiedades de los espacios que no cambian bajo transformaciones continuas?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Álgebra Lineal', 0, @IdPregunta), ('Cálculo Tensorial', 0, @IdPregunta), ('Topología', 1, @IdPregunta), ('Fractales', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 2. CATEGORÍA: BIOLOGÍA Y NATURALEZA
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Biología y Naturaleza';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿De qué color es la clorofila presente en las plantas?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿De qué color es la clorofila presente en las plantas?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Rojo', 0, @IdPregunta), ('Verde', 1, @IdPregunta), ('Azul', 0, @IdPregunta), ('Amarillo', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el único mamífero capaz de volar?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el único mamífero capaz de volar?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('El pingüino', 0, @IdPregunta), ('El murciélago', 1, @IdPregunta), ('El avestruz', 0, @IdPregunta), ('La ardilla voladora', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué gas absorben las plantas para realizar la fotosíntesis?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué gas absorben las plantas para realizar la fotosíntesis?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Oxígeno', 0, @IdPregunta), ('Nitrógeno', 0, @IdPregunta), ('Dióxido de carbono', 1, @IdPregunta), ('Hidrógeno', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el órgano más grande del cuerpo humano?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el órgano más grande del cuerpo humano?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('El hígado', 0, @IdPregunta), ('El cerebro', 0, @IdPregunta), ('La piel', 1, @IdPregunta), ('Los pulmones', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál de las siguientes bases nitrogenadas es exclusiva del ARN y no se encuentra en el ADN?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál de las siguientes bases nitrogenadas es exclusiva del ARN y no se encuentra en el ADN?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Timina', 0, @IdPregunta), ('Guanina', 0, @IdPregunta), ('Uracilo', 1, @IdPregunta), ('Adenina', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 3. CATEGORÍA: HISTORIA UNIVERSAL
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Historia Universal';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué célebre barco se hundió en su viaje inaugural en 1912?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué célebre barco se hundió en su viaje inaugural en 1912?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Santa María', 0, @IdPregunta), ('Titanic', 1, @IdPregunta), ('Britannic', 0, @IdPregunta), ('Lusitania', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿En qué año llegó Cristóbal Colón a América?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿En qué año llegó Cristóbal Colón a América?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('1492', 1, @IdPregunta), ('1502', 0, @IdPregunta), ('1488', 0, @IdPregunta), ('1519', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué civilización antigua construyó las pirámides de Guiza?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué civilización antigua construyó las pirámides de Guiza?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Los Mayas', 0, @IdPregunta), ('Los Incas', 0, @IdPregunta), ('Los Egipcios', 1, @IdPregunta), ('Los Romanos', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Quién fue el primer emperador del Imperio Romano?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Quién fue el primer emperador del Imperio Romano?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Julio César', 0, @IdPregunta), ('César Augusto', 1, @IdPregunta), ('Nerón', 0, @IdPregunta), ('Calígula', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿En qué año se firmó la Paz de Westfalia, dando fin a la Guerra de los Treinta Años?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿En qué año se firmó la Paz de Westfalia, dando fin a la Guerra de los Treinta Anos?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('1517', 0, @IdPregunta), ('1789', 0, @IdPregunta), ('1648', 1, @IdPregunta), ('1453', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 4. CATEGORÍA: GEOGRAFÍA
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Geografía';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es la capital de Francia?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es la capital de Francia?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Londres', 0, @IdPregunta), ('París', 1, @IdPregunta), ('Madrid', 0, @IdPregunta), ('Roma', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el río más largo del mundo?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el río más largo del mundo?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Río Nilo', 0, @IdPregunta), ('Río Amazonas', 1, @IdPregunta), ('Río Misisipi', 0, @IdPregunta), ('Río Yangtsé', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el país más grande del mundo por extensión territorial?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el país más grande del mundo por extensión territorial?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Canadá', 0, @IdPregunta), ('China', 0, @IdPregunta), ('Estados Unidos', 0, @IdPregunta), ('Rusia', 1, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué océano baña las costas orientales de África y occidentales de Australia?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué océano baña las costas orientales de África y occidentales de Australia?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Océano Atlántico', 0, @IdPregunta), ('Océano Pacífico', 0, @IdPregunta), ('Océano Índico', 1, @IdPregunta), ('Océano Ártico', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el punto más bajo en la superficie terrestre no cubierto por océano líquido?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el punto más bajo en la superficie terrestre no cubierto por océano líquido?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('El Mar Muerto', 0, @IdPregunta), ('La Depresión de Turpán', 0, @IdPregunta), ('La Fosa del Bentley', 1, @IdPregunta), ('El Lago Assal', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 5. CATEGORÍA: FÍSICA Y QUÍMICA
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Física y Química';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es la fórmula química del agua?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es la fórmula química del agua?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('CO2', 0, @IdPregunta), ('H2O', 1, @IdPregunta), ('NaCl', 0, @IdPregunta), ('O2', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué partícula subatómica tiene carga eléctrica negativa?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué partícula subatómica tiene carga eléctrica negativa?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Protón', 0, @IdPregunta), ('Neutrón', 0, @IdPregunta), ('Electrón', 1, @IdPregunta), ('Quark', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el elemento más abundante en el universo entero?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el elemento más abundante en el universo entero?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Helio', 0, @IdPregunta), ('Oxígeno', 0, @IdPregunta), ('Hidrógeno', 1, @IdPregunta), ('Carbono', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Quién formuló la teoría de la Relatividad General?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Quién formuló la teoría de la Relatividad General?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Isaac Newton', 0, @IdPregunta), ('Nikola Tesla', 0, @IdPregunta), ('Albert Einstein', 1, @IdPregunta), ('Niels Bohr', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué principio de la mecánica cuántica afirma que no se puede conocer con precisión simultánea la posición y el momento de una partícula?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué principio de la mecánica cuántica afirma que no se puede conocer con precisión simultánea la posición y el momento de una partícula?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Principio de Exclusión', 0, @IdPregunta), ('Efecto Fotoeléctrico', 0, @IdPregunta), ('Principio de Incertidumbre', 1, @IdPregunta), ('Efecto Túnel', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 6. CATEGORÍA: LITERATURA Y LENGUA
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Literatura y Lengua';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE (Texto = '¿Por qué le crecía la nariz a Pinocho?' OR Texto = '¿Por que le crecia la Nariz a Pinocho"?') AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Por qué le crecía la nariz a Pinocho?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Por decir la verdad', 0, @IdPregunta), ('Por contar chistes', 0, @IdPregunta), ('Por decir mentiras', 1, @IdPregunta), ('Por repetir las cosas', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Quién escribió la famosa obra "Don Quijote de la Mancha"?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Quién escribió la famosa obra "Don Quijote de la Mancha"?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Gabriel García Márquez', 0, @IdPregunta), ('Miguel de Cervantes', 1, @IdPregunta), ('Federico García Lorca', 0, @IdPregunta), ('William Shakespeare', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué tipo de palabra es "rápidamente"?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué tipo de palabra es "rápidamente"?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Sustantivo', 0, @IdPregunta), ('Verbo', 0, @IdPregunta), ('Adjetivo', 0, @IdPregunta), ('Adverbio', 1, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el nombre del poema épico anglosajón del siglo VIII que narra las hazañas de un héroe contra el monstruo Grendel?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el nombre del poema épico anglosajón del siglo VIII que narra las hazañas de un héroe contra el monstruo Grendel?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('La Odisea', 0, @IdPregunta), ('El Cantar de Roldán', 0, @IdPregunta), ('Beowulf', 1, @IdPregunta), ('Las Metamorfosis', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál de las siguientes figuras retóricas consiste en alterar el orden lógico convencional de las palabras en una oración?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál de las siguientes figuras retóricas consiste en alterar el orden lógico convencional de las palabras en una oración?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Oxímoron', 0, @IdPregunta), ('Metonimia', 0, @IdPregunta), ('Hipérbaton', 1, @IdPregunta), ('Sinécdoque', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 7. CATEGORÍA: TECNOLOGÍA E INFORMÁTICA
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Tecnología e Informática';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué significan las siglas "WWW" en una dirección web?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué significan las siglas "WWW" en una dirección web?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('World Wide Web', 1, @IdPregunta), ('Word Wide Website', 0, @IdPregunta), ('Web Window World', 0, @IdPregunta), ('Wireless Web Window', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el componente principal que actúa como el "cerebro" de una computadora?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el componente principal que actúa como el "cerebro" de una computadora?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Memoria RAM', 0, @IdPregunta), ('Disco Duro', 0, @IdPregunta), ('CPU', 1, @IdPregunta), ('Tarjeta Madre', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿En qué año se lanzó públicamente la primera versión del sistema operativo Linux?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿En qué año se lanzó públicamente la primera versión del sistema operativo Linux?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('1985', 0, @IdPregunta), ('1991', 1, @IdPregunta), ('1995', 0, @IdPregunta), ('2000', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué protocolo de red se encarga de asignar dinámicamente direcciones IP a los dispositivos en una red local?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué protocolo de red se encarga de asignar dinámicamente direcciones IP a los dispositivos en una red local?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('DNS', 0, @IdPregunta), ('HTTP', 0, @IdPregunta), ('DHCP', 1, @IdPregunta), ('FTP', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es la complejidad temporal en el peor de los casos del algoritmo de ordenación QuickSort estándar sin optimizaciones?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es la complejidad temporal en el peor de los casos del algoritmo de ordenación QuickSort estándar sin optimizaciones?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('O(n)', 0, @IdPregunta), ('O(n log n)', 0, @IdPregunta), ('O(n^2)', 1, @IdPregunta), ('O(2^n)', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 8. CATEGORÍA: CINE Y SÉPTIMO ARTE
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Cine y Séptimo Arte';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué premio anual entrega la Academia de Artes y Ciencias Cinematográficas de Hollywood?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué premio anual entrega la Academia de Artes y Ciencias Cinematográficas de Hollywood?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Grammy', 0, @IdPregunta), ('Oscar', 1, @IdPregunta), ('Goya', 0, @IdPregunta), ('Emmy', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Quién dirigió la famosa película de ciencia ficción "Inception" (El Origen)?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Quién dirigió la famosa película de ciencia ficción "Inception" (El Origen)?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Steven Spielberg', 0, @IdPregunta), ('Quentin Tarantino', 0, @IdPregunta), ('Christopher Nolan', 1, @IdPregunta), ('James Cameron', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué película de 1994 tiene la banda sonora más vendida de la historia de la animación?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué película de 1994 tiene la banda sonora más vendida de la historia de la animación?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Aladdín', 0, @IdPregunta), ('Toy Story', 0, @IdPregunta), ('El Rey León', 1, @IdPregunta), ('Pocahontas', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el nombre del icónico plano secuencia de tres minutos al inicio de la película "Touch of Evil" de Orson Welles?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el nombre del icónico plano secuencia de tres minutos al inicio de la película "Touch of Evil" de Orson Welles?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('El plano del espejo', 0, @IdPregunta), ('El plano de la frontera', 0, @IdPregunta), ('El plano de la bomba de tiempo', 1, @IdPregunta), ('El plano del callejón', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué director soviético desarrolló y teorizó el llamado "Montaje de Atracciones" en los albores del cine mudo?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué director soviético desarrolló y teorizó el llamado "Montaje de Atracciones" en los albores del cine mudo?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Dziga Vertov', 0, @IdPregunta), ('Lev Kuleshov', 0, @IdPregunta), ('Serguéi Eisenstein', 1, @IdPregunta), ('Vsévolod Pudovkin', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 9. CATEGORÍA: SERIES Y TELEVISIÓN
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Series y Televisión';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿En qué ciudad estadounidense viven "Los Simpson"?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿En qué ciudad estadounidense viven "Los Simpson"?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Shelbyville', 0, @IdPregunta), ('Springfield', 1, @IdPregunta), ('Capital City', 0, @IdPregunta), ('Quahog', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el nombre del químico que se convierte en traficante en la serie "Breaking Bad"?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el nombre del químico que se convierte en traficante en la serie "Breaking Bad"?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Jesse Pinkman', 0, @IdPregunta), ('Hank Schrader', 0, @IdPregunta), ('Walter White', 1, @IdPregunta), ('Saul Goodman', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cómo se llama el continente ficticio donde transcurre la mayor parte de la acción de "Game of Thrones"?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cómo se llama el continente ficticio donde transcurre la mayor parte de la acción de "Game of Thrones"?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Essos', 0, @IdPregunta), ('Sothoryos', 0, @IdPregunta), ('Westeros', 1, @IdPregunta), ('Ulthos', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = 'En la serie "Friends", ¿cuál es el segundo nombre (middle name) de Chandler Bing?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('En la serie "Friends", ¿cuál es el segundo nombre (middle name) de Chandler Bing?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Eustace', 0, @IdPregunta), ('Francis', 0, @IdPregunta), ('Muriel', 1, @IdPregunta), ('Geller', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué serie antológica de misterio de 1959, creada por Rod Serling, sirvió de pilar fundamental para la ciencia ficción televisiva moderna?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué serie antológica de misterio de 1959, creada por Rod Serling, sirvió de pilar fundamental para la ciencia ficción televisiva moderna?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('The Outer Limits', 0, @IdPregunta), ('Alfred Hitchcock Presents', 0, @IdPregunta), ('The Twilight Zone', 1, @IdPregunta), ('Tales from the Crypt', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 10. CATEGORÍA: MÚSICA DEL MUNDO
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Música del Mundo';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuántas notas musicales básicas existen en la escala diatónica tradicional?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuántas notas musicales básicas existen en la escala diatónica tradicional?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('5', 0, @IdPregunta), ('7', 1, @IdPregunta), ('12', 0, @IdPregunta), ('8', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿A qué legendaria banda de rock británica perteneció el cantante Freddie Mercury?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿A qué legendaria banda de rock británica perteneció el cantante Freddie Mercury?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('The Beatles', 0, @IdPregunta), ('Led Zeppelin', 0, @IdPregunta), ('Queen', 1, @IdPregunta), ('Pink Floyd', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué compositor clásico quedó completamente sordo en la última etapa de su vida?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué compositor clásico quedó completamente sordo en la última etapa de su vida?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Mozart', 0, @IdPregunta), ('Bach', 0, @IdPregunta), ('Beethoven', 1, @IdPregunta), ('Vivaldi', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué instrumento de viento de madera, tradicional de los aborígenes australianos, se fabrica a partir de troncos de eucalipto vaciados por termitas?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué instrumento de viento de madera, tradicional de los aborígenes australianos, se fabrica a partir de troncos de eucalipto vaciados por termitas?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Flauta de Pan', 0, @IdPregunta), ('Dulzaina', 0, @IdPregunta), ('Didgeridoo', 1, @IdPregunta), ('Ocarina', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál de los siguientes modos gregorianos o eclesiásticos es equivalente a la escala menor natural moderna?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál de los siguientes modos gregorianos o eclesiásticos es equivalente a la escala menor natural moderna?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Modo Dórico', 0, @IdPregunta), ('Modo Frigio', 0, @IdPregunta), ('Modo Eolio', 1, @IdPregunta), ('Modo Mixolidio', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 11. CATEGORÍA: VIDEOJUEGOS Y CULTURA GEEK
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Videojuegos y Cultura Geek';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cómo se llama el fontanero más famoso de la historia de los videojuegos?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cómo se llama el fontanero más famoso de la historia de los videojuegos?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Sonic', 0, @IdPregunta), ('Mario', 1, @IdPregunta), ('Link', 0, @IdPregunta), ('Luigi', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué franquicia de videojuegos nos sitúa en el reino de Hyrule controlando al héroe Link?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué franquicia de videojuegos nos sitúa en el reino de Hyrule controlando al héroe Link?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Final Fantasy', 0, @IdPregunta), ('Dark Souls', 0, @IdPregunta), ('The Legend of Zelda', 1, @IdPregunta), ('Skyrim', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué juego independiente de bloques de construcción, creado por Markus Persson (Notch), es el más vendido de la historia?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué juego independiente de bloques de construcción, creado por Markus Persson (Notch), es el más vendido de la historia?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Terraria', 0, @IdPregunta), ('Roblox', 0, @IdPregunta), ('Minecraft', 1, @IdPregunta), ('Tetris', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál fue el nombre original del juego "Donkey Kong" de 1981 para los bocetos iniciales de Shigeru Miyamoto?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál fue el nombre original del juego "Donkey Kong" de 1981 para los bocetos iniciales de Shigeru Miyamoto?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Stubborn Gorilla', 0, @IdPregunta), ('Jumpman Quest', 0, @IdPregunta), ('Popeye (Licencia fallida)', 1, @IdPregunta), ('Monkey Barrel', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué código u opción de configuración en la consola del "Doom" original (1993) activa el famoso modo Dios?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué código u opción de configuración en la consola del "Doom" original (1993) activa el famoso modo Dios?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('IDKFA', 0, @IdPregunta), ('IDSPISPOPD', 0, @IdPregunta), ('IDDQD', 1, @IdPregunta), ('IDCLIP', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 12. CATEGORÍA: FARÁNDULA Y ESPECTÁCULOS
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Farándula y Espectáculos';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Quién es conocida mundialmente como la "Reina del Pop"?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Quién es conocida mundialmente como la "Reina del Pop"?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Britney Spears', 0, @IdPregunta), ('Madonna', 1, @IdPregunta), ('Lady Gaga', 0, @IdPregunta), ('Beyoncé', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué actor protagonizó la famosa saga cinematográfica de acción "Misión Imposible"?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué actor protagonizó la famosa saga cinematográfica de acción "Misión Imposible"?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Brad Pitt', 0, @IdPregunta), ('Johnny Depp', 0, @IdPregunta), ('Tom Cruise', 1, @IdPregunta), ('Keanu Reeves', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué cantante británica compuso e interpretó el tema principal de la película de James Bond "Skyfall"?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué cantante británica compuso e interpretó el tema principal de la película de James Bond "Skyfall"?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Dua Lipa', 0, @IdPregunta), ('Amy Winehouse', 0, @IdPregunta), ('Adele', 1, @IdPregunta), ('Ellie Goulding', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el verdadero nombre de pila del actor y luchador conocido mundialmente como "The Rock" (La Roca)?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el verdadero nombre de pila del actor y luchador conocido mundialmente como "The Rock" (La Roca)?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('John', 0, @IdPregunta), ('Jason', 0, @IdPregunta), ('Dwayne', 1, @IdPregunta), ('Vin', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué actriz clásica de Hollywood inventó y patentó un sistema de comunicaciones por salto de frecuencia que sentó las bases del Wi-Fi moderno?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué actriz clásica de Hollywood inventó y patentó un sistema de comunicaciones por salto de frecuencia que sentó las bases del Wi-Fi moderno?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Audrey Hepburn', 0, @IdPregunta), ('Bette Davis', 0, @IdPregunta), ('Hedy Lamarr', 1, @IdPregunta), ('Ingrid Bergman', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 13. CATEGORÍA: FÚTBOL INTERNACIONAL
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Fútbol Internacional';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cada cuántos años se celebra la Copa Mundial de la FIFA?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cada cuántos años se celebra la Copa Mundial de la FIFA?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('2 años', 0, @IdPregunta), ('4 años', 1, @IdPregunta), ('5 años', 0, @IdPregunta), ('3 años', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué futbolista argentino es ampliamente conocido por el dorsal número 10 y jugar muchos años en el F.C. Barcelona?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué futbolista argentino es ampliamente conocido por el dorsal número 10 y jugar muchos años en el F.C. Barcelona?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Diego Maradona', 0, @IdPregunta), ('Sergio Agüero', 0, @IdPregunta), ('Lionel Messi', 1, @IdPregunta), ('Angel Di María', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué país sudamericano es el que más Copas Mundiales de la FIFA ha ganado en toda la historia?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué país sudamericano es el que más Copas Mundiales de la FIFA ha ganado en toda la historia?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Argentina', 0, @IdPregunta), ('Uruguay', 0, @IdPregunta), ('Brasil', 1, @IdPregunta), ('Alemania', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Quién fue el director técnico de la selección de Francia en el mundial de Francia 1998?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Quién fue el director técnico de la selección de Francia en el mundial de Francia 1998?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Didier Deschamps', 0, @IdPregunta), ('Michel Platini', 0, @IdPregunta), ('Aimé Jacquet', 1, @IdPregunta), ('Raymond Domenech', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué club europeo ganó la Copa de Campeones de Europa (Champions League) en el año 1986, venciendo al Barcelona en penales?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué club europeo ganó la Copa de Campeones de Europa (Champions League) en el año 1986, venciendo al Barcelona en penales?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Benfica', 0, @IdPregunta), ('Steaua de Bucarest', 1, @IdPregunta), ('Estrella Roja', 0, @IdPregunta), ('Hamburgo SV', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 14. CATEGORÍA: DEPORTES Y OLIMPÍADAS
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Deportes y Olimpíadas';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuántos anillos de diferentes colores componen el símbolo olímpico oficial?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuántos anillos de diferentes colores componen el símbolo olímpico oficial?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('4', 0, @IdPregunta), ('5', 1, @IdPregunta), ('6', 0, @IdPregunta), ('3', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Quién ostenta el récord mundial de los 100 metros llanos como el hombre más rápido del planeta?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Quién ostenta el récord mundial de los 100 metros llanos como el hombre más rápido del planeta?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Carl Lewis', 0, @IdPregunta), ('Tyson Gay', 0, @IdPregunta), ('Usain Bolt', 1, @IdPregunta), ('Yohan Blake', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué tenista masculino ha ganado más títulos de Roland Garros en toda la historia del tenis?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué tenista masculino ha ganado más títulos de Roland Garros en toda la historia del tenis?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Roger Federer', 0, @IdPregunta), ('Novak Djokovic', 0, @IdPregunta), ('Rafael Nadal', 1, @IdPregunta), ('Pete Sampras', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿A qué distancia exacta (en metros o kilómetros) se corre una maratón olímpica oficial?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿A qué distancia exacta (en metros o kilómetros) se corre una maratón olímpica oficial?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('40.00 km', 0, @IdPregunta), ('42.195 km', 1, @IdPregunta), ('45.50 km', 0, @IdPregunta), ('41.500 km', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿En qué edición de los Juegos Olímpicos modernos se permitió por primera vez la participación oficial de mujeres?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿En qué edición de los Juegos Olímpicos modernos se permitió por primera vez la participación oficial de mujeres?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Atenas 1896', 0, @IdPregunta), ('París 1900', 1, @IdPregunta), ('San Luis 1904', 0, @IdPregunta), ('Londres 1908', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 15. CATEGORÍA: GASTRONOMÍA Y BEBIDAS
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Gastronomía y Bebidas';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el ingrediente base e indispensable para elaborar chocolate?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el ingrediente base e indispensable para elaborar chocolate?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Trigo', 0, @IdPregunta), ('Cacao', 1, @IdPregunta), ('Café', 0, @IdPregunta), ('Vainilla', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué país es el lugar de origen de la pizza y los espaguetis de fama mundial?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué país es el lugar de origen de la pizza y los espaguetis de fama mundial?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Francia', 0, @IdPregunta), ('España', 0, @IdPregunta), ('Italia', 1, @IdPregunta), ('Grecia', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué hongo subterráneo es considerado uno de los manjares más costosos y codiciados de la alta cocina?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué hongo subterráneo es considerado uno de los manjares más costosos y codiciados de la alta cocina?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Champiñón', 0, @IdPregunta), ('Shiitake', 0, @IdPregunta), ('Trufa', 1, @IdPregunta), ('Portobello', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué especia, la más cara del mundo por peso, se obtiene manualmente de los estigmas de la flor Crocus sativus?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué especia, la más cara del mundo por peso, se obtiene manualmente de los estigmas de la flor Crocus sativus?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Cardamomo', 0, @IdPregunta), ('Vainilla en vaina', 0, @IdPregunta), ('Azafrán', 1, @IdPregunta), ('Comino', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el nombre de la clásica salsa francesa derivada, hecha a base de mantequilla clarificada, yemas de huevo y chalotas reducidas en vinagre y estragón?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el nombre de la clásica salsa francesa derivada, hecha a base de mantequilla clarificada, yemas de huevo y chalotas reducidas en vinagre y estragón?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Salsa Holandesa', 0, @IdPregunta), ('Salsa Velouté', 0, @IdPregunta), ('Salsa Bearnesa', 1, @IdPregunta), ('Salsa Bechamel', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 16. CATEGORÍA: MARCAS Y NEGOCIOS
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Marcas y Negocios';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué empresa multinacional de tecnología es famosa por su logotipo de una manzana mordida?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué empresa multinacional de tecnología es famosa por su logotipo de una manzana mordida?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Microsoft', 0, @IdPregunta), ('Apple', 1, @IdPregunta), ('Google', 0, @IdPregunta), ('Samsung', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Quién es el magnate fundador del gigante del comercio electrónico Amazon?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Quién es el magnate fundador del gigante del comercio electrónico Amazon?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Bill Gates', 0, @IdPregunta), ('Elon Musk', 0, @IdPregunta), ('Jeff Bezos', 1, @IdPregunta), ('Mark Zuckerberg', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿En qué país europeo se fundó la multinacional de muebles y decoración IKEA?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿En qué país europeo se fundó la multinacional de muebles y decoración IKEA?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Alemania', 0, @IdPregunta), ('Noruega', 0, @IdPregunta), ('Suecia', 1, @IdPregunta), ('Dinamarca', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué famosa empresa de refrescos adquirió y relanzó la marca de agua embotellada "Dasani" a finales de los años 90?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué famosa empresa de refrescos adquirió y relanzó la marca de agua embotellada "Dasani" a finales de los años 90?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('PepsiCo', 0, @IdPregunta), ('Nestlé', 0, @IdPregunta), ('The Coca-Cola Company', 1, @IdPregunta), ('Danone', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál era el nombre original de la corporación multinacional Sony antes de cambiar su denominación comercial en 1958?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál era el nombre original de la corporación multinacional Sony antes de cambiar su denominación comercial en 1958?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Nippon Electric', 0, @IdPregunta), ('Matsushita Corp', 0, @IdPregunta), ('Tokyo Tsushin Kogyo', 1, @IdPregunta), ('Standard Wave Technologies', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 17. CATEGORÍA: MITOS Y MITOLOGÍA
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Mitos y Mitología';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Quién es el dios del trueno y rey del Olimpo en la mitología griega antigua?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Quién es el dios del trueno y rey del Olimpo en la mitología griega antigua?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Poseidón', 0, @IdPregunta), ('Zeus', 1, @IdPregunta), ('Apolo', 0, @IdPregunta), ('Hades', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cómo se llama el imponente martillo que empuña el dios Thor en la mitología nórdica?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cómo se llama el imponente martillo que empuña el dios Thor en la mitología nórdica?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Gungnir', 0, @IdPregunta), ('Excalibur', 0, @IdPregunta), ('Mjölnir', 1, @IdPregunta), ('Yggdrasil', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué héroe mitológico griego derrotó al Minotauro dentro del laberinto de Creta?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué héroe mitológico griego derrotó al Minotauro dentro del laberinto de Creta?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Hércules', 0, @IdPregunta), ('Perseo', 0, @IdPregunta), ('Teseo', 1, @IdPregunta), ('Aquiles', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el nombre de la criatura canina de tres cabezas que custodia las puertas del Inframundo de Hades?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el nombre de la criatura canina de tres cabezas que custodia las puertas del Inframundo de Hades?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Quimera', 0, @IdPregunta), ('Ortro', 0, @IdPregunta), ('Cerbero', 1, @IdPregunta), ('Esfinge', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué deidad egipcia, representada habitualmente con cabeza de ibis, es considerada el dios de la sabiduría, la escritura y los conjuros?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué deidad egipcia, representada habitualmente con cabeza de ibis, es considerada el dios de la sabiduría, la escritura y los conjuros?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Anubis', 0, @IdPregunta), ('Osiris', 0, @IdPregunta), ('Thot', 1, @IdPregunta), ('Horus', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 18. CATEGORÍA: INVENTOS Y DESCUBRIMIENTOS
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Inventos y Descubrimientos';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Quién patentó con éxito la bombilla eléctrica incandescente comercial en 1879?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Quién patentó con éxito la bombilla eléctrica incandescente comercial en 1879?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Nikola Tesla', 0, @IdPregunta), ('Thomas Edison', 1, @IdPregunta), ('Alexander Graham Bell', 0, @IdPregunta), ('Benjamin Franklin', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué científico escocés descubrió la penicilina, el primer antibiótico, en el año 1928?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué científico escocés descubrió la penicilina, el primer antibiótico, en el año 1928?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Louis Pasteur', 0, @IdPregunta), ('Marie Curie', 0, @IdPregunta), ('Alexander Fleming', 1, @IdPregunta), ('Gregor Mendel', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué inventor alemán revolucionó la cultura universal al crear la imprenta de tipos móviles en el siglo XV?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué inventor alemán revolucionó la cultura universal al crear la imprenta de tipos móviles en el siglo XV?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Karl Benz', 0, @IdPregunta), ('Gottlieb Daimler', 0, @IdPregunta), ('Johannes Gutenberg', 1, @IdPregunta), ('Wilhelm Röntgen', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Quién desarrolló la primera vacuna exitosa contra la rabia, administrada por primera vez en humanos en 1885?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Quién desarrolló la primera vacuna exitosa contra la rabia, administrada por primera vez en humanos en 1885?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Edward Jenner', 0, @IdPregunta), ('Robert Koch', 0, @IdPregunta), ('Louis Pasteur', 1, @IdPregunta), ('Jonas Salk', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué físico descubrió accidentalmente los Rayos X en 1895 mientras experimentaba con tubos de rayos catódicos?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué físico descubrió accidentalmente los Rayos X en 1895 mientras experimentaba con tubos de rayos catódicos?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Max Planck', 0, @IdPregunta), ('Henri Becquerel', 0, @IdPregunta), ('Wilhelm Röntgen', 1, @IdPregunta), ('Ernest Rutherford', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 19. CATEGORÍA: RÉCORDS Y CURIOSIDADES
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Récords y Curiosidades';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es la montaña más alta del planeta Tierra sobre el nivel del mar?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es la montaña más alta del planeta Tierra sobre el nivel del mar?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('K2', 0, @IdPregunta), ('Monte Everest', 1, @IdPregunta), ('Kilimanjaro', 0, @IdPregunta), ('Aconcagua', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué animal marino posee el récord de ser el ser vivo más grande del planeta?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué animal marino posee el récord de ser el ser vivo más grande del planeta?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Tiburón Ballena', 0, @IdPregunta), ('Cachalote', 0, @IdPregunta), ('Ballena Azul', 1, @IdPregunta), ('Calamar Gigante', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el océano más profundo del mundo, albergando la Fosa de las Marianas?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el océano más profundo del mundo, albergando la Fosa de las Marianas?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Océano Atlántico', 0, @IdPregunta), ('Océano Índico', 0, @IdPregunta), ('Océano Pacífico', 1, @IdPregunta), ('Océano Ártico', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el desierto cálido más grande del mundo entero?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el desierto cálido más grande del mundo entero?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Desierto de Gobi', 0, @IdPregunta), ('Desierto de Atacama', 0, @IdPregunta), ('Desierto del Sahara', 1, @IdPregunta), ('Desierto de Kalahari', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué país del mundo cuenta con la mayor cantidad de husos horarios oficiales dentro de su territorio soberano global?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué país del mundo cuenta con la mayor cantidad de husos horarios oficiales dentro de su territorio soberano global?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Rusia', 0, @IdPregunta), ('Estados Unidos', 0, @IdPregunta), ('Francia (Incluyendo ultramar)', 1, @IdPregunta), ('China', 0, @IdPregunta);
    END
END

-- ==============================================================================
-- 20. CATEGORÍA: CULTURA GENERAL
-- ==============================================================================
SET @IdCategoria = NULL;
SELECT @IdCategoria = IdCategoria FROM Categorias WHERE Nombre = 'Cultura General';

IF @IdCategoria IS NOT NULL
BEGIN
    -- [Novato]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Novato';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuántos días tiene un año bisiesto?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuántos días tiene un año bisiesto?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('365', 0, @IdPregunta), ('366', 1, @IdPregunta), ('364', 0, @IdPregunta), ('360', 0, @IdPregunta);
    END

    -- [Aprendiz]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Aprendiz';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es el color primario que resulta de mezclar azul y amarillo?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es el color primario que resulta de mezclar azul y amarillo?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Rojo', 0, @IdPregunta), ('Morado', 0, @IdPregunta), ('Verde', 1, @IdPregunta), ('Naranja', 0, @IdPregunta);
    END

    -- [Conocedor]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Conocedor';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué país le regaló la famosa Estatua de la Libertad a los Estados Unidos?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué país le regaló la famosa Estatua de la Libertad a los Estados Unidos?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Reino Unido', 0, @IdPregunta), ('Alemania', 0, @IdPregunta), ('Francia', 1, @IdPregunta), ('Italia', 0, @IdPregunta);
    END

    -- [Maestro]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Maestro';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Qué filósofo clásico griego fue el tutor personal de Alejandro Magno?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Qué filósofo clásico griego fue el tutor personal de Alejandro Magno?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Sócrates', 0, @IdPregunta), ('Platón', 0, @IdPregunta), ('Aristóteles', 1, @IdPregunta), ('Pitágoras', 0, @IdPregunta);
    END

    -- [Omnisciente]
    SELECT @IdComplejidad = IdComplejidad FROM Complejidades WHERE Nombre = 'Omnisciente';
    IF NOT EXISTS (SELECT 1 FROM Preguntas WHERE Texto = '¿Cuál es la moneda oficial de curso legal en Japón?' AND IdCategoria = @IdCategoria)
    BEGIN
        INSERT INTO Preguntas (Texto, IdCategoria, IdComplejidad) VALUES ('¿Cuál es la moneda oficial de curso legal en Japón?', @IdCategoria, @IdComplejidad);
        SET @IdPregunta = SCOPE_IDENTITY();
        INSERT INTO Opciones (Texto, Valida, IdPregunta) VALUES ('Won', 0, @IdPregunta), ('Yuan', 0, @IdPregunta), ('Yen', 1, @IdPregunta), ('Ringgit', 0, @IdPregunta);
    END
END