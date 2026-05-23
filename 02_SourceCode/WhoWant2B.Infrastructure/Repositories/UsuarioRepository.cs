using Microsoft.EntityFrameworkCore;
using WhoWant2B.Core.Interfaces;
using WhoWant2B.Core.Models;
using WhoWant2B.Infrastructure.Data;

namespace WhoWant2B.Infrastructure.Repositories;

/// <summary>
/// Repositorio para la gestión directa de la entidad Usuario en la base de datos.
/// </summary>
public class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="UsuarioRepository"/>.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    public UsuarioRepository(ApplicationDbContext context) => _context = context;

    /// <summary>
    /// Obtiene un usuario basado en su login.
    /// </summary>
    /// <param name="login">Nombre de usuario.</param>
    /// <returns>Modelo del usuario o null si no se encuentra.</returns>
    public async Task<Usuario_model?> ObtenerPorLoginAsync(string login) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.Login == login);

    /// <summary>
    /// Agrega un nuevo usuario al contexto de forma asíncrona.
    /// </summary>
    /// <param name="usuario">Modelo del usuario a agregar.</param>
    public async Task AgregarUsuarioAsync(Usuario_model usuario) =>
        await _context.Usuarios.AddAsync(usuario);

    /// <summary>
    /// Persiste todos los cambios realizados en el contexto hacia la base de datos.
    /// </summary>
    public async Task GuardarCambiosAsync() =>
        await _context.SaveChangesAsync();
}
