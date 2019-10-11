using System.Linq;
using WR.Modelo.Domain.Entities;
using WR.Modelo.Domain.Interfaces.Base;
using WR.Modelo.Domain.Interfaces.Repositories;
using WR.Modelo.Repository.Base;

namespace WR.Modelo.Repository.Repositories
{
    public class ContaCorrenteRepository<TContext> : RepositoryBase<TContext, ContaCorrente, int>, IContaCorrenteRepository<TContext> where TContext : IUnitOfWork<TContext>
    {
        public ContaCorrenteRepository(IUnitOfWork<TContext> unitOfWork) : base(unitOfWork) { }

        public bool ExisteContaCorrente(int numero)
        {
            return DbSet.Any(c => c.Numero == numero);
        }

        public ContaCorrente ObterContaPorNumero(int numero)
        {
            return DbSet.FirstOrDefault(c => c.Numero == numero);
        }
    }
}