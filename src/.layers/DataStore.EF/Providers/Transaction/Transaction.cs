using System.Data;

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace Pharmacies.DataStore.EF.Providers
{
    internal sealed class TransactionFactory : IScopeFactory
    {
        private DbContext Context => this.Current.Context;

        public ICurrentDbContext Current { private get; set; }

        public IScope Create(IsolationLevel isolation)
            => new Transaction(this.Context, this.Context.Database.BeginTransaction(isolation));

        public IScope Create()
            => this.Create(IsolationLevel.Unspecified);
    }

    internal sealed class Transaction : IScope
    {
        private readonly IDbContextTransaction scope;

        private readonly DbContext context;

        public Transaction(DbContext context, IDbContextTransaction scope)
        {
            this.context = context;
            this.scope = scope;
        }

        public void Rollback()
            => this.scope.Rollback();

        public void Dispose()
            => this.scope.Dispose();

        public void Commit()
        {
            this.context.SaveChanges();
            this.scope.Commit();
        }
    }
}
