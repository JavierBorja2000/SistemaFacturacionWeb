using SistemaFacturacionWeb.Models.ViewModels;
namespace SistemaFacturacionWeb.Models
{
    public class Reporte_factura
    {
        public ReporteViewModel ReporteViewModel { get; set; }
        public List<ReporteFacturaViewModel>? Resultados { get; set; }
    }
}
