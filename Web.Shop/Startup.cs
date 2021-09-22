using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Shop.Constants;
using Web.Shop.Data;
using Web.Shop.Data.Entities.Identity;

namespace Web.Shop
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

            services.AddControllers();

            services.AddDbContext<AppEFContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<AppUser, AppRole>(options=>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 5;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<AppEFContext>()
                .AddDefaultTokenProviders();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web.Shop", Version = "v1" });
            });
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            RoleManager<AppRole> roleManager)
        {
            app.UseCors(options =>
                options.AllowAnyMethod().AllowAnyOrigin().AllowAnyHeader());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web.Shop v1"));
            }
            if(!roleManager.Roles.Any())
            {
                var result = roleManager.CreateAsync(new AppRole
                {
                    Name = Roles.Admin                
                }).Result;

                result = roleManager.CreateAsync(new AppRole
                {
                    Name = Roles.User
                }).Result;

                result = roleManager.CreateAsync(new AppRole
                {
                    Name = Roles.Operator
                }).Result;
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
