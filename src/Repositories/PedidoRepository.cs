namespace WebApi.Repositories;

using Dapper;
using WebApi.Entities;
using WebApi.Helpers;

public interface IPedidoRepository
{
    Task<IEnumerable<Pedido>?> GetAll();
    Task<IEnumerable<Pedido>?> GetAllEnqueued();
    Task<Pedido?> GetById(int id);
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

    public async Task<IEnumerable<Pedido>?> GetAll()
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT p.*, c.* FROM Pedidos p
            LEFT JOIN Clientes c ON
            c.ID = p.ClienteId
        """;
        var entities = await connection.QueryAsync<Pedido, Cliente, Pedido>(
            sql: sql,
            map: (p, c) => { p.Cliente = c; return p; }
        );

        if(entities?.Count() > 0){
            foreach(var entity in entities){
                if(entity is not null){
                    sql = """
                        SELECT i.Id ItemId, p.Id ProdutoId, p.Nome Produto, i.Preco, i.Quantidade FROM ItensPedidos i
                        INNER JOIN Produtos p ON
                        p.Id = i.ProdutoId
                        WHERE i.PedidoId = @id
                    """;
                    entity.Items = await connection.QueryAsync<PedidoDetalhe>(sql, new { id  = entity.Id});
                }
            }
        }

        return entities;
    }

    public async Task<IEnumerable<Pedido>?> GetAllEnqueued()
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Pedidos
            WHERE Status > 1
            ORDER BY DataPedido
        """;
        return await connection.QueryAsync<Pedido>(sql);
    }

    public async Task<Pedido?> GetById(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT p.*, c.* FROM Pedidos p
            LEFT JOIN Clientes c ON
            c.ID = p.ClienteId
            WHERE p.Id = @id
        """;
        var result = await connection.QueryAsync<Pedido, Cliente, Pedido>(
            sql: sql,
            map: (p, c) => { p.Cliente = c; return p; },
            param: new { id }
        );
        var entity = result?.ToList()?.FirstOrDefault();

        if(entity is not null){
            sql = """
                SELECT i.Id ItemId, p.Id ProdutoId, p.Nome Produto, i.Preco, i.Quantidade FROM ItensPedidos i
                INNER JOIN Produtos p ON
                p.Id = i.ProdutoId
                WHERE i.PedidoId = @id
            """;
            entity.Items = await connection.QueryAsync<PedidoDetalhe>(sql, new { id });
        }
        return entity;
    }

    public async Task<Pedido> Create(Pedido pedido)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            INSERT INTO Pedidos (ClienteId, Status, DataPedido)
            VALUES (@ClienteId, @Status, @DataPedido)
            RETURNING Id;
        """;
        pedido.Id = await connection.ExecuteScalarAsync<int>(sql, new { 
            ClienteId = pedido.Cliente?.Id,
            Status = pedido.Status,
            DataPedido = pedido.DataPedido
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