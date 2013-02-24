using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaSeguridad.Models
{
    public class FiltrosUsuarioModel
    {
        public int EmpresaId { get; set; }
        public int PerfilId { get; set; }
        public string UserName { get; set; }
    }
}