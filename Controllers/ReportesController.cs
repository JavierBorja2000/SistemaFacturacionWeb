using Microsoft.AspNetCore.Mvc;
using SistemaFacturacionWeb.DB;
using SistemaFacturacionWeb.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SistemaFacturacionWeb.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext DbContext;
        static string cadena = "Server=localhost\\SQLEXPRESS;Database=SistemaFacturacion;Trusted_Connection=true;MultipleActiveResultSets=true";

        public ReportesController(ApplicationDbContext context)
        {
            DbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Rep_Producto(Reportes reporte)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_ReporteProducto", cn);
                cmd.Parameters.AddWithValue("Fecha1", reporte.Fecha1);
                cmd.Parameters.AddWithValue("Fecha2", reporte.Fecha2);
                cmd.Parameters.AddWithValue("Nombre Producto", reporte.Nombre);

                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                
            }

                return View();
            }

        public IActionResult Rep_Cliente(Reportes reporte)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_ReporteProducto", cn);
                cmd.Parameters.AddWithValue("Fecha1", reporte.Fecha1);
                cmd.Parameters.AddWithValue("Fecha2", reporte.Fecha2);
                cmd.Parameters.AddWithValue("Nombre Producto", reporte.Nombre);

                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

            }

            return View();
        }

        public IActionResult Rep_Factura(Reportes reporte)
        {
            using (SqlConnection cn = new SqlConnection(cadena))
            {

                SqlCommand cmd = new SqlCommand("sp_ReporteProducto", cn);
                cmd.Parameters.AddWithValue("Fecha1", reporte.Fecha1);
                cmd.Parameters.AddWithValue("Fecha2", reporte.Fecha2);
                

                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

            }

            return View();
        }
    }
}





   

        


    

