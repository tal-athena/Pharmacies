using System;
using System.IO;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using DryIoc.Microsoft.DependencyInjection;
using DryIoc;

using Serilog;

using Pharmacies.DataStore.EF;
using Pharmacies.App.Models;

using DataStoreModule = Pharmacies.DataStore.EF.Module;
using DomainModule = Pharmacies.Domain.Module;

namespace Pharmacies.App
{
    public static class Program
    {
        private static IContainer Container
             => new Container()
                .With(rules => rules.With(propertiesAndFields: PropertiesAndFields.Auto));

        public static void Main(string[] args)
            => CreateHostBuilder(args).Build()
                                      .CreateFileStructure()
                                      .MigrateDatabase()
                                      .Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new DryIocServiceProviderFactory(Container))
                .UseSerilog()
                .ConfigureContainer<Container>((context, container) =>
                 {
                     DataStoreModule.Install(container);
                     DomainModule.Install(container);

                     container.Register<UserData>(Reuse.Singleton);
                 })
                .ConfigureHostConfiguration(configuration =>
                 {
                 })
                .ConfigureAppConfiguration((context, configuration)=>
                 {
                     configuration
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();
                 })
                .ConfigureWebHostDefaults(web =>
                 {
                     web.UseContentRoot(Environment.CurrentDirectory)
                        .UseStartup<Startup>()
   #if DEBUG
                        .UseUrls("https://localhost:5000")
#endif
                        .UseIIS();
                 });


        public static IHost CreateFileStructure(this IHost app)
        {
            var env = app.Services.GetService<IWebHostEnvironment>();

            Directory.CreateDirectory($@"{env.WebRootPath}\Pharmacy\Icons");

            Directory.CreateDirectory($@"{env.WebRootPath}\Documents"); 

            return app;
        }
    }
}
