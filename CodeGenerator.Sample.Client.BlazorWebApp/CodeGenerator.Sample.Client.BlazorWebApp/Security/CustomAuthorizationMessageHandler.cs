using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeGenerator.Sample.Client.BlazorWebApp.Security
{
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigation)
            : base(provider, navigation)
        {
            ConfigureHandler(
                authorizedUrls: new[] { "https://localhost:44322" },
                scopes: new[] { "CodeGenerator.Sample.Api" });
        }
    }
}
