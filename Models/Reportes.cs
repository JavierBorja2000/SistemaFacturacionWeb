using System.ComponentModel.DataAnnotations;

namespace SistemaFacturacionWeb.Models
{
    public class Reportes
    {

        [Display(Name = "Fecha Inicial")]
        [DataType(DataType.Date)]
        public DateTime? Fecha1 { get; set; }

        [Required(ErrorMessage = "El campo fecha Final no puede ir vacio")]
        [Display(Name = "Fecha Final")]
        [DataType(DataType.Date)]
        public DateTime? Fecha2 { get; set; }

        [Display(Name = "Nombre")]
        public string? Nombre { get; set; }

    }
}
