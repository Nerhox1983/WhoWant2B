using WhoWant2B.Core.Models;
using static WhoWant2B.Core.Enums.Comunes;

namespace WhoWant2B.Core.Interfaces
{
    public interface IJuegoService
    {
        Task<Pregunta_model?> ObtenerSiguientePreguntaAsync(int idComplejidad, List<int> respondidasIds);
        Task<Opcion_model?> ObtenerOpcionAsync(int idOpcion);
        Task RegistrarHistoricoAsync(int jugadorId, int puntaje, EstadoJuegoEnum estado);
    }
}