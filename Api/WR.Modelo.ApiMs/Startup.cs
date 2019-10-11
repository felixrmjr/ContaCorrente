using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using WebApiContrib.Core.Formatter.Csv;
using WR.Modelo.Api.Filters;
using WR.Modelo.IoC;
using WR.Modelo.ApiMs.Authorization;
using WR.Modelo.ApiMs.Helpers;
using WR.Modelo.Repository.Contexts;
using WR.Modelo.Domain.Interfaces.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WR.Modelo.JsonLocalizer.Extensions;

namespace WR.Modelo.ApiMs
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
                                                  .WriteTo.RollingFile(Path.Combine(AppContext.BaseDirectory, "Log/log-{Date}.txt"))
                                                  .CreateLogger();
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // IoC para autoMapper
            services.AddAutoMapper();

            // Obtendo parametros AuthorizationConfigs do appsettings.json
            services.Configure<AuthorizationConfigs>(Configuration.GetSection("AuthorizationConfigs"));

            // Configuração da internacionalização
            services.AddJsonLocalization(options => options.ResourcesPath = "Internationalization");

            WRIoC.Register(services);

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddScoped<IUsuarioBase, UsuarioBase>();

            services.AddDbContext<WRContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                                                               .ConfigureWarnings(x => x.Ignore(RelationalEventId.AmbientTransactionWarning)));

            // Configurando ServiceLocator
            ServiceLocator.Init(services.BuildServiceProvider());

            // Configuração AddIdentityServer
            services.AddIdentityServer()
                    .AddSigningCredential(AuthorizationConfig.GetCertificate())
                    .AddInMemoryApiResources(AuthorizationConfig.GetApiResources())
                    .AddInMemoryClients(AuthorizationConfig.GetClients())
                    .AddResourceOwnerValidator<GrantValidator>();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // Configuração Client
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options => AuthorizationConfig.GetServerAutentication(options));


            CorsAuthorization.SetAllowAll(services);

            services.AddCors(options =>
            {
                options.AddPolicy("ALL", builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials());
            });

            services.AddMvcCore().AddAuthorization();

            var csvFormatterOptions = new CsvFormatterOptions();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(DomainExceptionFilter));
                options.ModelBinderProviders.Insert(0, new ProviderModelBinder());
                options.RespectBrowserAcceptHeader = true;
                options.InputFormatters.Add(new CsvInputFormatter(csvFormatterOptions));
                options.OutputFormatters.Add(new CsvOutputFormatter(csvFormatterOptions));
                options.FormatterMappings.SetMediaTypeMappingForFormat("csv", "text/csv");
            })
            .AddCsvSerializerFormatters()
            .AddViewLocalization()
            .AddJsonOptions(x => SetDefaultSerializerSettings(x.SerializerSettings))
            .AddApplicationPart(typeof(Startup).Assembly);

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseMvc();
        }

        public void SetDefaultSerializerSettings(JsonSerializerSettings settings)
        {
            Formatting formatting = Formatting.None;
#if DEBUG
            formatting = Formatting.Indented;
#endif
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            settings.Formatting = formatting;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

    }
}