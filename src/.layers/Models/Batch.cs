using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using Pharmacies.Models.Base;

namespace Pharmacies.Models
{
    public sealed partial class Batch : Entity<int>
    {
        public ICollection<Document> Documents { get; set; }

        public ICollection<Meta> Meta { get; set; }

        [ForeignKey("TemplateId")]
        public Template Template { get; set; }

        [Required,
            ForeignKey("PharmacyId")]
        public Pharmacy Pharmacy { get; set; }

        [Column("Arrival", Order = 9)]
        public DateTime? Arrival { get; set; }

        [Column("Expiery", Order = 10)]
        public DateTime? Expiery { get; set; }

        [Required,
            Column("Created", Order = 11)]
        public DateTime Created { get; set; }

        [Required,
            Column("ProducerName", Order = 1),
            StringLength(50)]
        public string ProducerName { get; set; }

        [Required,
            Column("ProducersBatchID", Order = 2),
            StringLength(50)]
        public string ProducersBatchID { get; set; }

        [Column("ProductName", Order = 3),
            StringLength(50)]
        public string ProductName { get; set; }

        [Column("ProductType", Order = 4),
            StringLength(50)]
        public string ProductType { get; set; }

        [Column("Comments", Order = 8),
            StringLength(50)]
        public string Comments { get; set; }

        [Column("THC", Order = 5)]
        public decimal? THC { get; set; }

        [Column("CBD", Order = 6)]
        public decimal? CBD { get; set; }

        [Column("CBG", Order = 7)]
        public decimal? CBG { get; set; }
    }

    public sealed partial class Batch
    {
        public int PharmacyId { get; set; }
    }
}
