namespace LaboratorioKits.Models;

public class Kit
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public string Imagen { get; set; } = "";
    public int IdCategoria { get; set; }
    public bool Activo { get; set; }
    public string NombreCategoria { get; set; } = "";   // viene del JOIN con Categorias
}