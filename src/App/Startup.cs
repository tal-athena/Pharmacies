using System.Collections.Generic;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using AutoMapper;
using FluentValidation.AspNetCore;
using Newtonsoft.Json.Serialization;

using Pharmacies.App.Filters;
using Pharmacies.DataStore.EF;
using Pharmacies.Domain;

namespace Pharmacies.App
{
    public sealed class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = this.Configuration.GetConnectionString("Default");

            services
               .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
                   options =>
                   {
                       // ToDo logout
                       options.LoginPath = "/Login";
                   });

            services
               //.AddAuthentication(this.Configuration)
               .AddDistributedMemoryCache()
               .AddSwaggerGen(x =>
                {
                    x.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header,
                            },
                            new List<string>()
                        }
                    });

                    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Description =
                           "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                        Scheme = "Bearer",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        In = ParameterLocation.Header
                    });

                    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Pharmacies App API", Version = "v1" });
                 })
               .AddAutoMapper(x =>
                {
                    x.ForAllMaps((obj, options) => options.ForAllMembers(o => o.Condition((src, dest, member) => member != null)));
                }, typeof(Startup))
               .AddDataStore(connection);

            services.AddRazorPages();

            var mvc = 
                services
                   .AddMvc(options => options.Filters.Add(new ErrorFilter()));


            mvc.AddControllersAsServices()
               .AddFluentValidation(options => { options.RegisterValidatorsFromAssemblyContaining<Startup>(); })
               .AddNewtonsoftJson(options =>
                {
                    if (options.SerializerSettings.ContractResolver is DefaultContractResolver resolver)
                    {
                        resolver.NamingStrategy = new DefaultNamingStrategy();
                    }
                })
               .AddRazorPagesOptions(options =>
                {
                    options
                       .Conventions
                       .AuthorizePage("/Pharmacy")
                       .AuthorizePage("/Batches")
                       .AuthorizePage("/Users");

                    options
                       .Conventions
                       .AllowAnonymousToFolder("/swagger")
                       .AllowAnonymousToPage("/Index")
                       .AllowAnonymousToPage("/Login");
                })
               .AddRazorOptions(options =>
                {
                    options.PageViewLocationFormats.Add("/Pages/Partials/Modals/{0}.cshtml");
                    options.PageViewLocationFormats.Add("/Pages/Partials/{0}.cshtml");
                });
               //.AddJsonOptions(options =>
               // {
               //     options.JsonSerializerOptions.PropertyNamingPolicy = null;
               // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection()
               .UseStaticFiles()
               .UseRouting()
               .UseAuthentication()
               .UseAuthorization()
               .UseEndpoints(endpoints =>
                {
                   endpoints.MapControllers();
                   endpoints.MapRazorPages();
                })
               .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pharmacies App API V1");
                })
               .UseSwagger();
        }
    }
}
