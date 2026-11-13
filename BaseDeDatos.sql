    USE master
    GO


   IF EXISTS (SELECT * FROM sys.databases WHERE name = 'LaboratorioKits')
BEGIN
    ALTER DATABASE LaboratorioKits SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    DROP DATABASE LaboratorioKits
END
GO

    CREATE DATABASE LaboratorioKits
    GO

    USE LaboratorioKits
    GO



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


    INSERT INTO Bloques (Numero, HoraInicio, HoraFin) VALUES
    (1, '07:45', '08:25'),
    (2, '08:25', '09:05'),
    (3, '09:05', '09:45'),
    (4, '09:45', '10:25'),
    (5, '10:25', '11:05'),
    (6, '11:05', '11:45'),
    (7, '11:45', '12:25'),
    (8, '12:25', '13:05')

    INSERT INTO Categorias (Nombre) VALUES
    ('Circuitos'),    
    ('Microscopía'),  
    ('Robótica'),     
    ('Química'),      
    ('Física')        

    INSERT INTO Usuarios (NombreUsuario, Contrasena, Nombre, Apellido) VALUES
    ('ferran', '1234', 'Ferran',  'Gómez'),
    ('lucia',  '1234', 'Lucía',   'Fernández'),
    ('martin', '1234', 'Martín',  'Rodríguez'),
    ('sofia',  '1234', 'Sofía',   'López'),
    ('tomas',  '1234', 'Tomás',   'Pérez')


    INSERT INTO Kits (Nombre, Descripcion, Imagen, IdCategoria, Activo) VALUES
    ('Kit de circuitos básicos',           'Protoboard, LEDs, resistencias y cables para armar circuitos simples.',   'circuitos-basicos.jpg', 1, 1),
    ('Kit de circuitos serie y paralelo',  'Compará circuitos en serie y en paralelo con lamparitas y baterías.',     'circuitos-serie.jpg',   1, 1),
    ('Kit de sensores',                    'Sensores de luz, temperatura y distancia con placa de lectura.',          'sensores.jpg',          1, 1),
    ('Kit de soldadura',                   'Soldador, estaño y placas de práctica para aprender a soldar.',           'soldadura.jpg',         1, 1),
    ('Microscopio óptico',                 'Microscopio con tres objetivos y portaobjetos incluidos.',                'microscopio.jpg',       2, 1),
    ('Kit de preparados',                  'Colección de preparados de células vegetales y animales.',                'preparados.jpg',        2, 1),
    ('Lupa binocular',                     'Lupa con aumento 20x para observar muestras pequeñas.',                   'lupa.jpg',              2, 1),
    ('Kit Arduino inicial',                'Placa Arduino UNO, cable USB, protoboard y componentes básicos.',         'arduino.jpg',           3, 1),
    ('Kit de robot seguidor de línea',     'Chasis, motores y sensores infrarrojos para armar un seguidor de línea.', 'seguidor-linea.jpg',    3, 1),
    ('Kit de brazo robótico',              'Brazo de 4 servos con controlador para practicar movimientos.',           'brazo-robotico.jpg',    3, 1),
    ('Kit de servomotores',                'Servos, soportes y fuente para proyectos de movimiento.',                 'servos.jpg',            3, 1),
    ('Kit de reacciones químicas',         'Tubos de ensayo, gradilla y reactivos seguros para experimentos.',        'reacciones.jpg',        4, 1),
    ('Kit de medición de pH',              'Tiras reactivas y medidor digital de pH.',                                'ph.jpg',                4, 1),
    ('Kit de destilación',                 'Balón, refrigerante y soportes para destilación simple.',                 'destilacion.jpg',       4, 1),
    ('Kit de óptica',                      'Lentes, espejos y láser de baja potencia para estudiar la luz.',          'optica.jpg',            5, 1),
    ('Kit de mecánica',                    'Carritos, rampas y cronómetro para estudiar movimiento.',                 'mecanica.jpg',          5, 1),
    ('Kit de electromagnetismo',           'Imanes, bobinas y brújulas para ver campos magnéticos.',                  'electromagnetismo.jpg', 5, 1),
    ('Kit de péndulos',                    'Péndulos de distintos largos y masas con soporte.',                       'pendulos.jpg',          5, 1),
    ('Kit de energía solar',               'Mini panel solar, motor y multímetro.',                                   'solar.jpg',             5, 1),
    ('Kit de lupas de mano',              'Lupas de mano, pinzas y cajas de Petri para observar semillas, hojas e insectos.', 'lupas-mano.jpg', 2, 1),
    ('Kit de impresión 3D (en reparación)','Impresora 3D de escritorio, fuera de servicio por reparación.',           'impresora3d.jpg',       3, 0)

    INSERT INTO Prestamos (IdKit, IdUsuario, Fecha, IdBloque, Codigo, Cancelado, FechaCancelacion) VALUES

(1,  1, DATEADD(DAY, -4, CAST(GETDATE() AS DATE)), 1, 'A7K2P9', 0, NULL),
(8,  2, DATEADD(DAY, -3, CAST(GETDATE() AS DATE)), 3, 'B3M8X1', 0, NULL),
(3,  4, DATEADD(DAY, -3, CAST(GETDATE() AS DATE)), 4, 'K9D3S1', 0, NULL),
(5,  3, DATEADD(DAY, -2, CAST(GETDATE() AS DATE)), 2, 'C9T4Q6', 0, NULL),

(12, 1, DATEADD(DAY, 3, CAST(GETDATE() AS DATE)), 1, 'D2W7N5', 0, NULL),
(8,  1, DATEADD(DAY, 3, CAST(GETDATE() AS DATE)), 3, 'E6H3R8', 0, NULL),
(5,  2, DATEADD(DAY, 4, CAST(GETDATE() AS DATE)), 2, 'F1J9V4', 0, NULL),
(15, 3, DATEADD(DAY, 5, CAST(GETDATE() AS DATE)), 1, 'G8L5Z2', 0, NULL),
(17, 2, DATEADD(DAY, 6, CAST(GETDATE() AS DATE)), 6, 'J5P2C8', 0, NULL),

(9,  3, DATEADD(DAY, 5, CAST(GETDATE() AS DATE)), 5, 'H4N6B7', 1, DATEADD(DAY, -1, GETDATE()))
GO


  