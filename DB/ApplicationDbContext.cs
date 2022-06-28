using Microsoft.EntityFrameworkCore;
using SistemaFacturacionWeb.Models;

namespace SistemaFacturacionWeb.DB
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
