
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 02/04/2012 22:30:45
-- Generated from EDMX file: C:\Users\rchavez\Documents\Visual Studio 2010\Projects\SistemaSeguridad\SS.Core.Entities\Conor.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ALXout01DB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PerfilrelPerfilUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[relPerfilesUsuarios] DROP CONSTRAINT [FK_PerfilrelPerfilUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_EmpresasInstalaciones]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Instalaciones] DROP CONSTRAINT [FK_EmpresasInstalaciones];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoInstalacionInstalaciones]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Instalaciones] DROP CONSTRAINT [FK_TipoInstalacionInstalaciones];
GO
IF OBJECT_ID(N'[dbo].[FK_EstadosCiudades]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ciudades] DROP CONSTRAINT [FK_EstadosCiudades];
GO
IF OBJECT_ID(N'[dbo].[FK_EstadosZona]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Zonas] DROP CONSTRAINT [FK_EstadosZona];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoArmaIncidentes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Incidentes] DROP CONSTRAINT [FK_TipoArmaIncidentes];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoExtorcionIncidentes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Incidentes] DROP CONSTRAINT [FK_TipoExtorcionIncidentes];
GO
IF OBJECT_ID(N'[dbo].[FK_CantidadDelincuentesIncidentes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Incidentes] DROP CONSTRAINT [FK_CantidadDelincuentesIncidentes];
GO
IF OBJECT_ID(N'[dbo].[FK_LesionadosIncidentes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Incidentes] DROP CONSTRAINT [FK_LesionadosIncidentes];
GO
IF OBJECT_ID(N'[dbo].[FK_MedioAmenazaIncidentes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Incidentes] DROP CONSTRAINT [FK_MedioAmenazaIncidentes];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoIntrusionIncidentes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Incidentes] DROP CONSTRAINT [FK_TipoIntrusionIncidentes];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoVehiculoIncidentes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Incidentes] DROP CONSTRAINT [FK_TipoVehiculoIncidentes];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoIncidenteIncidentes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Incidentes] DROP CONSTRAINT [FK_TipoIncidenteIncidentes];
GO
IF OBJECT_ID(N'[dbo].[FK_UsuarioIncidentes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Incidentes] DROP CONSTRAINT [FK_UsuarioIncidentes];
GO
IF OBJECT_ID(N'[dbo].[FK_EmpresasIncidentes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Incidentes] DROP CONSTRAINT [FK_EmpresasIncidentes];
GO
IF OBJECT_ID(N'[dbo].[FK_EstadosIncidentes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Incidentes] DROP CONSTRAINT [FK_EstadosIncidentes];
GO
IF OBJECT_ID(N'[dbo].[FK_CiudadesIncidentes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Incidentes] DROP CONSTRAINT [FK_CiudadesIncidentes];
GO
IF OBJECT_ID(N'[dbo].[FK_ZonaIncidentes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Incidentes] DROP CONSTRAINT [FK_ZonaIncidentes];
GO
IF OBJECT_ID(N'[dbo].[FK_ConfiguracionDashboardFiltrosDashboard]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FiltrosDashboard] DROP CONSTRAINT [FK_ConfiguracionDashboardFiltrosDashboard];
GO
IF OBJECT_ID(N'[dbo].[FK_UsuarioUsuarioPerfil]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[relPerfilesUsuarios] DROP CONSTRAINT [FK_UsuarioUsuarioPerfil];
GO
IF OBJECT_ID(N'[dbo].[FK_PoligonoPoligonoDetalle]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PoligonosDetalle] DROP CONSTRAINT [FK_PoligonoPoligonoDetalle];
GO
IF OBJECT_ID(N'[dbo].[FK_NivelGeograficoPoligono]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Poligonos] DROP CONSTRAINT [FK_NivelGeograficoPoligono];
GO
IF OBJECT_ID(N'[dbo].[FK_InstalacionesIncidentes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Incidentes] DROP CONSTRAINT [FK_InstalacionesIncidentes];
GO
IF OBJECT_ID(N'[dbo].[FK_ZonaPoligono]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Zonas] DROP CONSTRAINT [FK_ZonaPoligono];
GO
IF OBJECT_ID(N'[dbo].[FK_EstadosPoligono]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Estados] DROP CONSTRAINT [FK_EstadosPoligono];
GO
IF OBJECT_ID(N'[dbo].[FK_PoligonoCiudades]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ciudades] DROP CONSTRAINT [FK_PoligonoCiudades];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoUnidadTiempoConfiguracionDashboard]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ConfiguracionesDashboard] DROP CONSTRAINT [FK_TipoUnidadTiempoConfiguracionDashboard];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoFiltroDashboardFiltrosDashboard]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FiltrosDashboard] DROP CONSTRAINT [FK_TipoFiltroDashboardFiltrosDashboard];
GO
IF OBJECT_ID(N'[dbo].[FK_ReportesDashboardConfiguracionDashboard]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ConfiguracionesDashboard] DROP CONSTRAINT [FK_ReportesDashboardConfiguracionDashboard];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoIncidenteAlertaIncidente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AlertasIncidente] DROP CONSTRAINT [FK_TipoIncidenteAlertaIncidente];
GO
IF OBJECT_ID(N'[dbo].[FK_EmpresaAlertaIncidente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AlertasIncidente] DROP CONSTRAINT [FK_EmpresaAlertaIncidente];
GO
IF OBJECT_ID(N'[dbo].[FK_ParametrosSistemaParametrosSistemaEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ParametrosSistemaEmpresa] DROP CONSTRAINT [FK_ParametrosSistemaParametrosSistemaEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_EmpresaParametrosSistemaEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ParametrosSistemaEmpresa] DROP CONSTRAINT [FK_EmpresaParametrosSistemaEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoAfectacionAfectacionIncidente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AfectacionesIncidente] DROP CONSTRAINT [FK_TipoAfectacionAfectacionIncidente];
GO
IF OBJECT_ID(N'[dbo].[FK_IncidenteAfectacionIncidente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AfectacionesIncidente] DROP CONSTRAINT [FK_IncidenteAfectacionIncidente];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoIncidenteParametroSistemaEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ParametrosSistemaEmpresa] DROP CONSTRAINT [FK_TipoIncidenteParametroSistemaEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_UsuarioEmpresaUsuario]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[relUsuarioEmpresa] DROP CONSTRAINT [FK_UsuarioEmpresaUsuario];
GO
IF OBJECT_ID(N'[dbo].[FK_UsuarioEmpresaEmpresa]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[relUsuarioEmpresa] DROP CONSTRAINT [FK_UsuarioEmpresaEmpresa];
GO
IF OBJECT_ID(N'[dbo].[FK_MotivoAmenazaIncidente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Incidentes] DROP CONSTRAINT [FK_MotivoAmenazaIncidente];
GO
IF OBJECT_ID(N'[dbo].[FK_TipoInstalacionIncidente]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Incidentes] DROP CONSTRAINT [FK_TipoInstalacionIncidente];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[CantidadDelincuentes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CantidadDelincuentes];
GO
IF OBJECT_ID(N'[dbo].[Estados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Estados];
GO
IF OBJECT_ID(N'[dbo].[Ciudades]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ciudades];
GO
IF OBJECT_ID(N'[dbo].[Empresas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Empresas];
GO
IF OBJECT_ID(N'[dbo].[Incidentes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Incidentes];
GO
IF OBJECT_ID(N'[dbo].[Instalaciones]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Instalaciones];
GO
IF OBJECT_ID(N'[dbo].[Lesionados]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Lesionados];
GO
IF OBJECT_ID(N'[dbo].[MediosAmenaza]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MediosAmenaza];
GO
IF OBJECT_ID(N'[dbo].[Perfiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Perfiles];
GO
IF OBJECT_ID(N'[dbo].[Poligonos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Poligonos];
GO
IF OBJECT_ID(N'[dbo].[relPerfilesUsuarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[relPerfilesUsuarios];
GO
IF OBJECT_ID(N'[dbo].[MotivosAmenaza]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MotivosAmenaza];
GO
IF OBJECT_ID(N'[dbo].[TiposArma]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TiposArma];
GO
IF OBJECT_ID(N'[dbo].[TiposExtorsion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TiposExtorsion];
GO
IF OBJECT_ID(N'[dbo].[TiposIncidente]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TiposIncidente];
GO
IF OBJECT_ID(N'[dbo].[TiposInstalacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TiposInstalacion];
GO
IF OBJECT_ID(N'[dbo].[TiposIntrusion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TiposIntrusion];
GO
IF OBJECT_ID(N'[dbo].[TiposVehiculo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TiposVehiculo];
GO
IF OBJECT_ID(N'[dbo].[Usuarios]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Usuarios];
GO
IF OBJECT_ID(N'[dbo].[Zonas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Zonas];
GO
IF OBJECT_ID(N'[dbo].[AlertasIncidente]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AlertasIncidente];
GO
IF OBJECT_ID(N'[dbo].[ParametrosSistemaEmpresa]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ParametrosSistemaEmpresa];
GO
IF OBJECT_ID(N'[dbo].[ConfiguracionesDashboard]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ConfiguracionesDashboard];
GO
IF OBJECT_ID(N'[dbo].[NivelesGeograficos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NivelesGeograficos];
GO
IF OBJECT_ID(N'[dbo].[TiposUnidadTiempo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TiposUnidadTiempo];
GO
IF OBJECT_ID(N'[dbo].[ParametrosSistema]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ParametrosSistema];
GO
IF OBJECT_ID(N'[dbo].[FiltrosDashboard]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FiltrosDashboard];
GO
IF OBJECT_ID(N'[dbo].[TiposFiltroDashboard]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TiposFiltroDashboard];
GO
IF OBJECT_ID(N'[dbo].[PoligonosDetalle]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PoligonosDetalle];
GO
IF OBJECT_ID(N'[dbo].[ReportesDashboard]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReportesDashboard];
GO
IF OBJECT_ID(N'[dbo].[TiposAfectacion]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TiposAfectacion];
GO
IF OBJECT_ID(N'[dbo].[AfectacionesIncidente]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AfectacionesIncidente];
GO
IF OBJECT_ID(N'[dbo].[relUsuarioEmpresa]', 'U') IS NOT NULL
    DROP TABLE [dbo].[relUsuarioEmpresa];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'CantidadDelincuentes'
CREATE TABLE [dbo].[CantidadDelincuentes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NombreDeCantidad] nvarchar(50)  NOT NULL,
    [Orden] int  NOT NULL
);
GO

-- Creating table 'Estados'
CREATE TABLE [dbo].[Estados] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [PoligonoId] int  NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL
);
GO

-- Creating table 'Ciudades'
CREATE TABLE [dbo].[Ciudades] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EstadoId] int  NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [PoligonoId] int  NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL
);
GO

-- Creating table 'Empresas'
CREATE TABLE [dbo].[Empresas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [TipoEmpresa] bit  NOT NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL,
    [GrupoId] int  NULL
);
GO

-- Creating table 'Incidentes'
CREATE TABLE [dbo].[Incidentes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FechaIncidente] datetime  NOT NULL,
    [EmpresaId] int  NOT NULL,
    [TipoIncidenteId] int  NOT NULL,
    [LesionadosId] int  NULL,
    [TipoInstalacionId] int  NULL,
    [InstalacionId] int  NULL,
    [MontoAfectacion] decimal(18,2)  NULL,
    [Comentarios] nvarchar(max)  NULL,
    [Latitud] float  NOT NULL,
    [Longitud] float  NOT NULL,
    [EstadoId] int  NOT NULL,
    [CiudadId] int  NOT NULL,
    [ZonaId] int  NULL,
    [TipoArmaId] int  NULL,
    [Detenidos] bit  NULL,
    [CantidadDelincuentesId] int  NULL,
    [TipoVehiculoId] int  NULL,
    [TipoExtorsionId] int  NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL,
    [UsuarioCancelacion] int  NULL,
    [FechaCancelacion] datetime  NULL,
    [UsuarioUltimaModificacion] int  NULL,
    [FechaUltimaMoldificacion] datetime  NULL,
    [MotivoAmenazaId] int  NULL,
    [MedioAmenazaId] int  NULL,
    [TipoIntrusionId] int  NULL,
    [Calle] nvarchar(50)  NOT NULL,
    [Colonia] nvarchar(50)  NOT NULL,
    [EntreCalles] nvarchar(80)  NULL,
    [ConVehiculo] bit  NULL
);
GO

-- Creating table 'Instalaciones'
CREATE TABLE [dbo].[Instalaciones] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [EmpresaId] int  NOT NULL,
    [TipoInstalacionId] int  NOT NULL,
    [Latitud] real  NULL,
    [Longitud] real  NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL
);
GO

-- Creating table 'Lesionados'
CREATE TABLE [dbo].[Lesionados] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL
);
GO

-- Creating table 'MediosAmenaza'
CREATE TABLE [dbo].[MediosAmenaza] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL
);
GO

-- Creating table 'Perfiles'
CREATE TABLE [dbo].[Perfiles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL
);
GO

-- Creating table 'Poligonos'
CREATE TABLE [dbo].[Poligonos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NivelGeograficoId] int  NOT NULL
);
GO

-- Creating table 'relPerfilesUsuarios'
CREATE TABLE [dbo].[relPerfilesUsuarios] (
    [UsuarioId] int  NOT NULL,
    [PerfilId] int  NOT NULL
);
GO

-- Creating table 'MotivosAmenaza'
CREATE TABLE [dbo].[MotivosAmenaza] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL
);
GO

-- Creating table 'TiposArma'
CREATE TABLE [dbo].[TiposArma] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL
);
GO

-- Creating table 'TiposExtorsion'
CREATE TABLE [dbo].[TiposExtorsion] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL
);
GO

-- Creating table 'TiposIncidente'
CREATE TABLE [dbo].[TiposIncidente] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [Descripcion] nvarchar(140)  NOT NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL,
    [Imagen] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'TiposInstalacion'
CREATE TABLE [dbo].[TiposInstalacion] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [Descripcion] nvarchar(140)  NOT NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL
);
GO

-- Creating table 'TiposIntrusion'
CREATE TABLE [dbo].[TiposIntrusion] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL
);
GO

-- Creating table 'TiposVehiculo'
CREATE TABLE [dbo].[TiposVehiculo] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL
);
GO

-- Creating table 'Usuarios'
CREATE TABLE [dbo].[Usuarios] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserName] nvarchar(12)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [ApellidoPaterno] nvarchar(50)  NOT NULL,
    [ApellidoMaterno] nvarchar(50)  NOT NULL,
    [Email] nvarchar(20)  NOT NULL,
    [Activo] bit  NOT NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL,
    [UsuarioBaja] int  NULL,
    [FechaBaja] datetime  NULL,
    [FechaUltimoLogin] datetime  NULL,
    [Bloqueado] bit  NULL
);
GO

-- Creating table 'Zonas'
CREATE TABLE [dbo].[Zonas] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [EstadoId] int  NOT NULL,
    [Descripcion] nvarchar(140)  NOT NULL,
    [PoligonoId] int  NOT NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL
);
GO

-- Creating table 'AlertasIncidente'
CREATE TABLE [dbo].[AlertasIncidente] (
    [EmpresaId] int  NOT NULL,
    [TipoIncidenteId] int  NOT NULL,
    [Emails] nvarchar(max)  NOT NULL,
    [RecibirOtrasEmpresas] bit  NOT NULL,
    [RecibirMiEmpresa] bit  NOT NULL
);
GO

-- Creating table 'ParametrosSistemaEmpresa'
CREATE TABLE [dbo].[ParametrosSistemaEmpresa] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmpresaId] int  NOT NULL,
    [TipoIncidenteId] int  NULL,
    [ValorInicial] int  NOT NULL,
    [Valorfinal] int  NULL,
    [UsuarioModificacion] int  NULL,
    [FechaUltimaModificacion] datetime  NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL,
    [ParametrosSistemaId] int  NOT NULL
);
GO

-- Creating table 'ConfiguracionesDashboard'
CREATE TABLE [dbo].[ConfiguracionesDashboard] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmpresaId] int  NOT NULL,
    [ReporteId] int  NOT NULL,
    [MontoAfectadoMinimo] float  NOT NULL,
    [MontoAfectadoMaximo] float  NOT NULL,
    [EsConsolidado] bit  NOT NULL,
    [SeIncluyeTabla] bit  NOT NULL,
    [EsReporteBase] bit  NOT NULL,
    [SegmentoReporteId] int  NOT NULL,
    [TipoUnidadTiempoId] int  NOT NULL,
    [ValorUnidadTiempo] int  NOT NULL,
    [ConfiguracionDashboardBaseId] int  NOT NULL,
    [UsuarioId] int  NOT NULL
);
GO

-- Creating table 'NivelesGeograficos'
CREATE TABLE [dbo].[NivelesGeograficos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'TiposUnidadTiempo'
CREATE TABLE [dbo].[TiposUnidadTiempo] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'ParametrosSistema'
CREATE TABLE [dbo].[ParametrosSistema] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [TipoDeParametro] int  NOT NULL,
    [TipoIncidenteEsRequerido] bit  NOT NULL,
    [ValorFinalEsRequerido] bit  NOT NULL
);
GO

-- Creating table 'FiltrosDashboard'
CREATE TABLE [dbo].[FiltrosDashboard] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FiltroDashboardId] int  NOT NULL,
    [TipoFiltroId] int  NOT NULL,
    [ValorFiltro] int  NOT NULL
);
GO

-- Creating table 'TiposFiltroDashboard'
CREATE TABLE [dbo].[TiposFiltroDashboard] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'PoligonosDetalle'
CREATE TABLE [dbo].[PoligonosDetalle] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PoligonoId] int  NOT NULL,
    [Latitud] float  NOT NULL,
    [Longitud] float  NOT NULL
);
GO

-- Creating table 'ReportesDashboard'
CREATE TABLE [dbo].[ReportesDashboard] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(120)  NOT NULL
);
GO

-- Creating table 'TiposAfectacion'
CREATE TABLE [dbo].[TiposAfectacion] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Nombre] nvarchar(50)  NOT NULL,
    [UsuarioAlta] int  NOT NULL,
    [FechaAlta] datetime  NOT NULL
);
GO

-- Creating table 'AfectacionesIncidente'
CREATE TABLE [dbo].[AfectacionesIncidente] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [IncidenteId] int  NOT NULL,
    [TipoAfectacionId] int  NOT NULL
);
GO

-- Creating table 'relUsuarioEmpresa'
CREATE TABLE [dbo].[relUsuarioEmpresa] (
    [UsuarioId] int  NOT NULL,
    [EmpresaId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'CantidadDelincuentes'
ALTER TABLE [dbo].[CantidadDelincuentes]
ADD CONSTRAINT [PK_CantidadDelincuentes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Estados'
ALTER TABLE [dbo].[Estados]
ADD CONSTRAINT [PK_Estados]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id], [EstadoId] in table 'Ciudades'
ALTER TABLE [dbo].[Ciudades]
ADD CONSTRAINT [PK_Ciudades]
    PRIMARY KEY CLUSTERED ([Id], [EstadoId] ASC);
GO

-- Creating primary key on [Id] in table 'Empresas'
ALTER TABLE [dbo].[Empresas]
ADD CONSTRAINT [PK_Empresas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Incidentes'
ALTER TABLE [dbo].[Incidentes]
ADD CONSTRAINT [PK_Incidentes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Instalaciones'
ALTER TABLE [dbo].[Instalaciones]
ADD CONSTRAINT [PK_Instalaciones]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Lesionados'
ALTER TABLE [dbo].[Lesionados]
ADD CONSTRAINT [PK_Lesionados]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MediosAmenaza'
ALTER TABLE [dbo].[MediosAmenaza]
ADD CONSTRAINT [PK_MediosAmenaza]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Perfiles'
ALTER TABLE [dbo].[Perfiles]
ADD CONSTRAINT [PK_Perfiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Poligonos'
ALTER TABLE [dbo].[Poligonos]
ADD CONSTRAINT [PK_Poligonos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UsuarioId], [PerfilId] in table 'relPerfilesUsuarios'
ALTER TABLE [dbo].[relPerfilesUsuarios]
ADD CONSTRAINT [PK_relPerfilesUsuarios]
    PRIMARY KEY NONCLUSTERED ([UsuarioId], [PerfilId] ASC);
GO

-- Creating primary key on [Id] in table 'MotivosAmenaza'
ALTER TABLE [dbo].[MotivosAmenaza]
ADD CONSTRAINT [PK_MotivosAmenaza]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TiposArma'
ALTER TABLE [dbo].[TiposArma]
ADD CONSTRAINT [PK_TiposArma]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TiposExtorsion'
ALTER TABLE [dbo].[TiposExtorsion]
ADD CONSTRAINT [PK_TiposExtorsion]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TiposIncidente'
ALTER TABLE [dbo].[TiposIncidente]
ADD CONSTRAINT [PK_TiposIncidente]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TiposInstalacion'
ALTER TABLE [dbo].[TiposInstalacion]
ADD CONSTRAINT [PK_TiposInstalacion]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TiposIntrusion'
ALTER TABLE [dbo].[TiposIntrusion]
ADD CONSTRAINT [PK_TiposIntrusion]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TiposVehiculo'
ALTER TABLE [dbo].[TiposVehiculo]
ADD CONSTRAINT [PK_TiposVehiculo]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Usuarios'
ALTER TABLE [dbo].[Usuarios]
ADD CONSTRAINT [PK_Usuarios]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Zonas'
ALTER TABLE [dbo].[Zonas]
ADD CONSTRAINT [PK_Zonas]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [EmpresaId], [TipoIncidenteId] in table 'AlertasIncidente'
ALTER TABLE [dbo].[AlertasIncidente]
ADD CONSTRAINT [PK_AlertasIncidente]
    PRIMARY KEY CLUSTERED ([EmpresaId], [TipoIncidenteId] ASC);
GO

-- Creating primary key on [Id] in table 'ParametrosSistemaEmpresa'
ALTER TABLE [dbo].[ParametrosSistemaEmpresa]
ADD CONSTRAINT [PK_ParametrosSistemaEmpresa]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ConfiguracionesDashboard'
ALTER TABLE [dbo].[ConfiguracionesDashboard]
ADD CONSTRAINT [PK_ConfiguracionesDashboard]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NivelesGeograficos'
ALTER TABLE [dbo].[NivelesGeograficos]
ADD CONSTRAINT [PK_NivelesGeograficos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TiposUnidadTiempo'
ALTER TABLE [dbo].[TiposUnidadTiempo]
ADD CONSTRAINT [PK_TiposUnidadTiempo]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ParametrosSistema'
ALTER TABLE [dbo].[ParametrosSistema]
ADD CONSTRAINT [PK_ParametrosSistema]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id], [FiltroDashboardId] in table 'FiltrosDashboard'
ALTER TABLE [dbo].[FiltrosDashboard]
ADD CONSTRAINT [PK_FiltrosDashboard]
    PRIMARY KEY CLUSTERED ([Id], [FiltroDashboardId] ASC);
GO

-- Creating primary key on [Id] in table 'TiposFiltroDashboard'
ALTER TABLE [dbo].[TiposFiltroDashboard]
ADD CONSTRAINT [PK_TiposFiltroDashboard]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PoligonosDetalle'
ALTER TABLE [dbo].[PoligonosDetalle]
ADD CONSTRAINT [PK_PoligonosDetalle]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReportesDashboard'
ALTER TABLE [dbo].[ReportesDashboard]
ADD CONSTRAINT [PK_ReportesDashboard]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TiposAfectacion'
ALTER TABLE [dbo].[TiposAfectacion]
ADD CONSTRAINT [PK_TiposAfectacion]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AfectacionesIncidente'
ALTER TABLE [dbo].[AfectacionesIncidente]
ADD CONSTRAINT [PK_AfectacionesIncidente]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [UsuarioId], [EmpresaId] in table 'relUsuarioEmpresa'
ALTER TABLE [dbo].[relUsuarioEmpresa]
ADD CONSTRAINT [PK_relUsuarioEmpresa]
    PRIMARY KEY NONCLUSTERED ([UsuarioId], [EmpresaId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [PerfilId] in table 'relPerfilesUsuarios'
ALTER TABLE [dbo].[relPerfilesUsuarios]
ADD CONSTRAINT [FK_PerfilrelPerfilUsuario]
    FOREIGN KEY ([PerfilId])
    REFERENCES [dbo].[Perfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PerfilrelPerfilUsuario'
CREATE INDEX [IX_FK_PerfilrelPerfilUsuario]
ON [dbo].[relPerfilesUsuarios]
    ([PerfilId]);
GO

-- Creating foreign key on [EmpresaId] in table 'Instalaciones'
ALTER TABLE [dbo].[Instalaciones]
ADD CONSTRAINT [FK_EmpresasInstalaciones]
    FOREIGN KEY ([EmpresaId])
    REFERENCES [dbo].[Empresas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EmpresasInstalaciones'
CREATE INDEX [IX_FK_EmpresasInstalaciones]
ON [dbo].[Instalaciones]
    ([EmpresaId]);
GO

-- Creating foreign key on [TipoInstalacionId] in table 'Instalaciones'
ALTER TABLE [dbo].[Instalaciones]
ADD CONSTRAINT [FK_TipoInstalacionInstalaciones]
    FOREIGN KEY ([TipoInstalacionId])
    REFERENCES [dbo].[TiposInstalacion]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoInstalacionInstalaciones'
CREATE INDEX [IX_FK_TipoInstalacionInstalaciones]
ON [dbo].[Instalaciones]
    ([TipoInstalacionId]);
GO

-- Creating foreign key on [EstadoId] in table 'Ciudades'
ALTER TABLE [dbo].[Ciudades]
ADD CONSTRAINT [FK_EstadosCiudades]
    FOREIGN KEY ([EstadoId])
    REFERENCES [dbo].[Estados]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EstadosCiudades'
CREATE INDEX [IX_FK_EstadosCiudades]
ON [dbo].[Ciudades]
    ([EstadoId]);
GO

-- Creating foreign key on [EstadoId] in table 'Zonas'
ALTER TABLE [dbo].[Zonas]
ADD CONSTRAINT [FK_EstadosZona]
    FOREIGN KEY ([EstadoId])
    REFERENCES [dbo].[Estados]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EstadosZona'
CREATE INDEX [IX_FK_EstadosZona]
ON [dbo].[Zonas]
    ([EstadoId]);
GO

-- Creating foreign key on [TipoArmaId] in table 'Incidentes'
ALTER TABLE [dbo].[Incidentes]
ADD CONSTRAINT [FK_TipoArmaIncidentes]
    FOREIGN KEY ([TipoArmaId])
    REFERENCES [dbo].[TiposArma]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoArmaIncidentes'
CREATE INDEX [IX_FK_TipoArmaIncidentes]
ON [dbo].[Incidentes]
    ([TipoArmaId]);
GO

-- Creating foreign key on [TipoExtorsionId] in table 'Incidentes'
ALTER TABLE [dbo].[Incidentes]
ADD CONSTRAINT [FK_TipoExtorcionIncidentes]
    FOREIGN KEY ([TipoExtorsionId])
    REFERENCES [dbo].[TiposExtorsion]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoExtorcionIncidentes'
CREATE INDEX [IX_FK_TipoExtorcionIncidentes]
ON [dbo].[Incidentes]
    ([TipoExtorsionId]);
GO

-- Creating foreign key on [CantidadDelincuentesId] in table 'Incidentes'
ALTER TABLE [dbo].[Incidentes]
ADD CONSTRAINT [FK_CantidadDelincuentesIncidentes]
    FOREIGN KEY ([CantidadDelincuentesId])
    REFERENCES [dbo].[CantidadDelincuentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CantidadDelincuentesIncidentes'
CREATE INDEX [IX_FK_CantidadDelincuentesIncidentes]
ON [dbo].[Incidentes]
    ([CantidadDelincuentesId]);
GO

-- Creating foreign key on [LesionadosId] in table 'Incidentes'
ALTER TABLE [dbo].[Incidentes]
ADD CONSTRAINT [FK_LesionadosIncidentes]
    FOREIGN KEY ([LesionadosId])
    REFERENCES [dbo].[Lesionados]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LesionadosIncidentes'
CREATE INDEX [IX_FK_LesionadosIncidentes]
ON [dbo].[Incidentes]
    ([LesionadosId]);
GO

-- Creating foreign key on [MedioAmenazaId] in table 'Incidentes'
ALTER TABLE [dbo].[Incidentes]
ADD CONSTRAINT [FK_MedioAmenazaIncidentes]
    FOREIGN KEY ([MedioAmenazaId])
    REFERENCES [dbo].[MediosAmenaza]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MedioAmenazaIncidentes'
CREATE INDEX [IX_FK_MedioAmenazaIncidentes]
ON [dbo].[Incidentes]
    ([MedioAmenazaId]);
GO

-- Creating foreign key on [TipoIntrusionId] in table 'Incidentes'
ALTER TABLE [dbo].[Incidentes]
ADD CONSTRAINT [FK_TipoIntrusionIncidentes]
    FOREIGN KEY ([TipoIntrusionId])
    REFERENCES [dbo].[TiposIntrusion]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoIntrusionIncidentes'
CREATE INDEX [IX_FK_TipoIntrusionIncidentes]
ON [dbo].[Incidentes]
    ([TipoIntrusionId]);
GO

-- Creating foreign key on [TipoVehiculoId] in table 'Incidentes'
ALTER TABLE [dbo].[Incidentes]
ADD CONSTRAINT [FK_TipoVehiculoIncidentes]
    FOREIGN KEY ([TipoVehiculoId])
    REFERENCES [dbo].[TiposVehiculo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoVehiculoIncidentes'
CREATE INDEX [IX_FK_TipoVehiculoIncidentes]
ON [dbo].[Incidentes]
    ([TipoVehiculoId]);
GO

-- Creating foreign key on [TipoIncidenteId] in table 'Incidentes'
ALTER TABLE [dbo].[Incidentes]
ADD CONSTRAINT [FK_TipoIncidenteIncidentes]
    FOREIGN KEY ([TipoIncidenteId])
    REFERENCES [dbo].[TiposIncidente]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoIncidenteIncidentes'
CREATE INDEX [IX_FK_TipoIncidenteIncidentes]
ON [dbo].[Incidentes]
    ([TipoIncidenteId]);
GO

-- Creating foreign key on [UsuarioAlta] in table 'Incidentes'
ALTER TABLE [dbo].[Incidentes]
ADD CONSTRAINT [FK_UsuarioIncidentes]
    FOREIGN KEY ([UsuarioAlta])
    REFERENCES [dbo].[Usuarios]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UsuarioIncidentes'
CREATE INDEX [IX_FK_UsuarioIncidentes]
ON [dbo].[Incidentes]
    ([UsuarioAlta]);
GO

-- Creating foreign key on [EmpresaId] in table 'Incidentes'
ALTER TABLE [dbo].[Incidentes]
ADD CONSTRAINT [FK_EmpresasIncidentes]
    FOREIGN KEY ([EmpresaId])
    REFERENCES [dbo].[Empresas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EmpresasIncidentes'
CREATE INDEX [IX_FK_EmpresasIncidentes]
ON [dbo].[Incidentes]
    ([EmpresaId]);
GO

-- Creating foreign key on [EstadoId] in table 'Incidentes'
ALTER TABLE [dbo].[Incidentes]
ADD CONSTRAINT [FK_EstadosIncidentes]
    FOREIGN KEY ([EstadoId])
    REFERENCES [dbo].[Estados]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EstadosIncidentes'
CREATE INDEX [IX_FK_EstadosIncidentes]
ON [dbo].[Incidentes]
    ([EstadoId]);
GO

-- Creating foreign key on [CiudadId], [EstadoId] in table 'Incidentes'
ALTER TABLE [dbo].[Incidentes]
ADD CONSTRAINT [FK_CiudadesIncidentes]
    FOREIGN KEY ([CiudadId], [EstadoId])
    REFERENCES [dbo].[Ciudades]
        ([Id], [EstadoId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CiudadesIncidentes'
CREATE INDEX [IX_FK_CiudadesIncidentes]
ON [dbo].[Incidentes]
    ([CiudadId], [EstadoId]);
GO

-- Creating foreign key on [ZonaId] in table 'Incidentes'
ALTER TABLE [dbo].[Incidentes]
ADD CONSTRAINT [FK_ZonaIncidentes]
    FOREIGN KEY ([ZonaId])
    REFERENCES [dbo].[Zonas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ZonaIncidentes'
CREATE INDEX [IX_FK_ZonaIncidentes]
ON [dbo].[Incidentes]
    ([ZonaId]);
GO

-- Creating foreign key on [Id] in table 'FiltrosDashboard'
ALTER TABLE [dbo].[FiltrosDashboard]
ADD CONSTRAINT [FK_ConfiguracionDashboardFiltrosDashboard]
    FOREIGN KEY ([Id])
    REFERENCES [dbo].[ConfiguracionesDashboard]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UsuarioId] in table 'relPerfilesUsuarios'
ALTER TABLE [dbo].[relPerfilesUsuarios]
ADD CONSTRAINT [FK_UsuarioUsuarioPerfil]
    FOREIGN KEY ([UsuarioId])
    REFERENCES [dbo].[Usuarios]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PoligonoId] in table 'PoligonosDetalle'
ALTER TABLE [dbo].[PoligonosDetalle]
ADD CONSTRAINT [FK_PoligonoPoligonoDetalle]
    FOREIGN KEY ([PoligonoId])
    REFERENCES [dbo].[Poligonos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PoligonoPoligonoDetalle'
CREATE INDEX [IX_FK_PoligonoPoligonoDetalle]
ON [dbo].[PoligonosDetalle]
    ([PoligonoId]);
GO

-- Creating foreign key on [NivelGeograficoId] in table 'Poligonos'
ALTER TABLE [dbo].[Poligonos]
ADD CONSTRAINT [FK_NivelGeograficoPoligono]
    FOREIGN KEY ([NivelGeograficoId])
    REFERENCES [dbo].[NivelesGeograficos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_NivelGeograficoPoligono'
CREATE INDEX [IX_FK_NivelGeograficoPoligono]
ON [dbo].[Poligonos]
    ([NivelGeograficoId]);
GO

-- Creating foreign key on [InstalacionId] in table 'Incidentes'
ALTER TABLE [dbo].[Incidentes]
ADD CONSTRAINT [FK_InstalacionesIncidentes]
    FOREIGN KEY ([InstalacionId])
    REFERENCES [dbo].[Instalaciones]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_InstalacionesIncidentes'
CREATE INDEX [IX_FK_InstalacionesIncidentes]
ON [dbo].[Incidentes]
    ([InstalacionId]);
GO

-- Creating foreign key on [PoligonoId] in table 'Zonas'
ALTER TABLE [dbo].[Zonas]
ADD CONSTRAINT [FK_ZonaPoligono]
    FOREIGN KEY ([PoligonoId])
    REFERENCES [dbo].[Poligonos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ZonaPoligono'
CREATE INDEX [IX_FK_ZonaPoligono]
ON [dbo].[Zonas]
    ([PoligonoId]);
GO

-- Creating foreign key on [PoligonoId] in table 'Estados'
ALTER TABLE [dbo].[Estados]
ADD CONSTRAINT [FK_EstadosPoligono]
    FOREIGN KEY ([PoligonoId])
    REFERENCES [dbo].[Poligonos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EstadosPoligono'
CREATE INDEX [IX_FK_EstadosPoligono]
ON [dbo].[Estados]
    ([PoligonoId]);
GO

-- Creating foreign key on [PoligonoId] in table 'Ciudades'
ALTER TABLE [dbo].[Ciudades]
ADD CONSTRAINT [FK_PoligonoCiudades]
    FOREIGN KEY ([PoligonoId])
    REFERENCES [dbo].[Poligonos]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PoligonoCiudades'
CREATE INDEX [IX_FK_PoligonoCiudades]
ON [dbo].[Ciudades]
    ([PoligonoId]);
GO

-- Creating foreign key on [TipoUnidadTiempoId] in table 'ConfiguracionesDashboard'
ALTER TABLE [dbo].[ConfiguracionesDashboard]
ADD CONSTRAINT [FK_TipoUnidadTiempoConfiguracionDashboard]
    FOREIGN KEY ([TipoUnidadTiempoId])
    REFERENCES [dbo].[TiposUnidadTiempo]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoUnidadTiempoConfiguracionDashboard'
CREATE INDEX [IX_FK_TipoUnidadTiempoConfiguracionDashboard]
ON [dbo].[ConfiguracionesDashboard]
    ([TipoUnidadTiempoId]);
GO

-- Creating foreign key on [TipoFiltroId] in table 'FiltrosDashboard'
ALTER TABLE [dbo].[FiltrosDashboard]
ADD CONSTRAINT [FK_TipoFiltroDashboardFiltrosDashboard]
    FOREIGN KEY ([TipoFiltroId])
    REFERENCES [dbo].[TiposFiltroDashboard]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoFiltroDashboardFiltrosDashboard'
CREATE INDEX [IX_FK_TipoFiltroDashboardFiltrosDashboard]
ON [dbo].[FiltrosDashboard]
    ([TipoFiltroId]);
GO

-- Creating foreign key on [ReporteId] in table 'ConfiguracionesDashboard'
ALTER TABLE [dbo].[ConfiguracionesDashboard]
ADD CONSTRAINT [FK_ReportesDashboardConfiguracionDashboard]
    FOREIGN KEY ([ReporteId])
    REFERENCES [dbo].[ReportesDashboard]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ReportesDashboardConfiguracionDashboard'
CREATE INDEX [IX_FK_ReportesDashboardConfiguracionDashboard]
ON [dbo].[ConfiguracionesDashboard]
    ([ReporteId]);
GO

-- Creating foreign key on [TipoIncidenteId] in table 'AlertasIncidente'
ALTER TABLE [dbo].[AlertasIncidente]
ADD CONSTRAINT [FK_TipoIncidenteAlertaIncidente]
    FOREIGN KEY ([TipoIncidenteId])
    REFERENCES [dbo].[TiposIncidente]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoIncidenteAlertaIncidente'
CREATE INDEX [IX_FK_TipoIncidenteAlertaIncidente]
ON [dbo].[AlertasIncidente]
    ([TipoIncidenteId]);
GO

-- Creating foreign key on [EmpresaId] in table 'AlertasIncidente'
ALTER TABLE [dbo].[AlertasIncidente]
ADD CONSTRAINT [FK_EmpresaAlertaIncidente]
    FOREIGN KEY ([EmpresaId])
    REFERENCES [dbo].[Empresas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ParametrosSistemaId] in table 'ParametrosSistemaEmpresa'
ALTER TABLE [dbo].[ParametrosSistemaEmpresa]
ADD CONSTRAINT [FK_ParametrosSistemaParametrosSistemaEmpresa]
    FOREIGN KEY ([ParametrosSistemaId])
    REFERENCES [dbo].[ParametrosSistema]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ParametrosSistemaParametrosSistemaEmpresa'
CREATE INDEX [IX_FK_ParametrosSistemaParametrosSistemaEmpresa]
ON [dbo].[ParametrosSistemaEmpresa]
    ([ParametrosSistemaId]);
GO

-- Creating foreign key on [EmpresaId] in table 'ParametrosSistemaEmpresa'
ALTER TABLE [dbo].[ParametrosSistemaEmpresa]
ADD CONSTRAINT [FK_EmpresaParametrosSistemaEmpresa]
    FOREIGN KEY ([EmpresaId])
    REFERENCES [dbo].[Empresas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EmpresaParametrosSistemaEmpresa'
CREATE INDEX [IX_FK_EmpresaParametrosSistemaEmpresa]
ON [dbo].[ParametrosSistemaEmpresa]
    ([EmpresaId]);
GO

-- Creating foreign key on [TipoAfectacionId] in table 'AfectacionesIncidente'
ALTER TABLE [dbo].[AfectacionesIncidente]
ADD CONSTRAINT [FK_TipoAfectacionAfectacionIncidente]
    FOREIGN KEY ([TipoAfectacionId])
    REFERENCES [dbo].[TiposAfectacion]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoAfectacionAfectacionIncidente'
CREATE INDEX [IX_FK_TipoAfectacionAfectacionIncidente]
ON [dbo].[AfectacionesIncidente]
    ([TipoAfectacionId]);
GO

-- Creating foreign key on [IncidenteId] in table 'AfectacionesIncidente'
ALTER TABLE [dbo].[AfectacionesIncidente]
ADD CONSTRAINT [FK_IncidenteAfectacionIncidente]
    FOREIGN KEY ([IncidenteId])
    REFERENCES [dbo].[Incidentes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_IncidenteAfectacionIncidente'
CREATE INDEX [IX_FK_IncidenteAfectacionIncidente]
ON [dbo].[AfectacionesIncidente]
    ([IncidenteId]);
GO

-- Creating foreign key on [TipoIncidenteId] in table 'ParametrosSistemaEmpresa'
ALTER TABLE [dbo].[ParametrosSistemaEmpresa]
ADD CONSTRAINT [FK_TipoIncidenteParametroSistemaEmpresa]
    FOREIGN KEY ([TipoIncidenteId])
    REFERENCES [dbo].[TiposIncidente]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoIncidenteParametroSistemaEmpresa'
CREATE INDEX [IX_FK_TipoIncidenteParametroSistemaEmpresa]
ON [dbo].[ParametrosSistemaEmpresa]
    ([TipoIncidenteId]);
GO

-- Creating foreign key on [UsuarioId] in table 'relUsuarioEmpresa'
ALTER TABLE [dbo].[relUsuarioEmpresa]
ADD CONSTRAINT [FK_UsuarioEmpresaUsuario]
    FOREIGN KEY ([UsuarioId])
    REFERENCES [dbo].[Usuarios]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [EmpresaId] in table 'relUsuarioEmpresa'
ALTER TABLE [dbo].[relUsuarioEmpresa]
ADD CONSTRAINT [FK_UsuarioEmpresaEmpresa]
    FOREIGN KEY ([EmpresaId])
    REFERENCES [dbo].[Empresas]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UsuarioEmpresaEmpresa'
CREATE INDEX [IX_FK_UsuarioEmpresaEmpresa]
ON [dbo].[relUsuarioEmpresa]
    ([EmpresaId]);
GO

-- Creating foreign key on [MotivoAmenazaId] in table 'Incidentes'
ALTER TABLE [dbo].[Incidentes]
ADD CONSTRAINT [FK_MotivoAmenazaIncidente]
    FOREIGN KEY ([MotivoAmenazaId])
    REFERENCES [dbo].[MotivosAmenaza]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MotivoAmenazaIncidente'
CREATE INDEX [IX_FK_MotivoAmenazaIncidente]
ON [dbo].[Incidentes]
    ([MotivoAmenazaId]);
GO

-- Creating foreign key on [TipoInstalacionId] in table 'Incidentes'
ALTER TABLE [dbo].[Incidentes]
ADD CONSTRAINT [FK_TipoInstalacionIncidente]
    FOREIGN KEY ([TipoInstalacionId])
    REFERENCES [dbo].[TiposInstalacion]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TipoInstalacionIncidente'
CREATE INDEX [IX_FK_TipoInstalacionIncidente]
ON [dbo].[Incidentes]
    ([TipoInstalacionId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------