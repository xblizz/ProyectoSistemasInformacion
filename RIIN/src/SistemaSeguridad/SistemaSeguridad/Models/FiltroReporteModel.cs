using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SS.Core.Entities;

namespace SistemaSeguridad.Models
{
    public class FiltroReporteModel
    {
        public int? EstadoId { get; set; }
        public int? CiudadId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int? ZonaId { get; set; }
        public int? TipoIncidenteId { get; set; }
    }
}