namespace WebApi.Repositories;

using Dapper;
using WebApi.Entities;
using WebApi.Helpers;

public interface IProdutoRepository
{
    Task<IEnumerable<Produto>?> GetAll();
    Task<IEnumerable<Produto>?> GetAllByCategoria(ProdutoCategoria categoria);
    Task<Produto?> GetById(int id);
    Task<Produto> Create(Produto produto);
    Task Update(Produto produto);
    Task Delete(int id);
}

public class ProdutoRepository : IProdutoRepository
{
    private DataContext _context;

    public ProdutoRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Produto>?> GetAll()
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Produtos
        """;
        return await connection.QueryAsync<Produto>(sql);
    }

    public async Task<IEnumerable<Produto>?> GetAllByCategoria(ProdutoCategoria categoria)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Produtos
            WHERE Categoria = @categoria
        """;
        return await connection.QueryAsync<Produto>(sql, new { categoria });
    }

    public async Task<Produto?> GetById(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            SELECT * FROM Produtos 
            WHERE Id = @id
        """;
        return await connection.QuerySingleOrDefaultAsync<Produto>(sql, new { id });
    }

    public async Task<Produto> Create(Produto produto)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            INSERT INTO Produtos (Nome, Descricao, Categoria, Preco)
            VALUES (@Nome, @Descricao, @Categoria, @Preco)
            RETURNING Id;
        """;
        produto.Id = await connection.ExecuteScalarAsync<int>(sql, produto);
        return produto;
    }

    public async Task Update(Produto produto)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            UPDATE Produtos 
            SET Nome = @Nome,
                Descricao = @Descricao,
                Categoria = @Categoria, 
                Preco = @Preco
            WHERE Id = @Id
        """;
        await connection.ExecuteAsync(sql, produto);
    }

    public async Task Delete(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
            DELETE FROM Produtos 
            WHERE Id = @id
        """;
        await connection.ExecuteAsync(sql, new { id });
    }
}