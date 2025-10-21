using System.ComponentModel.DataAnnotations;

namespace CrudPark.Api.Dtos;

public class CreateOperadorDto
{
    [Required] public string Nombre { get; set; } = string.Empty;
    [Required][EmailAddress] public string Email { get; set; } = string.Empty;
    [Required][MinLength(6)] public string Password { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
}