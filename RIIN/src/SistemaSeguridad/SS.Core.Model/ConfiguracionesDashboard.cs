//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SS.Core.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ConfiguracionesDashboard
    {
        public ConfiguracionesDashboard()
        {
            this.FiltrosDashboard = new HashSet<FiltrosDashboard>();
        }
    
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public int ReporteId { get; set; }
        public double MontoAfectadoMinimo { get; set; }
        public double MontoAfectadoMaximo { get; set; }
        public bool EsConsolidado { get; set; }
        public bool SeIncluyeTabla { get; set; }
        public bool EsReporteBase { get; set; }
        public int SegmentoReporteId { get; set; }
        public int TipoUnidadTiempoId { get; set; }
        public int ValorUnidadTiempo { get; set; }
        public int ConfiguracionDashboardBaseId { get; set; }
        public int UsuarioId { get; set; }
    
        public virtual ICollection<FiltrosDashboard> FiltrosDashboard { get; set; }
        public virtual ReportesDashboard ReportesDashboard { get; set; }
        public virtual TiposUnidadTiempo TiposUnidadTiempo { get; set; }
    }
}