using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoWant2B.Core.Models
{
    public class Pregunta_model
    {
        [Key]
        public int IdPregunta { get; set; }

        [Required]
        public required string Texto { get; set; }
        
        [Required]
        public int IdComplejidad { get; set; }

        [Required]
        public int IdCategoria { get; set; }

        [ForeignKey(nameof(IdCategoria))]
        public virtual Categoria_model? Categoria { get; set; }

        [ForeignKey(nameof(IdComplejidad))]
        public virtual Complejidad_model? Complejidad { get; set; }
        
        public virtual ICollection<Opcion_model> Opciones { get; set; } = new List<Opcion_model>();
    }
}