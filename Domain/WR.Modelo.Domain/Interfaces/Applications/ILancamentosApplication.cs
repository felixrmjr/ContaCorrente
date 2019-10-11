using WR.Modelo.Domain.Interfaces.Base;
using WR.Modelo.Domain.Entities;

namespace WR.Modelo.Domain.Interfaces.Applications
{
    public interface ILancamentosApplication<TContext> : IApplicationBase<TContext, Lancamentos, int> where TContext
                                                       : IUnitOfWork<TContext>
    {
        Lancamentos AdicionarTransacao(Lancamentos entity);
    }
}
