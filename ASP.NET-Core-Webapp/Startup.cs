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
using Microsoft.EntityFrameworkCore;
using ASP.NET_Core_Webapp.SeedData;

namespace ASP.NET_Core_Webapp
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IHostingEnvironment env;

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            this.configuration = configuration;
            this.env = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Seed seedTest = new Seed();
            seedTest.writeAllRows();

            services.AddCors();
            services.AddMvc();
            services.AddScoped<IHelloService, HelloService>();

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
                    });
            // After ApplicationContext is ready, remove comment backslashes!!!

            if (env.IsDevelopment())

            {
                services.AddDbContext<ApplicationContext>(builder =>
                        builder.UseInMemoryDatabase("InMemoryDatabase"));
            }
            if (env.IsProduction())
            {
                services.AddDbContext<ApplicationContext>(builder =>
                        builder.UseSqlServer(configuration.GetConnectionString("ProductionConnection")));
            }

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
