using Microsoft.Extensions.DependencyInjection;
using WR.Modelo.Domain.Interfaces.Repositories;
using WR.Modelo.Application;
using WR.Modelo.Application.Base;
using WR.Modelo.Domain.Interfaces.Applications;
using WR.Modelo.Domain.Interfaces.Base;
using WR.Modelo.Domain.Interfaces.Services;
using WR.Modelo.Repository.Base;
using WR.Modelo.Repository.Contexts;
using WR.Modelo.Repository.Repositories;
using WR.Modelo.Domain.Services;

namespace WR.Modelo.IoC
{
    public static class WRIoC
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork<WRContext>, WRContext>();

            // Base
            services.AddScoped(typeof(IApplicationBase<,,>), typeof(ApplicationBase<,,>));
            services.AddScoped(typeof(IServiceBase<,,>), typeof(ServiceBase<,,>));
            services.AddScoped(typeof(IRepositoryBase<,,>), typeof(RepositoryBase<,,>));

            // ContaCorrente
            services.AddScoped(typeof(IContaCorrenteApplication<>), typeof(ContaCorrenteApplication<>));
            services.AddScoped(typeof(IContaCorrenteService<>), typeof(ContaCorrenteService<>));
            services.AddScoped(typeof(IContaCorrenteRepository<>), typeof(ContaCorrenteRepository<>));

            // Lancamentos
            services.AddScoped(typeof(ILancamentosApplication<>), typeof(LancamentosApplication<>));
            services.AddScoped(typeof(ILancamentosService<>), typeof(LancamentosService<>));
            services.AddScoped(typeof(ILancamentosRepository<>), typeof(LancamentosRepository<>));

            // Usuario
            services.AddScoped(typeof(IUsuarioApplication<>), typeof(UsuarioApplication<>));
            services.AddScoped(typeof(IUsuarioService<>), typeof(UsuarioService<>));
            services.AddScoped(typeof(IUsuarioRepository<>), typeof(UsuarioRepository<>));
        }
    }
}
