using System.ComponentModel.DataAnnotations;

namespace SistemaFacturacionWeb.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
