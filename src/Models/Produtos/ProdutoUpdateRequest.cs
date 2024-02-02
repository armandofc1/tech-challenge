using System.Text.Json.Serialization;
using WebApi.Entities;
namespace WebApi.Models.Produtos;

public class ProdutoUpdateRequest
{
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public Double? Preco { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProdutoCategoria? Categoria { get; set; }
}