using ASP.NET_Core_Webapp.Data;
using ASP.NET_Core_Webapp.SeedData;
using ASP.NET_Core_Webapp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

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

            var allvariables = Environment.GetEnvironmentVariables();
            
            services.AddCors();
            services.AddMvc();
            services.AddScoped<IHelloService, HelloService>();

            if (env.IsDevelopment())
                
            {
                services.AddDbContext<ApplicationContext>(builder =>
                        builder.UseInMemoryDatabase("InMemoryDatabase"));
            }
            if (env.IsProduction())
            {
                //Debugger.Launch();
                services.AddDbContext<ApplicationContext>(builder =>
                        builder.UseSqlServer(configuration.GetConnectionString("environmentString"))
                        .EnableSensitiveDataLogging(true));
            }

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
            services.AddSingleton<IAuthService, AuthService>();
            services.AddScoped<SlackService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationContext applicationContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedDatabaseHandler seedDataFromObject = new SeedDatabaseHandler(applicationContext, configuration);
                seedDataFromObject.FillDatabaseFromObject();
            }
            if (env.IsProduction())
            {
                SeedDatabaseHandler seedDataFromObject = new SeedDatabaseHandler(applicationContext, configuration);

                seedDataFromObject.FillDatabaseFromObject();

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
