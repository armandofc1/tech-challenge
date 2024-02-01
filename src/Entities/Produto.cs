namespace WebApi.Entities;
public class Produto
{
    public int Id { get; set; }
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public ProdutoCategoria Categoria{ get; set; }
    public double? Preco { get; set; }
}