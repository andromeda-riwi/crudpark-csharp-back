namespace CrudPark.Api.Services; 

using CrudPark.Api.Models; 

public interface ITarifaService
{
    Task<IEnumerable<TarifaCatalogo>> GetCatalogoCompletoAsync();

    Task<TarifaCatalogo?> GetTarifaActivaAsync();

    Task<bool> SetTarifaActivaAsync(int tarifaId);
    //Task<Tarifa?> UpdateTarifaAsync(int id, UpdateTarifaDto dto);
}