using DisasterEye.ApiService.DTOs;
using DisasterEye.ApiService.Models;
using DisasterEye.ApiService.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DisasterEye.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TecnologiasController : ControllerBase
{
    private readonly ITecnologiaRepository _repository;

    public TecnologiasController(ITecnologiaRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Lista todas as tecnologias</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var lista = await _repository.GetAllAsync();
        return Ok(lista.Select(MapToDto));
    }

    /// <summary>Busca tecnologia por ID</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var t = await _repository.GetByIdAsync(id);
        if (t == null) return NotFound(new { mensagem = "Tecnologia não encontrada." });
        return Ok(MapToDto(t));
    }

    /// <summary>Estatísticas agregadas para o dashboard</summary>
    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var stats = await _repository.GetStatsAsync();
        return Ok(new
        {
            totalTecnologias         = stats.TotalTecnologias,
            totalMissoesMapeadas     = stats.TotalMissoesMapeadas,
            totalSetoresBeneficiados = stats.TotalSetoresBeneficiados,
            contagemPorCategoria     = stats.ContagemPorCategoria,
            ultimosCadastros         = stats.UltimosCadastros.Select(MapToDto)
        });
    }

    /// <summary>Cria nova tecnologia</summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TecnologiaCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var tecnologia = new Tecnologia
        {
            Nome              = dto.Nome,
            Descricao         = dto.Descricao,
            OrigemEspacial    = dto.OrigemEspacial,
            Latitude          = dto.Latitude,
            Longitude         = dto.Longitude,
            Status            = dto.Status,
            CategoriaImpactoId = dto.CategoriaImpactoId
        };

        var created = await _repository.CreateAsync(tecnologia);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, MapToDto(created));
    }

    /// <summary>Atualiza tecnologia existente</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TecnologiaCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var tecnologia = new Tecnologia
        {
            Nome              = dto.Nome,
            Descricao         = dto.Descricao,
            OrigemEspacial    = dto.OrigemEspacial,
            Latitude          = dto.Latitude,
            Longitude         = dto.Longitude,
            Status            = dto.Status,
            CategoriaImpactoId = dto.CategoriaImpactoId
        };

        var updated = await _repository.UpdateAsync(id, tecnologia);
        if (updated == null) return NotFound(new { mensagem = "Tecnologia não encontrada." });
        return Ok(MapToDto(updated));
    }

    /// <summary>Exclui tecnologia</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _repository.DeleteAsync(id);
        if (!deleted) return NotFound(new { mensagem = "Tecnologia não encontrada." });
        return NoContent();
    }

    private static TecnologiaResponseDto MapToDto(Tecnologia t) => new()
    {
        Id               = t.Id,
        Nome             = t.Nome,
        Descricao        = t.Descricao,
        OrigemEspacial   = t.OrigemEspacial,
        Latitude         = t.Latitude,
        Longitude        = t.Longitude,
        Status           = t.Status,
        DataCadastro     = t.DataCadastro,
        CategoriaImpactoId = t.CategoriaImpactoId,
        CategoriaNome    = t.CategoriaImpacto?.Nome ?? string.Empty
    };
}
