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
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Factura> Facturas { get; set; }
        public DbSet<Detalle_Factura> Detalle_Facturas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Detalle_Factura>(entity =>
            {
                entity.HasKey(e => new { e.Numero_factura, e.Codigo_producto });
            });
    }
    }
}
