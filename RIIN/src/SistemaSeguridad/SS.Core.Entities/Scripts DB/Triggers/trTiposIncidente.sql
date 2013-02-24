USE ALXout01DB
GO

IF EXISTS ( SELECT *
			FROM sysobjects 
			WHERE Name = N'trTiposIncidente'
				AND XType = 'TR'
			)
BEGIN
	DROP TRIGGER trTiposIncidente
	PRINT 'Eliminar trigger trTiposIncidente'
END
GO
-- ==============================================================================================
-- Autor: MexWare
-- Fecha:  20120203
-- Descripcion: No permite borrar /modificar los Tipos de Incidentes que tienen una interface asociada a ellos.
-- ==============================================================================================
CREATE TRIGGER trTiposIncidente ON TiposIncidente FOR DELETE, UPDATE
AS
BEGIN

	SET NOCOUNT ON

--1	Robo con violencia
--2	Robo sin violencia
--3	Secuestro de empleado en ruta
--4	Extorsión
--5	Amenaza
--6	Intrusión
	
	-- SI BORRARON
	DECLARE @str VARCHAR(1000)	
	IF EXISTS (	SELECT *
				FROM   Deleted D
						LEFT JOIN INSERTED AS I
								ON I.Id = D.Id
				WHERE  D.Id BETWEEN 1 AND 6
						AND I.Id IS NULL
				)
	BEGIN
		SET IDENTITY_INSERT TiposIncidente ON
		SET @str =  'No se puede eliminar los Tipos de Incidentes: 1)Robo con violencia, 2)Robo sin violencia, 3)Secuestro de empleado en ruta, 4)Extorsión, 5)Amenaza, 6)Intrusión'
		
		INSERT  TiposIncidente (Id, Nombre, Descripcion, UsuarioAlta, FechaAlta, Imagen)
		SELECT  Id, Nombre, Descripcion, UsuarioAlta, FechaAlta, Imagen
		FROM	DELETED
		WHERE   Id  BETWEEN 1 AND 6
		
		SET IDENTITY_INSERT TiposIncidente OFF
		
		RAISERROR (@str, 16,1)		
	END
	
	-- Si modificaron el Nombre
	IF EXISTS (	SELECT *
				FROM   Deleted D
						INNER JOIN INSERTED AS I
								ON I.Id = D.Id
								AND I.Nombre <> D.Nombre
				WHERE  D.Id BETWEEN 1 AND 6
				)
	BEGIN
		SET @str = 'No se puede modificar el Nombre de los Tipos de Incidentes: 1)Robo con violencia, 2)Robo sin violencia, 3)Secuestro de empleado en ruta, 4)Extorsión, 5)Amenaza, 6)Intrusión'
		
		UPDATE  T
		SET		Nombre  = D.Nombre
		FROM	TiposIncidente  AS T
				INNER JOIN Deleted D
						ON T.Id = D.Id
				INNER JOIN INSERTED AS I
						ON I.Id = D.Id
						AND I.Nombre <> D.Nombre
						AND D.Id BETWEEN 1 AND 6
				
		RAISERROR (@str, 16,1)		
	END

END
GO
PRINT 'Crear trigger trTiposIncidente'
GO