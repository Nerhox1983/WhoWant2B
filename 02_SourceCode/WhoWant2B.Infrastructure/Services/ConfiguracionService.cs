using Microsoft.EntityFrameworkCore;
using WhoWant2B.Core.Interfaces;
using WhoWant2B.Core.Models;
using WhoWant2B.Infrastructure.Data;

namespace WhoWant2B.Infrastructure.Services
{
    /// <summary>
    /// Implementación del servicio de configuración para el mantenimiento de categorías, preguntas y opciones.
    /// </summary>
    public class ConfiguracionService : IConfiguracionService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ConfiguracionService"/>.
        /// </summary>
        /// <param name="context">Contexto de acceso a datos.</param>
        public ConfiguracionService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region Categorías        
        /// <summary>
        /// Obtiene una lista paginada de categorías.
        /// </summary>
        /// <param name="pagina">Número de página actual.</param>
        /// <param name="cantidadPorPagina">Cantidad de registros a retornar.</param>
        /// <returns>Una tupla con la lista de ítems y el total de registros en la base de datos.</returns>
        public async Task<(List<Categoria_model> Items, int TotalRegistros)> ObtenerCategoriasPaginadasAsync(int pagina, int cantidadPorPagina)
        {
            var total = await _context.Categorias.CountAsync();
            var items = await _context.Categorias
                .Skip((pagina - 1) * cantidadPorPagina)
                .Take(cantidadPorPagina)
                .ToListAsync();

            return (items, total);
        }
        
        /// <summary>
        /// Retorna todas las categorías ordenadas alfabéticamente.
        /// </summary>
        /// <returns>Lista completa de categorías.</returns>
        public async Task<List<Categoria_model>> ObtenerTodasLasCategoriasAsync()
        {
            return await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync();
        }

        /// <summary>
        /// Busca una categoría por su identificador único.
        /// </summary>
        /// <param name="id">ID de la categoría.</param>
        /// <returns>Modelo de la categoría o null.</returns>
        public async Task<Categoria_model?> ObtenerCategoriaPorIdAsync(int id)
        {
            return await _context.Categorias.FindAsync(id);
        }
        
        /// <summary>
        /// Persiste una nueva categoría en la base de datos.
        /// </summary>
        /// <param name="categoria">Modelo de la categoría a guardar.</param>
        public async Task GuardarCategoriaAsync(Categoria_model categoria)
        {
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Actualiza los datos de una categoría existente.
        /// </summary>
        /// <param name="categoria">Modelo con los cambios.</param>
        public async Task ActualizarCategoriaAsync(Categoria_model categoria)
        {
            _context.Entry(categoria).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Elimina una categoría por ID si existe.
        /// </summary>
        /// <param name="id">ID de la categoría.</param>
        /// <returns>True si fue eliminada, False si no se encontró.</returns>
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
        /// <summary>
        /// Obtiene una lista paginada de preguntas incluyendo sus relaciones con categoría y complejidad.
        /// </summary>
        /// <param name="pagina">Página a consultar.</param>
        /// <param name="cantidadPorPagina">Tamaño de la página.</param>
        /// <returns>Tupla con ítems y conteo total.</returns>
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

        /// <summary>
        /// Obtiene todos los niveles de complejidad disponibles.
        /// </summary>
        /// <returns>Lista de complejidades.</returns>
        public async Task<List<Complejidad_model>> ObtenerTodasLasComplejidadesAsync()
        {
            return await _context.Set<Complejidad_model>().ToListAsync();
        }

        /// <summary>
        /// Obtiene una pregunta con su colección de opciones cargada.
        /// </summary>
        /// <param name="id">ID de la pregunta.</param>
        /// <returns>Modelo de la pregunta con opciones.</returns>
        public async Task<Pregunta_model?> ObtenerPreguntaConOpcionesPorIdAsync(int id)
        {
            return await _context.Preguntas
                .Include(p => p.Opciones)
                .FirstOrDefaultAsync(p => p.IdPregunta == id);
        }

        /// <summary>
        /// Guarda una nueva pregunta y sus opciones asociadas dentro de una transacción.
        /// </summary>
        /// <param name="pregunta">Modelo de la pregunta.</param>
        /// <param name="opciones">Lista de 4 opciones.</param>
        /// <param name="correctaIndex">Índice de la opción marcada como válida.</param>
        /// <returns>True si la transacción fue exitosa.</returns>
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

        /// <summary>
        /// Actualiza una pregunta y sus opciones. Elimina las opciones previas y registra las nuevas para asegurar integridad.
        /// </summary>
        /// <param name="pregunta">Modelo de la pregunta.</param>
        /// <param name="opciones">Nueva lista de opciones.</param>
        /// <param name="correctaIndex">Índice de la opción correcta.</param>
        /// <returns>True si la actualización fue exitosa.</returns>
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

        /// <summary>
        /// Elimina una pregunta y todas sus opciones asociadas mediante una transacción.
        /// </summary>
        /// <param name="id">ID de la pregunta.</param>
        /// <returns>True si se eliminó correctamente.</returns>
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