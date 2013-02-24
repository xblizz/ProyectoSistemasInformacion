using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SistemaSeguridad.Models
{
    internal interface ICoordList
    {
        /// <summary>
        /// A list of coordenates (Lat and Long) separated by commas
        /// </summary>
        string CoordsList { get; set; }

        int? PoligonoId { get; set; }
    }
}