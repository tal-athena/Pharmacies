using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using Pharmacies.DataStore.EF.Configurations;
using Pharmacies.Models;

namespace Pharmacies.DataStore.EF
{
    internal sealed partial class DataContext : DbContext
    {
        public DbSet<Document> Documents { get; }
        public DbSet<Template> Template { get; }
        public DbSet<Pharmacy> Pharmacy { get; }
        public DbSet<Batch> Batches { get; }
        public DbSet<User> Users { get; }
        public DbSet<Meta> Meta { get; }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DataContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                //
                // use only for creating/applying migrations
                //

                var builder =
                    new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                        .AddJsonFile("appsettings.json")
                        .Build();

                var connection = builder.GetConnectionString("Default");

                options.UseNpgsql(connection);
            }

            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
               .ApplyConfiguration(new DocumentSetup())
               .ApplyConfiguration(new TemplateSetup())
               .ApplyConfiguration(new PharmacySetup())
               .ApplyConfiguration(new BatchSetup())
               .ApplyConfiguration(new UserSetup())
               .ApplyConfiguration(new MetaSetup());

            base.OnModelCreating(builder);
        }
    }
}
