using Microsoft.AspNetCore.Mvc;
using SistemaFacturacionWeb.DB;
using SistemaFacturacionWeb.Models;

namespace SistemaFacturacionWeb.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Producto> listaProductos = _context.Productos;
            return View(listaProductos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Productos.Add(producto);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return View();
        }

        [HttpGet]
        public IActionResult Editar(int? Codigo_producto)
        {
            var producto = _context.Productos.Find(Codigo_producto);

            if (Codigo_producto.HasValue == false)
            {
                TempData["ErrorTitle"] = "Error !";
                TempData["ErrorDescription"] = "No se encontro el producto";
                TempData["ErrorCode"] = "404";
                return RedirectToAction("ErrorPage", "Home");
            }
            else
            {
                return View(producto);
            }
        }

        [HttpPost]
        public IActionResult Editar(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Update(producto);
                _context.SaveChanges();
                return RedirectToAction("Index");

            }
            return View();


        }

        [HttpGet]
        public IActionResult Eliminar(int? Codigo_producto)
        {
            var producto = _context.Productos.Find(Codigo_producto);

            if (Codigo_producto.HasValue == false)
            {
                TempData["ErrorTitle"] = "Error !";
                TempData["ErrorDescription"] = "No se encontro el producto";
                TempData["ErrorCode"] = "404";
                return RedirectToAction("ErrorPage", "Home");
            }

            var facturas = from d in _context.Detalle_Facturas where d.Codigo_producto == Codigo_producto select d;

            if (facturas.Count() > 0)
            {
                TempData["ErrorTitle"] = "Error";
                TempData["ErrorDescription"] = "No se puede eliminar el producto porque hay facturas asociadas a él.";
                TempData["ErrorCode"] = 403;
                return RedirectToAction("ErrorPage", "Home");
            }
            else
            {
                return View(producto);
            }
        }

        [HttpPost]
        public IActionResult Eliminar(Producto producto)
        {
            try
            {
                _context.Productos.Remove(producto);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("ErrorPage", "Home");
            }
        }
    }
}
