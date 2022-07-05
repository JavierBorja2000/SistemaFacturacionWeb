using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SistemaFacturacionWeb.Models.ViewModels
{
    public class Reporte2ViewModel
    {
        [Display(Name = "Fecha inicial")]
        [DataType(DataType.Date)]
        public DateTime? Fecha1 { get; set; }


        [Display(Name = "Fecha final")]
        [DataType(DataType.Date)]

        public DateTime? Fecha2 { get; set; }

        [Display(Name = "Nombre del cliente")]
        public string? Nombre_Completo { get; set; }
    }
}
