using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using WR.Modelo.Domain.Entities;
using WR.Modelo.Domain.Interfaces.Services;

namespace WR.Modelo.ApiMs.Authorization
{
    public class TokenIssuerService : ITokenIssuerService
    {
        private readonly ITokenService _tokenService;
        private readonly IdentityServerOptions _identityServerOptions;
        private readonly AuthorizationConfigs _autorizationOptions;

        public TokenIssuerService(ITokenService tokenService, IdentityServerOptions options, IOptions<AuthorizationConfigs> authConfig)
        {
            _tokenService = tokenService;
            _identityServerOptions = options;
            _autorizationOptions = authConfig.Value;
        }

        public async Task<string> GenerateToken(Usuario usuario)
        {
            var request = new TokenCreationRequest();
            var idServerPrincipal = IdentityServerPrincipal.Create(usuario.Id.ToString(), usuario.Login, AuthorizationConfig.GetClaims(usuario));

            request.Subject = idServerPrincipal;
            request.IncludeAllIdentityClaims = true;
            request.ValidatedRequest = new ValidatedRequest();
            request.ValidatedRequest.Subject = request.Subject;
            request.ValidatedRequest.SetClient(AuthorizationConfig.GetClients().First());
            request.Resources = new Resources(new IdentityResource[] { }, AuthorizationConfig.GetApiResources());
            request.ValidatedRequest.Options = _identityServerOptions;
            request.ValidatedRequest.ClientClaims = idServerPrincipal.Claims.ToArray();

            var Token = await _tokenService.CreateAccessTokenAsync(request);
            Token.Issuer = _autorizationOptions.AuthUrl;

            var TokenValue = await _tokenService.CreateSecurityTokenAsync(Token);
            return TokenValue;
        }
    }
}
