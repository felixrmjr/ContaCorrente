using WR.Modelo.Domain.Interfaces.Base;
using WR.Modelo.Domain.Entities;
using WR.Modelo.Domain.Enums;

namespace WR.Modelo.Domain.Interfaces.Services
{
    public interface IContaCorrenteService<TContext> : IServiceBase<TContext, ContaCorrente, int> where TContext : IUnitOfWork<TContext>
    {
        ContaCorrente ObterContaPorNumero(int numero);

        ContaCorrente CriarContaCorrente();

        void AtualizarSaldos(int contaNumero, TipoLancamento tipo, decimal valor);
    }
}
