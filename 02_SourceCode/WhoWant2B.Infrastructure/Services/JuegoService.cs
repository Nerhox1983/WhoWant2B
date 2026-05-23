using Microsoft.EntityFrameworkCore;
using WhoWant2B.Core.Interfaces;
using WhoWant2B.Core.Models;
using WhoWant2B.Infrastructure.Data;
using WhoWant2B.Models;
using static WhoWant2B.Core.Enums.Comunes;

namespace WhoWant2B.Infrastructure.Services;

public class JuegoService : IJuegoService
{
    private readonly ApplicationDbContext _context;

    public JuegoService(ApplicationDbContext context) => _context = context;

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

    public async Task<Opcion_model?> ObtenerOpcionAsync(int idOpcion) =>
        await _context.Opciones.FirstOrDefaultAsync(o => o.IdOpcion == idOpcion);

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
