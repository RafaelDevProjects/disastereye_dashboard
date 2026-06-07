using DisasterEye.ApiService.Data;
using DisasterEye.ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace DisasterEye.ApiService.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly AppDbContext _context;

    public UsuarioRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email && u.Ativo == 1);
    }

    public async Task<Usuario?> GetByIdAsync(int id)
    {
        return await _context.Usuarios.FindAsync(id);
    }

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        // BCrypt hash — NUNCA armazena senha em texto limpo
        usuario.SenhaHash    = BCrypt.Net.BCrypt.HashPassword(usuario.SenhaHash);
        usuario.DataCadastro = DateTime.UtcNow;
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<bool> ValidarSenhaAsync(string email, string senha)
    {
        var usuario = await GetByEmailAsync(email);
        if (usuario == null) return false;
        return BCrypt.Net.BCrypt.Verify(senha, usuario.SenhaHash);
    }
}
