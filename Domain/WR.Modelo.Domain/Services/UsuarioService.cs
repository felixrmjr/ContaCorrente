using WR.Modelo.Domain.Interfaces.Services;
using WR.Modelo.Domain.Entities;
using WR.Modelo.Domain.Interfaces.Repositories;
using WR.Modelo.Domain.Interfaces.Base;

namespace WR.Modelo.Domain.Services
{
    public class UsuarioService<TContext> : ServiceBase<TContext, Usuario, int>, IUsuarioService<TContext> where TContext : IUnitOfWork<TContext>
    {
        private readonly IUsuarioRepository<TContext> _repositorio;
        private readonly IUsuarioBase _usuarioLogado;

        public UsuarioService(IUsuarioRepository<TContext> repositorio, IUsuarioBase usuarioBase) : base(repositorio, usuarioBase)
        {
            this._repositorio = repositorio;
            this._usuarioLogado = usuarioBase;
            
        }

        public Usuario ObterUsuarioPorLogin(string login) => _repositorio.ObterUsuarioPorLogin(login);
    }
}