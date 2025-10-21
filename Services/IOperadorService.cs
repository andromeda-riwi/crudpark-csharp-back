using CrudPark.Api.Models;
using CrudPark.Api.Dtos;

namespace CrudPark.Api.Services;

public interface IOperadorService
{
    Task<IEnumerable<Operador>> GetAllOperadoresAsync();
    Task<Operador?> GetOperadorByIdAsync(long id);
    Task<Operador> CreateOperadorAsync(CreateOperadorDto operadorDto);
    Task<Operador?> UpdateOperadorAsync(long id, UpdateOperadorDto operadorDto);
    Task<bool> DeleteOperadorAsync(long id);
}