USE ALXout01DB
GO

IF EXISTS ( SELECT *
			FROM sysobjects 
			WHERE Name = N'trParametrosSistema'
				AND XType = 'TR'
			)
BEGIN
	DROP TRIGGER trParametrosSistema
	PRINT 'Eliminar trigger trParametrosSistema'
END
GO
-- ==============================================================================================
-- Autor: MexWare
-- Fecha:  20120204
-- Descripcion: No permite que se eliminen los Parametros del sistema del 1 al 10 por que tienen 
--			valores definidos y requeridos por el sistema.
-- ==============================================================================================
CREATE TRIGGER trParametrosSistema ON ParametrosSistema FOR UPDATE, DELETE
AS
BEGIN

	SET NOCOUNT ON
	DECLARE @ini INT,
			@fin INT
	SELECT @ini = 1, @fin = 11

	-- NO PERMITE BORRAR parametros 
	IF EXISTS ( SELECT *
				FROM	DELETED AS D
						LEFT JOIN INSERTED AS I
								ON I.Id = D.Id
				WHERE	D.Id BETWEEN @ini AND @fin 
						AND I.Id IS NULL)
	BEGIN
		SET IDENTITY_INSERT ParametrosSistema ON
			
		INSERT ParametrosSistema
			(	Id, Nombre, TipoDeParametro, TipoIncidenteEsRequerido, ValorFinalEsRequerido )
		SELECT	D.Id, D.Nombre, D.TipoDeParametro, D.TipoIncidenteEsRequerido, D.ValorFinalEsRequerido
		FROM	DELETED AS D
				LEFT JOIN INSERTED AS I
						ON I.Id = D.Id
		WHERE	D.Id BETWEEN @ini AND @fin
				AND I.Id IS NULL
		
		SET IDENTITY_INSERT ParametrosSistema OFF
		RAISERROR ('No se puede eliminar los parametros de configuracion del sistema',16,1)
	END

	
	-- NO PERMITE MODIFICAR los parametros  del 1 al 10..
	IF EXISTS ( SELECT *
				FROM	DELETED AS D
						INNER JOIN INSERTED AS I
								ON I.Id = D.Id
				WHERE	D.Id BETWEEN @ini AND @fin )
	BEGIN
			
		UPDATE	P
		SET		Nombre = D.Nombre,
				TipoDeParametro = D.TipoDeParametro,
				TipoIncidenteEsRequerido = D.TipoIncidenteEsRequerido,
				ValorFinalEsRequerido = D.ValorFinalEsRequerido
		FROM	ParametrosSistema AS P
				INNER JOIN DELETED AS D
						ON P.Id = D.Id
		WHERE	P.Id BETWEEN @ini AND @fin
				
		RAISERROR ('No se puede modificar los parametros de configuracion del sistema',16,1)
	END

END
GO
PRINT 'Crear trigger trParametrosSistema'
GO
