using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

using Pharmacies.Models;

namespace Pharmacies.DataStore.EF.Configurations
{
    public sealed class UserSetup : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
               .HasIndex(x => x.UserName)
               .IsUnique();

            builder
               .HasData(new
                {
                    Id = 1,
                    UserName = "Admin",
                    Password = "AQAAAAEAACcQAAAAEA8tq+ZME4yxbhl9durT1Q5ulnpvLcP1ELJETel6qO9a3G0PeAusD6R5hX0CL1SI2g==", // 123
                    IsSuperUser = true
                });
        }
    }
}
