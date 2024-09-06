using GestionTareas_DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionTareas_DataAccessLayer.Data
{
    public class GestionTareasDbContext : DbContext
    {
        public GestionTareasDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Tarea> Tarea { get; set; }
        public DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
