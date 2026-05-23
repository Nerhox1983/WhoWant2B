using WhoWant2B.Core.Models;

namespace WhoWant2B.Core.Interfaces
{
    public interface IConfiguracionService
    {        
        Task<(List<Categoria_model> Items, int TotalRegistros)> ObtenerCategoriasPaginadasAsync(int pagina, int cantidadPorPagina);
        Task<List<Categoria_model>> ObtenerTodasLasCategoriasAsync();
        Task<Categoria_model?> ObtenerCategoriaPorIdAsync(int id);
        Task GuardarCategoriaAsync(Categoria_model categoria);
        Task ActualizarCategoriaAsync(Categoria_model categoria);
        Task<bool> EliminarCategoriaAsync(int id);        
        Task<(List<Pregunta_model> Items, int TotalRegistros)> ObtenerPreguntasPaginadasAsync(int pagina, int cantidadPorPagina);
        Task<List<Complejidad_model>> ObtenerTodasLasComplejidadesAsync();
        Task<Pregunta_model?> ObtenerPreguntaConOpcionesPorIdAsync(int id);
        Task<bool> GuardarPreguntaConOpcionesAsync(Pregunta_model pregunta, List<Opcion_model> opciones, int correctaIndex);
        Task<bool> ActualizarPreguntaConOpcionesAsync(Pregunta_model pregunta, List<Opcion_model> opciones, int correctaIndex);
        Task<bool> EliminarPreguntaConOpcionesAsync(int id);
    }
}
