using System.ComponentModel.DataAnnotations;
namespace WebApi.Models.Pedidos;

public class PedidoItemRequest
{
    [Required]
    public int? ProdutoId { get; set; }

    [Required]
    public int? Quantidade { get; set; }
}