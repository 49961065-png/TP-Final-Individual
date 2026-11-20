using Microsoft.Data.SqlClient;
using Dapper;

namespace TP_Individual_prog.Models;

public class BD
{
private static string _connectionString = @"Server=localhost;DataBase=LaboratorioKits;Integrated Security=True;TrustServerCertificate=True;";
    public static List<Kit> ObtenerKits()
    {
        List<Kit> kits = new List<Kit>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT Kits.Id, Kits.Nombre, Kits.Descripcion, Kits.Imagen, Kits.IdCategoria, Kits.Activo, Categorias.Nombre AS NombreCategoria " +
                           "FROM Kits INNER JOIN Categorias ON Kits.IdCategoria = Categorias.Id " +
                           "WHERE Kits.Activo = 1 ORDER BY Kits.Id";
            kits = connection.Query<Kit>(query).ToList();
        }
        return kits;
    }

    public static List<Bloque> ObtenerBloques()
    {
        List<Bloque> bloques = new List<Bloque>();
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Bloques ORDER BY Numero";
            bloques = connection.Query<Bloque>(query).ToList();
        }
        return bloques;
    }

    public static Usuario? ObtenerUsuarioPorNombre(string nombreUsuario)
    {
        Usuario? usuario = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuarios WHERE NombreUsuario = @pNombreUsuario";
            usuario = connection.QueryFirstOrDefault<Usuario>(query, new { pNombreUsuario = nombreUsuario });
        }
        return usuario;
    }

    public static void AgregarUsuario(Usuario usuario)
    {
        string query = "INSERT INTO Usuarios (NombreUsuario, Contrasena, Nombre, Apellido) VALUES (@pNombreUsuario, @pContrasena, @pNombre, @pApellido)";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pNombreUsuario = usuario.NombreUsuario, pContrasena = usuario.Contrasena, pNombre = usuario.Nombre, pApellido = usuario.Apellido });
        }
    }

public static Kit? ObtenerKitPorId(int id)
{
    Kit? kit = null;
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = "SELECT Kits.Id, Kits.Nombre, Kits.Descripcion, Kits.Imagen, Kits.IdCategoria, Kits.Activo, Categorias.Nombre AS NombreCategoria " +
                       "FROM Kits INNER JOIN Categorias ON Kits.IdCategoria = Categorias.Id " +
                       "WHERE Kits.Id = @pId AND Kits.Activo = 1";
        kit = connection.QueryFirstOrDefault<Kit>(query, new { pId = id });
    }
    return kit;
}





public static List<Bloque> ObtenerBloquesLibres(int idKit, DateTime fecha)
{
    List<Bloque> bloques = new List<Bloque>();
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = "SELECT Bloques.* FROM Bloques " +
                       "LEFT JOIN Prestamos ON Prestamos.IdBloque = Bloques.Id " +
                       "AND Prestamos.IdKit = @pIdKit AND Prestamos.Fecha = @pFecha AND Prestamos.Cancelado = 0 " +
                       "WHERE Prestamos.Id IS NULL " +
                       "ORDER BY Bloques.Numero";
        bloques = connection.Query<Bloque>(query, new { pIdKit = idKit, pFecha = fecha.Date }).ToList();
    }
    return bloques;
}



public static Bloque? ObtenerBloquePorId(int id)
{
    Bloque? bloque = null;
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = "SELECT * FROM Bloques WHERE Id = @pId";
        bloque = connection.QueryFirstOrDefault<Bloque>(query, new { pId = id });
    }
    return bloque;
}

public static bool ExisteCodigo(string codigo)
{
    int cantidad = 0;
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = "SELECT COUNT(*) FROM Prestamos WHERE Codigo = @pCodigo";
        cantidad = connection.QueryFirstOrDefault<int>(query, new { pCodigo = codigo });
    }
    return cantidad > 0;
}

public static void AgregarPrestamo(int idKit, int idUsuario, DateTime fecha, int idBloque, string codigo)
{
    string query = "INSERT INTO Prestamos (IdKit, IdUsuario, Fecha, IdBloque, Codigo) VALUES (@pIdKit, @pIdUsuario, @pFecha, @pIdBloque, @pCodigo)";
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        connection.Execute(query, new { pIdKit = idKit, pIdUsuario = idUsuario, pFecha = fecha.Date, pIdBloque = idBloque, pCodigo = codigo });
    }
}
public static int ContarPrestamosDelDia(int idUsuario, DateTime fecha)
{
    int cantidad = 0;
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = "SELECT COUNT(*) FROM Prestamos WHERE IdUsuario = @pIdUsuario AND Fecha = @pFecha AND Cancelado = 0";
        cantidad = connection.QueryFirstOrDefault<int>(query, new { pIdUsuario = idUsuario, pFecha = fecha.Date });
    }
    return cantidad;
}

public static bool KitOcupado(int idKit, DateTime fecha, int idBloque)
{
    int cantidad = 0;
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = "SELECT COUNT(*) FROM Prestamos WHERE IdKit = @pIdKit AND Fecha = @pFecha AND IdBloque = @pIdBloque AND Cancelado = 0";
        cantidad = connection.QueryFirstOrDefault<int>(query, new { pIdKit = idKit, pFecha = fecha.Date, pIdBloque = idBloque });
    }
    return cantidad > 0;
}



public static List<Prestamo> ObtenerPrestamosDeUsuario(int idUsuario)
{
    List<Prestamo> prestamos = new List<Prestamo>();
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = "SELECT Prestamos.Id, Prestamos.IdKit, Prestamos.IdUsuario, Prestamos.Fecha, Prestamos.IdBloque, Prestamos.Codigo, Prestamos.Cancelado, " +
                       "Kits.Nombre AS NombreKit, Bloques.Numero AS NumeroBloque, Bloques.HoraInicio, Bloques.HoraFin " +
                       "FROM Prestamos " +
                       "INNER JOIN Kits ON Prestamos.IdKit = Kits.Id " +
                       "INNER JOIN Bloques ON Prestamos.IdBloque = Bloques.Id " +
                       "WHERE Prestamos.IdUsuario = @pIdUsuario " +
                       "ORDER BY Prestamos.Fecha, Bloques.Numero";
        prestamos = connection.Query<Prestamo>(query, new { pIdUsuario = idUsuario }).ToList();
    }
    return prestamos;
}


public static Prestamo? ObtenerPrestamoPorId(int id)
{
    Prestamo? prestamo = null;
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = "SELECT * FROM Prestamos WHERE Id = @pId";
        prestamo = connection.QueryFirstOrDefault<Prestamo>(query, new { pId = id });
    }
    return prestamo;
}

public static void CancelarPrestamo(int id)
{
    string query = "UPDATE Prestamos SET Cancelado = 1, FechaCancelacion = GETDATE() WHERE Id = @pId";
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        connection.Execute(query, new { pId = id });
    }
}


}