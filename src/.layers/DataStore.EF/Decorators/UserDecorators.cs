using System.Linq;

using Microsoft.EntityFrameworkCore;

using Pharmacies.Models;

namespace Pharmacies.DataStore.EF.Decorators
{
    public static class UserDecorators
    {
        public static IQueryable<User> WithPharmacy(this IQueryable<User> query)
            => query.Include(x => x.Pharmacy);
    }
}
