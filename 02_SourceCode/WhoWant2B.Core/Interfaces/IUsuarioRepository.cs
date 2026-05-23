using WhoWant2B.Core.Models;

namespace WhoWant2B.Core.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario_model?> ObtenerPorLoginAsync(string login);
        Task AgregarUsuarioAsync(Usuario_model usuario);
        Task GuardarCambiosAsync();
    }
}