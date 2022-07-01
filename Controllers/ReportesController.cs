using Microsoft.AspNetCore.Mvc;
using SistemaFacturacionWeb.DB;
using SistemaFacturacionWeb.Models;
using SistemaFacturacionWeb.Models.ViewModels;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace SistemaFacturacionWeb.Controllers
{
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext DbContext;
        static string cadena = "Server=localhost\\SQLEXPRESS;Database=SistemaFacturacion;Trusted_Connection=true";
       
        public ReportesController(ApplicationDbContext context)
        {
            DbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult Rep_Producto()
        {
            
            List<ReporteProductoViewModel1> ListadoReporte = new List<ReporteProductoViewModel1>();
            Reporte_producto modelo = new Reporte_producto();
            // Se crea el query y se abre la conexion
            string query = "sp_ReporteProducto";
            using (SqlConnection connection = new SqlConnection(cadena))
            {
                connection.Open();

                // Se envia el query
                SqlCommand sql = new SqlCommand(query, connection);
                sql.Parameters.AddWithValue("Fecha1", "");
                sql.Parameters.AddWithValue("Fecha2", "");
                sql.Parameters.AddWithValue("Nombre", "");
                sql.CommandType = CommandType.StoredProcedure;

                try
                {
                    // Se leen los datos
                    SqlDataReader reader = sql.ExecuteReader();
                    while (reader.Read())
                    {
                        ReporteProductoViewModel1 reportes = new ReporteProductoViewModel1();
                        reportes.Fecha = reader.GetDateTime(0);
                        reportes.Codigo_producto = reader.GetInt32(1);
                        reportes.Nombre_producto = reader.GetString(2);
                        reportes.Total = (float)reader.GetDouble(3);


                        ListadoReporte.Add(reportes);
                    }

                    reader.Close();
                    connection.Close();

                    
                    modelo.Resultados = ListadoReporte;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                }

            }  


            return View(modelo);
        }

        [HttpPost]
        public IActionResult Rep_Producto(Reporte_producto modelo)
        {
            List<ReporteProductoViewModel1> ListadoReporte = new List<ReporteProductoViewModel1>();
            modelo.Resultados = new List<ReporteProductoViewModel1>();
            if ((modelo.ReporteViewModel.Fecha1 == null && modelo.ReporteViewModel.Fecha2 != null) || (modelo.ReporteViewModel.Fecha2 == null && modelo.ReporteViewModel.Fecha1 != null)) {
                TempData["Error"] = "El rango de fechas debe contener Inicio y Final";
                return Redirect("Rep_Producto"); 

            }
           
            if(!(modelo.ReporteViewModel.Fecha2 > modelo.ReporteViewModel.Fecha1 || modelo.ReporteViewModel.Fecha1 == null || modelo.ReporteViewModel.Fecha2 == null))
            {
                TempData["Error"] = "La fecha final debe ser mayor a las inicial";
                return Redirect("Rep_Producto");
            }
            TempData["Error"] = "";
            // Se crea el query y se abre la conexion
            string query = "sp_ReporteProducto";
            using (SqlConnection connection = new SqlConnection(cadena))
            {
                connection.Open();

                // Se envia el query
                SqlCommand sql = new SqlCommand(query, connection);
               
                if (modelo.ReporteViewModel.Fecha1 == null) {
                    sql.Parameters.AddWithValue("Fecha1", "");
                }
                else
                {
                    sql.Parameters.AddWithValue("Fecha1", modelo.ReporteViewModel.Fecha1);
                }
                if (modelo.ReporteViewModel.Fecha2 == null)
                {
                    sql.Parameters.AddWithValue("Fecha2", "");
                }
                else
                {
                    sql.Parameters.AddWithValue("Fecha2", modelo.ReporteViewModel.Fecha2);
                }
                if (modelo.ReporteViewModel.Nombre == null)
                {
                    sql.Parameters.AddWithValue("Nombre", "");
                }
                else
                {
                    sql.Parameters.AddWithValue("Nombre", modelo.ReporteViewModel.Nombre);
                }
                sql.CommandType = CommandType.StoredProcedure;

                try
                {
                    // Se leen los datos
                    SqlDataReader reader = sql.ExecuteReader();
                    while (reader.Read())
                    {
                        ReporteProductoViewModel1 reportes = new ReporteProductoViewModel1();
                        reportes.Fecha = reader.GetDateTime(0);
                        reportes.Codigo_producto = reader.GetInt32(1);
                        reportes.Nombre_producto = reader.GetString(2);
                        reportes.Total = (float)reader.GetDouble(3);


                        ListadoReporte.Add(reportes);
                    }

                    reader.Close();
                    connection.Close();


                    modelo.Resultados = ListadoReporte;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);

                }

            }


            return View(modelo);
        }
       

        }
}





   

        


    

