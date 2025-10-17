namespace CrudPark.Api.Services; 
using CrudPark.Api.Models; 

public interface IOperadorService
{
    Task<IEnumerable<Operador>> GetAllOperadoresAsync();
    Task<Operador?> GetOperadorByIdAsync(long id);
    Task<Operador> CreateOperadorAsync(Operador operador);
    Task<Operador?> UpdateOperadorAsync(long id, Operador operador);
    Task<bool> DeleteOperadorAsync(long id);
}