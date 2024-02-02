namespace WebApi.Repositories;

using Dapper;
using WebApi.Entities;
using WebApi.Helpers;

public interface IClienteRepository
{
    Task<IEnumerable<Cliente>?> GetAll();
    Task<Cliente?> GetById(int id);
    Task<Cliente?> GetByCPF(string? cpf);
    Task<Cliente> Create(Cliente cliente);
    Task Update(Cliente cliente);
    Task Delete(int id);
}

public class ClienteRepository : IClienteRepository
{
    private DataContext _context;

    public ClienteRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Cliente>?> GetAll()
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Clientes
        """;
        return await connection.QueryAsync<Cliente>(sql);
    }

    public async Task<Cliente?> GetById(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Clientes 
            WHERE Id = @id
        """;
        return await connection.QuerySingleOrDefaultAsync<Cliente>(sql, new { id });
    }

    public async Task<Cliente?> GetByCPF(string? cpf)
    {
        if(!string.IsNullOrEmpty(cpf))
        {
            using var connection = _context.CreateConnection();
            var sql = """
                SELECT * FROM Clientes 
                WHERE CPF = @cpf
            """;
            return await connection.QuerySingleOrDefaultAsync<Cliente>(sql, new { cpf });
        }
        return null;
    }

    public async Task<Cliente> Create(Cliente cliente)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            INSERT INTO Clientes (Nome, SobreNome, CPF, Email, Senha)
            VALUES (@Nome, @SobreNome, @CPF, @Email, @Senha)
            RETURNING Id;
        """;
        cliente.Id = await connection.ExecuteScalarAsync<int>(sql, cliente);
        return cliente;
    }

    public async Task Update(Cliente cliente)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            UPDATE Clientes 
            SET Nome = @Nome,
                SobreNome = @SobreNome,
                CPF = @CPF, 
                Email = @Email, 
                Senha = @Senha
            WHERE Id = @Id
        """;
        await connection.ExecuteAsync(sql, cliente);
    }

    public async Task Delete(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            DELETE FROM Clientes 
            WHERE Id = @id
        """;
        await connection.ExecuteAsync(sql, new { id });
    }
}