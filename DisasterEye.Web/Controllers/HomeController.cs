using DisasterEye.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisasterEye.Web.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ApiService _api;
    public HomeController(ApiService api) => _api = api;

    public async Task<IActionResult> Index()
    {
        var stats = await _api.GetStatsAsync();
        return View(stats);
    }

    [AllowAnonymous]
    public IActionResult Error() => View();
}
