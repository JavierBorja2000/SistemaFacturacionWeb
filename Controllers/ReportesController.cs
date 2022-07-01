using Microsoft.AspNetCore.Mvc;

namespace SistemaFacturacionWeb.Controllers
{
    public class ReportesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
