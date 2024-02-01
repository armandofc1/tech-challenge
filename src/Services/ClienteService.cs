namespace WebApi.Services;

using AutoMapper;
using BCrypt.Net;
using WebApi.Entities;
using WebApi.Helpers;
using WebApi.Models.Cliente;
using WebApi.Repositories;

public interface IClienteService
{
    Task<IEnumerable<Cliente>> GetAll();
    Task<Cliente> GetById(int id);
    Task<Cliente> GetByCPF(string CPF);
    Task<Cliente> Create(CreateRequest model);
    Task Update(int id, UpdateRequest model);
    Task Delete(int id);
}

public class ClienteService : IClienteService
{
    private IClienteRepository _clienteRepository;
    private readonly IMapper _mapper;

    public ClienteService(
        IClienteRepository clienteRepository,
        IMapper mapper)
    {
        _clienteRepository = clienteRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Cliente>> GetAll()
        => await _clienteRepository.GetAll();

    public async Task<Cliente> GetById(int id)
    {
        var result = await _clienteRepository.GetById(id);
        if (result == null)
            throw new KeyNotFoundException("Cliente não encontrado");

        return result;
    }

    public async Task<Cliente> GetByCPF(string CPF)
    {
        var result = await _clienteRepository.GetByCPF(CPF);
        if (result == null)
            throw new KeyNotFoundException("Cliente não encontrado");

        return result;
    }

    public async Task<Cliente> Create(CreateRequest model)
    {
        // validate
        if (await _clienteRepository.GetByCPF(model.CPF!) != null)
            throw new AppException("Cliente com o CPF '" + model.CPF + "' já existe");

        // map model to new cliente object
        var cliente = _mapper.Map<Cliente>(model);

        // hash password
        cliente.Senha = BCrypt.HashPassword(model.Senha);

        // save cliente
        return await _clienteRepository.Create(cliente);
    }

    public async Task Update(int id, UpdateRequest model)
    {
        var cliente = await _clienteRepository.GetById(id);

        if (cliente == null)
            throw new KeyNotFoundException("Cliente não encontrado");

        // validate
        var cpfChanged = !string.IsNullOrEmpty(model.CPF) && cliente.CPF != model.CPF;
        if (cpfChanged && await _clienteRepository.GetByCPF(model.CPF!) != null)
            throw new AppException("Cliente com o CPF '" + model.CPF + "' já existe");

        // hash password if it was entered
        if (!string.IsNullOrEmpty(model.Senha))
            cliente.Senha = BCrypt.HashPassword(model.Senha);

        // copy model props to cliente
        _mapper.Map(model, cliente);

        // save cliente
        await _clienteRepository.Update(cliente);
    }

    public async Task Delete(int id)
        => await _clienteRepository.Delete(id);
}