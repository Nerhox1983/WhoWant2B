using System.ComponentModel.DataAnnotations;

namespace WhoWant2B.Models
{
    public class Categoria_model
    {
        [Key]
        public int IdCategoria { get; set; }
        [Required]
        [StringLength(100)]
        public required string Nombre { get; set; }
        [Required]
        [StringLength(100)]
        public required string Descripcion { get; set; }
    }
}
