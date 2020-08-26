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
    public sealed class TemplateController : BaseController
    {
        public IRepository<Template> Templates { private get; set; }

        // GET: api/Template/{id}
        [ProducesResponseType(typeof(object), (int) HttpStatusCode.OK),
            HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            return this.Ok("value");
        }

        // GET: api/Template
        [ProducesResponseType(typeof(object), (int) HttpStatusCode.OK),
            HttpGet]
        public IActionResult Get()
        {
            return this.Ok(new string[] { "value1", "value2" });
        }

        // PATCH: api/Template/1
        [ProducesResponseType((int) HttpStatusCode.OK),
            HttpPatch("{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] string value)
        {
            return this.Ok();
        }

        // DELETE: api/Template/1
        [ProducesResponseType((int) HttpStatusCode.OK),
            HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            return this.Ok();
        }

        // PUT: api/Template
        [ProducesResponseType(typeof(int), (int) HttpStatusCode.OK),
            HttpPut]
        public IActionResult Create([FromBody] string value)
        {
            return this.Ok("id");
        }
    }
}
