using Application.Interfaces;
namespace Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(IProdutoRepository produtoRepository)
        {
            Produtos = produtoRepository;
        }
        public IProdutoRepository Produtos { get; }
    }
}
