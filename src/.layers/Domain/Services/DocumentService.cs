using System.Data;

using Pharmacies.DataStore;
using Pharmacies.Models;

namespace Pharmacies.Domain.Services
{
    public class DocumentService
    {
        public IRepository<Document> Documents { private get; set; }

        public IScopeFactory Scope { private get; set; }

        public int Insert(Document document)
        {
            using (var unit = this.Scope.Create(IsolationLevel.ReadCommitted))
            {
                this.Documents.Insert(document);

                unit.Commit();
            }

            return document.Id;
        }

        public void Delete(int id)
        {
            using (var unit = this.Scope.Create(IsolationLevel.ReadCommitted))
            {
                this.Documents.Delete(new Document { Id = id });

                unit.Commit();
            }
        }
    }
}
