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
    
    public partial class ParametrosSistemaEmpresa
    {
        public int Id { get; set; }
        public int EmpresaId { get; set; }
        public Nullable<int> TipoIncidenteId { get; set; }
        public int ValorInicial { get; set; }
        public Nullable<int> Valorfinal { get; set; }
        public Nullable<int> UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaUltimaModificacion { get; set; }
        public int UsuarioAlta { get; set; }
        public System.DateTime FechaAlta { get; set; }
        public int ParametrosSistemaId { get; set; }
    
        public virtual Empresas Empresas { get; set; }
        public virtual ParametrosSistema ParametrosSistema { get; set; }
        public virtual TiposIncidente TiposIncidente { get; set; }
    }
}
