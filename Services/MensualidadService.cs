using Microsoft.EntityFrameworkCore;
using CrudPark.Api.Data;
using CrudPark.Api.Models;

public class MensualidadService : IMensualidadService
{
    private readonly AppDbContext _context;
    // private readonly IEmailService _emailService; // Descomentar cuando tengas el servicio de email

    public MensualidadService(AppDbContext context /*, IEmailService emailService */)
    {
        _context = context;
        // _emailService = emailService;
    }

    public async Task<IEnumerable<Mensualidad>> GetAllAsync()
    {
        return await _context.Mensualidades.ToListAsync();
    }

    public async Task<Mensualidad?> GetByIdAsync(long id)
    {
        return await _context.Mensualidades.FindAsync(id);
    }

    public async Task<Mensualidad> CreateAsync(Mensualidad mensualidad)
    {
        // --- ¡REGLA DE NEGOCIO CLAVE! ---
        // Validar que no exista otra mensualidad activa para la misma placa en un rango de fechas que se solape.
        var existeConflicto = await _context.Mensualidades
            .AnyAsync(m => m.PlacaVehiculo == mensualidad.PlacaVehiculo &&
                           m.Activo &&
                           m.FechaInicio <= mensualidad.FechaFin &&
                           m.FechaFin >= mensualidad.FechaInicio);

        if (existeConflicto)
        {
            throw new InvalidOperationException("Ya existe una mensualidad activa para esta placa en el rango de fechas seleccionado.");
        }

        mensualidad.FechaCreacion = DateTimeOffset.UtcNow;
        _context.Mensualidades.Add(mensualidad);
        await _context.SaveChangesAsync();

        // TODO: Aquí llamarías al servicio de correo.
        // await _emailService.EnviarCorreoBienvenida(mensualidad);

        return mensualidad;
    }

    public async Task<Mensualidad?> UpdateAsync(long id, Mensualidad mensualidad)
    {
        var existing = await _context.Mensualidades.FindAsync(id);
        if (existing == null) return null;

        existing.NombreCliente = mensualidad.NombreCliente;
        existing.CorreoCliente = mensualidad.CorreoCliente;
        existing.PlacaVehiculo = mensualidad.PlacaVehiculo;
        existing.FechaInicio = mensualidad.FechaInicio;
        existing.FechaFin = mensualidad.FechaFin;
        existing.Activo = mensualidad.Activo;
        existing.FechaActualizacion = DateTimeOffset.UtcNow;
        
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var mensualidad = await _context.Mensualidades.FindAsync(id);
        if (mensualidad == null) return false;
        
        mensualidad.Activo = false; // Soft delete
        await _context.SaveChangesAsync();
        return true;
    }
}