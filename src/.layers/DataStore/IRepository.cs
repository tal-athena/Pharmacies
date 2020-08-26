using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

using Pharmacies.DataStore.Models;

namespace Pharmacies.DataStore
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        IQueryable<TEntity> Query { get; }

        IEnumerable<TEntity> Many(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> All();

        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        TEntity Get(object id);

        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(object id);
    }
}
