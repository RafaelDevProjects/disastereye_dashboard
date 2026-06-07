using DisasterEye.ApiService.Data;
using DisasterEye.ApiService.Models;
using Microsoft.EntityFrameworkCore;

namespace DisasterEye.ApiService.Repositories;

public class TecnologiaRepository : ITecnologiaRepository
{
    private readonly AppDbContext _context;

    public TecnologiaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tecnologia>> GetAllAsync()
    {
        return await _context.Tecnologias
            .Include(t => t.CategoriaImpacto)
            .OrderByDescending(t => t.DataCadastro)
            .ToListAsync();
    }

    public async Task<Tecnologia?> GetByIdAsync(int id)
    {
        return await _context.Tecnologias
            .Include(t => t.CategoriaImpacto)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Tecnologia> CreateAsync(Tecnologia tecnologia)
    {
        tecnologia.DataCadastro = DateTime.UtcNow;
        _context.Tecnologias.Add(tecnologia);
        await _context.SaveChangesAsync();
        return tecnologia;
    }

    public async Task<Tecnologia?> UpdateAsync(int id, Tecnologia tecnologia)
    {
        var existing = await _context.Tecnologias.FindAsync(id);
        if (existing == null) return null;

        existing.Nome              = tecnologia.Nome;
        existing.Descricao         = tecnologia.Descricao;
        existing.OrigemEspacial    = tecnologia.OrigemEspacial;
        existing.Latitude          = tecnologia.Latitude;
        existing.Longitude         = tecnologia.Longitude;
        existing.Status            = tecnologia.Status;
        existing.CategoriaImpactoId = tecnologia.CategoriaImpactoId;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tecnologia = await _context.Tecnologias.FindAsync(id);
        if (tecnologia == null) return false;

        _context.Tecnologias.Remove(tecnologia);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<TecnologiaStats> GetStatsAsync()
    {
        var totalTecnologias = await _context.Tecnologias.CountAsync();

        var totalMissoes = await _context.Tecnologias
            .Select(t => t.OrigemEspacial)
            .Distinct()
            .CountAsync();

        var totalSetores = await _context.CategoriasImpacto.CountAsync();

        var contagemPorCategoria = await _context.Tecnologias
            .Include(t => t.CategoriaImpacto)
            .GroupBy(t => t.CategoriaImpacto!.Nome)
            .Select(g => new SetorContagem { Categoria = g.Key, Quantidade = g.Count() })
            .ToListAsync();

        var ultimosCadastros = await _context.Tecnologias
            .Include(t => t.CategoriaImpacto)
            .OrderByDescending(t => t.DataCadastro)
            .Take(5)
            .ToListAsync();

        return new TecnologiaStats
        {
            TotalTecnologias          = totalTecnologias,
            TotalMissoesMapeadas      = totalMissoes,
            TotalSetoresBeneficiados  = totalSetores,
            ContagemPorCategoria      = contagemPorCategoria,
            UltimosCadastros          = ultimosCadastros
        };
    }
}
