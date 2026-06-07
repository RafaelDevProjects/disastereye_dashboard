using DisasterEye.ApiService.Models;

namespace DisasterEye.ApiService.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<Usuario?> GetByIdAsync(int id);
    Task<Usuario> CreateAsync(Usuario usuario);
    Task<bool> ValidarSenhaAsync(string email, string senha);
}
