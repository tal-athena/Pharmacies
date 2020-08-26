using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Pharmacies.Models;

namespace Pharmacies.DataStore.EF.Configurations
{
    public sealed class BatchSetup : IEntityTypeConfiguration<Batch>
    {
        public void Configure(EntityTypeBuilder<Batch> builder)
        {
        }
    }
}
