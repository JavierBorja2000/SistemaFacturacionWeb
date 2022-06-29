using System.ComponentModel.DataAnnotations;

namespace SistemaFacturacionWeb.Models
{
    public class Producto
    {
        [Key]
        public int Codigo_producto { get; set; }

        [Required(ErrorMessage = "Este campo no puede estar vacío")]
        [Display(Name = "Nombre")]
        public string? Nombre{ get; set; }

        [Display(Name = "Descripcion")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "Este campo no puede estar vacío")]
        [Display(Name = "Precio")]
        public float? Precio { get; set; }

        [Required(ErrorMessage = "Este campo no puede estar vacío")]
        [Display(Name = "Costo")]
        public float? Costo { get; set; }

        [Required(ErrorMessage = "Este campo no puede estar vacío")]
        [Display(Name = "Existencia")]
        public float? Existencia { get; set; }

        [Required(ErrorMessage = "Este campo no puede estar vacío")]
        [Display(Name = "Estado")]
        public char? Estado { get; set; } // A - Activo I - Inactivo
    }
}
