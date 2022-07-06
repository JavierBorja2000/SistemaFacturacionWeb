using SistemaFacturacionWeb.Controllers;
using SistemaFacturacionWeb.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace SistemaFacturacionWeb.Filters
{
    public class VerificarSesion : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session.GetString("User");
            Usuario? usuario = session == null ? null : JsonConvert.DeserializeObject<Usuario>(session);

            if (usuario == null)
            {
                if (context.Controller is AccessController == false)
                {
                    context.HttpContext.Response.Redirect("/Access/Login");
                }
            }
            //else
            //{
            //    //if (context.Controller is AccessController == true)
            //    //{
            //    //    context.HttpContext.Response.Redirect("/Home/Index");
            //    //}
            //}

            base.OnActionExecuting(context);
        }
    }
}
