using WR.Modelo.Domain.Interfaces.Base;
using WR.Modelo.Domain.Entities;

namespace WR.Modelo.Domain.Interfaces.Applications
{
    public interface IContaCorrenteApplication<TContext> : IApplicationBase<TContext, ContaCorrente, int> where TContext
                                                         : IUnitOfWork<TContext>
    {
        ContaCorrente ObterContaPorNumero(int numero);

        ContaCorrente CriarContaCorrente();
    }
}
