USE ALXout01DB
GO

IF EXISTS ( SELECT *
			FROM sysobjects 
			WHERE Name = N'trParametrosSistemaEmpresa'
				AND XType = 'TR'
			)
BEGIN
	DROP TRIGGER trParametrosSistemaEmpresa
	PRINT 'Eliminar trigger trParametrosSistemaEmpresa'
END
GO
--8	Semaforización verde                              
--9	Semarofización amarillo                           
--10	Semaforización rojo
-- ==============================================================================================
-- Autor: MexWare
-- Fecha:  20120123
-- Descripcion: Realiza los ajustes a los valores de los Semaforos para las Empresas y para Conor.
--	1. Si se modifica la informacion de una empresa, ajusta el semaforo para la misma. Verde, Amarillo y Rojo.
--		Modifica los valores de Conor para el Tipo de Incidente de la empresa modificada.
-- ==============================================================================================
CREATE TRIGGER trParametrosSistemaEmpresa ON ParametrosSistemaEmpresa FOR INSERT, UPDATE, DELETE
AS
BEGIN

	SET NOCOUNT ON
	DECLARE @ini INT,
			@fin INT
	SELECT @ini = 1, @fin = 8

	DECLARE @ValorInicial INT,
			@ValorFinal INT,
			@ParametrosSistemaId INT,
			@EmpresaId INT,
			@TipoIncidenteId INT, 
			@Id INT,
			@ValorInicialVal INT,
			@ValorFinalVal INT

	-- NO PERMITE BORRAR parametros 
	IF EXISTS ( SELECT *
				FROM	DELETED AS D
						LEFT JOIN INSERTED AS I
								ON I.Id = D.Id
				WHERE	D.Id BETWEEN @ini AND @fin 
						AND I.Id IS NULL)
	BEGIN
		SET IDENTITY_INSERT ParametrosSistemaEmpresa ON
			
		INSERT ParametrosSistemaEmpresa 
			(	Id, EmpresaId, TipoIncidenteId, ValorInicial, Valorfinal, 
				UsuarioModificacion, FechaUltimaModificacion, UsuarioAlta, FechaAlta, ParametrosSistemaId)
		SELECT	D.Id, D.EmpresaId, D.TipoIncidenteId, D.ValorInicial, D.Valorfinal, 
				D.UsuarioModificacion, D.FechaUltimaModificacion, D.UsuarioAlta, D.FechaAlta, D.ParametrosSistemaId
		FROM	DELETED AS D
				LEFT JOIN INSERTED AS I
								ON I.Id = D.Id
		WHERE	D.Id BETWEEN @ini AND @fin  
				AND I.Id IS NULL
		
		SET IDENTITY_INSERT ParametrosSistemaEmpresa OFF
		RAISERROR ('No se puede eliminar los parametros de configuracion del sistema',16,1)
	END

	-- Busca los parametros del Semaforo que se Agregaron
	SELECT I.*
	INTO   #tempInsert
	FROM   Inserted I
			LEFT JOIN Deleted D
				ON I.Id = D.Id
	WHERE  I.ParametrosSistemaId IN (8,9,10) -- Cambio en Valores de Semaforo
		   AND D.Id IS NULL
	UNION
    -- Buscar los parametros del Semaforo que CAMBIARON en la informacion del Rango
	SELECT I.*
	FROM   Inserted I
			INNER JOIN Deleted D
				ON I.Id = D.Id
	WHERE  I.ParametrosSistemaId IN (8,9,10) -- Cambio en Valores de Semaforo
			AND ( ISNULL(I.ValorInicial,-1) <> ISNULL(D.ValorInicial,-1)
				  OR ISNULL(I.ValorFinal,-1) <> ISNULL(D.ValorFinal, -1)
				)

	-- Valida si el cambio afecto los otros rangos para la misma empresa.
	IF EXISTS ( SELECT * 
				FROM   #tempInsert 
				WHERE  ValorInicial IS NULL OR ValorFinal IS NULL
			)
		RAISERROR ('El rango para los valores del semaforo es requerido', 16,1)
	
	-- Lee los valores que se modificaron / agregaron
	SELECT  @ValorInicial = ValorInicial,
			@ValorFinal = ValorFinal,
			@ParametrosSistemaId = ParametrosSistemaId,
			@EmpresaId = EmpresaId,
			@TipoIncidenteId = TipoIncidenteId
	FROM	#tempInsert

	-- Valida si el cambio afecto a otros segmentos.
	IF @TipoIncidenteId IS NOT NULL 
	BEGIN
		-- 8: Verde, 9: Amarillo, 10: Rojo
		IF @ParametrosSistemaId = 8 -- verde
		BEGIN
			--  Valida el amarillo
			UPDATE  PSE
			SET		ValorInicial = @ValorFinal + 1,
					ValorFinal = CASE
									 WHEN ValorFinal <= @ValorFinal + 1 THEN @ValorFinal + 2
									 ELSE ValorFinal
									 END
			FROM	ParametrosSistemaEmpresa PSE
			WHERE	EmpresaId = @EmpresaId
					AND TipoIncidenteId = @TipoIncidenteId
					AND ParametrosSistemaId = 9
					AND ValorInicial <> @ValorFinal + 1	
			
			-- Valida Rojo
			SELECT  @ValorFinal = ValorFinal
			FROM	ParametrosSistemaEmpresa
			WHERE	EmpresaId = @EmpresaId
				AND TipoIncidenteId = @TipoIncidenteId
				AND ParametrosSistemaId = 9

			UPDATE  PSE
			SET		ValorInicial = @ValorFinal + 1,
					ValorFinal = CASE
									 WHEN ValorFinal <= @ValorFinal + 1 THEN @ValorFinal + 2
									 ELSE ValorFinal
									 END
			FROM	ParametrosSistemaEmpresa PSE
			WHERE	EmpresaId = @EmpresaId
					AND TipoIncidenteId = @TipoIncidenteId
					AND ParametrosSistemaId = 10
					AND ValorInicial <> @ValorFinal + 1	
		END -- IF @ParametrosSistemaId = 8 -- verde

		IF @ParametrosSistemaId = 9 -- amarillo
		BEGIN
			-- Valida Rojo
			UPDATE  PSE
			SET		ValorInicial = @ValorFinal + 1,
					ValorFinal = CASE
									 WHEN ValorFinal <= @ValorFinal + 1 THEN @ValorFinal + 2
									 ELSE ValorFinal
									 END
			FROM	ParametrosSistemaEmpresa PSE
			WHERE	EmpresaId = @EmpresaId
					AND TipoIncidenteId = @TipoIncidenteId
					AND ParametrosSistemaId = 10
					AND ValorInicial <> @ValorFinal + 1

			-- Valida Verde
			UPDATE  PSE
			SET		ValorInicial = 0,
					ValorFinal = CASE
									 WHEN ValorFinal <> @ValorInicial - 1 THEN @ValorInicial - 1
									 ELSE ValorFinal
									 END
			FROM	ParametrosSistemaEmpresa PSE
			WHERE	EmpresaId = @EmpresaId
					AND TipoIncidenteId = @TipoIncidenteId
					AND ParametrosSistemaId = 8
					AND ValorFinal <> @ValorInicial -1
		END -- IF @ParametrosSistemaId = 9 -- amarillo

		IF @ParametrosSistemaId = 10 -- rojo, 
		BEGIN
			-- valida el amarillo
			UPDATE  PSE
			SET		ValorInicial = CASE
									 WHEN ValorInicial >= @ValorInicial - 1 THEN @ValorInicial - 2
									 ELSE ValorInicial
									 END,
					ValorFinal = CASE
									 WHEN ValorFinal <> @ValorInicial - 1 THEN @ValorInicial - 1
									 ELSE ValorFinal
									 END
			FROM	ParametrosSistemaEmpresa PSE
			WHERE	EmpresaId = @EmpresaId
					AND TipoIncidenteId = @TipoIncidenteId
					AND ParametrosSistemaId = 9
					AND ValorFinal <> @ValorInicial -1

			-- valida el Verde
			SELECT @ValorInicial = ValorInicial
			FROM	ParametrosSistemaEmpresa PSE
			WHERE	EmpresaId = @EmpresaId
					AND TipoIncidenteId = @TipoIncidenteId
					AND ParametrosSistemaId = 9

			UPDATE  PSE
			SET		ValorInicial = 0,
					ValorFinal = CASE
									 WHEN ValorFinal <> @ValorInicial - 1 THEN @ValorInicial - 1
									 ELSE ValorFinal
									 END
			FROM	ParametrosSistemaEmpresa PSE
			WHERE	EmpresaId = @EmpresaId
					AND TipoIncidenteId = @TipoIncidenteId
					AND ParametrosSistemaId = 8
					AND ValorFinal <> @ValorInicial -1
		END -- IF @ParametrosSistemaId = 10

		-- Valida Parametros de CONOR, con el cambio de las empresas
		IF @EmpresaId <> 2 -- CONOR
		BEGIN
			-- Consulta los valores por empresas y genera el promedio
			SELECT  ParametrosSistemaId, 0 AS ValorInicial, 
					CEILING(SUM(ValorFinal)/COUNT(*))  AS ValorFinal
			INTO	#tempConor
			FROM	ParametrosSistemaEmpresa PSE
			WHERE	ParametrosSistemaId IN (8,9,10)
					AND EmpresaId <> 2
					AND TipoIncidenteId = @TipoIncidenteId
			GROUP BY  ParametrosSistemaId

			-- Modifica la informacion de los Rangos.
			SELECT @ValorFinal = ValorFinal FROM #tempConor WHERE ParametrosSistemaId = 8
			UPDATE  T
			SET		ValorInicial = @ValorFinal + 1,
					ValorFinal = CASE 
								WHEN ValorFinal <= @ValorFinal + 1 THEN @ValorFinal + 2
								ELSE ValorFinal
								END								
			FROM	#tempConor AS T										
			WHERE	ParametrosSistemaId = 9
			
			SELECT @ValorFinal = ValorFinal FROM #tempConor WHERE ParametrosSistemaId = 9
			UPDATE  T
			SET		ValorInicial = @ValorFinal + 1,
					ValorFinal = CASE 
								WHEN ValorFinal <= @ValorFinal + 1 THEN @ValorFinal + 2
								ELSE ValorFinal
								END								
			FROM	#tempConor AS T										
			WHERE	ParametrosSistemaId = 10
			
			-- MODIFICA LA INFORMACION DE LOS RANGOS YA EXISTENTES.
			UPDATE  PSE
			SET		ValorInicial = T.ValorInicial,
					ValorFinal = T.ValorFinal
			FROM	ParametrosSistemaEmpresa PSE
					INNER JOIN #tempConor AS T
							ON PSE.TipoIncidenteId = @TipoIncidenteId
							AND PSE.ParametrosSistemaId = T.ParametrosSistemaId
							AND PSE.EmpresaId = 2
	
			-- INSERTA LA INFORMACION DE LOS RANGOS YA EXISTENTES.
			INSERT  ParametrosSistemaEmpresa
			SELECT  2, @TipoIncidenteId, 
					T.ValorInicial, T.ValorFinal,
					NULL, NULL, 0, CURRENT_TIMESTAMP,
					T.ParametrosSistemaId 
			FROM	#tempConor AS T
					LEFT JOIN ParametrosSistemaEmpresa PSE
							ON PSE.TipoIncidenteId = @TipoIncidenteId
							AND PSE.ParametrosSistemaId = T.ParametrosSistemaId
							AND PSE.EmpresaId = 2
			WHERE	PSE.Id IS NULL
					
			
		END 	
	END -- IF @TipoIncidenteId IS NOT NULL 
END
GO
PRINT 'Crear trigger trParametrosSistemaEmpresa'
