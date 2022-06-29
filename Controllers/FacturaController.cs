using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaFacturacionWeb.DB;
using SistemaFacturacionWeb.Models;
using SistemaFacturacionWeb.Models.ViewModels;

namespace SistemaFacturacionWeb.Controllers
{
    public class FacturaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacturaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Factura> listaFacturas = _context.Facturas.Include(f => f.Cliente);
            return View(listaFacturas);
        }

        public IActionResult VerDetalles(int Numero_factura)
        {
            var factura = _context.Facturas.Find(Numero_factura);

            if (factura == null)
            {
                TempData["ErrorDescription"] = "El elemento no fue encontrado";
                TempData["ErrorCode"] = 404;
                return RedirectToAction("ErrorPage","Home");
            }

            factura.Cliente = _context.Clientes.Find(factura.Codigo_cliente);

            var detalles = new VerDetallesViewModel
            {
                Numero_factura = factura.Numero_factura,
                Fecha = factura.Fecha,
                Total_factura = factura.Total_factura,
                Anulada = factura.Anulada,
                Cliente = factura.Cliente
            };

            detalles.Productos = (from detalle in _context.Detalle_Facturas 
                             join producto in _context.Productos on detalle.Codigo_producto equals producto.Codigo_producto
                             where detalle.Numero_factura == Numero_factura select new ProductoFactura
                             {
                                 Codigo_producto = producto.Codigo_producto,
                                 Nombre = producto.Nombre,
                                 Descripcion = producto.Descripcion,
                                 Precio = detalle.Precio,
                                 Cantidad = detalle.Cantidad
                            
                             }).ToList();

            return View(detalles);
        }
    }
}
