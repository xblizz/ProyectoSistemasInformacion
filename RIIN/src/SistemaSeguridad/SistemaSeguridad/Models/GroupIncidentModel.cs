using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaSeguridad.Models
{
    public class GroupIncidentModel
    {
        public string Zona { get; set; }
        public string NombreTipoIncidente { get; set; }
        public string NumeroEventos { get; set; }
        public Color Semaforo { get; set; }
    }

    public enum Color
    {
        Rojo = 0,
        Azul = 1,
        Amarillo = 2
    }
}