using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace CodeGenerator.Sample.Client.MvcWebApp
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
            services.AddControllersWithViews();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = Configuration["Oidc:Authority"];
                options.ClientId = Configuration["Oidc:ClientId"];
                options.ClientSecret = Configuration["Oidc:ClientSecret"];
                options.ResponseType = OpenIdConnectResponseType.Code;

                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;

                var scopes = Configuration.GetSection("Oidc:DefaultScopes").Get<IList<string>>();
                foreach (var scope in scopes)
                {
                    options.Scope.Add(scope);
                }

                options.ClaimActions.Add(new JsonKeyClaimAction("role", "role", "role"));
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ManagerPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Manager");
                });
                options.AddPolicy("ContributorPolicy", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireRole("Contributor");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                                name: "default",
                                pattern: "{controller=Home}/{action=Index}/{id?}")
                         .RequireAuthorization();
            });
        }
    }
}
