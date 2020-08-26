using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pharmacies.App.Controllers.Base;
using Pharmacies.DataStore.EF.Decorators;
using Pharmacies.DataStore;
using Pharmacies.Domain.Providers;
using Pharmacies.Models;

using vm = Pharmacies.App.Models;

namespace Pharmacies.App.Controllers
{
    [AllowAnonymous]
    public sealed class AuthController : BaseController
    {
        public IRepository<User> Users { private get; set; }

        public TokenProvider Token  { private get; set; }

        public vm.UserData Data { private get; set; }

        private IEnumerable<Claim> GetClaims(User user)
             => new List<Claim>
             {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,           user.UserName),
                new Claim(ClaimTypes.Role,           user.IsSuperUser ? "Administrator" : "User")
             };

        [HttpPost("~/auth/login")]
        public async Task<IActionResult> Login([FromForm] vm.Authenticate model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            var user =
                this.Users
                    .Query
                    .WithPharmacy()
                    .FirstOrDefault(x => x.UserName == model.UserName);

            if (user == null)
            {
                return this.BadRequest("User not found");
            }

            if (!PasswordProvider.Verify(model.Password, user.Password))
            {
                return this.BadRequest("Incorrect user name or password");
            }

            var claims = this.GetClaims(user);

            var entity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await
             this.HttpContext
                 .SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(entity));

            var referer = new Uri(this.Request.Headers["Referer"].ToString());

            var request =
                HttpUtility.ParseQueryString(referer.Query)
                           .Get("ReturnUrl");

            if (user.PharmacyId.HasValue)
            {
                this.Data.Pharmacy.TryAdd(user.UserName, user.Pharmacy.Id);
            }

            return
               ! string.IsNullOrWhiteSpace(request)
                       ? this.Redirect(request)
                       : this.Redirect("/");
        }

        [HttpGet("~/auth/out")]
        public async Task<IActionResult> Out()
        {
            await this.HttpContext.SignOutAsync();

            return this.Redirect("/Login");
        }

        [ProducesResponseType(typeof(Microsoft.AspNetCore.Identity.SignInResult), 201),
            ProducesResponseType(typeof(BadRequestObjectResult), 401),
            Produces("application/json"),
            HttpPost("~/auth/token")]
        public IActionResult Get([FromForm] vm.Authenticate model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            var user = this.Users.Query.FirstOrDefault(x => x.UserName == model.UserName);

            if (user == null)
            {
                return this.BadRequest("User not found");
            }

            if (!PasswordProvider.Verify(model.Password, user.Password))
            {
                return this.BadRequest("Incorrect user name or password");
            }

            var claims = this.GetClaims(user);

            var token = this.Token.Generate(claims);

            return this.Ok(new
                {
                    access_token = token
                });
        }
    }
}
