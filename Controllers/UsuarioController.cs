using SistemaFacturacionWeb.DB;
using SistemaFacturacionWeb.Models;
using SistemaFacturacionWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace SistemaFacturacionWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Usuario> listaUsuarios = _context.Usuarios;
            return View(listaUsuarios);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RegistrarUsuarioViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                Usuario usuario = new Usuario();
                usuario.Email = modelo.Email;
                usuario.Password = ConvertirSha256(modelo.Password);

                _context.Add(usuario);
                _context.SaveChanges();
                return Redirect("/Usuario/Index");
            }

            return View(modelo);
        }

        public IActionResult Editar(int Id)
        {
            var usuario = _context.Usuarios.Find(Id);

            if (usuario == null)
            {
                return RedirectToAction("HttpError404");
            }

            EditarUsuarioViewModel modelo = new EditarUsuarioViewModel();
            modelo.Id = usuario.Id;
            modelo.Email = usuario.Email;

            return View(modelo);
        }

        [HttpPost]
        public IActionResult Editar(EditarUsuarioViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                var usuario = _context.Usuarios.Find(modelo.Id);
                usuario.Email = modelo.Email;
                usuario.Password = ConvertirSha256(modelo.Password);
                _context.Usuarios.Update(usuario);
                _context.SaveChanges();
                return Redirect("/Usuario/Index");
            }

            return View(modelo);
        }

        public IActionResult Eliminar(int Id)
        {
            var usuario = _context.Usuarios.Find(Id);

            if (usuario == null)
            {
                return RedirectToAction("HttpError404");
            }

            return View(usuario);
        }

        [HttpPost]
        public IActionResult Eliminar(Usuario usuario)
        {
            try
            {
                usuario = _context.Usuarios.Find(usuario.Id);
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();

                var session = HttpContext.Session.GetString("User");
                Usuario? usuarioActual = session == null ? null : JsonConvert.DeserializeObject<Usuario>(session);

                if (usuarioActual.Id == usuario.Id) {
                    HttpContext.Session.Remove("User");
                    return RedirectToAction("Login", "Access");
                }

                return Redirect("/Usuario/Index");
            }
            catch (Exception)
            {
                return RedirectToAction("HttpError404");
            }

        }

        public ActionResult HttpError404()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost, HttpGet]
        public JsonResult EmailExiste(string email, int id)
        {
            return Json(!_context.Usuarios.Any(x => x.Email == email && x.Id != id));
        }

        public static string ConvertirSha256(string password)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 sha256 = SHA256.Create())
            {
                Encoding encoding = Encoding.UTF8;
                byte[] resultado = sha256.ComputeHash(encoding.GetBytes(password));

                foreach (byte b in resultado)
                {
                    sb.Append(b.ToString("x2"));
                }
            }

            return sb.ToString();
        }
    }
}
