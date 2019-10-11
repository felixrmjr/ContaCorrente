using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WR.Modelo.ApiMs.Authorization
{
    public static class CorsAuthorization
    {
        public static void SetAllowAll(IServiceCollection services)
        {
            services.AddTransient<ICorsPolicyService>(p =>
            {
                return new DefaultCorsPolicyService(p.GetRequiredService<ILogger<DefaultCorsPolicyService>>())
                {
                    AllowAll = true
                };
            });
        }
    }
}