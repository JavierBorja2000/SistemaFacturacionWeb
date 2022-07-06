using System.ComponentModel.DataAnnotations;

namespace SistemaFacturacionWeb.Models
{
    public class Cliente
    {
        [Key]
        public int Codigo_cliente { get; set; }

        [Required(ErrorMessage = "El campo nombres no puede ir vacio")]
        [Display(Name = "Nombres")]

        public string Nombres { get; set; }

        [Required(ErrorMessage = "El campo apellidos no puede ir vacio")]
        [Display(Name = "Apellidos")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El campo Nit no puede ir vacio")]
        [Display(Name = "Nit")]

        public string Nit { get; set; }

        [Required(ErrorMessage = "El campo telefono no puede ir vacio")]
        [Display(Name = "Telefono")]

        public string Telefono { get; set; }

        [Required(ErrorMessage = "El campo Estado no puede ir vacio")]
        [Display(Name = "Estado")]
        public char Estado { get; set; } // A - Activo I - Inactivo

    }
}
