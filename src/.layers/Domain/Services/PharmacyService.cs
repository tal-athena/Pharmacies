using System.Collections.Generic;
using System.Data;
using System.Linq;

using Pharmacies.DataStore;
using Pharmacies.Models;

namespace Pharmacies.Domain.Services
{
    public class PharmacyService
    {
        public IRepository<Pharmacy> Pharmacy { private get; set; }

        public IScopeFactory Scope { private get; set; }

        public IEnumerable<Pharmacy> GetAll => this.Pharmacy.All();

        public IQueryable<Pharmacy> Query => this.Pharmacy.Query;

        public Pharmacy Get(int id) => this.Pharmacy.Get(id);

        public int Insert(Pharmacy pharmacy)
        {
            using (var unit = this.Scope.Create(IsolationLevel.ReadCommitted))
            {
                this.Pharmacy.Insert(pharmacy);

                unit.Commit();
            }

            return pharmacy.Id;
        }

        public void Update(Pharmacy pharmacy)
        {
            using (var unit = this.Scope.Create(IsolationLevel.ReadCommitted))
            {
                this.Pharmacy.Update(pharmacy);

                unit.Commit();
            }
        }

        public void Delete(int id)
        {
            using (var unit = this.Scope.Create(IsolationLevel.ReadCommitted))
            {
                this.Pharmacy.Delete(new Pharmacy { Id = id });

                unit.Commit();
            }
        }
    }
}
