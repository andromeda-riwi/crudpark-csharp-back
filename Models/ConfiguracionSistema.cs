using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Representa la configuración global del sistema de parqueadero.
/// Esta tabla está diseñada para tener una única fila (con Id = 1)
/// que almacena los ajustes principales de la aplicación.
/// </summary>
[Table("ConfiguracionSistema")]
public class ConfiguracionSistema
{
    /// <summary>
    /// La llave primaria de la tabla de configuración. Se espera que siempre sea 1.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// La llave foránea que apunta al Id de la tarifa en la tabla 'TarifasCatalogo'
    /// que está actualmente activa y en uso para los cálculos de cobro.
    /// </summary>
    [Column("tarifa_activa_id")]
    public int TarifaActivaId { get; set; }

    /// <summary>
    /// Propiedad de navegación de Entity Framework para cargar el objeto completo
    /// de la tarifa activa. Esto permite acceder a los detalles de la tarifa
    /// (ej. ValorHora, TopeDiario) directamente desde el objeto de configuración.
    /// </summary>
    [ForeignKey("TarifaActivaId")]
    public TarifaCatalogo? TarifaActiva { get; set; }
}

