using System.ComponentModel.DataAnnotations;

namespace WhoWant2B.Models
{
    public class Rol_model
    {
        [Key]
        public int IdRol { get; set; }
        public string Texto { get; set; }        
    }
}
