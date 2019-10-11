using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using WR.Modelo.Domain.Exceptions;
using WR.Modelo.Domain.Entities.Base;
using WR.Modelo.Domain.Interfaces.Base;

namespace WR.Modelo.ApiMs.Helpers
{

    public class UsuarioBase : IUsuarioBase
    {
        private IHttpContextAccessor _context;
        private UsuarioIdentity _usuarioLogado;
        public UsuarioBase(IHttpContextAccessor context) => _context = context;

        public UsuarioIdentity UsuarioLogado
        {
            get
            {
                if (_usuarioLogado != null) return this._usuarioLogado;

                ClaimsPrincipal user = _context.HttpContext.User;
                var id = user.Claims.FirstOrDefault(c => c.Type == "id");
                var email = user.Claims.FirstOrDefault(c => c.Type == "email");
                var login = user.Claims.FirstOrDefault(c => c.Type == "login");

                if (id == null || email == null || login == null)
                    throw new UsuarioExpiradoException(nameof(UsuarioBase), nameof(UsuarioLogado), "Usuário precisa refazer o login.");

                this._usuarioLogado = new UsuarioIdentity(Convert.ToInt32(id.Value), email.Value, login.Value);

                return this._usuarioLogado;
            }
        }
        UsuarioIdentity IUsuarioBase.UsuarioLogado => UsuarioLogado;

        public string Idioma => _context.HttpContext.ObterIdioma();

    }
}
