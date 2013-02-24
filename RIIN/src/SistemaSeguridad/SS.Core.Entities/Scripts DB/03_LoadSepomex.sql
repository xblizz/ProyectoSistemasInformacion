USE ALXout01DB
GO

SET IDENTITY_INSERT Estados ON
INSERT Estados
		(Id, Nombre, PoligonoId, UsuarioAlta, FechaAlta)
SELECT DISTINCT EstadoId, d_estado, Null, 1, CURRENT_TIMESTAMP
FROM MigracionSepomex
GO
PRINT 'Carga de Estados'
SET IDENTITY_INSERT Estados OFF

SET IDENTITY_INSERT Ciudades ON
INSERT Ciudades
		(CiudadId, EstadoId, Nombre, PoligonoId, UsuarioAlta, FechaAlta)
SELECT ROW_NUMBER() OVER (ORDER BY EstadoId, MunicipioId) AS CiudadId, EstadoId, d_mnpio, NULL, 1, CURRENT_TIMESTAMP
FROM MigracionSepomex
GO
PRINT 'Carga de Ciudades'
SET IDENTITY_INSERT Ciudades OFF


