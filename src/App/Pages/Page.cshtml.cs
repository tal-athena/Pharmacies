using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Pharmacies.App.Pages.Shared.Base;
using Pharmacies.DataStore.EF.Decorators;
using Pharmacies.DataStore;
using Pharmacies.Models;

namespace Pharmacies.App
{
    public class BatchPageModel : BasePage 
    {
        private readonly IRepository<Batch> batches;

        public List<Document> Documents { get; set; } = new List<Document>();

        public List<string> Images { get; set; } = new List<string>();

        public string ProducerName { get; set; }
        public string PharmacyName { get; set; }
        public string ProductName { get; set; }
        public string ProducersBatchID { get; set; }
        public string Arrival { get; set; }
        public string Expiery { get; set; }
        public string Logo { get; set; }
        public string THC { get; set; }
        public string CBD { get; set; }
        public string CBG { get; set; }


        public bool IsImagesExist => this.Images?.Any() ?? false;

        public BatchPageModel(IRepository<Batch> batches)
        {
            this.batches = batches;
        }

        public IActionResult OnGet(int? id)
        {
            if (!id.HasValue)
                return this.Redirect("/404");

            var batch =
                this.batches
                    .Query
                    .WithDocuments()
                    .WithPharmacy()
                    .Where(x => x.Id == id.Value)
                    .FirstOrDefault();

            if (batch == null)
                return this.Redirect("/404");

            this.ProducerName = batch.ProducerName;
            this.ProductName = batch.ProductName;
            this.ProducersBatchID = batch.ProducersBatchID;
            this.Arrival = batch.Arrival.HasValue ? batch.Arrival.Value.ToString("d.M.yyyy") : string.Empty;
            this.Expiery = batch.Expiery.HasValue ? batch.Expiery.Value.ToString("d.M.yyyy") : string.Empty;
            this.THC = batch.THC.HasValue ? batch.THC.Value.ToString() : string.Empty;
            this.CBD = batch.CBD.HasValue ? batch.CBD.Value.ToString() : string.Empty;
            this.CBG = batch.CBG.HasValue ? batch.CBG.Value.ToString() : string.Empty;
            this.PharmacyName = (batch.Pharmacy != null) ? batch.Pharmacy.Name : string.Empty;

            this.Documents =
                 batch.Documents
                      .Where(x => x.DisplayName.EndsWith(".pdf"))
                      .Select(x => new Document { Name = x.ActualName, Link = $"/Documents/{x.DisplayName}" })
                      .ToList();

            this.Images =
                 batch.Documents
                      .Where(x => x.DisplayName.EndsWith(".jpg")
                               || x.DisplayName.EndsWith(".png"))
                      .Select(x => $"/Documents/{x.DisplayName}")
                      .ToList();

            var logo =
                batch.Pharmacy
                     .Logo;

            this.Logo =
                string.IsNullOrWhiteSpace(logo)
                      ? string.Empty
                      : $"/Pharmacy/Icons/{logo}";

            return this.Page();
        }

        public class Document
        {
            public string Name { get; set; }
            public string Link { get; set; }
        }
    }
}
