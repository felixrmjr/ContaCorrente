using WR.Modelo.Domain.Interfaces.Base;
using WR.Modelo.Domain.Entities;

namespace WR.Modelo.Domain.Interfaces.Services
{
    public interface ILancamentosService<TContext> : IServiceBase<TContext, Lancamentos, int> where TContext : IUnitOfWork<TContext>
    {
        Lancamentos AdicionarTransacao(Lancamentos entity);
    }
}
