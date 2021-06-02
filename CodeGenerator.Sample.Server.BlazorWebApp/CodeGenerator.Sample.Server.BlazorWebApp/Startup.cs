using CodeGenerator.Sample.Server.BlazorWebApp.Security;
using CodeGenerator.Sample.Server.BlazorWebApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CodeGenerator.Sample.Server.BlazorWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddHttpClient();
            services.AddScoped<TokenProvider>();

            services.AddScoped<IWeatherForecastService, WeatherForecastService>();
            services.AddScoped<IAccessTokenService, AccessTokenService>();

            services.AddHttpClient<IWeatherForecastService, WeatherForecastService>(client =>
            {
                //var tokenProvider = new TokenProvider();
                client.BaseAddress = new Uri(Configuration["ApiBaseUrls:CodeGenerator.Sample.Api"]);
            });

            services.AddHttpClient<IAccessTokenService, AccessTokenService>(client =>
            {
                //var tokenProvider = new TokenProvider();
                client.BaseAddress = new Uri(Configuration["ApiBaseUrls:CodeGenerator.Sample.Api"]);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.SignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;

                options.Authority = Configuration["Oidc:Authority"];
                options.ClientId = Configuration["Oidc:ClientId"];
                options.ClientSecret = Configuration["Oidc:ClientSecret"];

                // When set to code, the middleware will use PKCE protection
                options.ResponseType = OpenIdConnectResponseType.Code;

                // Save the tokens we receive from the IDP
                options.SaveTokens = true;

                // It's recommended to always get claims from the UserInfoEndpoint during the flow.
                options.GetClaimsFromUserInfoEndpoint = true;

                var scopes = Configuration.GetSection("Oidc:DefaultScopes").Get<IList<string>>();
                foreach (var scope in scopes)
                {
                    options.Scope.Add(scope);
                }

                options.ClaimActions.Add(new JsonKeyClaimAction("role", "role", "role"));

                options.Events = new OpenIdConnectEvents
                {
                    // called if user clicks Cancel during login
                    OnAccessDenied = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/");
                        return Task.CompletedTask;
                    }
                };
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
                app.UseExceptionHandler("/Error");
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
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
