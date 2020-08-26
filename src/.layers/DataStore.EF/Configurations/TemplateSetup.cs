using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Pharmacies.Models;

namespace Pharmacies.DataStore.EF.Configurations
{
    public sealed class TemplateSetup : IEntityTypeConfiguration<Template>
    {
        public void Configure(EntityTypeBuilder<Template> builder)
        {
        }
    }
}
