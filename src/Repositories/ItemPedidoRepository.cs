namespace WebApi.Repositories;

using Dapper;
using WebApi.Entities;
using WebApi.Helpers;

public interface IItemPedidoRepository
{
    Task<IEnumerable<ItemPedido>?> GetAll();
    Task<IEnumerable<ItemPedido>?> GetAllByPedidoId(int pedidoId);
    Task<ItemPedido?> GetById(int id);
    Task<ItemPedido> Create(ItemPedido itemPedido);
    Task Update(ItemPedido itemPedido);
    Task Delete(int id);
    Task DeleteByPedidoId(int pedidoId);
}

public class ItemPedidoRepository : IItemPedidoRepository
{
    private DataContext _context;

    public ItemPedidoRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ItemPedido>?> GetAll()
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM ItensPedidos
        """;
        return await connection.QueryAsync<ItemPedido>(sql);
    }

    public async Task<IEnumerable<ItemPedido>?> GetAllByPedidoId(int pedidoId)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM ItensPedidos
            WHERE PedidoId = @pedidoId
        """;
        return await connection.QueryAsync<ItemPedido>(sql, new { pedidoId });
    }

    public async Task<ItemPedido?> GetById(int id)
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
            INSERT INTO ItensPedidos (PedidoId, ProdutoId, Preco, Quantidade)
            VALUES (@PedidoId, @ProdutoId, @Preco, @Quantidade)
            RETURNING Id;
        """;
        itemPedido.Id = await connection.ExecuteScalarAsync<int>(sql, new { 
            PedidoId = itemPedido.Pedido.Id,
            ProdutoId = itemPedido.Produto?.Id,
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
            SET ProdutoId = @ProdutoId,
                Preco = @Preco,
                Quantidade = @Quantidade
            WHERE Id = @Id
        """;
        await connection.ExecuteAsync(sql, new { 
            ProdutoId = itemPedido.Produto?.Id,
            itemPedido.Preco,
            itemPedido.Quantidade
        });
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

    public async Task DeleteByPedidoId(int pedidoId)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            DELETE FROM ItensPedidos 
            WHERE PedidoId = @pedidoId
        """;
        await connection.ExecuteAsync(sql, new { pedidoId });
    }
}