using WR.Modelo.Domain.Interfaces.Services;
using WR.Modelo.Domain.Entities;
using WR.Modelo.Domain.Interfaces.Repositories;
using WR.Modelo.Domain.Interfaces.Base;
using WR.Modelo.Domain.Enums;
using WR.Modelo.Domain.Exceptions;

namespace WR.Modelo.Domain.Services
{
    public class ContaCorrenteService<TContext> : ServiceBase<TContext, ContaCorrente, int>, IContaCorrenteService<TContext> where TContext
                                                : IUnitOfWork<TContext>
    {
        private readonly IContaCorrenteRepository<TContext> _repositorio;

        private readonly IUsuarioBase _usuarioLogado;

        public ContaCorrenteService(IContaCorrenteRepository<TContext> repositorio, IUsuarioBase usuarioBase) : base(repositorio, usuarioBase)
        {
            this._repositorio = repositorio;
            this._usuarioLogado = usuarioBase;
        }

        public ContaCorrente ObterContaPorNumero(int numero) => _repositorio.ObterContaPorNumero(numero);

        #region [ Criar ContaCorrente ]

        public ContaCorrente CriarContaCorrente()
        {
            var entity = new ContaCorrente(_usuarioLogado.UsuarioLogado.Id);
            GerarContaCorrente(entity);
            _repositorio.Save(entity);

            return entity;
        }

        private void GerarContaCorrente(ContaCorrente entity)
        {
            entity.GerarNumeroConta();

            var existe = true;

            while (existe)
            {
                existe = _repositorio.ExisteContaCorrente(entity.Numero);

                if (existe)
                    entity.GerarNumeroConta();
            }
        }

        #endregion

        #region [ Saldo ]

        public void AtualizarSaldos(int contaNumero, TipoLancamento tipo, decimal valor)
        {
            ValidarParametrosAtualizarSaldos(contaNumero, valor);

            var conta = _repositorio.ObterContaPorNumero(contaNumero);

            if (conta == null)
                throw new DomainException(nameof(ContaCorrenteService<TContext>), nameof(AtualizarSaldos), "campoObrigatorio", "conta");

            if (tipo == TipoLancamento.Credito)
                conta.AdicionarCredito(valor);
            else
                conta.AdicionarDebito(valor);

            _repositorio.Update(conta);
        }

        private void ValidarParametrosAtualizarSaldos(int contaNumero, decimal valor)
        {
            if (contaNumero <= 0)
                throw new DomainException(nameof(ContaCorrenteService<TContext>), nameof(AtualizarSaldos), "campoObrigatorio", "contaNumero");

            if (valor <= 0)
                throw new DomainException(nameof(ContaCorrenteService<TContext>), nameof(AtualizarSaldos), "campoObrigatorio", "valor");
        }

        

        #endregion
    }
}

