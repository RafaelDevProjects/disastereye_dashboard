using DisasterEye.Web.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace DisasterEye.Web.Services;

public class ApiService
{
    private readonly HttpClient _http;
    private readonly ILogger<ApiService> _logger;

    private static readonly JsonSerializerOptions _json = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ApiService(HttpClient http, ILogger<ApiService> logger)
    {
        _http   = http;
        _logger = logger;
    }

    // ── Tecnologias ──────────────────────────────────────────────

    public async Task<List<TecnologiaViewModel>> GetTecnologiasAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<TecnologiaViewModel>>("api/tecnologias", _json)
                   ?? new();
        }
        catch (Exception ex) { _logger.LogError(ex, "Erro ao buscar tecnologias."); return new(); }
    }

    public async Task<TecnologiaViewModel?> GetTecnologiaByIdAsync(int id)
    {
        try
        {
            return await _http.GetFromJsonAsync<TecnologiaViewModel>($"api/tecnologias/{id}", _json);
        }
        catch (Exception ex) { _logger.LogError(ex, "Erro ao buscar tecnologia {Id}.", id); return null; }
    }

    public async Task<bool> CreateTecnologiaAsync(TecnologiaFormViewModel model)
    {
        try
        {
            var r = await _http.PostAsJsonAsync("api/tecnologias", new
            {
                model.Nome, model.Descricao, model.OrigemEspacial,
                model.Latitude, model.Longitude, model.Status,
                model.CategoriaImpactoId
            });
            return r.IsSuccessStatusCode;
        }
        catch (Exception ex) { _logger.LogError(ex, "Erro ao criar tecnologia."); return false; }
    }

    public async Task<bool> UpdateTecnologiaAsync(int id, TecnologiaFormViewModel model)
    {
        try
        {
            var r = await _http.PutAsJsonAsync($"api/tecnologias/{id}", new
            {
                model.Nome, model.Descricao, model.OrigemEspacial,
                model.Latitude, model.Longitude, model.Status,
                model.CategoriaImpactoId
            });
            return r.IsSuccessStatusCode;
        }
        catch (Exception ex) { _logger.LogError(ex, "Erro ao atualizar tecnologia {Id}.", id); return false; }
    }

    public async Task<bool> DeleteTecnologiaAsync(int id)
    {
        try
        {
            var r = await _http.DeleteAsync($"api/tecnologias/{id}");
            return r.IsSuccessStatusCode;
        }
        catch (Exception ex) { _logger.LogError(ex, "Erro ao excluir tecnologia {Id}.", id); return false; }
    }

    public async Task<DashboardViewModel?> GetStatsAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<DashboardViewModel>("api/tecnologias/stats", _json);
        }
        catch (Exception ex) { _logger.LogError(ex, "Erro ao buscar stats."); return null; }
    }

    // ── Categorias ───────────────────────────────────────────────

    public async Task<List<CategoriaViewModel>> GetCategoriasAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<CategoriaViewModel>>("api/categorias", _json)
                   ?? new();
        }
        catch (Exception ex) { _logger.LogError(ex, "Erro ao buscar categorias."); return new(); }
    }

    // ── Auth ─────────────────────────────────────────────────────

    public async Task<LoginResponseModel?> LoginAsync(string email, string senha)
    {
        try
        {
            var r = await _http.PostAsJsonAsync("api/auth/login", new { email, senha });
            if (!r.IsSuccessStatusCode) return null;
            return await r.Content.ReadFromJsonAsync<LoginResponseModel>(_json);
        }
        catch (Exception ex) { _logger.LogError(ex, "Erro no login."); return null; }
    }

    public async Task<bool> RegisterAsync(RegisterViewModel model)
    {
        try
        {
            var r = await _http.PostAsJsonAsync("api/auth/register",
                new { model.Nome, model.Email, Senha = model.Senha, model.Perfil });
            return r.IsSuccessStatusCode;
        }
        catch (Exception ex) { _logger.LogError(ex, "Erro no registro."); return false; }
    }
}

public class LoginResponseModel
{
    public bool    Sucesso   { get; set; }
    public string? Mensagem  { get; set; }
    public int?    UsuarioId { get; set; }
    public string? Nome      { get; set; }
    public string? Perfil    { get; set; }
    public string? Email     { get; set; }
}
