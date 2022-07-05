using SistemaFacturacionWeb.Models.ViewModels;
namespace SistemaFacturacionWeb.Models
{
    public class Reporte_cliente
    {
        public Reporte2ViewModel Reporte2ViewModel { get; set; }
        public List<ReporteClienteViewModel>? ResultadosClientes { get; set; }
    }
}
