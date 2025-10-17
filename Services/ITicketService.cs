using CrudPark.Api.Models;
using CrudPark.Api.Enums;

public interface ITicketService
{
    Task<Ticket> RegistrarIngresoAsync(string placa, long operadorId);
    Task<(bool Exitoso, string Mensaje, Ticket? Ticket)> RegistrarSalidaAsync(long ticketId, long operadorId, MetodoPago metodoPago);
    Task<IEnumerable<Ticket>> GetVehiculosDentroAsync();
}