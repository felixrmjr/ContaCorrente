using WR.Modelo.Application.Base;
using WR.Modelo.Domain.Entities;
using WR.Modelo.Domain.Interfaces.Applications;
using WR.Modelo.Domain.Interfaces.Base;
using WR.Modelo.Domain.Interfaces.Services;

namespace WR.Modelo.Application
{
    public class UsuarioApplication<TContext> : ApplicationBase<TContext, Usuario, int>, IUsuarioApplication<TContext> where TContext 
                                              : IUnitOfWork<TContext>
    {
        private new readonly IUsuarioService<TContext> _service;

        public UsuarioApplication(IUnitOfWork<TContext> context, IUsuarioService<TContext> service) : base(context, service)
        {
            _service = service;
        }

        public Usuario ObterUsuarioPorLogin(string login) => _service.ObterUsuarioPorLogin(login);
    }
}
