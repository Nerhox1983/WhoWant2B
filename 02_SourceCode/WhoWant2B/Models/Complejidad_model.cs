using System.ComponentModel.DataAnnotations;

namespace WhoWant2B.Models
{
    public class Complejidad_Model
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
