using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using Pharmacies.Models.Base;

namespace Pharmacies.Models
{
    public sealed class Template : Entity<int>
    {
        [Required,
            Column("Content", Order = 1)]
        public string Content { get; set; }

        [Required,
            ForeignKey("BatchId")]
        public Batch Batch { get; set; }
    }
}
