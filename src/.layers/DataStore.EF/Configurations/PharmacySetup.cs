using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Pharmacies.Models;

namespace Pharmacies.DataStore.EF.Configurations
{
    public sealed class PharmacySetup : IEntityTypeConfiguration<Pharmacy>
    {
        public void Configure(EntityTypeBuilder<Pharmacy> builder)
        {
            builder
               .HasIndex(x => x.Name)
               .IsUnique();
        }
    }
}
