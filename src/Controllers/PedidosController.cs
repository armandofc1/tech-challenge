namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Pedidos;
using WebApi.Services;

[ApiController]
[Route("[controller]")]
public class PedidosController : ControllerBase
{
    private IPedidoService _pedidoService;

    public PedidosController(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _pedidoService.GetAll();
        if(result?.Count() > 0)
            return Ok(result);

        return Ok(new { success = true, message = "Nenhum Pedido" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _pedidoService.GetById(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PedidoCreateRequest model)
    {
        var result = await _pedidoService.Create(model);
        return Ok(new { success = true, result, message = $"Pedido {result.Id} criado" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] PedidoUpdateRequest model)
    {
        await _pedidoService.Update(id, model);
        return Ok(new { success = true, message = $"Pedido {id} alterado" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _pedidoService.Delete(id);
        return Ok(new { success = true, message = $"Pedido {id} deletado" });
    }

    [HttpPut("checkout/{id}")]
    public async Task<IActionResult> Checkout(int id)
    {
        await _pedidoService.Paid(id);
        await _pedidoService.Enqueue(id);
        return Ok(new { success = true, message = $"Pedido {id} pago e enviado para a fila" });
    }
}