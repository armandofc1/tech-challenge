namespace WebApi.Helpers;

using System.Data;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

public class DataContext
{
    private DbSettings _dbSettings;

    public DataContext(IOptions<DbSettings> dbSettings)
    {
        _dbSettings = dbSettings.Value;
    }

    public IDbConnection CreateConnection()
    {
        var connectionString = $"Host={_dbSettings.Server}; Database={_dbSettings.Database}; Username={_dbSettings.UserId}; Password={_dbSettings.Password};";
        return new NpgsqlConnection(connectionString);
    }

    public async Task Init()
    {
        var created = await _initDatabase();
        await _initTables();
        if(created)
            await _initData();
    }

    private async Task<bool> _initDatabase()
    {
        // create database if it doesn't exist
        var connectionString = $"Host={_dbSettings.Server}; Database=postgres; Username={_dbSettings.UserId}; Password={_dbSettings.Password};";
        using var connection = new NpgsqlConnection(connectionString);
        var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{_dbSettings.Database}';";
        var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount);
        if (dbCount == 0)
        {
            var sql = $"CREATE DATABASE \"{_dbSettings.Database}\"";
            await connection.ExecuteAsync(sql);
            return true;
        }
        return false;
    }

    private async Task _initTables()
    {
        // create tables if they don't exist
        using var connection = CreateConnection();
        await _initUsers();
        await _initCliente();
        await _initProduto();
        await _initPedido();
        await _initItensPedido();

        async Task _initUsers()
        {
            var sql = """
                CREATE TABLE IF NOT EXISTS Users (
                    Id SERIAL PRIMARY KEY,
                    Title VARCHAR(25),
                    FirstName VARCHAR(100),
                    LastName VARCHAR(200),
                    Email VARCHAR(250),
                    Role INTEGER,
                    Senha VARCHAR(100)
                );
            """;
            await connection.ExecuteAsync(sql);
        }

        async Task _initCliente()
        {
            var sql = """
                CREATE TABLE IF NOT EXISTS Clientes (
                    Id SERIAL PRIMARY KEY,
                    Nome VARCHAR(100),
                    SobreNome VARCHAR(200),
                    CPF VARCHAR(11),
                    Email VARCHAR(250),
                    Senha VARCHAR(100) 
                );
            """;
            await connection.ExecuteAsync(sql);
        }

        async Task _initProduto()
        {
            var sql = """
                CREATE TABLE IF NOT EXISTS Produtos (
                    Id SERIAL PRIMARY KEY,
                    Nome VARCHAR(100) NOT NULL,
                    Descricao VARCHAR(250),
                    Categoria INTEGER NOT NULL,
                    Preco NUMERIC(25,2) NOT NULL
                );
            """;
            await connection.ExecuteAsync(sql);
        }

        async Task _initPedido()
        {
            var sql = """
                CREATE TABLE IF NOT EXISTS Pedidos (
                    Id SERIAL PRIMARY KEY,
                    ClienteId INTEGER,
                    Status INTEGER NOT NULL,
                    DataPedido timestamp NOT NULL
                );
            """;
            await connection.ExecuteAsync(sql);
        }

        async Task _initItensPedido()
        {
            var sql = """
                CREATE TABLE IF NOT EXISTS ItensPedidos (
                    Id SERIAL PRIMARY KEY,
                    PedidoId INTEGER NOT NULL,
                    ProdutoId  INTEGER NOT NULL,
                    Preco NUMERIC(25,2) NOT NULL,
                    Quantidade INTEGER NOT NULL
                );
            """;
            await connection.ExecuteAsync(sql);
        }
    }

    private async Task _initData()
    {
        using var connection = CreateConnection();
        await _initDataCliente();
        await _initDataProduto();
        await _initDataPedido();

        async Task _initDataCliente()
        {
            var sql = """
                INSERT INTO Clientes (Nome, SobreNome, CPF, Email, Senha)
                VALUES ('Tester', 'Auto-Atendimento', '12345678912', 'tester@email.com', '$2a$11$gvC72vD3cM0/ZmDfMAmZ3ubO5pstPQWVJxXvQmawzLL1SBmL3W7mi');
            """;
            await connection.ExecuteAsync(sql);
        }

        async Task _initDataProduto()
        {
            var sql = """
                INSERT INTO Produtos (Nome, Descricao, Categoria, Preco)
                VALUES ('Big Mac', 'Dois alfaces, queijo', 0, 22.55);

                INSERT INTO Produtos (Nome, Descricao, Categoria, Preco)
                VALUES ('Sprite', 'Leve e refrescante', 2, 10.00);
            """;
            await connection.ExecuteAsync(sql);
        }

        async Task _initDataPedido()
        {
            var sql = """
                INSERT INTO Pedidos (ClienteId, Status, DataPedido)
                VALUES (1, 0, NOW());

                INSERT INTO ItensPedidos (PedidoId, ProdutoId, Preco, Quantidade)
                VALUES (1, 1, 22.55, 1);

                INSERT INTO ItensPedidos (PedidoId, ProdutoId, Preco, Quantidade)
                VALUES (1, 2, 10.00, 1);

                INSERT INTO Pedidos (ClienteId, Status, DataPedido)
                VALUES (1, 3, NOW());

                INSERT INTO ItensPedidos (PedidoId, ProdutoId, Preco, Quantidade)
                VALUES (2, 1, 22.55, 3);

                INSERT INTO Pedidos (ClienteId, Status, DataPedido)
                VALUES (1, 2, NOW());

                INSERT INTO ItensPedidos (PedidoId, ProdutoId, Preco, Quantidade)
                VALUES (3, 1, 22.55, 2);
            """;
            await connection.ExecuteAsync(sql);
        }
    }
}