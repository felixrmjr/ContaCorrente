using System.Collections.Generic;
using WR.Modelo.Domain.Entities.Base;
using WR.Modelo.Domain.Interfaces.Repositories;

namespace WR.Modelo.Domain.Interfaces.Base
{
    public interface IServiceBase<TContext, TEntity, TIdentity> where TEntity : EntityBase<TIdentity> where TContext : IUnitOfWork<TContext>
    {
        TEntity Save(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(TIdentity chave);
        TEntity Get(TIdentity id);
        ICollection<TEntity> GetAll(QueryFilter filter = null);
        IPagedList<TEntity> GetPaginated(QueryFilter filter, int start = 0, int limit = 10, bool orderByDescending = true);
        void Ativar(TIdentity id);
        void Inativar(TIdentity id);
    }
}