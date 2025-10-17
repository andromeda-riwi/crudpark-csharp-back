using Microsoft.AspNetCore.Mvc;
using CrudPark.Api.Models;
using CrudPark.Api.Services;

[ApiController]
[Route("api/[controller]")]
public class OperadoresController : ControllerBase

{
    private readonly IOperadorService _operadorService;

    public OperadoresController(IOperadorService operadorService)
    {
        _operadorService = operadorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOperadores()
    {
        var operadores = await _operadorService.GetAllOperadoresAsync();
        return Ok(operadores);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOperador(long id)
    {
        var operador = await _operadorService.GetOperadorByIdAsync(id);
        if (operador == null) return NotFound();
        return Ok(operador);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOperador([FromBody] Operador operador)
    {
        if (operador == null) return BadRequest();
        var createdOperador = await _operadorService.CreateOperadorAsync(operador);
        return CreatedAtAction(nameof(GetOperador), new { id = createdOperador.Id }, createdOperador);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOperador(long id, [FromBody] Operador operador)
    {
        var updatedOperador = await _operadorService.UpdateOperadorAsync(id, operador);
        if (updatedOperador == null) return NotFound();
        return Ok(updatedOperador);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOperador(long id)
    {
        var result = await _operadorService.DeleteOperadorAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}