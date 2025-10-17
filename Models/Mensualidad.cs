using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Mensualidades")]
public class Mensualidad
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [StringLength(150)]
    [Column("nombre_cliente")]
    public string NombreCliente { get; set; } = string.Empty;

    [StringLength(100)]
    [Column("correo_cliente")]
    public string? CorreoCliente { get; set; }

    [Required]
    [StringLength(20)]
    [Column("placa_vehiculo")]
    public string PlacaVehiculo { get; set; } = string.Empty;

    [Column("fecha_inicio")]
    public DateOnly FechaInicio { get; set; }

    [Column("fecha_fin")]
    public DateOnly FechaFin { get; set; }

    [Column("activo")]
    public bool Activo { get; set; } = true;

    [Column("fecha_creacion")]
    public DateTimeOffset FechaCreacion { get; set; }
    
    [Column("fecha_actualizacion")]
    public DateTimeOffset? FechaActualizacion { get; set; }
}