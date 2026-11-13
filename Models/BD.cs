using Microsoft.Data.SqlClient;
using Dapper;

namespace LaboratorioKits.Models;

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
}