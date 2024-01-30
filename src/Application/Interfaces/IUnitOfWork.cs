namespace Application.Interfaces
{
    public interface IUnitOfWork
    {
        IProdutoRepository Produtos { get; }
    }
}