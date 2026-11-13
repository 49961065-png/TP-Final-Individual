namespace LaboratorioKits.Models;

public class Bloque
{
    public int Id { get; set; }
    public int Numero { get; set; }
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
}