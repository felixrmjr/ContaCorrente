using WR.Modelo.Domain.Entities;
using WR.Modelo.Domain.Interfaces.Base;

namespace WR.Modelo.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository<TContext> : IRepositoryBase<TContext, Usuario, int> where TContext : IUnitOfWork<TContext>
    {
        Usuario ObterUsuarioPorLogin(string login);
    }
}
