using System.Net;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Pharmacies.App.Controllers.Base;
using Pharmacies.DataStore;
using Pharmacies.Models;

namespace Pharmacies.App.Controllers
{
    [ApiController,
        Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme),
        Route("api/[controller]")]
    public sealed class MetaController : BaseController
    {
        public IRepository<Meta> Meta { private get; set; }

        // GET: api/Meta/{id}
        [ProducesResponseType(typeof(object), (int) HttpStatusCode.OK),
            HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            return this.Ok("value");
        }

        // GET: api/Meta
        [ProducesResponseType(typeof(object), (int) HttpStatusCode.OK),
            HttpGet]
        public IActionResult Get()
        {
            return this.Ok(new string[] { "value1", "value2" });
        }

        // PATCH: api/Meta/1
        [ProducesResponseType((int) HttpStatusCode.OK),
            HttpPatch("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] string value)
        {
            return this.Ok();
        }

        // DELETE: api/Meta/1
        [ProducesResponseType((int) HttpStatusCode.OK),
            HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            return this.Ok();
        }

        // PUT: api/Meta
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK),
            HttpPut]
        public IActionResult Create([FromBody] string value)
        {
            return this.Ok("id");
        }
    }
}
