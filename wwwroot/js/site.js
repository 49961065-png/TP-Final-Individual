// ===== Registro =====

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

// ===== Catálogo =====

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
            '<a href="/Kits/DetalleKit/' + kit.id + '" class="btn btn-primary">Ver detalle</a>' +
            '</div>';

        contenedor.appendChild(tarjeta);
    }

    if (datos.hayMas === false) {
        boton.style.display = "none";
    }
}

// ===== Detalle del kit =====

async function cargarBloques() {
    let idKit = document.getElementById("IdKit").value;
    let fecha = document.getElementById("Fecha").value;
    let select = document.getElementById("IdBloque");
    let mensaje = document.getElementById("mensajeBloques");
    let boton = document.getElementById("botonPedir");

    select.innerHTML = "";
    boton.disabled = true;

    if (fecha === "") {
        mensaje.innerHTML = "Elegí una fecha.";
        return;
    }

    let respuesta = await fetch("/Kits/BloquesLibres?idKit=" + idKit + "&fecha=" + fecha);
    let bloques = await respuesta.json();

    if (bloques.length === 0) {
        mensaje.innerHTML = "No hay bloques libres para esa fecha.";
        return;
    }

    for (let i = 0; i < bloques.length; i++) {
        let opcion = document.createElement("option");
        opcion.value = bloques[i].id;
        opcion.innerHTML = bloques[i].numero + ".° bloque (" + bloques[i].horario + ")";
        select.appendChild(opcion);
    }

    mensaje.innerHTML = "";
    boton.disabled = false;
}   