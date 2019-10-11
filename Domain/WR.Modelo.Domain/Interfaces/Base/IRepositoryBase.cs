using System;
using System.Linq;
using System.Linq.Expressions;
using WR.Modelo.Domain.Entities.Base;
using WR.Modelo.Domain.Interfaces.Repositories;

namespace WR.Modelo.Domain.Interfaces.Base
{
    public interface IRepositoryBase<TContext, TEntity, TIdentity>  where TEntity : EntityBase<TIdentity> where TContext : IUnitOfWork<TContext>
    {
        IUnitOfWork<TContext> UnitOfWork { get; }
        TEntity Save(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(TIdentity id);
        TEntity Get(TIdentity id);
        IQueryable<TEntity> GetAll(QueryFilter filter = null);
        IPagedList<TEntity> GetPaginated(QueryFilter filter, int start = 0, int limit = 10, bool orderByDescending = true, params Expression<Func<TEntity, object>>[] includes);
    }
}