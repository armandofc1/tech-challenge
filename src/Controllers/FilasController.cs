namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Services;
using WebApi.Models.Filas;

[ApiController]
[Route("[controller]")]
public class FilasController : ControllerBase
{
    private IPedidoService _pedidoService;

    public FilasController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEnqueued()
    {
        var result = await _pedidoService.GetAllEnqueued();
        if(result?.Count() > 0)
        {
            IList<FilasResponse> fila = new List<FilasResponse>();
            foreach(var item in result)
            {
                fila.Add(new FilasResponse{
                    PedidoId = item.Id,
                    Status = item.Status
                });
            }
            return Ok(fila);
        }
        return Ok(new { success = true, message = "Sem Pedidos na fila" });
    }
}