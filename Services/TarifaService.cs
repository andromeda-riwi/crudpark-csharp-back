using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CrudPark.Api.Data;
using CrudPark.Api.Models;

namespace CrudPark.Api.Services;

public class TarifaService : ITarifaService
{
    private readonly AppDbContext _context;
    private readonly ILogger<TarifaService> _logger;

    public TarifaService(AppDbContext context, ILogger<TarifaService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<TarifaCatalogo>> GetCatalogoCompletoAsync()
    {
        return await _context.TarifasCatalogo.OrderBy(t => t.Nombre).ToListAsync();
    }

    public async Task<TarifaCatalogo?> GetTarifaActivaAsync()
    {
        var configuracion = await _context.ConfiguracionSistema
            .Include(c => c.TarifaActiva)
            .FirstOrDefaultAsync();

        return configuracion?.TarifaActiva;
    }

    public async Task<bool> SetTarifaActivaAsync(int tarifaId)
    {
        var tarifaExiste = await _context.TarifasCatalogo.AnyAsync(t => t.Id == tarifaId);
        if (!tarifaExiste)
        {
            _logger.LogWarning("Intento de activar una tarifa no existente con ID: {TarifaId}", tarifaId);
            return false;
        }

        var configuracion = await _context.ConfiguracionSistema.FirstOrDefaultAsync();

        if (configuracion == null)
        {
            _context.ConfiguracionSistema.Add(new ConfiguracionSistema
            {
                Id = 1,
                TarifaActivaId = tarifaId
            });
            _logger.LogInformation("Configuración inicial del sistema. Tarifa activa establecida a ID: {TarifaId}", tarifaId);
        }
        else
        {
            configuracion.TarifaActivaId = tarifaId;
            _logger.LogInformation("La tarifa activa fue cambiada a la tarifa con ID: {TarifaId}", tarifaId);
        }

        await _context.SaveChangesAsync();
        return true;
    }
}