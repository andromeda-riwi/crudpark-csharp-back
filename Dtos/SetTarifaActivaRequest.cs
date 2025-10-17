using System.ComponentModel.DataAnnotations;

// El namespace debe reflejar la nueva carpeta
namespace CrudPark.Api.Dtos;

/// <summary>
/// DTO (Data Transfer Object) para la petición de establecer una tarifa activa.
/// Define la estructura del JSON que el frontend debe enviar en el cuerpo de la petición.
/// </summary>
public class SetTarifaActivaRequest
{
    /// <summary>
    /// El ID de la tarifa del catálogo que se desea activar.
    /// El atributo [Required] asegura que este valor no puede ser nulo o cero.
    /// </summary>
    [Required(ErrorMessage = "El ID de la tarifa es obligatorio.")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID de la tarifa debe ser un número positivo.")]
    public int TarifaId { get; set; }
}