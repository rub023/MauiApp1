using Microsoft.EntityFrameworkCore;
using ServidoApi.Models;

namespace ServidoApi.Contenido
{
    public class AppDbContext:DbContext
    {
        public DbSet<Plato> Platos => Set<Plato>();//sera nombre de la tabla
        public AppDbContext(DbContextOptions<AppDbContext> opciones) : base(opciones)
        {

        }
    }
}
