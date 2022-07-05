using SistemaFacturacionWeb.Models.ViewModels;
namespace SistemaFacturacionWeb.Models
{
    public class Reporte_producto
    {
        public ReporteViewModel ReporteViewModel{ get; set; }
        public List<ReporteProductoViewModel1>? Resultados { get; set; }
    }
}
