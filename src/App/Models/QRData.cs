using System.Collections.Concurrent;

namespace Pharmacies.App.Models
{
    public class QRData
    {
        public bool WithBatch { get; set; }
        public string AdditionalText { get; set; }
        public bool WithLogo { get; set; }
    }
}
