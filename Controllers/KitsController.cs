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