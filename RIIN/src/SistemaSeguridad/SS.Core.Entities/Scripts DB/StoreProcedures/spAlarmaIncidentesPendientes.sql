USE ALXout01DB
GO
-- spAlarmaIncidentesPendientes
IF EXISTS ( SELECT * FROM Sysobjects WHERE NAME = 'spAlarmaIncidentesPendientes' AND Xtype = 'P' )
BEGIN
	DROP PROCEDURE spAlarmaIncidentesPendientes
	PRINT 'Eliminar spAlarmaIncidentesPendientes'
END
GO
CREATE PROCEDURE spAlarmaIncidentesPendientes 
AS 
BEGIN
	SET NOCOUNT ON
	-- ========================================
	-- Incidentes con Informacion pendiente
	-- ========================================
	-- Dias a monitorear.
	-- El default del numero de dias para monitorear es 7. Si no esta capturado se toma ese valor.
	DECLARE @DiaMaximo INT,
			@DiaMinimo INT
	SET  @DiaMaximo = (  SELECT ValorInicial
					FROM	ParametrosSistemaEmpresa AS PSE
					WHERE   PSE.EmpresaId = 0 -- CONOR
							AND PSE.ParametrosSistemaId = 2 -- Dias de Alarma
				)
	SET @DiaMaximo =  COALESCE(@DiaMaximo,7)
	SET @DiaMinimo = @DiaMaximo - 2
	IF @DiaMinimo < 0 SET @DiaMinimo = 0
	
	-- ==============================================
	-- Consulta de incidentes con Info Pendiente
	-- ==============================================
	--Datos Generales: 
	--       Fecha, Hora, Grupo, Empresa, Tipo Incidente, Tipo Afectacion, Lesionados, Tipo Instalacion, 
	--       Instalacion, Monto Afectacion, Comentario
	--Ubicacion: 
	--	   Estado, Ciudad, Zona, Calle, Colonia, Entre Calles, Latitud, Longitud, 
	--Robo con Violencia:	TipoArma, Detenidos,CantidadDelincuentes, TipoVehiculo
	--Robo sin Violencia:  Detenidos
	--Secuestro de Empleado en Ruta: Con o sin Vehiculo, Cantidad de Delincuentes, Tipo Arma
	--Extorsion : Tipo 
	--Amenaza: Medio, Motivo
	--Intrusion: Tipo
	 SELECT I.Id, I.UsuarioAlta, @DiaMaximo - DATEDIFF(d,FechaAlta, GETDATE()) AS DiasModificacion, CAST(NULL AS VARCHAR(1000)) AS strId 
	 INTO   #tempResultado 
	 FROM   Incidentes AS I
			LEFT JOIN AfectacionesIncidente AS AI
					ON I.Id = AI.IncidenteId
	 WHERE  I.FechaCancelacion IS NULL -- Que no este Cancelado
			AND DATEDIFF(d,FechaAlta, GETDATE()) BETWEEN @DiaMinimo AND @DiaMaximo
			AND ( -- Valida Informacion General Incompleta
					I.LesionadosId IS NULL 
					OR I.TipoInstalacionId IS NULL
					OR I.InstalacionId IS NULL 
					OR I.MontoAfectacion IS NULL
					OR I.Comentarios IS NULL	
					OR AI.IncidenteId IS NULL -- No tiene Tipos de Afectacion
				  -- Ubicacion: 
					OR I.Calle IS NULL
					OR I.Colonia IS NULL
				  -- Robo con violencia: TipoArma, Detenidos,CantidadDelincuentes, TipoVehiculo
					OR ( I.TipoIncidenteId = 1 
						AND (	I.TipoArmaId IS NULL 
								OR I.Detenidos IS NULL 
								OR I.CantidadDelincuentesId IS NULL 
								OR TipoVehiculoId IS NULL 
							)
						)
				  -- Robo sin violencia
					OR ( I.TipoIncidenteId = 2 AND I.Detenidos IS NULL )					
				  -- Secuestro de empleado en ruta : Con o sin Vehiculo, Cantidad de Delincuentes, Tipo Arma
					OR ( I.TipoIncidenteId = 3 
						AND ( I.ConVehiculo IS NULL 
							  OR I.CantidadDelincuentesId IS NULL 
							  OR I.TipoArmaId IS NULL
							)
					   )
				  -- Extorsion
					OR ( I.TipoIncidenteId = 4 AND I.TipoExtorsionId IS NULL)					
				  -- Amenaza
					OR ( I.TipoIncidenteId = 5 
						-- AND ( I.MedioAmenazaId IS NULL OR I.MotivoAmenazaId IS NULL)
						)
				  -- Intrusion
					OR ( I.TipoIncidenteId = 6 AND I.TipoIntrusionId IS NULL )		
			)
			ORDER BY I.UsuarioAlta, I.Id

		-- Si tiene datos,sigue con el procedimiento..
		IF EXISTS ( SELECT * FROM #tempResultado)
		BEGIN
			-- Agrupa la informacion por Usuario
			DECLARE @str AS VARCHAR(1000),
					@UsuarioId AS INT
			SELECT @str = NULL, @UsuarioId = -1;
			UPDATE  T
			SET		@str = strId = CASE	
								   WHEN UsuarioAlta <> @UsuarioId THEN CAST(Id AS VARCHAR) 
								   ELSE @str + ', ' + CAST(Id AS VARCHAR) 
								   END,
					@UsuarioId = UsuarioAlta								
			FROM	#tempResultado T

			-- ENVIO DEL CORREO
			DECLARE @Subject AS VARCHAR(100),
					@Title AS VARCHAR(100),
					@Cont AS INT,
					@Max AS INT,
					@Id AS INT,
					@Email AS VARCHAR(30),
					@recipients AS VARCHAR(100),
					@body AS VARCHAR(1000),
					@strId AS VARCHAR(1000)
				
			DECLARE crsEnvioMail CURSOR FOR
				SELECT  T.Id, T.strId, U.Email
				FROM	#tempResultado T
						INNER JOIN (SELECT  MAX(Id) AS Id, UsuarioAlta
									FROM	#tempResultado T
									GROUP BY UsuarioAlta) AS X
								ON T.Id = X.Id
						INNER JOIN Usuarios AS U
								ON U.Id = T.UsuarioAlta	
				OPEN crsEnvioMail
				FETCH NEXT FROM crsEnvioMail INTO @Id, @strId, @Email	
			WHILE @@FETCH_STATUS =0 
			BEGIN

				IF CHARINDEX(',',@strId) <= 0 
					SELECT @body = 'Alerta: información pendiente de Capturar
El incidente con el identificador [ID] tiene información pendiente de capturar'
				ELSE
					SELECT @body = 'Alerta: información pendiente de Capturar
Los incidentes con los identificadores [ID] tienen información pendiente de capturar'

				SELECT @body = REPLACE(@body,'[ID]', @strId)

				EXEC	msdb.dbo.sp_send_dbmail
						@recipients = @Email,
						@subject = 'Incidente con Información pendiente',
						@body = @body

				FETCH NEXT FROM crsEnvioMail INTO @Id, @strId, @Email	
			END
			CLOSE crsEnvioMail
			DEALLOCATE crsEnvioMail
	END --IF EXISTS ( SELECT * FROM #tempResultado)
END
GO