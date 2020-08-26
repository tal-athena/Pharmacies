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

using Pharmacies.App.Controllers.Base;
using Pharmacies.Domain.Services;
using Pharmacies.Models;

namespace Pharmacies.App.Controllers
{
    [ApiController,
        Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme),
        Route("api/[controller]")]
    public sealed class DocumentController : BaseController
    {
        public IWebHostEnvironment Environment { private get; set; }

        public DocumentService Documents { private get; set; }

        // DELETE: api/Document/1
        [ProducesResponseType((int) HttpStatusCode.OK),
            HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            this.Documents.Delete(id);

            return this.Ok();
        }

        // POST: api/Document
        [ProducesResponseType((int) HttpStatusCode.OK),
            HttpPost]
        public async Task<IActionResult> Upload([FromForm] int batch, List<IFormFile> files)
        {
            var size = files.Sum(x => x.Length);

            var id = 0;

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var name = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                    var path = $@"{this.Environment.WebRootPath}\Documents\{name}";

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    id = this.Documents.Insert(
                        new Document
                        {
                            DisplayName = name,
                            ActualName = file.FileName,
                            BatchId = batch
                        });
                }
            }

            return this.Ok(new { count = files.Count, size, id = id });
        }
    }
}
