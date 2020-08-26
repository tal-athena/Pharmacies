using System.Collections.Concurrent;

namespace Pharmacies.App.Models
{
    public sealed class UserData
    {
        public ConcurrentDictionary<string, int> Pharmacy { get; } = new ConcurrentDictionary<string, int>();
    }
}
