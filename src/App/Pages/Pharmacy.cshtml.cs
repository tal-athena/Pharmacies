using Microsoft.AspNetCore.Mvc;

using Pharmacies.App.Pages.Shared.Base;

namespace Pharmacies.App
{
    public class PharmacyModel : BasePage 
    {
        public IActionResult OnGet()
        {
            if (!this.IsSuperUser)
                return this.Redirect("/");

            return this.Page();
        }
    }
}
