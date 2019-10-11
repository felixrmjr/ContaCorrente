using WR.Modelo.Domain.Entities;
using WR.Modelo.Domain.Interfaces.Base;

namespace WR.Modelo.Domain.Interfaces.Repositories
{
    public interface IContaCorrenteRepository<TContext> : IRepositoryBase<TContext, ContaCorrente, int> where TContext
                                                        : IUnitOfWork<TContext>
    {
        bool ExisteContaCorrente(int numero);
        ContaCorrente ObterContaPorNumero(int numero);
    }
}
