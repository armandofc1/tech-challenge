namespace WebApi.Entities;
public class Pedido
{
    public int Id { get; set; }
    public Cliente Cliente { get; set; } = new Cliente();
    public PedidoStatus Status { get; set; }
    public DateTime? DataPedido { get; set; }
    public IEnumerable<PedidoDetalhe>? Items { get; set; }
}