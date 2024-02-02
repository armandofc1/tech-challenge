namespace WebApi.Entities;
public class PedidoDetalhe
{
    public int ItemId { get; set; }
    public int? ProdutoId { get; set; }
    public string? Produto { get; set; }
    public double? Preco { get; set; }
    public int? Quantidade { get; set; }
}