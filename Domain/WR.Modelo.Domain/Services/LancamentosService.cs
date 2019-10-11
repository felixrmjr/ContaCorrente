using WR.Modelo.Domain.Interfaces.Services;
using WR.Modelo.Domain.Entities;
using WR.Modelo.Domain.Interfaces.Repositories;
using WR.Modelo.Domain.Interfaces.Base;
using WR.Modelo.Domain.Exceptions;
using WR.Modelo.Domain.Enums;

namespace WR.Modelo.Domain.Services
{
    public class LancamentosService<TContext> : ServiceBase<TContext, Lancamentos, int>, ILancamentosService<TContext> where TContext
                                              : IUnitOfWork<TContext>
    {
        private readonly ILancamentosRepository<TContext> _repositorio;
        private readonly IContaCorrenteRepository<TContext> _contaCorrenteRepositorio;
        private readonly IUsuarioBase _usuarioLogado;

        public LancamentosService(ILancamentosRepository<TContext> repositorio,
                                  IContaCorrenteRepository<TContext> contaCorrenteRepositorio,
                                  IUsuarioBase usuarioBase) : base(repositorio, usuarioBase)
        {
            this._repositorio = repositorio;
            this._contaCorrenteRepositorio = contaCorrenteRepositorio;
            this._usuarioLogado = usuarioBase;
        }

        #region [ Transação ]

        public Lancamentos AdicionarTransacao(Lancamentos entity)
        {
            try
            {

                ValidarTransacao(entity);
                ValidarTransferencia(entity);

                _repositorio.Save(entity);

                return entity;

            }
            catch (System.Exception ex)
            {

                throw;
            }
        }

        private void ValidarTransacao(Lancamentos entity)
        {
            if (!_contaCorrenteRepositorio.ExisteContaCorrente(entity.ContaOrigem))
                throw new DomainException(nameof(LancamentosService<TContext>), nameof(AdicionarTransacao), "campoObrigatorio", "contaOrigem");

            if (entity.Valor <= 0)
                throw new DomainException(nameof(LancamentosService<TContext>), nameof(AdicionarTransacao), "campoObrigatorio", "valor");
        }

        private void ValidarTransferencia(Lancamentos entity)
        {
            if (entity.Tipo != TipoLancamento.Transferencia)
                return;

            if (!entity.ContaDestino.HasValue)
                throw new DomainException(nameof(LancamentosService<TContext>), nameof(ValidarTransferencia), "campoObrigatorio", "contaDestino");

            if (entity.ContaOrigem == entity.ContaDestino)
                throw new DomainException(nameof(LancamentosService<TContext>), nameof(ValidarTransferencia), "contasIguais", "contaOrigem", "contaDestino");
        }

        #endregion
    }
}

