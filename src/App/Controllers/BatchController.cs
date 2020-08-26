using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme),
        Route("api/[controller]")]
    public sealed class BatchController : BaseController
    {
        public IWebHostEnvironment Environment { private get; set; }

        public BatchService Batches { private get; set; }

        public IMapper Mapper { private get; set; }

        // GET: api/Batch/{id}
        [ProducesResponseType(typeof(vm.Batch), (int) HttpStatusCode.OK),
            HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var data = this.Batches.Get(id);

            var map = this.Mapper.Map<vm.Batch>(data);

            return this.Ok(map);
        }

        // GET: api/Batch
        [ProducesResponseType(typeof(List<vm.Batch>), (int) HttpStatusCode.OK),
            HttpGet]
        public IActionResult Get([FromQuery] Page options = null)
        {
            var query = this.Batches.Query;

            if (!this.IsSuperUser)
            {
                int? pharmacy = null;

                try
                {
                    pharmacy = this.Pharmacy;
                }
                catch (InvalidOperationException)
                {
                }

                if (!pharmacy.HasValue)
                {
                    return this.Ok(new List<vm.Batch>());
                }

                query = query.Where(x => x.PharmacyId == this.Pharmacy);
            }


            var data =
                options != null
                    ? query.WithDocuments()
                           .WithPharmacy()
                           .OrderBy(x => x.Id)
                           .Skip(options.Skip)
                           .Take(options.Take)
                           .ToList()

                    : query.WithDocuments()
                           .WithPharmacy()
                           .ToList();

            var map = this.Mapper.Map<List<vm.Batch>>(data);

            return this.Ok(map);
        }

        // PUT: api/Batch
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK),
            HttpPut]
        public IActionResult Create([FromBody] vm.Batch value)
        {
            var data = this.Mapper.Map<Batch>(value);

            data.PharmacyId = this.Pharmacy;

            var id = this.Batches.Insert(data);

            return this.Ok(id);
        }

        // PATCH: api/Batch
        [ProducesResponseType((int) HttpStatusCode.OK),
            HttpPatch]
        public IActionResult Update([FromBody] vm.Batch value)
        {
            var data = this.Mapper.Map<Batch>(value);

            data.PharmacyId = this.Pharmacy;

            this.Batches.Update(data);

            return this.Ok();
        }

        // DELETE: api/Batch/1
        [ProducesResponseType((int) HttpStatusCode.OK),
            HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            this.Batches.Delete(id);

            return this.Ok();
        }
    }
}
