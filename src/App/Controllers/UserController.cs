using System.Collections.Generic;
using System.Linq;
using System.Net;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AutoMapper;

using Pharmacies.App.Controllers.Base;
using Pharmacies.App.Models.Common;
using Pharmacies.DataStore.EF.Decorators;
using Pharmacies.Domain.Services;
using Pharmacies.Models;

using vm = Pharmacies.App.Models;

namespace Pharmacies.App.Controllers
{
    [ApiController,
        Authorize(Roles = "Administrator", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme),
        Route("api/[controller]")]
    public sealed class UserController : BaseController
    {
        public UserService Users { private get; set; }

        public IMapper Mapper { private get; set; }

        // GET: api/User/{id}
        [ProducesResponseType(typeof(vm.User), (int) HttpStatusCode.OK),
            HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var data = this.Users.Get(id);

            var map = this.Mapper.Map<vm.User>(data);

            return this.Ok(map);
        }

        // GET: api/User
        [ProducesResponseType(typeof(List<vm.User>), (int) HttpStatusCode.OK),
            HttpGet]
        public IActionResult Get([FromQuery] Page options = null)
        {
            var data =
                options != null
                    ? this.Users
                          .Query
                          .WithPharmacy()
                          .OrderBy(x => x.Id)
                          .Skip(options.Skip)
                          .Take(options.Take)
                          .ToList()

                    : this.Users
                          .Query
                          .WithPharmacy()
                          .ToList();

            var map = this.Mapper.Map<List<vm.User>>(data);

            return this.Ok(map);
        }

        // PUT: api/User
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK),
            HttpPut]
        public IActionResult Create([FromBody] vm.User value)
        {
            var data = this.Mapper.Map<User>(value);

            var id = this.Users.Insert(data);

            return this.Ok(id);
        }

        // PATCH: api/User
        [ProducesResponseType((int) HttpStatusCode.OK),
            HttpPatch]
        public IActionResult Update([FromBody] vm.User value)
        {
            var data = this.Mapper.Map<User>(value);

            if (data.PharmacyId.HasValue)
            {
                this.UpdatePharmacy(data.UserName, data.PharmacyId.Value);
            }

            this.Users.Update(data);

            return this.Ok();
        }

        // DELETE: api/User/1
        [ProducesResponseType((int) HttpStatusCode.OK),
            HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            this.Users.Delete(id);

            return this.Ok();
        }
    }
}
