using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP_Individual_prog.Models;

namespace TP_Individual_prog.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

   public IActionResult Index()
{
    HttpContext.Session.SetString("Prueba", "La sesión funciona");
    return View();
}

public IActionResult Privacy()
{
    ViewBag.Prueba = HttpContext.Session.GetString("Prueba");
    return View();
}

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
