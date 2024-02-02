namespace WebApi.Services;

using AutoMapper;
using WebApi.Entities;
using WebApi.Models.Produtos;
using WebApi.Repositories;

public interface IProdutoService
{
    Task<IEnumerable<Produto>?> GetAll();
    Task<IEnumerable<Produto>?> GetAllByCategoria(ProdutoCategoria categoria);
    Task<Produto?> GetById(int id);
    Task<Produto> Create(ProdutoCreateRequest model);
    Task Update(int id, ProdutoUpdateRequest model);
    Task Delete(int id);
}

public class ProdutoService : IProdutoService
{
    private IProdutoRepository _produtoRepository;
    private readonly IMapper _mapper;

    public ProdutoService(
        IProdutoRepository produtoRepository,
        IMapper mapper)
    {
        _produtoRepository = produtoRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Produto>?> GetAll()
        => await _produtoRepository.GetAll();

    public async Task<IEnumerable<Produto>?> GetAllByCategoria(ProdutoCategoria categoria)
        => await _produtoRepository.GetAllByCategoria(categoria);

    public async Task<Produto?> GetById(int id)
    {
        var result = await _produtoRepository.GetById(id);
        if (result == null)
            throw new KeyNotFoundException($"Produto {id} não encontrado");

        return result;
    }

    public async Task<Produto> Create(ProdutoCreateRequest model)
    {
        var produto = _mapper.Map<Produto>(model);
        return await _produtoRepository.Create(produto);
    }

    public async Task Update(int id, ProdutoUpdateRequest model)
    {
        var produto = await _produtoRepository.GetById(id);
        if (produto == null)
            throw new KeyNotFoundException($"Produto {id} não encontrado");

        _mapper.Map(model, produto);
        await _produtoRepository.Update(produto);
    }

    public async Task Delete(int id)
    {
        var produto = await _produtoRepository.GetById(id);
        if (produto == null)
            throw new KeyNotFoundException($"Produto {id} não encontrado");

        await _produtoRepository.Delete(id);
    }
}