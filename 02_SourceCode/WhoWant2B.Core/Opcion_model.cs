using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoWant2B.Core.Models
{
    public class Opcion_model
    {
        [Key]
        public int IdOpcion { get; set; }
        [Required]
        [StringLength(200)]
        public string Texto { get; set; } = string.Empty;        
        public bool Valida { get; set; }
        [ForeignKey(nameof(Pregunta))]
        public int IdPregunta { get; set; }
        public virtual Pregunta_model? Pregunta { get; set; }
    }
}

