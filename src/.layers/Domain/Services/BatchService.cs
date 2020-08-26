using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Pharmacies.DataStore;
using Pharmacies.Models;

namespace Pharmacies.Domain.Services
{
    public class BatchService
    {
        public IRepository<Batch> Batches { private get; set; }

        public IScopeFactory Scope { private get; set; }

        public IEnumerable<Batch> GetAll => this.Batches.All();

        public IQueryable<Batch> Query => this.Batches.Query;

        public Batch Get(int id) => this.Batches.Get(id);

        public int Insert(Batch batch)
        {
            batch.Created = DateTime.Now;

            using (var unit = this.Scope.Create(IsolationLevel.ReadCommitted))
            {
                this.Batches.Insert(batch);

                unit.Commit();
            }

            return batch.Id;
        }

        public void Update(Batch batch)
        {
            batch.IgnoreOnUpdate.Add(nameof(batch.Created));

            using (var unit = this.Scope.Create(IsolationLevel.ReadCommitted))
            {
                this.Batches.Update(batch);

                unit.Commit();
            }
        }

        public void Delete(int id)
        {
            using (var unit = this.Scope.Create(IsolationLevel.ReadCommitted))
            {
                this.Batches.Delete(new Batch { Id = id });

                unit.Commit();
            }
        }
    }
}
