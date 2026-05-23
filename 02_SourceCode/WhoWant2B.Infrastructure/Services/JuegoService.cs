using Microsoft.EntityFrameworkCore;
using WhoWant2B.Core.Interfaces;
using WhoWant2B.Core.Models;
using WhoWant2B.Infrastructure.Data;
using WhoWant2B.Models;
using static WhoWant2B.Core.Enums.Comunes;

namespace WhoWant2B.Infrastructure.Services;

/// <summary>
/// Servicio que implementa la lógica del motor del juego, incluyendo selección de preguntas y registro de resultados.
/// </summary>
public class JuegoService : IJuegoService
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="JuegoService"/>.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    public JuegoService(ApplicationDbContext context) => _context = context;

    /// <summary>
    /// Obtiene una pregunta aleatoria que coincida con la complejidad solicitada y que no haya sido respondida aún.
    /// </summary>
    /// <param name="idComplejidad">Nivel de dificultad (1-5).</param>
    /// <param name="respondidasIds">Lista de IDs de preguntas que ya salieron en la partida actual.</param>
    /// <returns>Un modelo de pregunta con sus opciones, o null si no hay preguntas disponibles.</returns>
    public async Task<Pregunta_model?> ObtenerSiguientePreguntaAsync(int idComplejidad, List<int> respondidasIds)
    {
        return await _context.Preguntas
            .Include(p => p.Opciones)
            .Include(p => p.Complejidad)
            .Include(p => p.Categoria)
            .Where(p => p.IdComplejidad == idComplejidad && !respondidasIds.Contains(p.IdPregunta))
            .OrderBy(p => Guid.NewGuid())
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Recupera una opción de respuesta específica por su ID.
    /// </summary>
    /// <param name="idOpcion">ID de la opción.</param>
    /// <returns>Modelo de la opción.</returns>
    public async Task<Opcion_model?> ObtenerOpcionAsync(int idOpcion) =>
        await _context.Opciones.FirstOrDefaultAsync(o => o.IdOpcion == idOpcion);

    /// <summary>
    /// Registra el resultado final de una partida en la tabla de históricos.
    /// </summary>
    /// <param name="jugadorId">ID del jugador que finalizó.</param>
    /// <param name="puntaje">Puntaje total obtenido.</param>
    /// <param name="estado">Estado final (Ganado, Perdido, Retirado).</param>
    public async Task RegistrarHistoricoAsync(int jugadorId, int puntaje, EstadoJuegoEnum estado)
    {
        var registro = new Historico_model
        {
            PuntosAcumulados = puntaje,
            DineroAcumulado = puntaje,
            Fecha = DateTime.UtcNow,
            IdJugador = jugadorId,
            IdEstadoJuego = (int)estado,
            Usuario = null!,
            EstadoJuego = null!
        };

        _context.Historicos.Add(registro);
        await _context.SaveChangesAsync();
    }
}
