using System;
using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace Pharmacies.Domain.Providers.Models
{
    public class AuthOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public TimeSpan LifeTime { get; set; }

        public string SecureKey { get; set; }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
            => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.SecureKey));
    }
}
