using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SistemaFacturacionWeb.DB;
using SistemaFacturacionWeb.Models;
using SistemaFacturacionWeb.Models.ViewModels;
using System.Data;

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
            Factura factura = new Factura();
            factura.Codigo_cliente = 1;
            factura.Fecha = DateTime.Now;
            factura.Total_factura = 104;
            factura.Anulada = 'N';

            _context.Add(factura);
            _context.SaveChanges();

            var facturas = _context.Facturas.ToList();
            int id = facturas.Last().Numero_factura;

            List<ProductoFactura> listado = new List<ProductoFactura>();

            var producto1 = new ProductoFactura{
                Codigo_producto = 1,
                Cantidad = 6,
                Precio = 10
            };
            var producto2 = new ProductoFactura{
                Codigo_producto = 2,
                Cantidad = 8,
                Precio = 5
            };
            var producto3 = new ProductoFactura
            {
                Codigo_producto = 3,
                Cantidad = 2,
                Precio = 2
            };

            listado.Add(producto1);
            listado.Add(producto2);
            listado.Add(producto3);

            var productos = _context.Productos.ToList();

            foreach (var producto in listado)
            {
                _context.Database.ExecuteSqlRaw($"sp_CrearFactura {id}, {producto.Codigo_producto}, {producto.Cantidad}, {producto.Precio}");
            }

            factura.Total_factura = 164;
            factura.Numero_factura = id;
            factura.Codigo_cliente = 2;

            _context.Update(factura);
            _context.SaveChanges();

            producto1.Cantidad = 12;

            var producto4 = new ProductoFactura
            {
                Codigo_producto = 4,
                Cantidad = 0,
                Precio = 20
            };

            listado = new List<ProductoFactura>();
            listado.Add(producto1);
            listado.Add(producto2);
            listado.Add(producto3);
            listado.Add(producto4);

            var listadoAntiguo = _context.Detalle_Facturas.ToList();

            foreach (var producto in listado)
            {
                
                if(listadoAntiguo.Count(x => x.Codigo_producto == producto.Codigo_producto && x.Numero_factura == id) > 0
                    || producto.Cantidad > 0){
                    _context.Database.ExecuteSqlRaw($"sp_EditarFactura {id}, {producto.Codigo_producto}, {producto.Cantidad}, {producto.Precio}");
                }
                
            }

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

        [AllowAnonymous]
        [HttpPost, HttpGet]
        public JsonResult CantidadValida(int cantidad, int numero_factura, int codigo_producto)
        {
            var producto = _context.Productos.Find(codigo_producto);
            if(numero_factura == null)
            {
                if(cantidad > producto.Existencia)
                {
                    return Json(false);
                }
                return Json(true);
            }
            else
            {
                var cantidadAntigua = _context.Detalle_Facturas.Find(numero_factura, codigo_producto).Cantidad;

                if(cantidadAntigua >= cantidad)
                {
                    return Json(true);
                }
                else
                {
                    int diferencia = cantidad - cantidadAntigua;

                    if(diferencia > producto.Existencia)
                    {
                        return Json(false);
                    }
                    return Json(true);
                }
            }
        }
    }
}
