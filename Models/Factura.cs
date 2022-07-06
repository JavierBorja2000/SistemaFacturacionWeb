using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaFacturacionWeb.Models
{
    public class Factura
    {
        [Key]
        [Display(Name = "Numero de Factura")]
        public int Numero_factura { get; set; }

        [Required(ErrorMessage = "El campo fecha no puede ir vacio")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Display(Name = "Total")]
        public float Total_factura { get; set; }

        [Display(Name = "Anulada")]
        public char Anulada { get; set; } // A - Anulada N - No Anulada


        [Display(Name = "Cliente")]
        public int Codigo_cliente { get; set; }

        [ForeignKey("Codigo_cliente")]
        public virtual Cliente? Cliente { get; set; }
    }
}
