using System;
using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;

using Pharmacies.DataStore.EF.Decorators;
using Pharmacies.DataStore;
using Pharmacies.Models;

using vm = Pharmacies.App.Models;

namespace Pharmacies.App.Controllers.Base
{
    public abstract class BaseController : ControllerBase
    {
        public IRepository<User> users { private get; set; }

        public vm.UserData data { private get; set; }

        public bool IsAuthenticated => this.HttpContext?.User?.Identity.IsAuthenticated ?? false;

        public bool IsSuperUser
        {
            get
            {
                if (!this.IsAuthenticated)
                    return false;

                var role =
                    this.HttpContext
                       ?.User
                       ?.Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.Role)
                       ?.Value;

                return role == "Administrator";

            }
        }

        public int Pharmacy
        {
            get
            {
                var name = this.HttpContext.User.Identity.Name;

                var success =
                    this.data
                        .Pharmacy
                        .TryGetValue(name, out var pharmacy);

                if (success)
                    return pharmacy;

                var user =
                    this.users
                        .Query
                        .WithPharmacy()
                        .FirstOrDefault(x => x.UserName == name);

                if (user.Pharmacy == null)
                    throw new InvalidOperationException("User dosent related to any Pharmacy");

                this.data.Pharmacy.TryAdd(name, user.Pharmacy.Id);

                return user.Pharmacy.Id;
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void UpdatePharmacy(string user, int pharmacy)
            => this.data.Pharmacy.AddOrUpdate(user, pharmacy, (k, v) => pharmacy);
    }
}
