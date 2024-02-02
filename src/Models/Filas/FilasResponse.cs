using WebApi.Entities;

namespace WebApi.Models.Filas;

public class FilasResponse
{
    public int PedidoId { get; set; }
    public PedidoStatus Status { get; set; }
}