using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Pharmacies.Domain.Providers.Models;

namespace Pharmacies.Domain.Providers
{
    public class TokenProvider
    {
        public IOptions<AuthOptions> Options { private get; set; }

        public string Generate(IEnumerable<Claim> claims)
        {
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                this.Options.Value.Issuer,
                this.Options.Value.Audience,
                notBefore: now,
                expires: now.Add(this.Options.Value.LifeTime),
                claims: claims,
                signingCredentials:
                    new SigningCredentials(this.Options.Value.GetSymmetricSecurityKey(),
                                           SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
