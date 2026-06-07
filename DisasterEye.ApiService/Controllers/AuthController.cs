using DisasterEye.ApiService.DTOs;
using DisasterEye.ApiService.Models;
using DisasterEye.ApiService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DisasterEye.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUsuarioRepository _repository;

    public AuthController(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Valida credenciais e retorna dados do usuário</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var valido = await _repository.ValidarSenhaAsync(dto.Email, dto.Senha);
        if (!valido)
            return Unauthorized(new LoginResponseDto { Sucesso = false, Mensagem = "Credenciais inválidas." });

        var usuario = await _repository.GetByEmailAsync(dto.Email);
        return Ok(new LoginResponseDto
        {
            Sucesso   = true,
            UsuarioId = usuario!.Id,
            Nome      = usuario.Nome,
            Perfil    = usuario.Perfil,
            Email     = usuario.Email
        });
    }

    /// <summary>Cadastra novo usuário com senha BCrypt</summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UsuarioCreateDto dto)
    {
        var existente = await _repository.GetByEmailAsync(dto.Email);
        if (existente != null)
            return Conflict(new { mensagem = "E-mail já cadastrado." });

        var usuario = new Usuario
        {
            Nome      = dto.Nome,
            Email     = dto.Email,
            SenhaHash = dto.Senha, // será hasheado no repository com BCrypt
            Perfil    = dto.Perfil
        };

        var criado = await _repository.CreateAsync(usuario);
        return Created($"/api/auth/{criado.Id}", new
        {
            criado.Id,
            criado.Nome,
            criado.Email,
            criado.Perfil
        });
    }
}
