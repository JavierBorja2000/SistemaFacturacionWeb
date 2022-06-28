using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaFacturacionWeb.Models
{
    public class Detalle_Factura
    {
        public int Numero_factura { get; set; }
        public int Codigo_producto { get; set; }
        public int Cantidad { get; set; }
        public double Total_factura { get; set; }

        [ForeignKey("Codigo_producto")]
        public virtual Producto? Producto { get; set; } = null!;

        [ForeignKey("Numero_factura")]
        public virtual Factura? Factura { get; set; } = null!;
    }
}
