-- Crear la base de datos
CREATE DATABASE IPS_Citas;
GO

USE IPS_Citas;
GO

-- Tabla Paciente
CREATE TABLE Paciente (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombres NVARCHAR(100) NOT NULL,
    apellidos NVARCHAR(100) NOT NULL,
    documento NVARCHAR(20) NOT NULL UNIQUE,
    fechanacimiento DATE NOT NULL,
    telefono NVARCHAR(20)
);
GO

-- Tabla Medico
CREATE TABLE Medico (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nombre NVARCHAR(100) NOT NULL,
    especialidad NVARCHAR(50) NOT NULL
);
GO

-- Tabla Cita
CREATE TABLE Cita (
    id INT IDENTITY(1,1) PRIMARY KEY,
    especialidad NVARCHAR(50) NOT NULL,
    fechahora DATETIME NOT NULL,
    estado NVARCHAR(20) NOT NULL, -- 'Disponible', 'Reservada'
    idmedico INT NOT NULL,
    idpaciente INT NULL,
    FOREIGN KEY (idmedico) REFERENCES Medico(id),
    FOREIGN KEY (idpaciente) REFERENCES Paciente(id)
);
GO

-- Insertar datos de prueba en Paciente
INSERT INTO Paciente (nombres, apellidos, documento, fechanacimiento, telefono) VALUES
('Juan', 'Pérez', '12345678', '1990-05-10', '3001234567'),
('Ana', 'Gómez', '87654321', '1985-11-22', '3007654321');
GO

-- Insertar datos de prueba en Medico
INSERT INTO Medico (nombre, especialidad) VALUES
('Dr. Carlos Ruiz', 'Medicina general'),
('Dra. Laura Torres', 'Examen odontológico');
GO

-- Insertar datos de prueba en Cita
INSERT INTO Cita (especialidad, fechahora, estado, idmedico) VALUES
('Medicina general', '2024-06-10 09:00', 'Disponible', 1),
('Medicina general', '2024-06-10 10:00', 'Disponible', 1),
('Medicina general', '2024-06-10 11:00', 'Disponible', 1),
('Examen odontológico', '2024-06-11 09:00', 'Disponible', 2),
('Examen odontológico', '2024-06-11 10:00', 'Disponible', 2);
GO

-- Procedimiento: AutenticarPaciente
CREATE PROCEDURE AutenticarPaciente
    @documento NVARCHAR(20),
    @fechanacimiento DATE
AS
BEGIN
    SELECT * FROM Paciente
    WHERE documento = @documento AND fechanacimiento = @fechanacimiento;
END
GO

-- Procedimiento: ObtenerCitasDisponibles
CREATE PROCEDURE ObtenerCitasDisponibles
    @especialidad NVARCHAR(50)
AS
BEGIN
    SELECT TOP 5 c.id, c.especialidad, c.fechahora, c.estado, c.idmedico, m.nombre AS medico_nombre
    FROM Cita c
    INNER JOIN Medico m ON c.idmedico = m.id
    WHERE c.especialidad = @especialidad AND c.estado = 'Disponible'
    ORDER BY c.fechahora ASC;
END
GO

-- Procedimiento: ReservarCita
CREATE PROCEDURE ReservarCita
    @citaId INT,
    @pacienteId INT
AS
BEGIN
    UPDATE Cita
    SET estado = 'Reservada', idpaciente = @pacienteId
    WHERE id = @citaId AND estado = 'Disponible';

    IF @@ROWCOUNT = 1
    BEGIN
        SELECT 
            1 AS Exito,
            p.nombres + ' ' + p.apellidos AS Paciente,
            FORMAT(c.fechahora, 'dddd, dd ''de'' MMMM ''de'' yyyy', 'es-ES') AS Fecha,
            FORMAT(c.fechahora, 'hh:mm tt', 'es-ES') AS Hora,
            c.especialidad AS Especialidad,
            m.nombre AS Profesional,
            '57' + p.telefono AS Numero
        FROM Cita c
        INNER JOIN Paciente p ON c.idpaciente = p.id
        INNER JOIN Medico m ON c.idmedico = m.id
        WHERE c.id = @citaId;
    END
    ELSE
    BEGIN
        SELECT 0 AS Exito, NULL AS Paciente, NULL AS Fecha, NULL AS Hora, NULL AS Especialidad, NULL AS Profesional, NULL AS Numero;
    END
END
GO