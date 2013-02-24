using System.ComponentModel.DataAnnotations;
using SS.Core.DataAnnotations.Extensions;

namespace SistemaSeguridad.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

    }
    
    public class  RetrivePasswordModel
    {
        public string Username { get; set; }
        [Email]
        public string Email { get; set; }
    }

    public class PasswordModel
    {
        [Required]
        [StrongPassword]
        [Display(Name = "Password actual")]
        public string OldPassword { get; set; }

        [Required]
        [StrongPassword]
        [Display(Name = "Nuevo password")]
        public string NewPassword { get; set; }

        [Required]
        [StrongPassword]
        [EqualsTo("NewPassword")]
        [Display(Name = "Confirmar password")]
        public string ConfirmPassword { get; set; }
    }
}