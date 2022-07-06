using System.ComponentModel.DataAnnotations;
namespace SistemaFacturacionWeb.Models.ViewModels
{
    public class ReporteClienteViewModel
    {
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        public string? Nombre { get; set; }

        public string? Apellido { get; set; }

        public float? Total { get; set; }
    }
}
