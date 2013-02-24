USE ALXout01DB
GO
IF EXISTS ( SELECT * FROM SysObjects WHERE NAME = 'NombreMeses' AND XType = 'U' )
BEGIN
	DROP TABLE NombreMeses
	PRINT 'Drop Table NombreMeses'
END
GO
CREATE TABLE NombreMeses (
	MesId	INT NOT NULL,
	Nombre  Varchar(20) NOT NULL,
	CONSTRAINT PK_NombreMeses
	PRIMARY KEY (MesId)
)
GO
PRINT 'Create Table NombreMeses'
GO
-- INSERTA LA INFORMACION DE LOS MESES --
INSERT NombreMeses VALUES(1,'Enero')
INSERT NombreMeses VALUES(2,'Febrero')
INSERT NombreMeses VALUES(3,'Marzo')
INSERT NombreMeses VALUES(4,'Abril')
INSERT NombreMeses VALUES(5,'Mayo')
INSERT NombreMeses VALUES(6,'Junio')
INSERT NombreMeses VALUES(7,'Julio')
INSERT NombreMeses VALUES(8,'Agosto')
INSERT NombreMeses VALUES(9,'Septiembre')
INSERT NombreMeses VALUES(10,'Octubre')
INSERT NombreMeses VALUES(11,'Noviembre')
INSERT NombreMeses VALUES(12,'Diciembre')