USE ALXout01DB
GO

CREATE PROCEDURE spReporteTendencia 
	@EmpresaId INT,
	@strTipoIncidente VARCHAR(1000) = NULL,
	@EstadoId	INT = NULL,
	@CiudadId	INT = NULL,
	@ZonaId		INT = NULL, -- NULL: TODAS, -1: Fuera de Zona, Valor..
	@ConsolidadoFlg BIT = 1,
	@UnidadTiempoId INT, --  1: Mes, 2: Dia, 3: Hora
	@MesInicial INT = NULL,
	@MesFinal INT = NULL,
	@AnioInicial INT = NULL,
	@AnioFinal INT = NULL,
	@FechaInicio Datetime = NULL,
	@FechaFinal DateTime = NULL,
	@HoraInicio varchar(5) = NULL,
	@HoraFinal varchar(5) = NULL
AS 
BEGIN
	
	-- Consulta por MES	
	IF @UnidadTiempoId = 1
	BEGIN
		IF @MesInicial IS NULL OR @AnioInicial IS NULL OR @MesFinal IS NULL OR @AnioFinal IS NULL 
		BEGIN 
			RAISERROR ('Para la consulta por Mes, es requerido el Mes y Año inicial y final',16,1)
			RETURN -1
		END 
	
		SELECT  @FechaInicio = CONVERT(Datetime, CONVERT(VARCHAR,@AnioInicial*10000+@MesInicial*100+1)),
				@FechaFinal = CONVERT(Datetime, CONVERT(VARCHAR,@AnioFinal*10000+
							  CASE	
							  WHEN @MesFinal IN (1,3,5,7,8,10,11) THEN 31
							  WHEN @MesFinal = 2 AND ISDATE(CONVERT(VARCHAR,@AnioFinal*10000+229)) = 1 THEN 29
							  WHEN @MesFinal = 2 THEN 28
							  ELSE 30
							 END * 100) + '23:59:59.000');

		-- pendiente..mostrar todos los meses comprendidos en ese tiempo.. aun sin informacion.. ?
        -- Resultado a desplegar
		SELECT  I.TipoIncidenteId, COUNT(*) AS CantidadIncidentes, 
				DATEPART(m, I.FechaAlta) AS Mes, DATEPART(y, I.FechaAlta) AS Anio
		FROM	Incidentes AS I
				LEFT JOIN dbo.SplitString(@strTipoIncidente,',') AS T
						ON I.TipoIncidenteId = T.Valor
		WHERE	I.FechaAlta BETWEEN @FechaInicio AND @FechaFinal
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
		GROUP BY I.TipoIncidenteId, DATEPART(m, I.FechaAlta), DATEPART(y, I.FechaAlta)
		

						
	
	END -- Consulta por Mes

END
GO

