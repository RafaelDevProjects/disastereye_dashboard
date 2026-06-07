using DisasterEye.ApiService.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DisasterEye.ApiService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categorias = await _context.CategoriasImpacto
            .Select(c => new { c.Id, c.Nome, c.Descricao, c.TipoDesastre })
            .ToListAsync();
        return Ok(categorias);
    }
}
