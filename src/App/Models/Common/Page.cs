namespace Pharmacies.App.Models.Common
{
    public sealed class Page
    {
        private readonly int max = 100;
 
        private int size = 36;

        public int Skip { get; set; }

        public int Take
        {
            get => this.size;
            set => this.size = (value > this.max) ? this.max : value;
        }
    }
}
