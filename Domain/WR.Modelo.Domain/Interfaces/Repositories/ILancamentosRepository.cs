using WR.Modelo.Domain.Entities;
using WR.Modelo.Domain.Interfaces.Base;

namespace WR.Modelo.Domain.Interfaces.Repositories
{
    public interface ILancamentosRepository<TContext> : IRepositoryBase<TContext, Lancamentos, int> where TContext
                                                      : IUnitOfWork<TContext>
    { }
}
