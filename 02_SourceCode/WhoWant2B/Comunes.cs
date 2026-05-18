using System.ComponentModel;

namespace WhoWant2B
{
    public class Comunes
    {
        public enum EstadoJuegoEnum
        {
            [Description("En curso")] EnCurso = 1,
            [Description("Ganado")] Ganado = 2,
            [Description("Retirado")] Retirado = 3,
            [Description("Perdido")] Perdido = 4
        }
    }
}
