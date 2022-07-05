using System.ComponentModel.DataAnnotations;
namespace SistemaFacturacionWeb.Models.ViewModels
{
    public class ReporteFacturaViewModel
    {
        [DataType(DataType.Date)]
        public DateTime? Fecha { get; set; }

        public int Cantidad_facturas { get; set; }

        public float? Total_facturado { get; set; }

        public int Cantidad_productos { get; set; }
    }
}
