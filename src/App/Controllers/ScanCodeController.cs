using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using QRCoder;

using Pharmacies.App.Controllers.Base;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Microsoft.AspNetCore.Hosting;
using Svg;

namespace Pharmacies.App.Controllers
{
    [ApiController,
        Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme),
        Route("api/[controller]")]
    public sealed class ScanCodeController : BaseController
    {

        private readonly IWebHostEnvironment _hostEnvironment;
        public ScanCodeController(IWebHostEnvironment hostingEnvironment)
        {
            this._hostEnvironment = hostingEnvironment;
        }

        // GET: api/ScanCode/{id}
        [ProducesResponseType(typeof(string), (int) HttpStatusCode.OK),
            HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        {
            var qrCodeImage = this.GenerateQRCode(id);
            var img = this.BitmapToBytes(qrCodeImage);
            return this.Ok(Convert.ToBase64String(img));
        }

        // POST: api/ScanCode/{id}
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK),
            HttpPost("{id}")]
        public IActionResult Post([FromRoute] int id, [FromBody] Models.QRData qrData)
        {
            var qrCodeImage = this.GenerateQRCode(id, qrData.WithLogo);
            if (qrData.WithBatch)
                qrCodeImage = this.AddText(qrCodeImage, $"Batch id: {id}");
            if (!string.IsNullOrEmpty(qrData.AdditionalText))
                qrCodeImage = this.AddText(qrCodeImage, qrData.AdditionalText, false);
            var img = this.BitmapToBytes(qrCodeImage);
            return this.Ok(Convert.ToBase64String(img));
        }

        private Bitmap GenerateQRCode(int id, bool withLogo = false)
        {
            var address = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode($"{address}/page/{id}", QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);

            // https://github.com/codebude/QRCoder/issues/151
            // https://rextester.com/ORXUS66866
            
            var ppm = 100 / qrCodeData.ModuleMatrix.Count();
            if (!withLogo)
                return qrCode.GetGraphic(ppm);
            var path = this._hostEnvironment.WebRootFileProvider.GetFileInfo("Copeia.svg")?.PhysicalPath;
            var svgDocument = SvgDocument.Open(path);
            var logo = svgDocument.Draw();
            return qrCode.GetGraphic(ppm, Color.Black, Color.White, logo, 25, 2);
        }

        private Bitmap AddText(Bitmap source, string text, bool beforeSource = true)
        {
            var textImageHeight = 13 * (int)Math.Ceiling((decimal)text.Length / 13);
            var result = new Bitmap(source.Width, source.Height + textImageHeight);
            using (var textImage = new Bitmap(source.Width, textImageHeight))
            {
                var rectf = new RectangleF(0, 0, textImage.Width, textImage.Height);

                using (var g = Graphics.FromImage(textImage))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    var format = new StringFormat()
                    {
                        Alignment = StringAlignment.Center,
                        LineAlignment = StringAlignment.Near
                    };
                    g.DrawString(text, new Font("Tahoma", 8), Brushes.Black, rectf, format);
                    g.Flush();
                }

                using (var canvas = Graphics.FromImage(result))
                {
                    canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    if (beforeSource)
                    {
                        canvas.DrawImage(textImage, 0, 0);
                        canvas.DrawImage(source, 0, textImage.Height);
                    }
                    else
                    {
                        canvas.DrawImage(source, 0, 0);
                        canvas.DrawImage(textImage, 0, source.Height);
                    }
                    canvas.Save();
                }
            }

            return result;
        }

        private Byte[] BitmapToBytes(Bitmap img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
