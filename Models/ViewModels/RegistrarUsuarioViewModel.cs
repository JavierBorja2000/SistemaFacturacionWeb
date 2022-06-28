using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SistemaFacturacionWeb.Models.ViewModels
{
    public class RegistrarUsuarioViewModel
    {
        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres", MinimumLength = 5)]
        [Display(Name = "Email")]
        [Remote("EmailExiste", "Access", ErrorMessage = "Email se encuentra en uso")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El password es obligatorio")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres", MinimumLength = 6)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Confirmar contraseña")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no son iguales")]
        public string? ConfirmPassword { get; set; }
    }
    
    public class EditarUsuarioViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres", MinimumLength = 5)]
        [Display(Name = "Email")]
        [Remote("EmailExiste", "Usuario",AdditionalFields = "Id", ErrorMessage = "Email se encuentra en uso")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El password es obligatorio")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres", MinimumLength = 6)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Confirmar contraseña")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no son iguales")]
        public string? ConfirmPassword { get; set; }
    }
}
