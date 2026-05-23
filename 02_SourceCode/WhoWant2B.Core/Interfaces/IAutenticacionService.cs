using WhoWant2B.Core.Models;

namespace WhoWant2B.Core.Interfaces;

public interface IAutenticacionService
{
    Task<Usuario_model?> ObtenerPorLoginAsync(string login);
    Task<Usuario_model?> ObtenerPorNombreRealAsync(string nombreReal);    
    Task<Usuario_model> RegistrarJugadorAsync(string login, byte[] passwordHash, int idRol, string nombreReal);
}