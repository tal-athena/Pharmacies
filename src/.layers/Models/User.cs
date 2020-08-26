using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using Pharmacies.Models.Base;

namespace Pharmacies.Models
{
    public sealed partial class User : Entity<int>
    {
        [ForeignKey("PharmacyId")]
        public Pharmacy Pharmacy { get; set; }

        [Column("FirstName", Order = 1),
            StringLength(50)]
        public string FirstName { get; set; }

        [Column("LastName", Order = 2),
            StringLength(50)]
        public string LastName { get; set; }

        [Required,
            Column("UserName", Order = 3),
            StringLength(50)]
        public string UserName { get; set; }

        [Required,
            Column("Password", Order = 4)]
        public string Password { get; set; }

        [Required,
             Column("IsSuperUser", Order = 5)]
        public bool IsSuperUser { get; set; }
    }

    public sealed partial class User
    {
        public int? PharmacyId { get; set; }
    }
}
