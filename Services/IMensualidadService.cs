using CrudPark.Api.Models;

public interface IMensualidadService
{
    Task<IEnumerable<Mensualidad>> GetAllAsync();
    Task<Mensualidad?> GetByIdAsync(long id);
    Task<Mensualidad> CreateAsync(Mensualidad mensualidad);
    Task<Mensualidad?> UpdateAsync(long id, Mensualidad mensualidad);
    Task<bool> DeleteAsync(long id);
}