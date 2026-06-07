using DisasterEye.Web.Models;
using DisasterEye.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DisasterEye.Web.Controllers;

public class AccountController : Controller
{
    private readonly ApiService _api;
    public AccountController(ApiService api) => _api = api;

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");
        ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid) return View(model);

        var resultado = await _api.LoginAsync(model.Email, model.Senha);
        if (resultado == null || !resultado.Sucesso)
        {
            ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
            return View(model);
        }

        // Injeção de Claims — perfil do usuário via cookie
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name,  resultado.Nome  ?? ""),
            new(ClaimTypes.Email, resultado.Email ?? ""),
            new("Perfil",         resultado.Perfil ?? "Pesquisador"),
            new("UsuarioId",      resultado.UsuarioId?.ToString() ?? "0")
        };

        var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            principal,
            new AuthenticationProperties { IsPersistent = true });

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var sucesso = await _api.RegisterAsync(model);
        if (!sucesso)
        {
            ModelState.AddModelError(string.Empty, "Erro ao cadastrar. E-mail pode já estar em uso.");
            return View(model);
        }

        TempData["Sucesso"] = "Cadastro realizado! Faça login para continuar.";
        return RedirectToAction(nameof(Login));
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction(nameof(Login));
    }

    public IActionResult AcessoNegado() => View();
}
