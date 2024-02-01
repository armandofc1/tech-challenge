namespace WebApi.Services;

using AutoMapper;
using WebApi.Entities;
using WebApi.Models.Produto;
using WebApi.Repositories;

public interface IProdutoService
{
    Task<IEnumerable<Produto>> GetAll();
    Task<IEnumerable<Produto>> GetAllByCategoria(string categoria);
    Task<Produto> GetById(int id);
    Task<Produto> Create(CreateRequest model);
    Task Update(int id, UpdateRequest model);
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

    public async Task<IEnumerable<Produto>> GetAll()
        => await _produtoRepository.GetAll();

    public async Task<IEnumerable<Produto>> GetAllByCategoria(string categoria)
        => await _produtoRepository.GetAllByCategoria(categoria);

    public async Task<Produto> GetById(int id)
    {
        var result = await _produtoRepository.GetById(id);
        if (result == null)
            throw new KeyNotFoundException("Produto não encontrado");

        return result;
    }

    public async Task<Produto> Create(CreateRequest model)
    {
        var produto = _mapper.Map<Produto>(model);
        return await _produtoRepository.Create(produto);
    }

    public async Task Update(int id, UpdateRequest model)
    {
        var produto = await _produtoRepository.GetById(id);
        if (produto == null)
            throw new KeyNotFoundException("Produto não encontrado");

        _mapper.Map(model, produto);
        await _produtoRepository.Update(produto);
    }

    public async Task Delete(int id)
        => await _produtoRepository.Delete(id);
}