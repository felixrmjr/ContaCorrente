using WR.Modelo.Application.Base;
using WR.Modelo.Domain.Entities;
using WR.Modelo.Domain.Interfaces.Applications;
using WR.Modelo.Domain.Interfaces.Base;
using WR.Modelo.Domain.Interfaces.Services;

namespace WR.Modelo.Application
{

    public class ContaCorrenteApplication<TContext> : ApplicationBase<TContext, ContaCorrente, int>, IContaCorrenteApplication<TContext> where TContext
                                                    : IUnitOfWork<TContext>
    {
        private new readonly IContaCorrenteService<TContext> _service;

        public ContaCorrenteApplication(IUnitOfWork<TContext> context, IContaCorrenteService<TContext> service) : base(context, service)
        {
            _service = service;
        }

        public ContaCorrente ObterContaPorNumero(int numero) => _service.ObterContaPorNumero(numero);

        public ContaCorrente CriarContaCorrente()
        {
            // Salva a conta corrente 
            var contaCorrente = _service.CriarContaCorrente();

            // Commit
            _unitOfWork.Commit();

            // Retorno
            return contaCorrente;
        }
    }
}
