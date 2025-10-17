using Microsoft.AspNetCore.Mvc;
using CrudPark.Api.Models;
using CrudPark.Api.Services;

[ApiController]
[Route("api/[controller]")]
public class MensualidadesController : ControllerBase
{
    private readonly IMensualidadService _mensualidadService;

    public MensualidadesController(IMensualidadService mensualidadService)
    {
        _mensualidadService = mensualidadService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMensualidades()
    {
        var mensualidades = await _mensualidadService.GetAllAsync();
        return Ok(mensualidades);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMensualidad(long id)
    {
        var mensualidad = await _mensualidadService.GetByIdAsync(id);
        if (mensualidad == null) return NotFound();
        return Ok(mensualidad);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMensualidad([FromBody] Mensualidad mensualidad)
    {
        try
        {
            var created = await _mensualidadService.CreateAsync(mensualidad);
            return CreatedAtAction(nameof(GetMensualidad), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            // Devolvemos un error 400 (Bad Request) con el mensaje de la validación.
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            // Error genérico por si algo más falla
            return StatusCode(500, "Ocurrió un error interno al crear la mensualidad.");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMensualidad(long id, [FromBody] Mensualidad mensualidad)
    {
        var updated = await _mensualidadService.UpdateAsync(id, mensualidad);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMensualidad(long id)
    {
        var result = await _mensualidadService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}