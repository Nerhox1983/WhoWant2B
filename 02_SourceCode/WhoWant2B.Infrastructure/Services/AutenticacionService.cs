using Microsoft.EntityFrameworkCore;
using WhoWant2B.Core.Interfaces;
using WhoWant2B.Core.Models;
using WhoWant2B.Infrastructure.Data;

namespace WhoWant2B.Infrastructure.Services;

/// <summary>
/// Servicio encargado de la validación y registro de usuarios en el sistema.
/// </summary>
public class AutenticacionService : IAutenticacionService
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="AutenticacionService"/>.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    public AutenticacionService(ApplicationDbContext context) => _context = context;

    /// <summary>
    /// Busca un usuario por su nombre de usuario (login), ignorando mayúsculas y espacios.
    /// </summary>
    /// <param name="login">El nombre de usuario a buscar.</param>
    /// <returns>El modelo del usuario si existe; de lo contrario, null.</returns>
    public async Task<Usuario_model?> ObtenerPorLoginAsync(string login) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.Login.ToLower() == login.Trim().ToLower());

    /// <summary>
    /// Busca un usuario por su nombre real, útil para procesos de recuperación o validación AJAX.
    /// </summary>
    /// <param name="nombreReal">Nombre completo del jugador.</param>
    /// <returns>El modelo del usuario si existe; de lo contrario, null.</returns>
    public async Task<Usuario_model?> ObtenerPorNombreRealAsync(string nombreReal) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreReal.ToLower() == nombreReal.Trim().ToLower());

    /// <summary>
    /// Registra un nuevo jugador en la base de datos con su respectivo hash de contraseña.
    /// </summary>
    /// <param name="login">Login elegido.</param>
    /// <param name="passwordHash">Hash de la contraseña generado por el servicio de seguridad.</param>
    /// <param name="idRol">Identificador del rol asignado.</param>
    /// <param name="nombreReal">Nombre completo para el perfil.</param>
    /// <returns>El usuario recién creado.</returns>
    public async Task<Usuario_model> RegistrarJugadorAsync(string login, byte[] passwordHash, int idRol, string nombreReal)
    {
        var nuevoUsuario = new Usuario_model
        {
            Login = login.Trim(),
            Password = passwordHash,
            IdRol = idRol,
            NombreReal = nombreReal.Trim()
        };

        _context.Usuarios.Add(nuevoUsuario);
        await _context.SaveChangesAsync();
        return nuevoUsuario;
    }
}
