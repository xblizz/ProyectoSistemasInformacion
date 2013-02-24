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
    
    public partial class Poligonos
    {
        public Poligonos()
        {
            this.Ciudades = new HashSet<Ciudades>();
            this.Estados = new HashSet<Estados>();
            this.PoligonosDetalle = new HashSet<PoligonosDetalle>();
            this.Zonas = new HashSet<Zonas>();
        }
    
        public int Id { get; set; }
        public int NivelGeograficoId { get; set; }
    
        public virtual ICollection<Ciudades> Ciudades { get; set; }
        public virtual ICollection<Estados> Estados { get; set; }
        public virtual NivelesGeograficos NivelesGeograficos { get; set; }
        public virtual ICollection<PoligonosDetalle> PoligonosDetalle { get; set; }
        public virtual ICollection<Zonas> Zonas { get; set; }
    }
}