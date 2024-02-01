namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Pedido;
using WebApi.Services;

[ApiController]
[Route("[controller]")]
public class PedidoContoller : ControllerBase
{
    private IPedidoService _pedidoService;

    public PedidoContoller(IPedidoService pedidoService)
    {
        _pedidoService = pedidoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _pedidoService.GetAll();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _pedidoService.GetById(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRequest model)
    {
        await _pedidoService.Create(model);
        return Ok(new { message = "Pedido criado" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRequest model)
    {
        await _pedidoService.Update(id, model);
        return Ok(new { message = "Pedido alterado" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _pedidoService.Delete(id);
        return Ok(new { message = "Pedido deletado" });
    }
}