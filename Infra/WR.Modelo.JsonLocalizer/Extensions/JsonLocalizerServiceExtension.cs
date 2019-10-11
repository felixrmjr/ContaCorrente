using WR.Modelo.JsonLocalizer.Localizer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;

namespace WR.Modelo.JsonLocalizer.Extensions
{
    /// <summary>Métodos de extensão para configurar o JsonLocalizerServiceExtension em um <see cref = "IServiceCollection" /></summary>
    public static class JsonLocalizerServiceExtension
    {
        public static IServiceCollection AddJsonLocalization(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddOptions();
            AddJsonLocalizationServices(services);
            return services;
        }

        public static IServiceCollection AddJsonLocalization(this IServiceCollection services, Action<JsonLocalizationOptions> setupAction)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (setupAction == null)
            {
                Console.Error.WriteLine("Setup Action seems to be null, The localization options will not be override. For any helps create an issue at ");
                AddJsonLocalizationServices(services);
            }

            AddJsonLocalizationServices(services, setupAction);
            return services;
        }

        internal static void AddJsonLocalizationServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
            services.AddSingleton<IStringLocalizer, JsonStringLocalizer>();
        }

        internal static void AddJsonLocalizationServices(IServiceCollection services, Action<JsonLocalizationOptions> setupAction)
        {
            AddJsonLocalizationServices(services);
            services.Configure(setupAction);
        }
    }
}
