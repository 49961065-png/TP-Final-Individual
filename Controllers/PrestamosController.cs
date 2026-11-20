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
}