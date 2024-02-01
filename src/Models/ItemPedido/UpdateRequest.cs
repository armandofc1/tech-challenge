using System.ComponentModel.DataAnnotations;
namespace WebApi.Models.ItemPedido;

public class UpdateRequest
{
    [Required]
    public int? PedidoId { get; set; }
    
    [Required]
    public Double? Preco { get; set; }

    [Required]
    public int? Quantidade { get; set; }
}