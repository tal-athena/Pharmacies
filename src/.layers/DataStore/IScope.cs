using System;
using System.Data;

namespace Pharmacies.DataStore
{
    public interface IScopeFactory
    {
        IScope Create(IsolationLevel isolation);
        IScope Create();
    }

    public interface IScope : IDisposable
    {
        void Rollback();
        void Commit();
    }
}
