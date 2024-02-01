using System.ComponentModel.DataAnnotations;
using WebApi.Entities;
namespace WebApi.Models.Produto;

public class UpdateRequest
{
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public Double? Preco { get; set; }

    [Required]
    [EnumDataType(typeof(ProdutoCategoria))]
    public string? Categoria { get; set; }
}