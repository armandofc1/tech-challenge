using System.ComponentModel.DataAnnotations;
using WebApi.Entities;
namespace WebApi.Models.Pedido;

public class UpdateRequest
{
    [Required]
    [EnumDataType(typeof(PedidoStatus))]
    public string? Status { get; set; }
}