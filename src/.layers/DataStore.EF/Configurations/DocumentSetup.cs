using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Pharmacies.Models;

namespace Pharmacies.DataStore.EF.Configurations
{
    public sealed class DocumentSetup : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
        }
    }
}
