using System.Collections.Generic;

using Pharmacies.App.Models.Common;

namespace Pharmacies.App.Models
{
    public sealed partial class Batch
    {
        public List<Document> Documents { get; set; }

        public NameWithId Pharmacy { get; set; }

        public string Arrival { get; set; }

        public string Expiery { get; set; }

        public string Created { get; set; }

        public string ProducerName { get; set; }

        public string ProducersBatchID { get; set; }

        public string ProductName { get; set; }

        public string ProductType { get; set; }

        public string Comments { get; set; }

        public decimal? THC { get; set; }

        public decimal? CBD { get; set; }

        public decimal? CBG { get; set; }

        public int Id { get; set; }
    }
}
