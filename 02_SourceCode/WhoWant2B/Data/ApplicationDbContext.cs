using Microsoft.EntityFrameworkCore;
using WhoWant2B.Models;

namespace WhoWant2B.Data
{
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pregunta_model> Preguntas { get; set; }
        public DbSet<Opcion_model> Opciones { get; set; }
        public DbSet<Categoria_model> Categorias { get; set; }
        public DbSet<Complejidad_Model> Complejidades { get; set; }
        public DbSet<EstadosJuego_model> EstadosJuego { get; set; }
        public DbSet<Usuario_model> Usuarios { get; set; }
        public DbSet<Historico_model> Historicos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);            
            modelBuilder.Entity<Pregunta_model>().ToTable("Preguntas");
            modelBuilder.Entity<Opcion_model>().ToTable("Opciones");
            modelBuilder.Entity<Categoria_model>().ToTable("Categorias");
            modelBuilder.Entity<Complejidad_Model>().ToTable("Complejidades");
            modelBuilder.Entity<EstadosJuego_model>().ToTable("EstadosJuego");
            
            modelBuilder.Entity<Opcion_model>()
                .HasOne(o => o.Pregunta)          // Opcion tiene la propiedad de objeto "Pregunta"
                .WithMany(p => p.Opciones)         // Pregunta tiene la colección "Opciones"
                .HasForeignKey(o => o.IdPregunta); // La columna física es "IdPregunta"

            // 3. Relaciones de la Pregunta con Categoría y Complejidad
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
