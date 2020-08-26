#pragma warning disable CS0168 // Variable is declared but never used

using System;

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using DryIoc;

using Pharmacies.DataStore.EF.Providers;

namespace Pharmacies.DataStore.EF
{
    public static class Module
    {
        public static IServiceCollection AddDataStore(this IServiceCollection service, string connection)
            => service.AddDbContext<DataContext>(options =>
               {
                   options.UseNpgsql(connection, x => x.MigrationsHistoryTable("_Migrations"));
               });

        public static IHost MigrateDatabase(this IHost app)
        {
            using (var scope = app.Services.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetService<DataContext>())
                {
                    try
                    {
                        context?.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        // use only for debug
                        throw;
                    }
                }
            }

            return app;
        }

        public static void Install(Container container)
        {
            container.RegisterDelegate(x =>
                    x.Resolve<DbContext>().Database.GetService<ICurrentDbContext>(), Reuse.Transient);

            container.Register<IScopeFactory, TransactionFactory>(Reuse.Transient);

            container.Register(typeof(IRepository<>), typeof(Repository<>), Reuse.Transient);

            container.Register<DbContext, DataContext>(Reuse.Scoped);
        }
    }
}
