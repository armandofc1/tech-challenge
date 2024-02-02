namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Models.Produtos;
using WebApi.Services;

[ApiController]
[Route("[controller]")]
public class ProdutosController : ControllerBase
{
    private IProdutoService _produtoService;

    public ProdutosController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _produtoService.GetAll();
        if(result?.Count() > 0)
            return Ok(result);

        return Ok(new { success = true, message = "Nenhum Produto" });
    }

    [HttpGet("categoria/{categoria}")]
    public async Task<IActionResult> GetAllByCategoria(ProdutoCategoria categoria)
    {
        var result = await _produtoService.GetAllByCategoria(categoria);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _produtoService.GetById(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProdutoCreateRequest model)
    {
        var result = await _produtoService.Create(model);
        return Ok(new { success = true, result, message = $"Produto {result.Id} criado" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProdutoUpdateRequest model)
    {
        await _produtoService.Update(id, model);
        return Ok(new { success = true, message = $"Produto {id} alterado" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _produtoService.Delete(id);
        return Ok(new { success = true, message = $"Produto {id} deletado" });
    }
}