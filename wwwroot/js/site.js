function validarRegistro() {
    let nombre = document.getElementById("Nombre").value;
    let apellido = document.getElementById("Apellido").value;
    let usuario = document.getElementById("NombreUsuario").value;
    let contrasena = document.getElementById("Contrasena").value;
    let repetir = document.getElementById("repetirContrasena").value;
    let mensaje = document.getElementById("mensajeError");

    if (nombre === "" || apellido === "" || usuario === "" || contrasena === "" || repetir === "") {
        mensaje.innerHTML = "Completá todos los campos.";
        return false;
    }

    if (contrasena.length < 4) {
        mensaje.innerHTML = "La contraseña tiene que tener al menos 4 caracteres.";
        return false;
    }

    if (contrasena !== repetir) {
        mensaje.innerHTML = "Las contraseñas no coinciden.";
        return false;
    }

    mensaje.innerHTML = "";
    return true;
}