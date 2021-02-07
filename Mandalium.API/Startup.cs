using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Mandalium.API.Helpers;
using Mandalium.Core.Context;
using Mandalium.Core.Interfaces;
using Mandalium.Core.Models;
using Mandalium.Infrastructure.Repositories;

namespace Mandalium.API
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
            services.AddDbContext<DataContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), m => m.MigrationsAssembly("Mandalium.Core")));
            services.AddScoped<IBlogRepository<BlogEntry>, BlogRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthRepository<User>, AuthRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddAutoMapper(config => config.AddProfile<AutoMapperProfile>());
            services.AddMemoryCache();
            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
            services.AddCors();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext context)
        {

            Extensions.FromMail = context.SystemSettings.FirstOrDefault(x => x.Key == "FromMail").Value;
            Extensions.FromMailPassword = context.SystemSettings.FirstOrDefault(x => x.Key == "FromMailPassword").Value;


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseDefaultFiles();
            app.UseStaticFiles();



            app.Use(async (httpContext, next) =>
            {
                httpContext.Response.Headers.Remove("Server");
                httpContext.Response.Headers.Remove("X-Powered-By");
                httpContext.Response.Headers.Add("X-Frame-Options", "DENY");
                httpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                httpContext.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
                await next();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
