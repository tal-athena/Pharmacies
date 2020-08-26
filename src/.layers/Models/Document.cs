using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using Pharmacies.Models.Base;

namespace Pharmacies.Models
{
    public sealed partial class Document : Entity<int>
    {
        [Required,
            Column("Display", Order = 1),
            StringLength(50)]
        public string DisplayName { get; set; }

        [Required,
            Column("Actual", Order = 2),
            StringLength(50)]
        public string ActualName { get; set; }

        [Required,
            ForeignKey("BatchId")]
        public Batch Batch { get; set; }
    }

    public sealed partial class Document
    {
        public int BatchId { get; set; }
    }
}
