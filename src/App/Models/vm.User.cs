using Pharmacies.App.Models.Common;

namespace Pharmacies.App.Models
{
    public sealed partial class User
    {
        public NameWithId Pharmacy { get; set; }

        public string NewPassword { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public bool IsSuper { get; set; }

        public int Id { get; set; }
    }
}
