using CrudPark.Api.Data; // Asumiendo que tu DbContext está aquí
using CrudPark.Api.Models;
using CrudPark.Api.Dtos;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace CrudPark.Api.Services;

public class OperadorService : IOperadorService
{
    private readonly AppDbContext _context;

    public OperadorService(AppDbContext context) { _context = context; }
    
    public async Task<IEnumerable<Operador>> GetAllOperadoresAsync() => await _context.Operadores.ToListAsync();
    
    public async Task<Operador?> GetOperadorByIdAsync(long id) => await _context.Operadores.FindAsync(id);

    public async Task<Operador> CreateOperadorAsync(CreateOperadorDto operadorDto)
    {
        var operador = new Operador
        {
            Nombre = operadorDto.Nombre,
            Email = operadorDto.Email,
            Activo = operadorDto.Activo,
            FechaCreacion = DateTimeOffset.UtcNow,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(operadorDto.Password)
        };

        await _context.Operadores.AddAsync(operador);
        await _context.SaveChangesAsync();
        return operador;
    }
    
    public async Task<Operador?> UpdateOperadorAsync(long id, UpdateOperadorDto operadorDto)
    {
        var operadorExistente = await _context.Operadores.FindAsync(id);
        if (operadorExistente == null) return null;

        operadorExistente.Nombre = operadorDto.Nombre;
        operadorExistente.Email = operadorDto.Email;
        operadorExistente.Activo = operadorDto.Activo;
        
        if (!string.IsNullOrEmpty(operadorDto.Password))
        {
            operadorExistente.PasswordHash = BCrypt.Net.BCrypt.HashPassword(operadorDto.Password);
        }

        await _context.SaveChangesAsync();
        return operadorExistente;
    }
    
    public async Task<bool> DeleteOperadorAsync(long id)
    {
        var operador = await _context.Operadores.FindAsync(id);
        if (operador == null) return false;

        _context.Operadores.Remove(operador);
        await _context.SaveChangesAsync();
        return true;
    }
}