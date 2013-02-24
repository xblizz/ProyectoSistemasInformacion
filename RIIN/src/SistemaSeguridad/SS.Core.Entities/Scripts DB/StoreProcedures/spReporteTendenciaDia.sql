USE [ALXout01DB]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spReporteTendenciaDia] 
	@EmpresaId INT,
	@strTipoIncidente VARCHAR(1000) = NULL, -- 1,2,5,6
	@EstadoId	INT = NULL,
	@CiudadId	INT = NULL,
	@ZonaId		INT = NULL,					-- NULL: TODAS, -1: Fuera de Zona, Valor..
	@ConsolidadoFlg BIT = NULL,
	
	-- Dia
	@FechaInicio Datetime = NULL,
	@FechaFinal DateTime = NULL
AS 

BEGIN

	SET NOCOUNT ON
		
		IF @FechaInicio IS NULL OR @FechaFinal IS NULL
		BEGIN 
			RAISERROR ('Para la consulta por Dia, es requerido el dia inicial y final',16,1)
			RETURN -1
		END 
	
		SELECT  @FechaInicio = CONVERT(Datetime, CONVERT(VARCHAR,@FechaInicio,112)),
				@FechaFinal = CONVERT(Datetime, CONVERT(VARCHAR,@FechaFinal,112) + ' 23:59:59.000')

	-- Consulta de incidentes.
	SELECT  I.TipoIncidenteId, I.FechaIncidente
	INTO   #tempResultado
	FROM	Incidentes AS I
			LEFT JOIN dbo.SplitString(@strTipoIncidente,',') AS T
					ON I.TipoIncidenteId = T.Valor
	WHERE	I.FechaIncidente BETWEEN @FechaInicio AND @FechaFinal
			AND I.FechaCancelacion IS NULL -- Que no este cancelado
			-- Consulta de toda las empresas, o solo de la que pertenece
			AND (@ConsolidadoFlg = 1 OR I.EmpresaId = @EmpresaId) 
			-- Tipo de Incidente
			AND (@strTipoIncidente IS NULL  -- Cualquier tipo de Incidente
				OR T.Valor IS NOT NULL )					
			-- Consulta por Estado
			AND (@EstadoId IS NULL OR I.EstadoId = @EstadoId)
			-- Consulta por Ciudad
			AND (@CiudadId IS NULL OR I.CiudadId = @CiudadId)
			-- Consulta por Zona
			AND  (@ZonaId IS NULL  -- No importa la zona
				  OR (@ZonaId = -1 AND I.ZonaId IS NULL) --El incidente no tiene zona
				  OR  (@ZonaId IS NOT NULL AND I.ZonaId = @ZonaId))
		
		-- AHORA HACE LOS AGRUPAMIENTOS y muestra los rangos de Informacion
		SELECT  I.TipoIncidenteId, COUNT(*) AS CantidadIncidentes, 
			CONVERT(DATETIME,CONVERT(varchar,I.FechaIncidente,112)) AS Dia
			INTO	#tempResultado2 
			FROM	#tempResultado AS I
			GROUP BY I.TipoIncidenteId, CONVERT(DATETIME,CONVERT(varchar,I.FechaIncidente,112))

			Select @FechaInicio = MIN(Dia) FROM #tempResultado2
			Select @FechaFinal = MAX(Dia) FROM #tempResultado2

			-- Busca los dias que estan comprendidos entre el rango de fechas.
			SELECT  COALESCE(TI.Nombre,'') AS TipoIncidente, COALESCE(T.CantidadIncidentes,0) AS CantidadIncidentes, X.Dia
			FROM	(
						SELECT  DATEADD(d, T.Number - 1, @FechaInicio)  AS Dia
						FROM	Tally AS T
						WHERE	Number <= DATEDIFF(d,@FechaInicio, @FechaFinal) + 1) AS X
					LEFT JOIN #tempResultado2 AS T ON T.Dia = X.Dia
					LEFT JOIN TiposIncidente AS TI ON T.TipoIncidenteId = TI.Id
END