using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pharmacies.App.Pages.Shared.Base
{
    public abstract class BasePage : PageModel
    {
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

        public BasePage()
        {
        }
    }
}
