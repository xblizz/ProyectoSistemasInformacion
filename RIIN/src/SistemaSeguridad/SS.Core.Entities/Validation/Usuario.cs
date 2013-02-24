using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SS.Core.DataAnnotations.Extensions;

namespace SS.Core.Entities
{
    [MetadataType(typeof(ValidacionUsuario))]
    public partial class Usuario
    {
        public virtual ICollection<int> PerfilId { get; set; }
        public virtual ICollection<int> EmpresaId { get; set; }
    }

    public class ValidacionUsuario
    {
        [Max(12)]
        [Requerido]
        //[Remote("ExistUser","Usuario")]
        public string UserName { get; set; }

        
        [Min(6)]
        //[StrongPassword]
        [Requerido]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Requerido]
        [Max(50)]
        public string Nombre { get; set; }

        [Max(50)]
        [Requerido]
        public string ApellidoPaterno { get; set; }

        [Max(50)]
        public string ApellidoMaterno { get; set; }

        [Max(120)]
        [Requerido]
        [Email]
        public string Email { get; set; }

       
    }
}
