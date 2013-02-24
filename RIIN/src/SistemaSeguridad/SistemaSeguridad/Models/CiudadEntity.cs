using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SS.Core.Entities;

namespace SistemaSeguridad.Models
{
    public class CiudadEntity : Ciudad, ICoordList
    {
        public string CoordsList { get; set; }
        public string geocoordinates { get; set; }

        int? ICoordList.PoligonoId
        {
            get
            {
                return base.PoligonoId;
            }
            set
            {
                base.PoligonoId = value;
            }
        }
    }
}