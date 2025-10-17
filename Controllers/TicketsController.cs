using Microsoft.AspNetCore.Mvc;
using CrudPark.Api.Dtos;
using CrudPark.Api.Services;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet("dentro")]
    public async Task<IActionResult> GetVehiculosDentro()
    {
        var vehiculos = await _ticketService.GetVehiculosDentroAsync();
        return Ok(vehiculos);
    }

    [HttpPost("ingreso")]
    public async Task<IActionResult> RegistrarIngreso([FromBody] IngresoRequestDto request)
    {
        try
        {
            // TODO: En un sistema real, el operadorId vendría de un token de autenticación.
            // Por ahora, usaremos un valor fijo para la prueba, ej: 1.
            long operadorId = 1;
            var nuevoTicket = await _ticketService.RegistrarIngresoAsync(request.Placa, operadorId);
            return Ok(nuevoTicket);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/salida")]
    public async Task<IActionResult> RegistrarSalida(long id, [FromBody] SalidaRequestDto request)
    {
        // TODO: Igual que arriba, el operadorId vendría de un token.
        long operadorId = 1;
        var (exitoso, mensaje, ticket) = await _ticketService.RegistrarSalidaAsync(id, operadorId, request.MetodoPago);

        if (!exitoso)
        {
            return BadRequest(new { message = mensaje });
        }

        return Ok(new { message = mensaje, ticket });
    }
}