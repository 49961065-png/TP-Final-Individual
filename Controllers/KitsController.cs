using Microsoft.AspNetCore.Mvc;
using TP_Individual_prog.Models;

namespace TP_Individual_prog.Controllers;

public class KitsController : Controller
{
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("IdUsuario") == null)
        {
            return RedirectToAction("Login", "Cuenta");
        }

        List<Kit> todos = BD.ObtenerKits();
        ViewBag.Kits = ArmarPagina(todos, 0);
        ViewBag.HayMas = todos.Count > 8;
        return View();
    }

    public IActionResult VerMas(int desde)
    {
        if (HttpContext.Session.GetString("IdUsuario") == null)
        {
            return Json(new { kits = new List<Kit>(), hayMas = false });
        }

        if (desde < 0)
        {
            desde = 0;
        }

        List<Kit> todos = BD.ObtenerKits();
        List<Kit> pagina = ArmarPagina(todos, desde);
        bool hayMas = desde + 8 < todos.Count;

        return Json(new { kits = pagina, hayMas = hayMas });
    }

    private List<Kit> ArmarPagina(List<Kit> todos, int desde)
    {
        List<Kit> pagina = new List<Kit>();
        for (int i = desde; i < desde + 8 && i < todos.Count; i++)
        {
            pagina.Add(todos[i]);
        }
        return pagina;
    }
}