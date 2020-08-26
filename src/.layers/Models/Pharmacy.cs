using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using Pharmacies.Models.Base;

namespace Pharmacies.Models
{
    public sealed class Pharmacy : Entity<int>
    {
        public ICollection<Batch> Batches { get; set; }
        public ICollection<User> Users { get; set; }

        [Required,
            Column("Name", Order = 1),
            StringLength(50)]
        public string Name { get; set; }

        [Column("Address", Order = 2),
            StringLength(50)]
        public string Address { get; set; }

        [Column("Zip", Order = 3),
            StringLength(50)]
        public string Zip { get; set; }

        [Column("City", Order = 4),
            StringLength(50)]
        public string City { get; set; }

        [Column("Phone", Order = 5),
            StringLength(50)]
        public string Phone { get; set; }

        [Column("Country", Order = 6),
            StringLength(50)]
        public string Country { get; set; }

        [Column("Email", Order = 7),
            StringLength(50)]
        public string Email { get; set; }

        [Column("Contact", Order = 8),
            StringLength(50)]
        public string Contact { get; set; }

        [Column("Logo", Order = 9),
            StringLength(50)]
        public string Logo { get; set; }
    }
}
