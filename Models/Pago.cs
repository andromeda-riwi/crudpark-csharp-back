using System.ComponentModel.DataAnnotations.Schema;
using CrudPark.Api.Enums;
namespace CrudPark.Api.Models; 


[Table("Pagos")]
public class Pago
{
    public long Id { get; set; }
    public long TicketId { get; set; } // Cada pago está ligado a un ticket
    [ForeignKey("TicketId")]
    public Ticket? Ticket { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Monto { get; set; }
    public DateTimeOffset FechaPago { get; set; }
    public MetodoPago Metodo { get; set; }
    
    public long OperadorId { get; set; }
    [ForeignKey("OperadorId")]
    public Operador? Operador { get; set; }
}