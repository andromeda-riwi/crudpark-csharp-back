using System.ComponentModel.DataAnnotations;

public class UpdateTarifaDto
{
    [Required] public string Nombre { get; set; }
    [Required] public decimal ValorHora { get; set; }
    public decimal ValorFraccion { get; set; }
    public decimal TopeDiario { get; set; }
    [Required] public int TiempoGraciaMinutos { get; set; }
}