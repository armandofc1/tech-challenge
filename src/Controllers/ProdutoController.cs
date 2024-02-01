namespace WebApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Produto;
using WebApi.Services;

[ApiController]
[Route("[controller]")]
public class ProdutoController : ControllerBase
{
    private IProdutoService _produtoService;

    public ProdutoController(IProdutoService produtoService)
    {
        _produtoService = produtoService;
    }
/*
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _produtoService.GetAll();
        return Ok(result);
    }

    [HttpGet("categoria/{categoria}")]
    public async Task<IActionResult> GetAllByCategoria(string categoria)
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
    public async Task<IActionResult> Create([FromBody] CreateRequest model)
    {
        await _produtoService.Create(model);
        return Ok(new { message = "Produto criado" });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRequest model)
    {
        await _produtoService.Update(id, model);
        return Ok(new { message = "Produto alterado" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _produtoService.Delete(id);
        return Ok(new { message = "Produto deletado" });
    }
    */
}