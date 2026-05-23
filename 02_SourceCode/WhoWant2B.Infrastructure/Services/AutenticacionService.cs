using Microsoft.EntityFrameworkCore;
using WhoWant2B.Core.Interfaces;
using WhoWant2B.Core.Models;
using WhoWant2B.Infrastructure.Data;

namespace WhoWant2B.Infrastructure.Services;

public class AutenticacionService : IAutenticacionService
{
    private readonly ApplicationDbContext _context;

    public AutenticacionService(ApplicationDbContext context) => _context = context;

    public async Task<Usuario_model?> ObtenerPorLoginAsync(string login) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.Login.ToLower() == login.Trim().ToLower());

    public async Task<Usuario_model?> ObtenerPorNombreRealAsync(string nombreReal) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreReal.ToLower() == nombreReal.Trim().ToLower());

    public async Task<Usuario_model> RegistrarJugadorAsync(string login, byte[] passwordHash, int idRol, string nombreReal)
    {
        var nuevoUsuario = new Usuario_model
        {
            Login = login.Trim(),
            Password = passwordHash, // Ahora ambos serán byte[] y compilará sin chistar
            IdRol = idRol,
            NombreReal = nombreReal.Trim()
        };

        _context.Usuarios.Add(nuevoUsuario);
        await _context.SaveChangesAsync();
        return nuevoUsuario;
    }
}
