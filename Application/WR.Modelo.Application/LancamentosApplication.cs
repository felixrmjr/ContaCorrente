using WR.Modelo.Application.Base;
using WR.Modelo.Domain.Entities;
using WR.Modelo.Domain.Interfaces.Applications;
using WR.Modelo.Domain.Interfaces.Base;
using WR.Modelo.Domain.Interfaces.Services;

namespace WR.Modelo.Application
{

    public class LancamentosApplication<TContext> : ApplicationBase<TContext, Lancamentos, int>, ILancamentosApplication<TContext> where TContext
                                                  : IUnitOfWork<TContext>
    {
        private new readonly ILancamentosService<TContext> _lancamentoService;
        private new readonly IContaCorrenteService<TContext> _contaCorrenteService;


        public LancamentosApplication(IUnitOfWork<TContext> context, 
                                      ILancamentosService<TContext> lancamentoService,
                                      IContaCorrenteService<TContext> contaCorrenteService) : base(context, lancamentoService)
        {
            _lancamentoService = lancamentoService;
            _contaCorrenteService = contaCorrenteService;
        }

        public Lancamentos AdicionarTransacao(Lancamentos entity)
        {
            // Salva a transação
            var lancamento = _lancamentoService.AdicionarTransacao(entity);

            // Atualiza o saldo da conta
            _contaCorrenteService.AtualizarSaldos(entity.ContaOrigem, entity.Tipo, entity.Valor);

            // Commit
            _unitOfWork.Commit();

            // Retorno
            return lancamento;
        }
    }
}
