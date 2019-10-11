using WR.Modelo.Domain.Entities;
using WR.Modelo.Domain.Interfaces.Base;
using WR.Modelo.Domain.Interfaces.Repositories;
using WR.Modelo.Repository.Base;

namespace WR.Modelo.Repository.Repositories
{
    public class LancamentosRepository<TContext> : RepositoryBase<TContext, Lancamentos, int>, ILancamentosRepository<TContext> where TContext
                                                 : IUnitOfWork<TContext>
    {
        public LancamentosRepository(IUnitOfWork<TContext> unitOfWork) : base(unitOfWork) { }
    }
}