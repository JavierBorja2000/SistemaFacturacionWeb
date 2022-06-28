using SistemaFacturacionWeb.DB;
using SistemaFacturacionWeb.Models;
using SistemaFacturacionWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace SistemaFacturacionWeb.Controllers
{
    public class AccessController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccessController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Usuario usuarioIngresado)
        {
            var usuario = _context.Usuarios.FromSqlRaw($"sp_VerificarLogin '{usuarioIngresado.Email}', '{ConvertirSha256(usuarioIngresado.Password)}'").ToList();

            if (usuario.Count() > 0)
            {
                HttpContext.Session.SetString("User", JsonConvert.SerializeObject(usuario.First()));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Usuario o password incorrectos";
                return View();
            }

        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Remove("User");
            return Redirect("Login");
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(RegistrarUsuarioViewModel modelo)
        {
            if (!ModelState.IsValid)
            {
                return View(modelo);
            }

            Usuario usuario = new Usuario();
            usuario.Email = modelo.Email;
            usuario.Password = ConvertirSha256(modelo.Password);

            _context.Add(usuario);
            _context.SaveChanges();

            return Redirect("Login");
        }



        [AllowAnonymous]
        [HttpPost, HttpGet]
        public JsonResult EmailExiste(string email)
        {
            return Json(!_context.Usuarios.Any(x => x.Email == email));
        }

        public static string ConvertirSha256(string password)
        {
            StringBuilder sb = new StringBuilder();
            using(SHA256 sha256 = SHA256.Create())
            {
                Encoding encoding = Encoding.UTF8;
                byte[] resultado = sha256.ComputeHash(encoding.GetBytes(password));

                foreach(byte b in resultado)
                {
                    sb.Append(b.ToString("x2"));
                }
            }

            return sb.ToString();
        }
    }
}
