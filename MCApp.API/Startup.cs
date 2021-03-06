﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using MCApp.API.BackgroundServices;
using MCApp.API.Data;
using MCApp.API.Helpers;
using MCApp.API.ScheduledServices;
using MCApp.API.TypedHttpClients;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MCApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var key = System.Text.Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:token").Value);
            services.AddDbContext<DataContext>(x => x
                .UseSqlServer(Configuration.GetConnectionString("MCConnection"))
                // .UseSqlite(Configuration.GetConnectionString("DefaultConnection"))
                .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.IncludeIgnoredWarning)));
            services.AddMvc(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.InputFormatters.Add(new XmlSerializerInputFormatter(config));
                config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            }
            ).AddJsonOptions(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            }
            ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // services.AddTransient<Seed>();
            // services.BuildServiceProvider().GetService<DataContext>().Database.Migrate();
            services.AddCors();
            // services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddAutoMapper();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IMicroCreditRepository, MicroCreditRepository>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
            services.AddScoped<LogUserActivity>();
            // services.AddSingleton<IHostedService, Poller>();
            ScheduleTable.AddScopedServices(services);
            TemplateHttpClient.AddServices(services);
            ScheduleTable.AddScheduledServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }
            var urls = app.ServerFeatures.Get<IServerAddressesFeature>().Addresses;
            Console.WriteLine($"aantal urls={urls.Count}");
            foreach (var url in urls) {
                Console.WriteLine($"url={url}");
            }

            // seeder.SeedUsers();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                if (env.IsDevelopment())
                {
                    routes.MapSpaFallbackRoute(
                        name: "spa.fallback",
                        defaults: new { controller = "Fallback", action = "Index" }
                    );
                }
            });
        }
    }
}
