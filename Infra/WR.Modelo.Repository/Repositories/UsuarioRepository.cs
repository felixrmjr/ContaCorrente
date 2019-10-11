using System;
using System.Linq;
using WR.Modelo.Repository.Base;
using WR.Modelo.Domain.Entities;
using WR.Modelo.Domain.Interfaces.Repositories;
using WR.Modelo.Domain.Interfaces.Base;

namespace WR.Modelo.Repository.Repositories
{
    public class UsuarioRepository<TContext> : RepositoryBase<TContext, Usuario, int>, IUsuarioRepository<TContext> where TContext : IUnitOfWork<TContext>
    {
        private readonly IUsuarioBase _user;

        public UsuarioRepository(IUnitOfWork<TContext> unitOfWork, IUsuarioBase user) : base(unitOfWork)
        {
            _user = user;
        }

        public Usuario ObterUsuarioPorLogin(string login) => DbSet.FirstOrDefault(x => string.Equals(x.Login, login, StringComparison.CurrentCultureIgnoreCase) && x.Ativo);
    }
}
