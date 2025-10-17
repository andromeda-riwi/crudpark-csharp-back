using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Representa una plantilla de tarifa predefinida en el catálogo del sistema.
/// El administrador selecciona una de estas tarifas para que sea la activa.
/// </summary>
[Table("TarifasCatalogo")]
public class TarifaCatalogo
{
    /// <summary>
    /// Identificador único de la plantilla de tarifa.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Nombre descriptivo de la tarifa, visible para el administrador.
    /// Ejemplo: "Tarifa Carros Medellín 2025", "Tarifa Motos Medellín 2025".
    /// </summary>
    [Required]
    [StringLength(100)]
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// El costo por una hora completa de estacionamiento.
    /// </summary>
    [Required]
    [Column("valor_hora", TypeName = "decimal(10, 2)")]
    public decimal ValorHora { get; set; }

    /// <summary>
    /// El costo por una fracción de tiempo (ej. cada 15 minutos).
    /// </summary>
    [Required]
    [Column("valor_fraccion", TypeName = "decimal(10, 2)")]
    public decimal ValorFraccion { get; set; }

    /// <summary>
    /// El monto máximo que se puede cobrar en un periodo de 24 horas.
    /// </summary>
    [Required]
    [Column("tope_diario", TypeName = "decimal(10, 2)")]
    public decimal TopeDiario { get; set; }

    /// <summary>
    /// El tiempo en minutos que un vehículo puede permanecer sin generar cobro.
    /// </summary>
    [Required]
    [Column("tiempo_gracia_minutos")]
    public int TiempoGraciaMinutos { get; set; }
}