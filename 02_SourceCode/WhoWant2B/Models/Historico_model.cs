using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhoWant2B.Models
{
    [Table("Historicos")]
    public class Historico_model
    {
        [Key]
        public int IdHistorico { get; set; }

        public int PuntosAcumulados { get; set; }

        public decimal DineroAcumulado { get; set; }
        
        public int IdJugador { get; set; }

        [ForeignKey("IdJugador")]
        public virtual required Usuario_model Usuario { get; set; }

        public int IdEstadoJuego { get; set; }
        
        [ForeignKey("IdEstadoJuego")]
        public virtual required EstadosJuego_model EstadoJuego { get; set; }

        public DateTime Fecha { get; set; }
    }
}
