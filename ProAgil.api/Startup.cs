using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using ProAgil.Repositorio;
using AutoMapper;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ProAgil.Dominio.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ProAgil.api
{
    public class Startup
    {
        
        public Startup(IConfiguration configuration) 
        {
            this.Configuration = configuration;
               
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ProAgil.Repositorio.ProAgilContext>(x => x.UseSqlServer(Configuration.GetConnectionString("dbAgileTeste")));
            services.AddScoped<IProAgilRepositorio, ProAgilRepositorio>();
            services.AddAutoMapper();

            IdentityBuilder builder = services.AddIdentityCore<User>(option => {
                            option.Password.RequireDigit = false;
                            option.Password.RequireNonAlphanumeric = false;
                            option.Password.RequireLowercase = false;
                            option.Password.RequireUppercase= false;
                            option.Password.RequiredLength = 4;  
            });

            builder = new IdentityBuilder(builder.UserType, typeof(Role), builder.Services);
            builder.AddEntityFrameworkStores<ProAgilContext>();
            builder.AddRoleValidator<RoleValidator<Role>>();
            builder.AddRoleManager<RoleManager<Role>>();
            builder.AddSignInManager<SignInManager<User>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(option => {
                                option.TokenValidationParameters = new TokenValidationParameters
                                {
                                    ValidateIssuerSigningKey = true,
                                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                                    ValidateIssuer = false,
                                    ValidateAudience = false
                                };
                        });
            


            services.AddMvc(option => {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                option.Filters.Add(new AuthorizeFilter(policy));
            } )
               .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
               .AddJsonOptions(option => option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


            services.AddCors();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseStaticFiles();
            app.UseStaticFiles(
                new StaticFileOptions() {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                    RequestPath = new PathString("/Resources")
                }
            );
        }
    }
}
