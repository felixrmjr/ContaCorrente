using IdentityModel;
using IdentityServer4;
using IdentityServer4.AccessTokenValidation;
using IdentityServer4.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using WR.Modelo.IoC;
using WR.Modelo.Domain.Entities;

namespace WR.Modelo.ApiMs.Authorization
{
    public static class AuthorizationConfig
    {
        private static readonly AuthorizationConfigs _config = ServiceLocator.Resolve<IOptions<AuthorizationConfigs>>().Value;

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource(_config.ClientId) { UserClaims = { JwtClaimTypes.Id, JwtClaimTypes.Email, "login", "origem" } },
            };
        }

        public static IEnumerable<Client> GetClients() => new List<Client>
        {
            new Client
            {
                ClientId = _config.ClientId,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets ={ new Secret(_config.Secret.Sha256()) },
                AllowedScopes = { _config.ClientId, IdentityServerConstants.StandardScopes.OfflineAccess, IdentityServerConstants.StandardScopes.Profile, },
                AlwaysIncludeUserClaimsInIdToken = true,
                AllowOfflineAccess = true,
                RefreshTokenExpiration = TokenExpiration.Sliding,
                AccessTokenLifetime = 86400,
                IdentityTokenLifetime = 86400,
                AlwaysSendClientClaims = true,
                UpdateAccessTokenClaimsOnRefresh=true
            }
        };

        public static RsaSecurityKey GetCertificate()
        {
            var filename = Path.Combine(Directory.GetCurrentDirectory(), "tempkey.rsa");

            if (File.Exists(filename))
            {
                var keyFile = File.ReadAllText(filename);
                var tempKey = JsonConvert.DeserializeObject<TemporaryRsaKey>(keyFile, new JsonSerializerSettings { ContractResolver = new RsaKeyContractResolver() });

                return CreateRsaSecurityKey(tempKey.Parameters, tempKey.KeyId);
            }
            else
            {
                var key = CreateRsaSecurityKey();
                RSAParameters parameters;

                if (key.Rsa != null)
                    parameters = key.Rsa.ExportParameters(includePrivateParameters: true);
                else
                    parameters = key.Parameters;

                var tempKey = new TemporaryRsaKey
                {
                    Parameters = parameters,
                    KeyId = key.KeyId
                };

                File.WriteAllText(filename, JsonConvert.SerializeObject(tempKey, new JsonSerializerSettings { ContractResolver = new RsaKeyContractResolver() }));
                return key;
            }
        }

        public static IdentityServerAuthenticationOptions GetServerAutentication(IdentityServerAuthenticationOptions options)
        {
            options.ApiName = _config.ClientId;
            options.Authority = _config.AuthUrl;
            options.RequireHttpsMetadata = false; // only for development
            options.ApiSecret = _config.Secret;
            options.SupportedTokens = SupportedTokens.Both;

            return options;
        }

        public static Claim[] GetClaims(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(JwtClaimTypes.Role, string.Empty),
                new Claim(JwtClaimTypes.Id, usuario.Id.ToString()),
                new Claim(JwtClaimTypes.Email, usuario.Login),
                new Claim("login", usuario.Login)
            };

            return claims;
        }
        private static RsaSecurityKey CreateRsaSecurityKey()
        {
            var rsa = RSA.Create();
            RsaSecurityKey key;

            if (rsa is RSACryptoServiceProvider)
            {
                rsa.Dispose();
                var cng = new RSACng(2048);

                var parameters = cng.ExportParameters(includePrivateParameters: true);
                key = new RsaSecurityKey(parameters);
            }
            else
            {
                rsa.KeySize = 2048;
                key = new RsaSecurityKey(rsa);
            }

            key.KeyId = CryptoRandom.CreateUniqueId(16);
            return key;
        }

        private static RsaSecurityKey CreateRsaSecurityKey(RSAParameters parameters, string id)
        {
            var key = new RsaSecurityKey(parameters) { KeyId = id };
            return key;
        }
        private class RsaKeyContractResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                var property = base.CreateProperty(member, memberSerialization);
                property.Ignored = false;
                return property;
            }
        }
    }
}
