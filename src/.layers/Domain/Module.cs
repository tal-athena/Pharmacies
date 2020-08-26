using System;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using DryIoc;

using Pharmacies.Domain.Providers.Models;
using Pharmacies.Domain.Providers;
using Pharmacies.Domain.Services;

namespace Pharmacies.Domain
{
    public static class Module
    {
        public static IServiceCollection AddAuthentication(this IServiceCollection collection, IConfiguration configuration)
        {
            var section = configuration.GetSection(nameof(AuthOptions));
            var auth = section.Get<AuthOptions>();

            collection
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidAudience = auth.Audience,
                        ValidIssuer = auth.Issuer,
                        ClockSkew = TimeSpan.Zero,

                        IssuerSigningKey = auth.GetSymmetricSecurityKey()
                    };
                });

            collection.Configure<AuthOptions>(section);

            return collection;
        }

        public static void Install(Container container)
        {
            container.Register<TokenProvider>(Reuse.Transient);

            container.Register<DocumentService>(Reuse.Transient);
            container.Register<PharmacyService>(Reuse.Transient);
            container.Register<BatchService>(Reuse.Transient);
            container.Register<UserService>(Reuse.Transient);
        }
    }
}
