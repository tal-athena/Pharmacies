using System.Collections.Generic;

namespace Pharmacies.DataStore.Models
{
    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
    }

    public interface IEntity
    {
        List<string> IgnoreOnUpdate { get; }
    }
}
