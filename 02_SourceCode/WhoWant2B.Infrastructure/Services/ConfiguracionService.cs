using Microsoft.EntityFrameworkCore;
using WhoWant2B.Core.Interfaces;
using WhoWant2B.Core.Models;
using WhoWant2B.Infrastructure.Data;

namespace WhoWant2B.Infrastructure.Services
{
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly ApplicationDbContext _context;

        public ConfiguracionService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region Categorías        
        public async Task<(List<Categoria_model> Items, int TotalRegistros)> ObtenerCategoriasPaginadasAsync(int pagina, int cantidadPorPagina)
        {
            var total = await _context.Categorias.CountAsync();
            var items = await _context.Categorias
                .Skip((pagina - 1) * cantidadPorPagina)
                .Take(cantidadPorPagina)
                .ToListAsync();

            return (items, total);
        }
        
        public async Task<List<Categoria_model>> ObtenerTodasLasCategoriasAsync()
        {
            return await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync();
        }

        public async Task<Categoria_model?> ObtenerCategoriaPorIdAsync(int id)
        {
            return await _context.Categorias.FindAsync(id);
        }
        
        public async Task GuardarCategoriaAsync(Categoria_model categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarCategoriaAsync(Categoria_model categoria)
        {
            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EliminarCategoriaAsync(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null) return false;

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
            return true;
        }

        #endregion

        #region Preguntas y Opciones

        
        public async Task<(List<Pregunta_model> Items, int TotalRegistros)> ObtenerPreguntasPaginadasAsync(int pagina, int cantidadPorPagina)
        {
            var total = await _context.Preguntas.CountAsync();
            var items = await _context.Preguntas
                .Include(p => p.Categoria)
                .Include(p => p.Complejidad)
                .Skip((pagina - 1) * cantidadPorPagina)
                .Take(cantidadPorPagina)
                .ToListAsync();

            return (items, total);
        }

        public async Task<List<Complejidad_model>> ObtenerTodasLasComplejidadesAsync()
        {
            return await _context.Set<Complejidad_model>().ToListAsync();
        }

        public async Task<Pregunta_model?> ObtenerPreguntaConOpcionesPorIdAsync(int id)
        {
            return await _context.Preguntas
                .Include(p => p.Opciones)
                .FirstOrDefaultAsync(p => p.IdPregunta == id);
        }

        public async Task<bool> GuardarPreguntaConOpcionesAsync(Pregunta_model pregunta, List<Opcion_model> opciones, int correctaIndex)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Preguntas.Add(pregunta);
                await _context.SaveChangesAsync();

                for (int i = 0; i < opciones.Count; i++)
                {
                    opciones[i].IdPregunta = pregunta.IdPregunta;
                    opciones[i].Valida = (i == correctaIndex);
                    _context.Opciones.Add(opciones[i]);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> ActualizarPreguntaConOpcionesAsync(Pregunta_model pregunta, List<Opcion_model> opciones, int correctaIndex)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Entry(pregunta).State = EntityState.Modified;

                var opcionesAnteriores = await _context.Opciones
                    .Where(o => o.IdPregunta == pregunta.IdPregunta)
                    .ToListAsync();
                _context.Opciones.RemoveRange(opcionesAnteriores);

                for (int i = 0; i < opciones.Count; i++)
                {
                    opciones[i].IdPregunta = pregunta.IdPregunta;
                    opciones[i].Valida = (i == correctaIndex);
                    _context.Opciones.Add(opciones[i]);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> EliminarPreguntaConOpcionesAsync(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var pregunta = await _context.Preguntas.FindAsync(id);
                if (pregunta == null) return false;

                var opciones = await _context.Opciones.Where(o => o.IdPregunta == id).ToListAsync();
                _context.Opciones.RemoveRange(opciones);

                _context.Preguntas.Remove(pregunta);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
        #endregion
    }
}