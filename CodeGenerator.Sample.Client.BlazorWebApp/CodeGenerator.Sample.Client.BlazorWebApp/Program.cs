using CodeGenerator.Sample.Client.BlazorWebApp.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator.Sample.Client.BlazorWebApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped<CustomAuthorizationMessageHandler>();

            builder.Services.AddHttpClient("CodeGenerator.Sample.Api", options =>
            {
                options.BaseAddress = new Uri(builder.Configuration["ApiBaseUrls:CodeGenerator.Sample.Api"]);
            })
            .AddHttpMessageHandler<CustomAuthorizationMessageHandler>();

            builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient("CodeGenerator.Sample.Api"));

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("Oidc", options.ProviderOptions);
                options.UserOptions.RoleClaim = "role";
            })
            .AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();

            builder.Services.AddAuthorizationCore(options =>
            {
                options.AddPolicy("ManagerPolicy", new AuthorizationPolicyBuilder()
                                                    .RequireAuthenticatedUser()
                                                    .RequireClaim("role", "Manager")
                                                    .Build());

                options.AddPolicy("ContributorPolicy", new AuthorizationPolicyBuilder()
                                                    .RequireAuthenticatedUser()
                                                    .RequireClaim("role", "Contributor")
                                                    .Build());
            });

            await builder.Build().RunAsync();
        }
    }
}
