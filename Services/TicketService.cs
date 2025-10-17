using Microsoft.EntityFrameworkCore;
using CrudPark.Api.Data;
using CrudPark.Api.Models;
using CrudPark.Api.Enums;

public class TicketService : ITicketService
{
    private readonly AppDbContext _context;

    public TicketService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Ticket>> GetVehiculosDentroAsync()
    {
        return await _context.Tickets.Where(t => t.Estado == EstadoTicket.DENTRO).ToListAsync();
    }

    public async Task<Ticket> RegistrarIngresoAsync(string placa, long operadorId)
    {
        // Regla de Negocio: No se puede registrar un ingreso si ya hay un ticket abierto para la misma placa.
        var ticketAbierto = await _context.Tickets.AnyAsync(t => t.Placa == placa && t.Estado == EstadoTicket.DENTRO);
        if (ticketAbierto)
        {
            throw new InvalidOperationException($"La placa '{placa}' ya tiene un ingreso registrado y no ha salido.");
        }

        // Regla de Negocio: Verificar si la placa corresponde a una mensualidad activa.
        var fechaActual = DateOnly.FromDateTime(DateTime.UtcNow);
        var mensualidadActiva = await _context.Mensualidades
            .FirstOrDefaultAsync(m => m.PlacaVehiculo == placa && m.Activo && fechaActual >= m.FechaInicio && fechaActual <= m.FechaFin);

        var nuevoTicket = new Ticket
        {
            Placa = placa,
            FechaIngreso = DateTimeOffset.UtcNow,
            OperadorIngresoId = operadorId,
            Tipo = mensualidadActiva != null ? TipoIngreso.Mensualidad : TipoIngreso.Invitado,
            MensualidadId = mensualidadActiva?.Id
        };

        _context.Tickets.Add(nuevoTicket);
        await _context.SaveChangesAsync();

        return nuevoTicket;
    }

    public async Task<(bool Exitoso, string Mensaje, Ticket? Ticket)> RegistrarSalidaAsync(long ticketId, long operadorId, MetodoPago metodoPago)
    {
        // Usamos una transacción para asegurar que todas las operaciones (actualizar ticket, crear pago) se completen o ninguna lo haga.
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket == null) return (false, "El ticket no existe.", null);
            if (ticket.Estado != EstadoTicket.DENTRO) return (false, "Este ticket ya fue procesado.", null);

            ticket.FechaSalida = DateTimeOffset.UtcNow;
            ticket.OperadorSalidaId = operadorId;
            
            // Si es de mensualidad, la salida es sin cobro.
            if (ticket.Tipo == TipoIngreso.Mensualidad)
            {
                ticket.Estado = EstadoTicket.PAGADO; // Lo marcamos como "pagado" para cerrar el ciclo.
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return (true, "Salida de mensualidad registrada exitosamente.", ticket);
            }

            // --- LÓGICA DE COBRO PARA INVITADOS ---
            var tarifaActiva = await _context.ConfiguracionSistema
                .Include(cs => cs.TarifaActiva)
                .Select(cs => cs.TarifaActiva)
                .FirstOrDefaultAsync();
            
            if (tarifaActiva == null) return (false, "No hay una tarifa activa configurada en el sistema.", null);

            var tiempoEstadia = ticket.FechaSalida.Value - ticket.FechaIngreso;
            var minutosTotales = (int)tiempoEstadia.TotalMinutes;

            // Regla de Negocio: Tiempo de gracia.
            if (minutosTotales <= tarifaActiva.TiempoGraciaMinutos)
            {
                ticket.MontoPagado = 0;
                ticket.Estado = EstadoTicket.PAGADO;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return (true, $"Salida registrada dentro del tiempo de gracia ({tarifaActiva.TiempoGraciaMinutos} min). Sin cobro.", ticket);
            }

            // Regla de Negocio: Cálculo del cobro.
            // (Esta es una lógica simple, puede hacerse tan compleja como se quiera)
            int horasCompletas = minutosTotales / 60;
            int minutosFraccion = minutosTotales % 60;
            decimal montoTotal = horasCompletas * tarifaActiva.ValorHora;
            if (minutosFraccion > 0)
            {
                montoTotal += tarifaActiva.ValorFraccion; // Lógica simple: si hay fracción, se cobra una completa.
            }

            // Regla de Negocio: Tope diario.
            if (montoTotal > tarifaActiva.TopeDiario)
            {
                montoTotal = tarifaActiva.TopeDiario;
            }
            
            ticket.MontoPagado = montoTotal;
            ticket.Estado = EstadoTicket.PAGADO;

            // Creamos el registro del pago.
            var nuevoPago = new Pago
            {
                TicketId = ticket.Id,
                Monto = montoTotal,
                FechaPago = DateTimeOffset.UtcNow,
                Metodo = metodoPago,
                OperadorId = operadorId
            };
            _context.Pagos.Add(nuevoPago);
            
            await _context.SaveChangesAsync();
            await transaction.CommitAsync(); // Confirmamos todos los cambios en la base de datos.

            return (true, "Salida y pago registrados exitosamente.", ticket);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(); // Si algo falla, revertimos todos los cambios.
            return (false, $"Error interno: {ex.Message}", null);
        }
    }
}