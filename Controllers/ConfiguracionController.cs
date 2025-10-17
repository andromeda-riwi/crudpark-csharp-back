// --- USINGS ESENCIALES ---
// Permite usar [ApiController], [Route], ControllerBase, etc.
using Microsoft.AspNetCore.Mvc; 
// Permite usar ILogger para registrar eventos.
using Microsoft.Extensions.Logging; 
// Permite usar Task para programación asíncrona.
using System.Threading.Tasks; 
// Permite usar los atributos de validación como [Required].
using System.ComponentModel.DataAnnotations;

// --- USINGS ESPECÍFICOS DE TU PROYECTO ---
// Dirección donde se encuentra tu nuevo DTO.
using CrudPark.Api.Dtos;
// Dirección donde se encuentra la interfaz de tu servicio.
using CrudPark.Api.Services;

// --- NAMESPACE DEL ARCHIVO ---
// Asegúrate de que coincida con la ubicación de tu archivo.
namespace CrudPark.Api.Controllers;

/// <summary>
/// Controlador para gestionar la configuración global del sistema.
/// </summary>
[ApiController]
[Route("api/configuracion")] // Ruta base para todos los endpoints de configuración.
public class ConfiguracionController : ControllerBase
{
    private readonly ITarifaService _tarifaService;
    private readonly ILogger<ConfiguracionController> _logger;

    public ConfiguracionController(ITarifaService tarifaService, ILogger<ConfiguracionController> logger)
    {
        _tarifaService = tarifaService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene el catálogo completo de todas las plantillas de tarifas disponibles.
    /// </summary>
    /// <returns>Una lista de todas las tarifas predefinidas.</returns>
    [HttpGet("tarifas/catalogo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCatalogoDeTarifas()
    {
        var catalogo = await _tarifaService.GetCatalogoCompletoAsync();
        return Ok(catalogo);
    }

    /// <summary>
    /// Obtiene la tarifa que está actualmente activa en el sistema.
    /// </summary>
    /// <returns>Los detalles de la tarifa activa.</returns>
    [HttpGet("tarifas/activa")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTarifaActiva()
    {
        var tarifaActiva = await _tarifaService.GetTarifaActivaAsync();
        if (tarifaActiva == null)
        {
            _logger.LogWarning("Se intentó obtener la tarifa activa, pero no hay ninguna configurada.");
            return NotFound(new { message = "No hay una tarifa activa configurada en el sistema." });
        }
        return Ok(tarifaActiva);
    }

    /// <summary>
    /// Establece una tarifa del catálogo como la nueva tarifa activa del sistema.
    /// </summary>
    /// <param name="request">Un objeto que contiene el ID de la tarifa a activar.</param>
    /// <returns>Un mensaje de confirmación.</returns>
    [HttpPut("tarifas/activa")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetTarifaActiva([FromBody] SetTarifaActivaRequest request)
    {
        // El framework valida automáticamente el [Required] del DTO.
        // Si el JSON llega vacío o sin "tarifaId", la petición se rechaza con un 400
        // antes de que llegue a este punto del código.

        var success = await _tarifaService.SetTarifaActivaAsync(request.TarifaId);

        if (!success)
        {
            return BadRequest(new { message = $"No se encontró una tarifa con el ID {request.TarifaId} en el catálogo." });
        }
        
        _logger.LogInformation("La tarifa activa del sistema fue cambiada a la tarifa con ID: {TarifaId}", request.TarifaId);
        return Ok(new { message = "La nueva tarifa ha sido activada correctamente." });
    }
}