namespace WebApi.Services;

using AutoMapper;
using WebApi.Entities;
using WebApi.Models.Pedido;
using WebApi.Repositories;

public interface IPedidoService
{
    Task<IEnumerable<Pedido>> GetAll();
    Task<Pedido> GetById(int id);
    Task<Pedido> Create(CreateRequest model);
    Task Update(int id, UpdateRequest model);
    Task Delete(int id);
}

public class PedidoService : IPedidoService
{
    private IPedidoRepository _pedidoRepository;
    private IClienteRepository _clienteRepository;
    private readonly IMapper _mapper;

    public PedidoService(
        IPedidoRepository pedidoRepository,
        IClienteRepository clienteRepository,
        IMapper mapper)
    {
        _pedidoRepository = pedidoRepository;
        _clienteRepository = clienteRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Pedido>> GetAll()
        => await _pedidoRepository.GetAll();

    public async Task<Pedido> GetById(int id)
    {
        var result = await _pedidoRepository.GetById(id);
        if (result == null)
            throw new KeyNotFoundException("Pedido não encontrado");

        return result;
    }

    public async Task<Pedido> Create(CreateRequest model)
    {
        var pedido = _mapper.Map<Pedido>(model);
        pedido.DataPedido = DateTime.Now;
        pedido.Status = PedidoStatus.Recebido;

        var cliente = await _clienteRepository.GetByCPF(model?.Cliente?.CPF);
        if(cliente is not null){
            pedido.Cliente = cliente;
        }
        
        pedido = await _pedidoRepository.Create(pedido);

        return pedido ?? new Pedido();
    }

    public async Task Update(int id, UpdateRequest model)
    {
        var pedido = await _pedidoRepository.GetById(id);
        if (pedido == null)
            throw new KeyNotFoundException("Pedido não encontrado");

        _mapper.Map(model, pedido);
        await _pedidoRepository.Update(pedido);
    }

    public async Task Delete(int id)
        => await _pedidoRepository.Delete(id);
}