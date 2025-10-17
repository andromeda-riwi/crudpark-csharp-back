using System.ComponentModel.DataAnnotations.Schema;
using CrudPark.Api.Enums;
namespace CrudPark.Api.Models; 

[Table("Tickets")]
public class Ticket
{
    public long Id { get; set; }
    public string Placa { get; set; } = string.Empty;
    public TipoIngreso Tipo { get; set; }
    public DateTimeOffset FechaIngreso { get; set; }
    public DateTimeOffset? FechaSalida { get; set; } // Nullable: si es null, el vehículo está dentro.
    public EstadoTicket Estado { get; set; } = EstadoTicket.DENTRO;

    public long OperadorIngresoId { get; set; }
    [ForeignKey("OperadorIngresoId")]
    public Operador? OperadorIngreso { get; set; }

    public long? OperadorSalidaId { get; set; }
    [ForeignKey("OperadorSalidaId")]
    public Operador? OperadorSalida { get; set; }
    
    public long? MensualidadId { get; set; }
    [ForeignKey("MensualidadId")]
    public Mensualidad? Mensualidad { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? MontoPagado { get; set; }
}