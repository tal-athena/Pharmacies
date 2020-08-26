using System.Collections.Generic;
using System.Net;
using System.Text;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

using Pharmacies.App.Models.Common;

namespace Pharmacies.App.Filters
{
    public sealed class ErrorFilter : ExceptionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            var env = (IWebHostEnvironment)context.HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment));
            var message = new StringBuilder("Server error occurred: ");

            var exception = context.Exception;
            var error = string.Empty;
            while (exception != null)
            {
                error = exception.Message;
                message.AppendLine($"Message : {error}");

                if (env.IsDevelopment())
                {
                    message.AppendLine($"Source : {exception.Source}");
                    message.AppendLine($"StackTrace : {exception.StackTrace}");
                }

                exception = exception.InnerException;
            }

            context.HttpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            context.Result = new ObjectResult(new AggregatedError
            {
                Errors = new List<Authenticate> { new Authenticate { Title = error, Detail = message.ToString() } }
            });
        }
    }
}
