using GestionTareas_DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestionTareas_DataAccessLayer.Data
{
    //public class GestionTareasDbContext : DbContext
    public class GestionTareasDbContext : IdentityDbContext<Usuario,IdentityRole<int>, int>
    {
        public GestionTareasDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Tarea> Tarea { get; set; }        

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
