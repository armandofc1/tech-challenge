namespace WebApi.Services;

using AutoMapper;
using WebApi.Entities;
using WebApi.Models.Pedidos;
using WebApi.Repositories;

public interface IPedidoService
{
    Task<IEnumerable<Pedido>?> GetAll();
    Task<IEnumerable<Pedido>?> GetAllEnqueued();
    Task<Pedido?> GetById(int id);
    Task<Pedido> Create(PedidoCreateRequest model);
    Task Update(int id, PedidoUpdateRequest model);
    Task Delete(int id);
    Task Paid(int id);
    Task Enqueue(int id);
}

public class PedidoService : IPedidoService
{
    private IPedidoRepository _pedidoRepository;
    private IItemPedidoRepository _itemPedidoRepository;
    private IProdutoRepository _produtoRepository;
    private IClienteRepository _clienteRepository;
    private readonly IMapper _mapper;

    public PedidoService(
        IPedidoRepository pedidoRepository,
        IItemPedidoRepository itemPedidoRepository,
        IProdutoRepository produtoRepository,
        IClienteRepository clienteRepository,
        IMapper mapper)
    {
        _pedidoRepository = pedidoRepository;
        _itemPedidoRepository = itemPedidoRepository;
        _produtoRepository = produtoRepository;
        _clienteRepository = clienteRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Pedido>?> GetAll()
        => await _pedidoRepository.GetAll();

    public async Task<IEnumerable<Pedido>?> GetAllEnqueued()
        => await _pedidoRepository.GetAllEnqueued();

    public async Task<Pedido?> GetById(int id)
    {
        var result = await _pedidoRepository.GetById(id);
        if (result == null)
            throw new KeyNotFoundException($"Pedido {id} não encontrado");

        return result;
    }

    public async Task<Pedido> Create(PedidoCreateRequest model)
    {
        var pedido = _mapper.Map<Pedido>(model);
        pedido.DataPedido = DateTime.Now;
        pedido.Status = PedidoStatus.Recebido;

        var cliente = await _clienteRepository.GetByCPF(model.ClienteCPF);
        if(cliente is not null){
            pedido.Cliente = cliente;
        }
        
        pedido = await _pedidoRepository.Create(pedido);
        if(model?.Items?.Count() > 0){
            foreach(var item in model.Items){
                var produto = await _produtoRepository.GetById(item.ProdutoId ?? default);
                if(produto is not null)
                {
                    await _itemPedidoRepository.Create(new ItemPedido{ 
                        Pedido = pedido,
                        Produto = produto,
                        Preco = produto.Preco,
                        Quantidade = item.Quantidade
                    });
                }
            }
        }

        return pedido ?? new Pedido();
    }

    public async Task Update(int id, PedidoUpdateRequest model)
    {
        var pedido = await _pedidoRepository.GetById(id);
        if (pedido == null)
            throw new KeyNotFoundException($"Pedido {id} não encontrado");

        _mapper.Map(model, pedido);
        await _pedidoRepository.Update(pedido);
        if(model?.Items?.Count() > 0){
            await _itemPedidoRepository.DeleteByPedidoId(pedido.Id);
            foreach(var item in model.Items){
                var produto = await _produtoRepository.GetById(item.ProdutoId ?? default);
                if(produto is not null)
                {
                    await _itemPedidoRepository.Create(new ItemPedido{ 
                        Pedido = pedido,
                        Produto = produto,
                        Preco = produto.Preco,
                        Quantidade = item.Quantidade
                    });
                }
            }
        }
    }
    
    public async Task Delete(int id)
    {
        var pedido = await _pedidoRepository.GetById(id);
        if (pedido == null)
            throw new KeyNotFoundException($"Pedido {id} não encontrado");
        
        await _pedidoRepository.Delete(id);
    }

    public async Task Paid(int id)
    {
        var pedido = await _pedidoRepository.GetById(id);
        if (pedido == null)
            throw new KeyNotFoundException($"Pedido {id} não encontrado");
        
        pedido.Status = PedidoStatus.Pago;
        await _pedidoRepository.Update(pedido);
    }

    public async Task Enqueue(int id)
    {
        var pedido = await _pedidoRepository.GetById(id);
        if (pedido == null)
            throw new KeyNotFoundException($"Pedido {id} não encontrado");
        
        pedido.Status = PedidoStatus.EmPreparacao;
        await _pedidoRepository.Update(pedido);
    }
}