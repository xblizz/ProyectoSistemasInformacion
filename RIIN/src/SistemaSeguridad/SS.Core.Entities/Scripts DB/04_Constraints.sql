-- AFTER generating model and BEFORE fill with default data
-- run this script to ADD complex constraints to model. Since Entity Framework Data Model Tools doesn't work with it

-- Adds a constraint using three FKs
ALTER TABLE [dbo].[ParametrosSistemaEmpresa]
ADD CONSTRAINT UK_ParametrosSistemaEmpresa UNIQUE ([EmpresaId],[TipoIncidenteId],[ParametrosSistemaId])