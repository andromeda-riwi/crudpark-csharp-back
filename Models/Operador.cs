using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CrudPark.Api.Models; 


[Table("Operadores")]
public class Operador
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Required]
    [StringLength(100)]
    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(100)]
    [Column("email")]
    public string? Email { get; set; }

    [Required]
    [StringLength(255)]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("activo")]
    public bool Activo { get; set; } = true;

    [Column("fecha_creacion")]
    public DateTimeOffset FechaCreacion { get; set; }
}