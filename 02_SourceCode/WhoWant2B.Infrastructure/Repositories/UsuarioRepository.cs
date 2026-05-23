using Microsoft.EntityFrameworkCore;
using WhoWant2B.Core.Interfaces;
using WhoWant2B.Core.Models;
using WhoWant2B.Infrastructure.Data;

namespace WhoWant2B.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly ApplicationDbContext _context;

    public UsuarioRepository(ApplicationDbContext context) => _context = context;

    public async Task<Usuario_model?> ObtenerPorLoginAsync(string login) =>
        await _context.Usuarios.FirstOrDefaultAsync(u => u.Login == login);

    public async Task AgregarUsuarioAsync(Usuario_model usuario) =>
        await _context.Usuarios.AddAsync(usuario);

    public async Task GuardarCambiosAsync() =>
        await _context.SaveChangesAsync();
}
