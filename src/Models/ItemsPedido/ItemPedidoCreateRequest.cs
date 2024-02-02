using System.ComponentModel.DataAnnotations;
namespace WebApi.Models.ItemsPedido;

public class ItemPedidoCreateRequest
{
    [Required]
    public int? PedidoId { get; set; }
    
    [Required]
    public int? ProdutoId { get; set; }

    [Required]
    public int? Quantidade { get; set; }
}