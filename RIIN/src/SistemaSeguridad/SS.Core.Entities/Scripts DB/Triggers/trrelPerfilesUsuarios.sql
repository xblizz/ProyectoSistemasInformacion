USE ALXout01DB
GO

IF EXISTS ( SELECT *
			FROM sysobjects 
			WHERE Name = N'trrelPerfilesUsuarios'
				AND XType = 'TR'
			)
BEGIN
	DROP TRIGGER trrelPerfilesUsuarios
	PRINT 'Eliminar trigger trrelPerfilesUsuarios'
END
GO
-- ==========================================================================================================
-- Autor: MexWare
-- Fecha:  20120204
-- Descripcion: No se permite asignar el perfil de Administrador General a un usuario que no sea de CONOR. 
-- ==========================================================================================================
CREATE TRIGGER trrelPerfilesUsuarios ON relPerfilesUsuarios FOR INSERT
AS
BEGIN

	SET NOCOUNT ON

	
	IF EXISTS (	SELECT *
				FROM   INSERTED I
						INNER JOIN relUsuarioEmpresa AS U -- Busca las empresas del usuario
								ON I.UsuarioId = U.UsuarioId
								AND U.EmpresaId <> 2 -- No es Conor 
				WHERE  I.PerfilId = 1 -- Administrador General
				) 
	BEGIN
		DELETE  D
		FROM	relPerfilesUsuarios AS D
				INNER JOIN INSERTED AS I
						ON D.UsuarioId = I.UsuarioId
						AND D.PerfilId = I.PerfilId
						AND I.PerfilId = 1 -- Administrador General
		
		RAISERROR ('No se puede asignar el Rol de Administrador General, a un usuario que no sea de Conor.', 16,1)
	END
	
END
GO
PRINT 'Crear trigger trrelPerfilesUsuarios'
