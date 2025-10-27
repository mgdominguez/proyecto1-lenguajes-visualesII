using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiCodeFirst.Models;

namespace WebApiCodeFirst.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUsuario>
    {
        //constructor para las configuraciones
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        //agregar las entidades
        public DbSet<Categoria> categoria { get; set; }

        public DbSet<Usuario> usuario { get; set; }

        public DbSet<AppUsuario> appUsuarios { get; set; }

        public DbSet<Pelicula> pelicula { get; set; }

        public DbSet<LogApi> logApi { get; set; }
    }
}