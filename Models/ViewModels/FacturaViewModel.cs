using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaFacturacionWeb.Models.ViewModels
{
    public class VerDetallesViewModel
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
        public int? Codigo_cliente { get; set; }

        [ForeignKey("Codigo_cliente")]
        public virtual Cliente? Cliente { get; set; }

        public IEnumerable<ProductoFactura> Productos { get; set; }
    }

    public class ProductoFactura {
        [Key]
        public int Codigo_producto { get; set; }

        [Required(ErrorMessage = "Este campo no puede estar vacío")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Este campo no puede estar vacío")]
        [Display(Name = "Precio")]
        public float? Precio { get; set; }

        public int Cantidad { get; set; }

        public int? Existencias { get; set; }
    }

}
