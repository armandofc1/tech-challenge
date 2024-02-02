using System.ComponentModel.DataAnnotations;
using WebApi.Entities;
using System.Text.Json.Serialization;

namespace WebApi.Models.Pedidos;

public class PedidoUpdateRequest
{
    public IEnumerable<PedidoItemRequest>? Items { get; set; }

    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PedidoStatus? Status { get; set; }
}