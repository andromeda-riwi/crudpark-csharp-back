using Microsoft.EntityFrameworkCore;
using CrudPark.Api.Models;
namespace CrudPark.Api.Data 
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Operador> Operadores { get; set; }
        
        public DbSet<Mensualidad> Mensualidades { get; set; } public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TarifaCatalogo> TarifasCatalogo { get; set; }
        public DbSet<ConfiguracionSistema> ConfiguracionSistema { get; set; }
        public DbSet<Pago> Pagos { get; set; } 

    }
}