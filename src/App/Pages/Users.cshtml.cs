using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using Pharmacies.App.Pages.Shared.Base;
using Pharmacies.DataStore;
using Pharmacies.Models;

namespace Pharmacies.App
{
    public class UsersModel : BasePage 
    {
        private readonly IRepository<Pharmacy> pharmacy;

        [BindProperty]
        public List<SelectListItem> PharmacyData { get; set; }

        public UsersModel(IRepository<Pharmacy> pharmacy)
        {
            this.PharmacyData =
                 new List<SelectListItem>
                 {
                     new SelectListItem
                     {
                        Value = "-1",
                        Text = string.Empty
                     }
                 };

            this.pharmacy = pharmacy;
        }

        public IActionResult OnGet()
        {
            if (!this.IsSuperUser)
                return this.Redirect("/");

            this.PharmacyData.AddRange(
                this.pharmacy.All()
                    .Select(x =>
                        new SelectListItem
                        {
                            Value = x.Id.ToString(),
                            Text = x.Name
                        }));

            return this.Page();
        }
    }
}
