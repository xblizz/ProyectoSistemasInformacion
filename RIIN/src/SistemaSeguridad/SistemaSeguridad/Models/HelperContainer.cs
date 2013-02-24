using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SistemaSeguridad.Models
{
    public class HelperContainer:DbContext
    {
        public HelperContainer()
            : base("name=HelperContainer")
        {
        }
        public DbSet<LoginModel> Login { get; set; }
        public DbSet<UsuarioModel> Usuario { get; set; }
    }
}