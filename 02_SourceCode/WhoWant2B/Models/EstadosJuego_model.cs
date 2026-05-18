using System.ComponentModel.DataAnnotations;

namespace WhoWant2B.Models
{
    public class EstadosJuego_model
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string NombreEstado { get; set; }

        public string? Descripcion { get; set; }
    }
}