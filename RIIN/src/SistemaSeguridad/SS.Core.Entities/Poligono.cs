//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace SS.Core.Entities
{
    public partial class Poligono
    {
        public Poligono()
        {
            this.PoligonoDetalles = new HashSet<PoligonoDetalle>();
            this.Zona = new HashSet<Zona>();
            this.Estado = new HashSet<Estado>();
            this.Ciudades = new HashSet<Ciudad>();
        }
    
        public int Id { get; set; }
        public int NivelGeograficoId { get; set; }
    
        public virtual ICollection<PoligonoDetalle> PoligonoDetalles { get; set; }
        public virtual NivelGeografico NivelGeografico { get; set; }
        public virtual ICollection<Zona> Zona { get; set; }
        public virtual ICollection<Estado> Estado { get; set; }
        public virtual ICollection<Ciudad> Ciudades { get; set; }
    }
    
}
