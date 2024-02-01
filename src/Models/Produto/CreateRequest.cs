using System.ComponentModel.DataAnnotations;
using WebApi.Entities;
namespace WebApi.Models.Produto;

public class CreateRequest
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string? Nome { get; set; }

    [MaxLength(250)]
    public string? Descricao { get; set; }

    [Required]
    [EnumDataType(typeof(ProdutoCategoria))]
    public string? Categoria { get; set; }

    [Required]
    public Double? Preco { get; set; }
}