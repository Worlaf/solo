using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Solo.Api.App.Authentication;
using Solo.Api.Controllers;
using Solo.Data.DatabaseInitializers;
using Solo.Data.Infrastructure;

namespace solo.api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public Func<IRegistrationBuilder<object, object, object>, IRegistrationBuilder<object, object, object>> LifetimeScopeConfigurator { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
            LifetimeScopeConfigurator = registrationBuilder => registrationBuilder.InstancePerLifetimeScope();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddDefaultPolicy(b => b
                .WithOrigins("http://localhost:3000", "https://localhost:3000", "https://solo-290412.web.app")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()));

            var authenticationSchemeName = "Basic";
            services.AddAuthentication(authenticationSchemeName).AddScheme<AuthenticationSchemeOptions, SimplestEmailAuthenticationHandler>(authenticationSchemeName, null);
            // services.AddAuthorization

            services
                .AddMvc(c => c.EnableEndpointRouting = false)
                .ConfigureApiBehaviorOptions(o => o.SuppressMapClientErrors = true)
                .AddControllersAsServices();

            // services.AddSignalR todo: why not?

            services.AddControllers();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new DataDiRegistrationModule(LifetimeScopeConfigurator));
            builder.RegisterAssemblyTypes(typeof(ParkObjectsController).Assembly).AssignableTo<ControllerBase>().AsSelf().PropertiesAutowired();
            builder.RegisterType<HttpContextAccessor>().AsImplementedInterfaces().SingleInstance();

            var serviceTypePostfixes = new[] {"Service", "Resolver"};
            builder.RegisterAssemblyTypes(typeof(Startup).Assembly).Where(t => serviceTypePostfixes.Any(p => t.Name.EndsWith(p))).AsImplementedInterfaces();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                var soloDbContextConnectionStringName = _configuration.GetConnectionString(SoloDbContext.ConnectionStringName);
                if (!SqlServerDbUtil.DatabaseExists(soloDbContextConnectionStringName))
                {
                    SqlServerDbUtil.CreateDatabase(soloDbContextConnectionStringName, collation: "SQL_Latin1_General_CP1_CI_AI");

                    var dbInitializer = app.ApplicationServices.GetService<DevDatabaseInitializer>();
                    dbInitializer.Initialize();
                }
            }

            // todo: есть шанс что успею взяться за авторизацию
            // if (env.IsDevelopment() || env.EnvironmentName == "Demo")
            //     IdentityModelEventSource.ShowPII = true;

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}