using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeGenerator.Sample.Server.BlazorWebApp.Pages
{
    public class LoginModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync(string redirectUri)
        {
            // just to remove compiler warning
            await Task.CompletedTask;

            if (string.IsNullOrWhiteSpace(redirectUri))
            {
                redirectUri = Url.Content("~/");
            }

            // If user is already logged in, we can redirect directly...
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Response.Redirect(redirectUri);
            }

            var authenticationProperty = new AuthenticationProperties
            {
                RedirectUri = redirectUri
            };

            return Challenge(authenticationProperty, OpenIdConnectDefaults.AuthenticationScheme);
        }
    }
}
