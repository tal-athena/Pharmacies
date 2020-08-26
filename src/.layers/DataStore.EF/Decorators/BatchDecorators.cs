using System.Linq;

using Microsoft.EntityFrameworkCore;

using Pharmacies.Models;

namespace Pharmacies.DataStore.EF.Decorators
{
    public static class BatchDecorators
    {
        public static IQueryable<Batch> WithDocuments(this IQueryable<Batch> query)
            => query.Include(x => x.Documents);

        public static IQueryable<Batch> WithPharmacy(this IQueryable<Batch> query)
            => query.Include(x => x.Pharmacy);
    }
}
