using Microsoft.AspNetCore.Mvc;
using CrudPark.Api.Data;
using CrudPark.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace CrudPark.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("metrics")]
    public async Task<IActionResult> GetDashboardMetrics()
    {
        // 1. Definir los puntos de tiempo de forma clara
        var today = DateTime.UtcNow.Date; // Un DateTime puro para el día de hoy a las 00:00 UTC
        var todayAsDateOnly = DateOnly.FromDateTime(today); // La versión DateOnly para comparar con Mensualidades
        var expiringLimit = todayAsDateOnly.AddDays(7);
        var todayEnd = today.AddDays(1); // El inicio del día de mañana

        try
        {
            var metrics = new DashboardMetricsDto
            {
                // Correcto: Busca tickets sin fecha de salida
                VehiculosDentro = await _context.Tickets.CountAsync(t => t.FechaSalida == null),
                
                // Correcto: Busca pagos que ocurrieron entre hoy a las 00:00 y mañana a las 00:00
                IngresosHoy = await _context.Pagos
                    .Where(p => p.FechaPago >= today && p.FechaPago < todayEnd)
                    .SumAsync(p => p.Monto),
                
                // Correcto: Compara la propiedad FechaFin (DateOnly) con una variable DateOnly
                MensualidadesActivas = await _context.Mensualidades
                    .CountAsync(m => m.Activo && m.FechaFin >= todayAsDateOnly),

                // Correcto: Compara FechaFin (DateOnly) con un rango de DateOnly
                MensualidadesProximasAVencer = await _context.Mensualidades
                    .CountAsync(m => m.Activo && m.FechaFin >= todayAsDateOnly && m.FechaFin <= expiringLimit)
            };
            
            return Ok(metrics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error en 'metrics': {ex.Message}. Revise el log del servidor para más detalles.");
        }
    }

    [HttpGet("ingresos-semanales")]
    public async Task<IActionResult> GetIngresosSemanales()
    {
        try
        {
            var today = DateTime.UtcNow.Date;
            var startDate = today.AddDays(-6);
            
            var dailyTotals = await _context.Pagos
                .Where(p => p.FechaPago >= startDate && p.FechaPago < today.AddDays(1))
                .GroupBy(p => p.FechaPago.Date) // Agrupa por la parte de la fecha
                .Select(g => new { Date = g.Key, Total = g.Sum(p => p.Monto) })
                .ToDictionaryAsync(x => x.Date, x => x.Total);
                
            var chartData = new ChartDataDto();
            
            for (int i = 0; i < 7; i++)
            {
                var day = startDate.AddDays(i);
                chartData.Labels.Add(day.ToString("dd/MM"));
                chartData.Data.Add(dailyTotals.TryGetValue(day, out var total) ? total : 0);
            }

            return Ok(chartData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error en 'ingresos-semanales': {ex.Message}.");
        }
    }
}