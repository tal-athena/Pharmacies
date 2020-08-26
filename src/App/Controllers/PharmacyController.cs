using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using AutoMapper;

using Pharmacies.App.Controllers.Base;
using Pharmacies.App.Models.Common;
using Pharmacies.Domain.Services;
using Pharmacies.Models;

using vm = Pharmacies.App.Models;

namespace Pharmacies.App.Controllers
{
    [ApiController,
        Authorize(Roles = "Administrator", AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme),
        Route("api/[controller]")]
    public sealed class PharmacyController : BaseController
    {
        public IWebHostEnvironment Environment { private get; set; }

        public PharmacyService Pharmacies { private get; set; }

        public IMapper Mapper { private get; set; }

        // GET: api/Pharmacy/{id}
        [ProducesResponseType(typeof(vm.Pharmacy), (int) HttpStatusCode.OK),
            HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var data = this.Pharmacies.Get(id);

            var map = this.Mapper.Map<vm.Pharmacy>(data);

            return this.Ok(map);
        }

        // GET: api/Pharmacy
        [ProducesResponseType(typeof(List<vm.Pharmacy>), (int) HttpStatusCode.OK),
            HttpGet]
        public IActionResult Get([FromQuery] Page options = null)
        {
            var data =
                options != null
                    ? this.Pharmacies
                          .Query
                          .OrderBy(x => x.Id)
                          .Skip(options.Skip)
                          .Take(options.Take)
                          .ToList()

                    : this.Pharmacies.GetAll;

            var map = this.Mapper.Map<List<vm.Pharmacy>>(data);

            return this.Ok(map);
        }

        // PUT: api/Pharmacy
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK),
            HttpPut]
        public IActionResult Create([FromBody] vm.Pharmacy value)
        {
            var data = this.Mapper.Map<Pharmacy>(value);

            var id = this.Pharmacies.Insert(data);

            return this.Ok(id);
        }

        // PATCH: api/Pharmacy
        [ProducesResponseType((int) HttpStatusCode.OK),
            HttpPatch]
        public IActionResult Update([FromBody] vm.Pharmacy value)
        {
            var data = this.Mapper.Map<Pharmacy>(value);

            this.Pharmacies.Update(data);

            return this.Ok();
        }

        // DELETE: api/Pharmacy/1
        [ProducesResponseType((int) HttpStatusCode.OK),
            HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            this.Pharmacies.Delete(id);

            return this.Ok();
        }

        // POST: api/Pharmacy/1
        [ProducesResponseType((int) HttpStatusCode.OK),
            HttpPost("{id}")]
        public async Task<IActionResult> Upload([FromRoute] int id, [FromForm] string pharmacy, List<IFormFile> files)
        {
            var size = files.Sum(x => x.Length);

            var name = string.Empty;

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    name = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                    var path = $@"{this.Environment.WebRootPath}\Pharmacy\Icons\{name}";

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    this.Pharmacies.Update(
                        new Pharmacy
                        {
                            Name = pharmacy,
                            Logo = name,
                            Id = id
                        });
                }
            }

            return this.Ok(new { count = files.Count, size, name = name });
        }
    }
}
