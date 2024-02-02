using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebApi.Entities;
namespace WebApi.Models.Produtos;

public class ProdutoCreateRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string? Nome { get; set; }

    [MaxLength(250)]
    public string? Descricao { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProdutoCategoria? Categoria { get; set; }

    [Required]
    public Double? Preco { get; set; }
}