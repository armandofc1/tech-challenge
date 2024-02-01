namespace WebApi.Repositories;

using Dapper;
using WebApi.Entities;
using WebApi.Helpers;

public interface IItemPedidoRepository
{
    Task<IEnumerable<ItemPedido>> GetAll();
    Task<ItemPedido> GetById(int id);
    Task<ItemPedido> Create(ItemPedido itemPedido);
    Task Update(ItemPedido itemPedido);
    Task Delete(int id);
}

public class ItemPedidoRepository : IItemPedidoRepository
{
    private DataContext _context;

    public ItemPedidoRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ItemPedido>> GetAll()
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM ItensPedidos
        """;
        return await connection.QueryAsync<ItemPedido>(sql);
    }

    public async Task<ItemPedido> GetById(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM ItensPedidos 
            WHERE Id = @id
        """;
        return await connection.QuerySingleOrDefaultAsync<ItemPedido>(sql, new { id });
    }

    public async Task<ItemPedido> Create(ItemPedido itemPedido)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            INSERT INTO ItensPedidos (PedidoId, Preco, Quantidade)
            VALUES (@PedidoId, @Status, @Quantidade)
            RETURNING Id;
        """;
        itemPedido.Id = await connection.ExecuteAsync(sql, new { 
            PedidoId = itemPedido.Pedido.Id,
            itemPedido.Preco,
            itemPedido.Quantidade
        });
        return itemPedido;
    }

    public async Task Update(ItemPedido itemPedido)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            UPDATE ItensPedidos 
            SET Preco = @Preco,
                Quantidade = @Quantidade
            WHERE Id = @Id
        """;
        await connection.ExecuteAsync(sql, itemPedido);
    }

    public async Task Delete(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            DELETE FROM ItensPedidos 
            WHERE Id = @id
        """;
        await connection.ExecuteAsync(sql, new { id });
    }
}