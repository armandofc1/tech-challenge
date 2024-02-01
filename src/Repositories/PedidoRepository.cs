namespace WebApi.Repositories;

using Dapper;
using WebApi.Entities;
using WebApi.Helpers;

public interface IPedidoRepository
{
    Task<IEnumerable<Pedido>> GetAll();
    Task<Pedido> GetById(int id);
    Task<Pedido> Create(Pedido pedido);
    Task Update(Pedido pedido);
    Task Delete(int id);
}

public class PedidoRepository : IPedidoRepository
{
    private DataContext _context;

    public PedidoRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Pedido>> GetAll()
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Pedidos
        """;
        return await connection.QueryAsync<Pedido>(sql);
    }

    public async Task<Pedido> GetById(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Pedidos 
            WHERE Id = @id
        """;
        return await connection.QuerySingleOrDefaultAsync<Pedido>(sql, new { id });
    }

    public async Task<Pedido> Create(Pedido pedido)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            INSERT INTO Pedidos (ClienteId, Status, DataPedido)
            VALUES (@ClienteId, @Status, @DataPedido)
            RETURNING Id;
        """;
        pedido.Id = await connection.ExecuteAsync(sql, new { 
            ClienteId = pedido?.Cliente?.Id,
            Status = pedido?.Status,
            DataPedido = pedido?.DataPedido
        });
        return pedido;
    }

    public async Task Update(Pedido pedido)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            UPDATE Pedidos 
            SET Status = @Status
            WHERE Id = @Id
        """;
        await connection.ExecuteAsync(sql, pedido);
    }

    public async Task Delete(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            DELETE FROM Pedidos 
            WHERE Id = @id
        """;
        await connection.ExecuteAsync(sql, new { id });
    }
}