using CrudPark.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using CrudPark.Api.Models;
using CrudPark.Api.Services;

[ApiController]
[Route("api/[controller]")]
public class OperadoresController : ControllerBase

{
    private readonly IOperadorService _operadorService;

    public OperadoresController(IOperadorService operadorService) => _operadorService = operadorService;

    [HttpGet]
    public async Task<IActionResult> GetOperadores() => Ok(await _operadorService.GetAllOperadoresAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOperador(long id)
    {
        var operador = await _operadorService.GetOperadorByIdAsync(id);
        return operador == null ? NotFound() : Ok(operador);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOperador([FromBody] CreateOperadorDto operadorDto)
    {
        var createdOperador = await _operadorService.CreateOperadorAsync(operadorDto);
        return CreatedAtAction(nameof(GetOperador), new { id = createdOperador.Id }, createdOperador);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOperador(long id, [FromBody] UpdateOperadorDto operadorDto)
    {
        var updatedOperador = await _operadorService.UpdateOperadorAsync(id, operadorDto);
        return updatedOperador == null ? NotFound() : Ok(updatedOperador);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOperador(long id)
    {
        var result = await _operadorService.DeleteOperadorAsync(id);
        return !result ? NotFound() : NoContent();
    }
}