using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using RMISDemo.API.Models;
using RMISDemo.API.Models.DataManager;
using RMISDemo.API.Models.Dto;
using RMISDemo.API.Models.Repository;

namespace RMISDemo.API
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
            // Define the database context with the server connection string defined in the app settings.
            services.AddDbContext<RmisDbContext>(
                opts => opts.UseSqlServer(Configuration["ConnectionString:RmisDatabase"])
            );


            // Add JWT Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Grab the JWT Token from app settings
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(
                                Configuration.GetSection("Appsettings:Token").Value)),

                        ValidateIssuerSigningKey = true,
                        // Note: Turned off while in development, turn on in Prod.
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                });

            // Dependency Injection for the data repository for the Manager classes.
            services.AddScoped<IDataRepository<BlogPost, BlogPostDto>, BlogPostDataManager>();
            services.AddScoped<IDataRepository<BlogPostUrlCategory, BlogPostUrlTypeDto>, BlogPostUrlTypeDataManager>();
            services.AddScoped<IAuthRepository, AuthRepository>();

            // Here we ignore reference looping when the JSON object is returned from the controllers.
            // This will prevent lazy-loading circular references between items.
            // Install-Package Microsoft.AspNetCore.Mvc.NewtonsoftJson -Version 3.1.6
            services.AddControllers()
                .AddNewtonsoftJson(
                    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                ); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
