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
    
    public partial class TiposInstalacion
    {
        public TiposInstalacion()
        {
            this.Incidentes = new HashSet<Incidentes>();
            this.Instalaciones = new HashSet<Instalaciones>();
        }
    
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int UsuarioAlta { get; set; }
        public System.DateTime FechaAlta { get; set; }
    
        public virtual ICollection<Incidentes> Incidentes { get; set; }
        public virtual ICollection<Instalaciones> Instalaciones { get; set; }
    }
}