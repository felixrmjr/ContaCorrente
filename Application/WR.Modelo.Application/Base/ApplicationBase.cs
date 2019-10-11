using System.Collections.Generic;
using WR.Modelo.Domain.Entities.Base;
using WR.Modelo.Domain.Interfaces.Repositories;
using WR.Modelo.Domain.Interfaces.Base;

namespace WR.Modelo.Application.Base
{
    public class ApplicationBase<TContext, TEntity, TIdentity> : IApplicationBase<TContext, TEntity, TIdentity> where TEntity
                                                               : EntityBase<TIdentity> where TContext : IUnitOfWork<TContext>
    {
        protected readonly IServiceBase<TContext, TEntity, TIdentity> _service;
        protected readonly IUnitOfWork<TContext> _unitOfWork;

        public ApplicationBase(IUnitOfWork<TContext> context, IServiceBase<TContext, TEntity, TIdentity> service)
        {
            this._service = service;
            this._unitOfWork = context;
        }

        public virtual TEntity Save(TEntity entity)
        {
            _service.Save(entity);
            _unitOfWork.Commit();

            return entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            _service.Update(entity);
            _unitOfWork.Commit();

            return entity;
        }

        public virtual void Delete(TIdentity chave)
        {
            _service.Delete(chave);
            _unitOfWork.Commit();
        }

        public virtual TEntity Get(TIdentity id)
        {
            var entidade = _service.Get(id);
            return entidade;
        }

        public virtual ICollection<TEntity> GetAll(QueryFilter filter = null)
        {
            return _service.GetAll(filter);
        }

        public virtual IPagedList<TEntity> GetPaginated(QueryFilter filter, int start = 0, int limit = 10, bool orderByDescending = true)
        {
            return _service.GetPaginated(filter, start, limit);
        }

        public virtual void Ativar(TIdentity id)
        {
            _service.Ativar(id);
            _unitOfWork.Commit();
        }

        public virtual void Inativar(TIdentity id)
        {
            _service.Inativar(id);
            _unitOfWork.Commit();
        }
    }
}
