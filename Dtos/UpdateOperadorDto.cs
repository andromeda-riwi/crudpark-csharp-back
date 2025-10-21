using System.ComponentModel.DataAnnotations;

namespace CrudPark.Api.Dtos;

public class UpdateOperadorDto
{
    [Required] public string Nombre { get; set; } = string.Empty;
    [Required][EmailAddress] public string Email { get; set; } = string.Empty;
    // La contraseña es opcional al editar
    [MinLength(6)] public string? Password { get; set; }
    public bool Activo { get; set; }
}