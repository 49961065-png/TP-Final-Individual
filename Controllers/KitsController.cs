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

    public IActionResult DetalleKit(int id)
    {
        if (HttpContext.Session.GetString("IdUsuario") == null)
        {
            return RedirectToAction("Login", "Cuenta");
        }

        Kit? kit = BD.ObtenerKitPorId(id);

        if (kit == null)
        {
            return RedirectToAction("Index");
        }

        ViewBag.Kit = kit;
        return View();
    }

    public IActionResult BloquesLibres(int idKit, DateTime fecha)
    {
        List<object> resultado = new List<object>();

        if (HttpContext.Session.GetString("IdUsuario") == null || fecha.Date < DateTime.Today)
        {
            return Json(resultado);
        }

        List<Bloque> libres = BD.ObtenerBloquesLibres(idKit, fecha);

        foreach (Bloque bloque in libres)
        {
            bool yaPaso = fecha.Date == DateTime.Today && bloque.HoraInicio <= DateTime.Now.TimeOfDay;

            if (!yaPaso)
            {
                string horario = bloque.HoraInicio.ToString().Substring(0, 5) + " a " + bloque.HoraFin.ToString().Substring(0, 5);
                resultado.Add(new { id = bloque.Id, numero = bloque.Numero, horario = horario });
            }
        }

        return Json(resultado);
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

[HttpPost]
public IActionResult Pedir(int idKit, DateTime fecha, int idBloque)
{
    string? idTexto = HttpContext.Session.GetString("IdUsuario");
    if (idTexto == null)
    {
        return RedirectToAction("Login", "Cuenta");
    }
    int idUsuario = int.Parse(idTexto);

    Kit? kit = BD.ObtenerKitPorId(idKit);
    if (kit == null)
    {
        return RedirectToAction("Index");
    }

    ViewBag.Kit = kit;

    string error = ValidarPedido(kit, idUsuario, fecha, idBloque);
    if (error != "")
    {
        ViewBag.Error = error;
        return View("DetalleKit");
    }

    string codigo = GenerarCodigo();
    BD.AgregarPrestamo(idKit, idUsuario, fecha, idBloque, codigo);

    ViewBag.Exito = "Préstamo guardado. Tu código es " + codigo + ".";
    return View("DetalleKit");
}

private string ValidarPedido(Kit kit, int idUsuario, DateTime fecha, int idBloque)
{
  if (BD.ContarPrestamosDelDia(idUsuario, fecha) >= 2)
    {
        return "Ya pediste 2 kits para el " + NombreDia(fecha) + " " + fecha.ToString("dd/MM") + ". El máximo es 2 por día.";
    }

    if (fecha.Date < DateTime.Today)
    {
        return "No se puede pedir un kit para una fecha que ya pasó.";
    }

    Bloque? bloque = BD.ObtenerBloquePorId(idBloque);   
    if (bloque == null)
    {
        return "Elegí un bloque.";
    }

    if (fecha.Date == DateTime.Today && bloque.HoraInicio <= DateTime.Now.TimeOfDay)
    {
        return "Ese bloque ya empezó. Elegí otro.";
    }

   

    return "";
}

private string GenerarCodigo()
{
    string caracteres = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
    Random azar = new Random();
    string codigo = "";

    do
    {
        codigo = "";
        for (int i = 0; i < 6; i++)
        {
            codigo = codigo + caracteres[azar.Next(caracteres.Length)];
        }
    } while (BD.ExisteCodigo(codigo));

    return codigo;
}

private string NombreDia(DateTime fecha)
{
    string[] dias = { "domingo", "lunes", "martes", "miércoles", "jueves", "viernes", "sábado" };
    return dias[(int)fecha.DayOfWeek];
}



}