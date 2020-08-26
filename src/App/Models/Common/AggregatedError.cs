using System.Collections.Generic;

namespace Pharmacies.App.Models.Common
{
    public class AggregatedError
    {
        public List<Authenticate> Errors { get; set; }
    }
}
