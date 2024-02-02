namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Clientes;
using WebApi.Services;

[ApiController]
[Route("[controller]")]
public class ClientesController : ControllerBase
{
    private IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _clienteService.GetAll();
        if(result?.Count() > 0)
            return Ok(result);

        return Ok(new { success = true, message = "Nenhum Cliente" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _clienteService.GetById(id);
        return Ok(result);
    }

    [HttpGet("cpf/{cpf}")]
    public async Task<IActionResult> GetByCPF(string cpf)
    {
        var result = await _clienteService.GetByCPF(cpf);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClienteCreateRequest model)
    {
        var result = await _clienteService.Create(model);
        return Ok(new { success = true, result, message = $"Cliente {result.Id} criado" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateRequest model)
    {
        await _clienteService.Update(id, model);
        return Ok(new { success = true, message = $"Cliente {id} alterado" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _clienteService.Delete(id);
        return Ok(new { success = true, message = $"Cliente {id} deletado" });
    }
}