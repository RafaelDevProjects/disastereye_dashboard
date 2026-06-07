using DisasterEye.Web.Models;
using DisasterEye.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisasterEye.Web.Controllers;

[Authorize]
public class TecnologiasController : Controller
{
    private readonly ApiService _api;
    public TecnologiasController(ApiService api) => _api = api;

    public async Task<IActionResult> Index()
    {
        var lista = await _api.GetTecnologiasAsync();
        return View(lista);
    }

    public async Task<IActionResult> Details(int id)
    {
        var t = await _api.GetTecnologiaByIdAsync(id);
        if (t == null) return NotFound();
        return View(t);
    }

    // ── Create ── protegido por Claim Administrador ──────────────

    [Authorize(Policy = "ApenasAdmin")]
    public async Task<IActionResult> Create()
    {
        return View(new TecnologiaFormViewModel
        {
            Categorias = await _api.GetCategoriasAsync()
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    [Authorize(Policy = "ApenasAdmin")]
    public async Task<IActionResult> Create(TecnologiaFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categorias = await _api.GetCategoriasAsync();
            return View(model);
        }

        if (!await _api.CreateTecnologiaAsync(model))
        {
            ModelState.AddModelError(string.Empty, "Erro ao criar tecnologia. Tente novamente.");
            model.Categorias = await _api.GetCategoriasAsync();
            return View(model);
        }

        TempData["Sucesso"] = "Tecnologia cadastrada com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    // ── Edit ─────────────────────────────────────────────────────

    [Authorize(Policy = "ApenasAdmin")]
    public async Task<IActionResult> Edit(int id)
    {
        var t = await _api.GetTecnologiaByIdAsync(id);
        if (t == null) return NotFound();

        return View(new TecnologiaFormViewModel
        {
            Id               = t.Id,
            Nome             = t.Nome,
            Descricao        = t.Descricao,
            OrigemEspacial   = t.OrigemEspacial,
            Latitude         = t.Latitude,
            Longitude        = t.Longitude,
            Status           = t.Status,
            CategoriaImpactoId = t.CategoriaImpactoId,
            Categorias       = await _api.GetCategoriasAsync()
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    [Authorize(Policy = "ApenasAdmin")]
    public async Task<IActionResult> Edit(int id, TecnologiaFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Categorias = await _api.GetCategoriasAsync();
            return View(model);
        }

        if (!await _api.UpdateTecnologiaAsync(id, model))
        {
            ModelState.AddModelError(string.Empty, "Erro ao atualizar. Tente novamente.");
            model.Categorias = await _api.GetCategoriasAsync();
            return View(model);
        }

        TempData["Sucesso"] = "Tecnologia atualizada com sucesso!";
        return RedirectToAction(nameof(Index));
    }

    // ── Delete ── protegido por Claim Administrador ──────────────

    [Authorize(Policy = "ApenasAdmin")]
    public async Task<IActionResult> Delete(int id)
    {
        var t = await _api.GetTecnologiaByIdAsync(id);
        if (t == null) return NotFound();
        return View(t);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    [Authorize(Policy = "ApenasAdmin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!await _api.DeleteTecnologiaAsync(id))
            TempData["Erro"] = "Erro ao excluir tecnologia.";
        else
            TempData["Sucesso"] = "Tecnologia excluída com sucesso!";

        return RedirectToAction(nameof(Index));
    }
}
