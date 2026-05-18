using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoWant2B.Models
{
    public class Usuario_model
    {
        [Key]
        public int IdUsuario { get; set; }
        public string Login { get; set; }

        // CAMBIO: De string a byte[] para que coincida con VARBINARY(MAX)
        public byte[] Password { get; set; }

        public int IdRol { get; set; }
        [ForeignKey("IdRol")]
        public virtual Rol_model Rol { get; set; }
        public string NombreReal { get; set; }
    }
}
