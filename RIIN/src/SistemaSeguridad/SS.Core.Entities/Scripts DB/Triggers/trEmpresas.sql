USE ALXout01DB
GO

IF EXISTS ( SELECT *
			FROM sysobjects 
			WHERE Name = N'trEmpresas'
				AND XType = 'TR'
			)
BEGIN
	DROP TRIGGER trEmpresas
	PRINT 'Eliminar trigger trEmpresas'
END
GO
-- ==============================================================================================
-- Autor: MexWare
-- Fecha:  20120131
-- Descripcion: No permite borrar el Grupo/Empresa de Conor.
-- ==============================================================================================
CREATE TRIGGER trEmpresas ON Empresas FOR DELETE
AS
BEGIN

	SET NOCOUNT ON

	
	IF EXISTS (	SELECT *
				FROM   Deleted D
				WHERE  D.Id IN (1,2)
				) 
	BEGIN
		SET IDENTITY_INSERT Empresas ON
		
		INSERT Empresas (Id, Nombre, TipoEmpresa, UsuarioAlta, FechaAlta, GrupoId)
		SELECT Id, Nombre, TipoEmpresa, UsuarioAlta, FechaAlta, GrupoId
		FROM DELETED
		WHERE  Id IN (1,2)
		
		SET IDENTITY_INSERT Empresas OFF
		
		RAISERROR ('No se puede eliminar el Grupo y Empresa de Conor.', 16,1)
	END
	
END
GO
PRINT 'Crear trigger trEmpresas'
