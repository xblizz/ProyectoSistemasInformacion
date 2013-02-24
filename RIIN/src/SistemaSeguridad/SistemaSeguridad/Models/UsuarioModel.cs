
using System.ComponentModel.DataAnnotations;
using SS.Core.DataAnnotations.Extensions;

namespace SistemaSeguridad.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }


        [Required]
        public string Nombre { get; set; }

        [Required]
        public string ApellidoPaterno { get; set; }

        [Required]
        public string ApellidoMaterno { get; set; }

        [Required]
        [Email]
        public string Email { get; set; }

        [Required]
        public bool Activo { get; set; }
    }
}