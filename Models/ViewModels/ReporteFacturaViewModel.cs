using System.ComponentModel.DataAnnotations;
namespace SistemaFacturacionWeb.Models.ViewModels
{
    public class ReporteFacturaViewModel
    {
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Display(Name = "Cantidad de Facturas")]
        public int Cantidad_facturas { get; set; }

        [Display(Name = "Total Facturado")]
        public float? Total_facturado { get; set; }

        [Display(Name = "Cantidad de Productos")]
        public int Cantidad_productos { get; set; }
    }
}
