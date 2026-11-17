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

// ===== El catalogp =====

async function verMas() {
    let contenedor = document.getElementById("catalogo");
    let boton = document.getElementById("botonVerMas");
    let desde = document.getElementsByClassName("tarjeta").length;

    let respuesta = await fetch("/Kits/VerMas?desde=" + desde);
    let datos = await respuesta.json();

    for (let i = 0; i < datos.kits.length; i++) {
        let kit = datos.kits[i];

        let tarjeta = document.createElement("article");
        tarjeta.className = "tarjeta";
        tarjeta.innerHTML =
            '<img src="/img/' + kit.imagen + '" alt="' + kit.nombre + '">' +
            '<div class="tarjeta-info">' +
            '<h2>' + kit.nombre + '</h2>' +
            '<p class="categoria">' + kit.nombreCategoria + '</p>' +
            '<a href="/Kits/Detalle/' + kit.id + '" class="btn btn-primary">Ver detalle</a>' +
            '</div>';

        contenedor.appendChild(tarjeta);
    }

    if (datos.hayMas === false) {
        boton.style.display = "none";
    }
}