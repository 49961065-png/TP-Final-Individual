namespace TP_Individual_prog.Models;

public class Prestamo
{
    public int Id { get; set; }
    public int IdKit { get; set; }
    public int IdUsuario { get; set; }
    public DateTime Fecha { get; set; }
    public int IdBloque { get; set; }
    public string Codigo { get; set; } = "";
    public bool Cancelado { get; set; }
    public string NombreKit { get; set; } = "";
    public int NumeroBloque { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
}