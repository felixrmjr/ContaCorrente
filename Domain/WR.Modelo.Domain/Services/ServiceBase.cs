using WR.Modelo.Domain.Entities.Base;
using WR.Modelo.Domain.Interfaces.Repositories;
using WR.Modelo.Domain.Interfaces.Base;
using System.Collections.Generic;
using System.Linq;

namespace WR.Modelo.Domain.Services
{
    public class ServiceBase<TContexto, TEntity, TIdentity> : IServiceBase<TContexto, TEntity, TIdentity> where TEntity
                                                            : EntityBase<TIdentity> where TContexto
                                                            : IUnitOfWork<TContexto>
    {
        protected readonly IUsuarioBase _user;
        protected readonly IRepositoryBase<TContexto, TEntity, TIdentity> _repository;

        public ServiceBase(IRepositoryBase<TContexto, TEntity, TIdentity> repository, IUsuarioBase user)
        {
            this._user = user;
            this._repository = repository;
        }

        public virtual TEntity Save(TEntity entidade)
        {
            entidade.AtualizarUsuarioCriacao(_user.UsuarioLogado.Id);
            entidade.AtualizarDataCriacao();
            entidade.Validate();
            _repository.Save(entidade);

            return entidade;
        }

        public virtual TEntity Update(TEntity entidade)
        {
            entidade.AtualizarUsuarioAlteracao(_user.UsuarioLogado.Id);
            entidade.AtualizarDataAlteracao();
            entidade.Validate();
            _repository.Update(entidade);

            return entidade;
        }

        public virtual void Delete(TIdentity chave) => _repository.Delete(chave);

        public virtual TEntity Get(TIdentity id) => _repository.Get(id);

        public virtual ICollection<TEntity> GetAll(QueryFilter filter = null) => _repository.GetAll(filter).ToList();

        public virtual IPagedList<TEntity> GetPaginated(QueryFilter filter, int start = 0, int limit = 10, bool orderByDescending = true)
        {
            return _repository.GetPaginated(filter, start, limit, orderByDescending);
        }

        public virtual void Ativar(TIdentity id)
        {
            var entidade = _repository.Get(id);
            entidade.Ativar();
        }

        public virtual void Inativar(TIdentity id)
        {
            var entidade = _repository.Get(id);
            entidade.Inativar();
        }
    }
}
