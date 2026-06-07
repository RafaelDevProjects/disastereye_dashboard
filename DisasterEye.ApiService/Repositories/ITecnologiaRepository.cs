using DisasterEye.ApiService.Models;

namespace DisasterEye.ApiService.Repositories;

public interface ITecnologiaRepository
{
    Task<IEnumerable<Tecnologia>> GetAllAsync();
    Task<Tecnologia?> GetByIdAsync(int id);
    Task<Tecnologia> CreateAsync(Tecnologia tecnologia);
    Task<Tecnologia?> UpdateAsync(int id, Tecnologia tecnologia);
    Task<bool> DeleteAsync(int id);
    Task<TecnologiaStats> GetStatsAsync();
}

public class TecnologiaStats
{
    public int TotalTecnologias { get; set; }
    public int TotalMissoesMapeadas { get; set; }
    public int TotalSetoresBeneficiados { get; set; }
    public List<SetorContagem> ContagemPorCategoria { get; set; } = new();
    public List<Tecnologia> UltimosCadastros { get; set; } = new();
}

public class SetorContagem
{
    public string Categoria { get; set; } = string.Empty;
    public int Quantidade { get; set; }
}
