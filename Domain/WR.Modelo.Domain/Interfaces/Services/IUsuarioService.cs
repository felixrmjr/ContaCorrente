using WR.Modelo.Domain.Interfaces.Base;
using WR.Modelo.Domain.Entities;

namespace WR.Modelo.Domain.Interfaces.Services
{
    public interface IUsuarioService<TContext> : IServiceBase<TContext, Usuario, int> where TContext : IUnitOfWork<TContext>
    {
        Usuario ObterUsuarioPorLogin(string login);
    }
}
