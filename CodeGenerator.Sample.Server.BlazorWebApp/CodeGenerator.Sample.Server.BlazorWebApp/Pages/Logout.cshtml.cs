using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace CodeGenerator.Sample.Server.BlazorWebApp.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly IConfiguration configuration;
        public LogoutModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        //In a normal case, one can use this
        public async Task<IActionResult> OnGetAsync()
        {
            // just to remove compiler warning
            await Task.CompletedTask;

            var authenticationProperty = new AuthenticationProperties
            {
                RedirectUri = configuration["ApplicationUrl"]
            };

            return SignOut(authenticationProperty, 
                                OpenIdConnectDefaults.AuthenticationScheme,
                                CookieAuthenticationDefaults.AuthenticationScheme);
        }
		
        //To protect from XSRF, brute force attacks, use POST
		public async Task<IActionResult> OnPostAsync()
        {
			// just to remove warning
            await Task.CompletedTask;

            var authenticationProperty = new AuthenticationProperties
            {
                RedirectUri = configuration["ApplicationUrl"]
            };

            return SignOut(authenticationProperty, 
                                OpenIdConnectDefaults.AuthenticationScheme,
                                CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
