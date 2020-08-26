using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using Pharmacies.Models.Base;

namespace Pharmacies.Models
{
    public sealed class Meta : Entity<int>
    {
        [Required,
            Column("Value", Order = 1),
            StringLength(50)]
        public string Value { get; set; }

        [Required,
            Column("Type", Order = 2),
            StringLength(50)]
        public string Type { get; set; }

        [Required,
            Column("Key", Order = 3),
            StringLength(50)]
        public string Key { get; set; }

        [Required,
            ForeignKey("BatchId")]
        public Batch Batch { get; set; }
    }
}
