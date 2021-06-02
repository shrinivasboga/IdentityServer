using CodeGenerator.IdentityServer.Models;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CodeGenerator.IdentityServer.Services
{
    public class CustomProfileService : IProfileService
    {
        private readonly UserManager<User> _userManager;

        public CustomProfileService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            User user = await _userManager.GetUserAsync(context.Subject);
            IList<string> userRoles = await _userManager.GetRolesAsync(user);

            IList<Claim> roleClaims = new List<Claim>();

            foreach (string userRole in userRoles)
            {
                roleClaims.Add(new Claim(JwtClaimTypes.Role, userRole));
            }
            context.IssuedClaims.AddRange(roleClaims);
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Email, $"{user.Email}"));
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.Name, $"{user.FirstName} {user.LastName}"));
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.GivenName, $"{user.FirstName}"));
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.FamilyName, $"{user.LastName}"));
            context.IssuedClaims.Add(new Claim(JwtClaimTypes.PreferredUserName, $"{user.FirstName} {user.LastName}"));
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.CompletedTask;
        }
    }
}
