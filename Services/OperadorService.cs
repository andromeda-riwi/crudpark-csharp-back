using Microsoft.EntityFrameworkCore;
using CrudPark.Api.Data; 
using CrudPark.Api.Models;
using CrudPark.Api.Services;


public class OperadorService : IOperadorService
{
    private readonly AppDbContext _context;

    public OperadorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Operador>> GetAllOperadoresAsync()
    {
        return await _context.Operadores.ToListAsync();
    }

    public async Task<Operador?> GetOperadorByIdAsync(long id)
    {
        return await _context.Operadores.FindAsync(id);
    }

    public async Task<Operador> CreateOperadorAsync(Operador operador)
    {
        // TODO: ¡IMPORTANTE! Aquí debes hashear la contraseña antes de guardarla.
        // operador.PasswordHash = BCrypt.Net.BCrypt.HashPassword(operador.PasswordHash);
        operador.FechaCreacion = DateTimeOffset.UtcNow;
        _context.Operadores.Add(operador);
        await _context.SaveChangesAsync();
        return operador;
    }

    public async Task<Operador?> UpdateOperadorAsync(long id, Operador operador)
    {
        var existingOperador = await _context.Operadores.FindAsync(id);
        if (existingOperador == null) return null;

        existingOperador.Nombre = operador.Nombre;
        existingOperador.Email = operador.Email;
        existingOperador.Activo = operador.Activo;
        await _context.SaveChangesAsync();
        return existingOperador;
    }

    public async Task<bool> DeleteOperadorAsync(long id)
    {
        var operador = await _context.Operadores.FindAsync(id);
        if (operador == null) return false;
        
        operador.Activo = false; // Soft delete
        await _context.SaveChangesAsync();
        return true;
    }
}