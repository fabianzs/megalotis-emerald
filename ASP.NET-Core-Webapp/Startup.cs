using Microsoft.AspNetCore.Http;
using ASP.NET_Core_Webapp.Helpers;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using ASP.NET_Core_Webapp.Data;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Core_Webapp
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().AddJsonOptions(
            options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddDbContext<ApplicationContext>(
                builder => builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthorization(auth =>
                    {
                        auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                            .RequireAuthenticatedUser().Build());
                    });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            ValidateIssuerSigningKey = true,
                            ValidateLifetime = true,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:Jwt:Secret"])),
                            ClockSkew = TimeSpan.Zero
                        };
                        options.Events = new JwtBearerEvents()
                        {
                            OnAuthenticationFailed = c =>
                            {
                                c.NoResult();
                                c.Response.StatusCode = 401;
                                c.Response.ContentType = "application/json";
                                c.Response.WriteAsync(JsonConvert.SerializeObject(new CustomErrorMessage("Unauthorized"))).Wait();
                                return Task.CompletedTask;
                            },
                            OnChallenge = c =>
                            {
                                c.HandleResponse();
                                return Task.CompletedTask;
                            }
                        };
                    });

            services.AddScoped<IHelloService, HelloService>();
            services.AddSingleton<IAuthService, AuthService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Auth}/{action=Login}");
                });

            app.UseAuthentication();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        }
    }
}
