using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SistemaFacturacionWeb.Models.ViewModels
{
    public class ReporteViewModel
    {
        [Display(Name = "Fecha Inicial")]
        [DataType(DataType.Date)]
        public DateTime? Fecha1 { get; set; }

       
        [Display(Name = "Fecha Final")]
        [DataType(DataType.Date)]
        
        public DateTime? Fecha2 { get; set; }

        [Display(Name = "Nombre producto")]
        public string? Nombre { get; set; }
    }
}
