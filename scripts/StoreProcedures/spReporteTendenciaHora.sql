USE [ALXout01DB]
GO
/****** Object:  StoredProcedure [dbo].[spReporteTendenciaHora]    Script Date: 02/05/2012 13:12:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spReporteTendenciaHora] 
	@EmpresaId INT,
	@strTipoIncidente VARCHAR(1000) = NULL, -- 1,2,5,6
	@EstadoId	INT = NULL,
	@CiudadId	INT = NULL,
	@ZonaId		INT = NULL,					-- NULL: TODAS, -1: Fuera de Zona, Valor..
	@ConsolidadoFlg BIT = NULL,

	-- Hora + FechaInicio
	-- Dias (Es necesario tambien usar la fecha)
	@FechaInicio Datetime = NULL,
	@HoraInicio varchar(5) = NULL,
	@HoraFinal varchar(5)
	
AS 
BEGIN
	SET NOCOUNT ON
	
	DECLARE @FechaFinal DateTime = NULL

	-- Consulta por Horas
	IF @FechaInicio IS NULL OR @HoraInicio IS NULL OR @HoraFinal IS NULL 
	BEGIN 
		RAISERROR ('Para la consulta por Horas, es requerido dia, la hora inicial y final',16,1)
		RETURN -1
	END 
	
	SELECT  @FechaInicio = CONVERT(Datetime, CONVERT(VARCHAR,@FechaInicio,112) + ' ' + @HoraInicio + ':00.000'),
			@FechaFinal = CONVERT(Datetime, CONVERT(VARCHAR,@FechaInicio,112) + ' ' +  @HoraFinal + ':59.000')
	
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
		SELECT @HoraInicio = RIGHT('00' + CONVERT(VARCHAR,DATEPART(hh,MIN(FechaIncidente))),2)+ ':' +
							RIGHT('00' + CONVERT(VARCHAR,DATEPART(mi,MIN(FechaIncidente))),2)
		FROM #tempResultado

		SELECT @HoraFinal = RIGHT('00' + CONVERT(VARCHAR,DATEPART(hh,MAX(FechaIncidente))),2)+ ':' +
							RIGHT('00' + CONVERT(VARCHAR,DATEPART(mi,MAX(FechaIncidente))),2)
		FROM #tempResultado

		;WITH tblMediaHora (number,Id, HoraInicial, HoraFinal)
		AS (
			SELECT ROW_NUMBER() OVER(Order By Id), Id, HoraInicial, HoraFinal
			FROM (
					SELECT  Id, HoraInicial, HoraFinal
					FROM	MediasHoras AS M
					WHERE	@HoraInicio BETWEEN HoraInicial AND HoraFinal
					UNION 
					SELECT  Id, HoraInicial, HoraFinal
					FROM	MediasHoras AS M
					WHERE	HoraInicial BETWEEN @HoraInicio AND @HoraFinal
				) AS X
		) 
			SELECT  I.TipoIncidenteId, Tipo.Nombre, COUNT(*) AS CantidadIncidentes, T.HoraInicial +'-' + T2.HoraFinal AS Hora
			FROM	tblMediaHora AS T
					LEFT JOIN MediasHoras AS T2
							ON T.Id + 1 = T2.Id
					LEFT JOIN #tempResultado AS I
							ON RIGHT('00' + CONVERT(VARCHAR,DATEPART(hh,I.FechaIncidente)),2)+ ':' + 
							RIGHT('00' + CONVERT(VARCHAR,DATEPART(mi,I.FechaIncidente)),2) BETWEEN T.HoraInicial AND T2.HoraFinal
					LEFT JOIN TiposIncidente AS Tipo ON Tipo.Id = I.TipoIncidenteId				
			WHERE	T.Number % 2 = 1 -- Busca el primer rango
			GROUP BY I.TipoIncidenteId, Tipo.Nombre, T.Number, T.HoraInicial +'-' + T2.HoraFinal
			ORDER BY T.Number, I.TipoIncidenteId
END