namespace WebApi.Models.Pedidos;

public class PedidoCreateRequest
{
    public string? ClienteCPF { get; set; }
    public IEnumerable<PedidoItemRequest>? Items { get; set; }
}