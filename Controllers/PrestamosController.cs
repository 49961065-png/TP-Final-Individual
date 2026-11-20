using Microsoft.AspNetCore.Mvc;
using TP_Individual_prog.Models;

namespace TP_Individual_prog.Controllers;

public class PrestamosController : Controller
{
    public IActionResult MisPrestamos()
    {
        string? idTexto = HttpContext.Session.GetString("IdUsuario");
        if (idTexto == null)
        {
            return RedirectToAction("Login", "Cuenta");
        }
        int idUsuario = int.Parse(idTexto);

        List<Prestamo> todos = BD.ObtenerPrestamosDeUsuario(idUsuario);
        List<Prestamo> proximos = new List<Prestamo>();
        List<Prestamo> pasados = new List<Prestamo>();

        foreach (Prestamo p in todos)
        {
            bool yaPaso = p.Fecha.Date < DateTime.Today ||
                          (p.Fecha.Date == DateTime.Today && p.HoraInicio <= DateTime.Now.TimeOfDay);

            if (p.Cancelado == false && yaPaso == false)
            {
                proximos.Add(p);
            }
            else
            {
                pasados.Add(p);
            }
        }

        ViewBag.Proximos = proximos;
        ViewBag.Pasados = pasados;
        return View();
    }

[HttpPost]
public IActionResult Cancelar(int id)
{
    string? idTexto = HttpContext.Session.GetString("IdUsuario");
    if (idTexto == null)
    {
        return Json(new { ok = false, mensaje = "Tenés que iniciar sesión." });
    }
    int idUsuario = int.Parse(idTexto);

    Prestamo? prestamo = BD.ObtenerPrestamoPorId(id);

    if (prestamo == null || prestamo.IdUsuario != idUsuario)
    {
        return Json(new { ok = false, mensaje = "No se encontró ese préstamo." });
    }

    if (prestamo.Cancelado)
    {
        return Json(new { ok = false, mensaje = "Ese préstamo ya estaba cancelado." });
    }

    Bloque? bloque = BD.ObtenerBloquePorId(prestamo.IdBloque);
    bool yaPaso = prestamo.Fecha.Date < DateTime.Today ||
                  (prestamo.Fecha.Date == DateTime.Today && bloque != null && bloque.HoraInicio <= DateTime.Now.TimeOfDay);

    if (yaPaso)
    {
        return Json(new { ok = false, mensaje = "Ese préstamo ya pasó, no se puede cancelar." });
    }

    BD.CancelarPrestamo(id);
    return Json(new { ok = true, mensaje = "Préstamo cancelado." });
}


}