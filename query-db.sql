
CREATE TABLE PermissionsTypes
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Description NVARCHAR(MAX),
    CreationDate DATETIME DEFAULT GETDATE(),
    ModificationDate DATETIME NULL
);

-- Insertar registros por defecto en PermissionsTypes
INSERT INTO PermissionsTypes (Description) VALUES ('Permission Type One');
INSERT INTO PermissionsTypes (Description) VALUES ('Permission Type Two');

-- Crear la tabla Permissions con la clave foránea
CREATE TABLE Permissions
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeForename NVARCHAR(MAX),
    EmployeeSurname NVARCHAR(MAX),
    PermissionsType INT NOT NULL,
    PermissionsDate DATETIME NOT NULL,
    CreationDate DATETIME DEFAULT GETDATE(),
    ModificationDate DATETIME NULL,
    CONSTRAINT FK_Permissions_PermissionsTypes
        FOREIGN KEY (PermissionsType)
        REFERENCES PermissionsTypes(Id)
);

