using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using Autofac.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Solo.Data.DatabaseInitializers;
using Solo.Data.Repositories;

namespace Solo.Data.Infrastructure
{
    public class DataDiRegistrationModule : DiRegistrationModuleBase
    {
        public DataDiRegistrationModule(
            Func<IRegistrationBuilder<object, object, object>, IRegistrationBuilder<object, object, object>> lifetimeScopeConfigurator) : base(
            lifetimeScopeConfigurator)
        {
        }

        protected override IEnumerable<IRegistrationBuilder<object, object, object>> RegisterTypesWithDefaultLifetimeScope(ContainerBuilder builder)
        {
            yield return builder.RegisterAssemblyTypes(typeof(UserRepository).Assembly).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces().AsSelf();

            yield return builder.Register(c =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<SoloDbContext>();
                optionsBuilder.UseSqlServer(c.Resolve<IConfiguration>().GetConnectionString(SoloDbContext.ConnectionStringName));

                return optionsBuilder.Options;
            }).As<DbContextOptions>();

            yield return builder.RegisterType<SoloDbContext>().AsSelf();
            yield return builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();

            yield return builder.RegisterAssemblyTypes(typeof(DevDatabaseInitializer).Assembly)
                .Where(t => new[] { "Service", "Provider", "Resolver", "Initializer" }.Any(p => t.Name.EndsWith(p))).AsImplementedInterfaces().AsSelf();
        }
    }
}
