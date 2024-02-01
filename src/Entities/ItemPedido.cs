namespace WebApi.Entities;
public class ItemPedido
{
    public int Id { get; set; }
    public Pedido Pedido { get; set; }
    public double? Preco { get; set; }
    public int? Quantidade { get; set; }
}