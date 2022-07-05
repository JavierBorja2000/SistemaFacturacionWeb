using System.ComponentModel.DataAnnotations;
namespace SistemaFacturacionWeb.Models.ViewModels
{
    public class ReporteProductoViewModel1
    {



        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        public int Codigo_producto { get; set; }

        public string Nombre_producto { get; set; }

        public float? Total { get; set; }

    }
}
