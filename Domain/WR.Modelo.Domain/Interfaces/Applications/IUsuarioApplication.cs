using WR.Modelo.Domain.Interfaces.Repositories;
using WR.Modelo.Domain.Entities;
using WR.Modelo.Domain.Interfaces.Base;

namespace WR.Modelo.Domain.Interfaces.Applications
{
    public interface IUsuarioApplication<TContext> : IApplicationBase<TContext, Usuario, int> where TContext : IUnitOfWork<TContext>
    {
        Usuario ObterUsuarioPorLogin(string login);
    }
}