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

        public IActionResult Anular(int Numero_factura)
        {
            var factura = _context.Facturas.Find(Numero_factura);

            if (factura == null)
            {
                TempData["ErrorDescription"] = "El elemento no fue encontrado";
                TempData["ErrorCode"] = 404;
                return RedirectToAction("ErrorPage", "Home");
            }

            if (factura.Anulada == 'A')
            {
                TempData["ErrorTitle"] = "Error";
                TempData["ErrorDescription"] = "la factura ya fue anulada";
                TempData["ErrorCode"] = 403;
                return RedirectToAction("ErrorPage", "Home");
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
                                  where detalle.Numero_factura == Numero_factura
                                  select new ProductoFactura
                                  {
                                      Codigo_producto = producto.Codigo_producto,
                                      Nombre = producto.Nombre,
                                      Descripcion = producto.Descripcion,
                                      Precio = detalle.Precio,
                                      Cantidad = detalle.Cantidad

                                  }).ToList();

            return View(detalles);
        }

        [HttpPost]
        public IActionResult Anular(VerDetallesViewModel modelo)
        {
            var factura = _context.Facturas.Find(modelo.Numero_factura);

            if(factura == null)
            {
                TempData["ErrorDescription"] = "El elemento no fue encontrado";
                TempData["ErrorCode"] = 404;
                return RedirectToAction("ErrorPage", "Home");
            }

            if(factura.Anulada == 'A')
            {
                TempData["ErrorTitle"] = "Error";
                TempData["ErrorDescription"] = "la factura ya fue anulada";
                TempData["ErrorCode"] = 403;
                return RedirectToAction("ErrorPage", "Home");
            }

            var i = _context.Database.ExecuteSqlRaw($"sp_AnularFactura {modelo.Numero_factura}");

            return Redirect("Index");
        }

        [HttpGet]
        public IActionResult SelectCliente()
        {
            IEnumerable<Cliente> listaClientes = _context.Clientes;
            return View(listaClientes);
        }

        
        [HttpGet]
        public IActionResult AgregarProductos(int? codigo)
        {
            if(codigo == null)
            {
                TempData["ErrorTitle"] = "Error !";
                TempData["ErrorDescription"] = "No se encontro el cliente";
                TempData["ErrorCode"] = "404";
                return RedirectToAction("ErrorPage", "Home");
            }

            var cliente = _context.Clientes.Find(codigo);


            var modelo = new VerDetallesViewModel
            {
                Codigo_cliente = codigo,
                Cliente = cliente
            };


            var productos = _context.Productos.ToList();

            //Filtro los productos para que solo obtener lo que si cuenten con existencias disponibles
            List<Producto> productosFiltrados = productos.Where(p => p.Existencia > 0).ToList();

            List<ProductoFactura> emp = new List<ProductoFactura>();
            foreach (var producto in productosFiltrados)
            {
                emp.Add(new ProductoFactura
                {
                    Codigo_producto = producto.Codigo_producto,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = producto.Precio,
                    Cantidad = 0,
                    Existencias = (int?)producto.Existencia
                }); 

            }

            modelo.Productos = emp;
            
            return View(modelo);
        }
    }
}
