using System.ComponentModel.DataAnnotations;

namespace WhoWant2B.Core.Models
{
    public class Complejidad_model
    {
        [Key]
        public int IdComplejidad { get; set; }
        [Required]
        [StringLength(100)]
        public required string Nombre { get; set; }
        [Required]
        [StringLength(100)]
        public required string Descripcion { get; set; }
    }
}
