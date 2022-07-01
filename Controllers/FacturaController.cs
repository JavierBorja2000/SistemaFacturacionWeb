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
            //factura.Total_factura = 164;
            //factura.Numero_factura = id;
            //factura.Codigo_cliente = 2;

            //_context.Update(factura);
            //_context.SaveChanges();

            //producto1.Cantidad = 12;

            //var producto4 = new ProductoFactura
            //{
            //    Codigo_producto = 4,
            //    Cantidad = 0,
            //    Precio = 20
            //};

            //listado = new List<ProductoFactura>();
            //listado.Add(producto1);
            //listado.Add(producto2);
            //listado.Add(producto3);
            //listado.Add(producto4);

            //var listadoAntiguo = _context.Detalle_Facturas.ToList();

            //foreach (var producto in listado)
            //{

            //    if(listadoAntiguo.Count(x => x.Codigo_producto == producto.Codigo_producto && x.Numero_factura == id) > 0
            //        || producto.Cantidad > 0){
            //        _context.Database.ExecuteSqlRaw($"sp_EditarFactura {id}, {producto.Codigo_producto}, {producto.Cantidad}, {producto.Precio}");
            //    }

            //}

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
            if (codigo == null)
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
            List<Producto> productosFiltrados = productos.Where(p => p.Existencia > 0 && p.Estado == 'A').ToList();

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

        [HttpPost]
        public IActionResult AgregarProductos(VerDetallesViewModel modelo)
        {
            modelo.Fecha = DateTime.Now;
            modelo.Anulada = 'N';
            modelo.Total_factura = 0;

            foreach (var producto in modelo.Productos)
            {
                modelo.Total_factura += (producto.Cantidad * producto.Precio);
            }

            if (modelo.Total_factura == 0)
            {
                return RedirectToAction("AgregarProductos", new { codigo = modelo.Codigo_cliente });
            }

            Factura factura = new Factura
            {
                Anulada = modelo.Anulada,
                Fecha = modelo.Fecha,
                Total_factura = (float)modelo.Total_factura,
                Codigo_cliente = (int)modelo.Codigo_cliente
            };

            _context.Add(factura);
            _context.SaveChanges();

            var facturas = _context.Facturas.ToList();
            int id = facturas.Last().Numero_factura;

            foreach (var producto in modelo.Productos)
            {
                if (producto.Cantidad > 0)
                {
                    _context.Database.ExecuteSqlRaw($"sp_CrearFactura {id}, {producto.Codigo_producto}, {producto.Cantidad}, {producto.Precio}");
                }
            }

            return Redirect("Index");
        }

        [AllowAnonymous]
        [HttpPost, HttpGet]
        public JsonResult CantidadValida(int cantidad, int numero_factura, int codigo_producto)
        {
            var producto = _context.Productos.Find(codigo_producto);
            if (numero_factura == 0)
            {
                if (cantidad > producto.Existencia)
                {
                    return Json(false);
                }
                return Json(true);
            }
            else
            {
                var cantidadAntigua = _context.Detalle_Facturas.Find(numero_factura, codigo_producto).Cantidad;

                if (cantidadAntigua >= cantidad)
                {
                    return Json(true);
                }
                else
                {
                    int diferencia = cantidad - cantidadAntigua;

                    if (diferencia > producto.Existencia)
                    {
                        return Json(false);
                    }
                    return Json(true);
                }
            }
        }

        [HttpGet]
        public IActionResult Editar(int Numero_factura)
        {
            var factura = _context.Facturas.Find(Numero_factura);

            if (Numero_factura == null || factura == null)
            {
                TempData["ErrorTitle"] = "Error !";
                TempData["ErrorDescription"] = "No se encontro la factura";
                TempData["ErrorCode"] = "404";
                return RedirectToAction("ErrorPage", "Home");
            }

            var cliente = _context.Clientes.Find(factura.Codigo_cliente);

            var modelo = new VerDetallesViewModel
            {
                Codigo_cliente = factura.Codigo_cliente,
                Numero_factura = factura.Numero_factura,
                Fecha = factura.Fecha,
                Anulada = factura.Anulada,
                Cliente = cliente
            };

            var productos = _context.Productos.ToList();

            var productosAgregados = (from detalle in _context.Detalle_Facturas
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

            //Filtro los productos para que solo obtener lo que si cuenten con existencias disponibles
            List<Producto> productosFiltrados = productos.Where(p => (p.Estado == 'A' && p.Existencia > 0)
                                                                || (productosAgregados.Find(x => x.Codigo_producto == p.Codigo_producto) != null)).ToList();

            List<ProductoFactura> temp = new List<ProductoFactura>();
            foreach (var producto in productosFiltrados)
            {
                var productoAgregado = productosAgregados.Find(x => x.Codigo_producto == producto.Codigo_producto);
                int cantidadInicial = 0;
                float? precio = producto.Precio;

                if (productoAgregado != null)
                {
                    producto.Existencia = producto.Existencia + productoAgregado.Cantidad;
                    cantidadInicial = productoAgregado.Cantidad;
                    precio = productoAgregado.Precio;
                }

                temp.Add(new ProductoFactura
                {
                    Codigo_producto = producto.Codigo_producto,
                    Nombre = producto.Nombre,
                    Descripcion = producto.Descripcion,
                    Precio = precio,
                    Cantidad = cantidadInicial,
                    Existencias = (int?)producto.Existencia
                });
            }

            modelo.Productos = temp;
            modelo.listClientes = _context.Clientes.ToList();

            return View(modelo);
        }

    }


}
