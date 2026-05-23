using Microsoft.EntityFrameworkCore;
using WhoWant2B.Core.Models;
using WhoWant2B.Models;

namespace WhoWant2B.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pregunta_model> Preguntas { get; set; }
        public DbSet<Opcion_model> Opciones { get; set; }
        public DbSet<Categoria_model> Categorias { get; set; }
        public DbSet<Complejidad_model> Complejidades { get; set; }
        public DbSet<EstadosJuego_model> EstadosJuego { get; set; }
        public DbSet<Usuario_model> Usuarios { get; set; }
        public DbSet<Historico_model> Historicos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);            
            modelBuilder.Entity<Pregunta_model>().ToTable("Preguntas");
            modelBuilder.Entity<Opcion_model>().ToTable("Opciones");
            modelBuilder.Entity<Categoria_model>().ToTable("Categorias");
            modelBuilder.Entity<Complejidad_model>().ToTable("Complejidades");
            modelBuilder.Entity<EstadosJuego_model>().ToTable("EstadosJuego");
            
            modelBuilder.Entity<Opcion_model>()
                .HasOne(o => o.Pregunta)
                .WithMany(p => p.Opciones)
                .HasForeignKey(o => o.IdPregunta);
            
            modelBuilder.Entity<Pregunta_model>()
                .HasOne(p => p.Categoria)
                .WithMany()
                .HasForeignKey(p => p.IdCategoria);

            modelBuilder.Entity<Pregunta_model>()
                .HasOne(p => p.Complejidad)
                .WithMany()
                .HasForeignKey(p => p.IdComplejidad);
        }
    }
}
