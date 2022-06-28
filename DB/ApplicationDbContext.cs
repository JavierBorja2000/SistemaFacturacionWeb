using Microsoft.EntityFrameworkCore;

namespace SistemaFacturacionWeb.DB
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
    }
}
