namespace CrudPark.Api.Dtos;

public class DashboardMetricsDto
{
    public int VehiculosDentro { get; set; }
    public decimal IngresosHoy { get; set; }
    public int MensualidadesActivas { get; set; }
    public int MensualidadesProximasAVencer { get; set; }
}