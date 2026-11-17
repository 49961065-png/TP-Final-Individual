using Microsoft.AspNetCore.Mvc;
using TP_Individual_prog.Models;

namespace TP_Individual_prog.Controllers;

public class CuentaController : Controller
{
    public IActionResult Registro()
    {
        return View();
    }

    public IActionResult Login()
{
    return View();
}

[HttpPost]
public IActionResult Login(string nombreUsuario, string contrasena)
{
    Usuario? usuario = BD.ObtenerUsuarioPorNombre(nombreUsuario);

    if (usuario == null || usuario.Contrasena != contrasena)
    {
        ViewBag.Error = "Usuario o contraseña incorrectos.";
        ViewBag.NombreUsuario = nombreUsuario;
        return View();
    }

    HttpContext.Session.SetString("IdUsuario", usuario.Id.ToString());
    HttpContext.Session.SetString("Nombre", usuario.Nombre);
   return RedirectToAction("Index", "Kits");
}

public IActionResult Logout()
{
    HttpContext.Session.Clear();
    return RedirectToAction("Login");
}

    [HttpPost]
    public IActionResult Registro(Usuario usuario, string repetirContrasena)
    {
        string error = ValidarRegistro(usuario, repetirContrasena);

        if (error != "")
        {
            ViewBag.Error = error;
            ViewBag.Nombre = usuario.Nombre;
            ViewBag.Apellido = usuario.Apellido;
            ViewBag.NombreUsuario = usuario.NombreUsuario;
            return View();
        }

        BD.AgregarUsuario(usuario);
        ViewBag.Exito = "Cuenta creada correctamente.";
        return View();
    }

    private string ValidarRegistro(Usuario usuario, string repetirContrasena)
    {
        if (string.IsNullOrWhiteSpace(usuario.Nombre) || string.IsNullOrWhiteSpace(usuario.Apellido) ||
            string.IsNullOrWhiteSpace(usuario.NombreUsuario) || string.IsNullOrWhiteSpace(usuario.Contrasena))
        {
            return "Completá todos los campos.";
        }

        if (usuario.Nombre.Length > 50 || usuario.Apellido.Length > 50 ||
            usuario.NombreUsuario.Length > 50 || usuario.Contrasena.Length > 50)
        {
            return "Ningún campo puede tener más de 50 caracteres.";
        }

        if (usuario.Contrasena.Length < 4)
        {
            return "La contraseña tiene que tener al menos 4 caracteres.";
        }

        if (usuario.Contrasena != repetirContrasena)
        {
            return "Las contraseñas no coinciden.";
        }

        if (BD.ObtenerUsuarioPorNombre(usuario.NombreUsuario) != null)
        {
            return "Ese nombre de usuario ya existe. Elegí otro.";
        }

        return "";
    }
}