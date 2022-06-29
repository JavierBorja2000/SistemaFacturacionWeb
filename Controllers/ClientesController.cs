using Microsoft.AspNetCore.Mvc;
using SistemaFacturacionWeb.DB;
using SistemaFacturacionWeb.Models;


namespace SistemaFacturacionWeb.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext DbContext;

        public ClientesController(ApplicationDbContext context)
        {
            DbContext = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Cliente> listaClientes = DbContext.Clientes;
            return View(listaClientes);
         
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                DbContext.Clientes.Add(cliente);
                DbContext.SaveChanges();
                return RedirectToAction("Index");

            }
            return View();
        }

        [HttpGet]
        public IActionResult Editar(int? Codigo_cliente)
        {
            var cliente = DbContext.Clientes.Find(Codigo_cliente);

            if (Codigo_cliente.HasValue == false)
            {
                return NotFound();

            }
            else
            {
                return View(cliente);
            }
        }

        [HttpPost]
        public IActionResult Editar(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                DbContext.Update(cliente);
                DbContext.SaveChanges();
                return RedirectToAction("Index");

            }
            return View();


        }
        [HttpGet]
        public IActionResult Eliminar(int? Codigo_cliente)
        {
            var cliente = DbContext.Clientes.Find(Codigo_cliente);

            if (Codigo_cliente.HasValue == false)
            {
                return NotFound();
            }

            var facturas = from d in DbContext.Facturas where d.Codigo_cliente == Codigo_cliente select d;
            
            if(facturas.Count() > 0)
            {
                TempData["ErrorTitle"] = "Error";
                TempData["ErrorDescription"] = "No se puede eliminar el cliente porque hay facturas asociadas a él.";
                TempData["ErrorCode"] = 403;
                return RedirectToAction("ErrorPage", "Home");
            }
            else
            {
                return View(cliente);
            }
        }
        [HttpPost]
        public IActionResult Eliminar(Cliente cliente)
        {
            try
            {
                DbContext.Clientes.Remove(cliente);
                DbContext.SaveChanges();
                return RedirectToAction("Index");

            }
            catch (Exception e)
            {

            }
            return View(cliente);
        }


    }
}
