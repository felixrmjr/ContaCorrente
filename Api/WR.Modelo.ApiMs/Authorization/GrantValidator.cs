using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using WR.Modelo.Domain.Interfaces.Applications;
using WR.Modelo.Repository.Contexts;

namespace WR.Modelo.ApiMs.Authorization
{
    public class GrantValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUsuarioApplication<WRContext> _usuarioApplication;
        private readonly AuthorizationConfigs _config;

        public GrantValidator(IUsuarioApplication<WRContext> usuarioApplication, IOptions<AuthorizationConfigs> config)
        {
            _usuarioApplication = usuarioApplication;
            _config = config.Value;
        }
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Validando Login");
            
            var usuario = _usuarioApplication.ObterUsuarioPorLogin(context.UserName);

            if (usuario != null)
                Console.WriteLine($"Usuario Obtido do banco: " + usuario.Nome);

            if (usuario == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient, "Login inválido.");
                return Task.FromResult(0);
            }

            if (usuario.Senha != SHA.Encrypt(SHA.Algorithm.SHA512, context.Password))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient, "Senha inválida.");
                return Task.FromResult(0);
            }

            context.Result = new GrantValidationResult(subject: usuario.Id.ToString(), authenticationMethod: "custom", claims: AuthorizationConfig.GetClaims(usuario));
            Console.WriteLine($"Usuario no Claims " + usuario.Nome);
            Console.ResetColor();
            return Task.FromResult(0);
        }
    }
}