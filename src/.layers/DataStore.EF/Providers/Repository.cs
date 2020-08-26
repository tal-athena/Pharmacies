using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;

using Pharmacies.DataStore.Models;

namespace Pharmacies.DataStore.EF.Providers
{
    internal sealed class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity

    {
        private DbSet<TEntity> Set => this.Context.Set<TEntity>();

        private DbContext Context => this.Current.Context;

        public ICurrentDbContext Current { private get; set; }

        public IQueryable<TEntity> Query
            => this.Set;

        public IEnumerable<TEntity> Many(Expression<Func<TEntity, bool>> predicate)
            => this.Query.Where(predicate).AsEnumerable();
        public IEnumerable<TEntity> All()
            => this.Query.AsEnumerable();

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
            => this.Query.FirstOrDefault(predicate);
        public TEntity Get(object id)
        {
            var parameterExp = Expression.Parameter(typeof (TEntity));
            var propertyExp = Expression.Property(parameterExp, "Id");
            var idExp = Expression.Constant(id);
            var eqExp = Expression.Equal(propertyExp, idExp);
            var expr = Expression.Lambda<Func<TEntity, bool>>(eqExp, parameterExp);

            return this.Query.FirstOrDefault(expr);
        }

        public void Insert(TEntity entity)
            => this.Context.Add(entity);

        public void Update(TEntity entity)
        {
            var attached = this.Context.Entry(entity);

            attached.State = EntityState.Modified; 

            foreach(var ignore in entity.IgnoreOnUpdate)
            {
                attached.Property(ignore).IsModified = false;
            }
        }

        public void Delete(TEntity entity)
            => this.Set.Remove(entity);
        public void Delete(object id)
        {
            var entity = this.Get(id);

            if (entity != null)
            {
                this.Delete(entity);
            }
        }
    }
}
