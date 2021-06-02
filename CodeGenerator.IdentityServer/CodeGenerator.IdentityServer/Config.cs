// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using CodeGenerator.IdentityServer.Data;
using CodeGenerator.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.AspNetCore.Builder;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace CodeGenerator.IdentityServer
{
    public static class Config
    {
        private static readonly List<string> Roles = new()
        {
            "Manager",
            "Contributor"
        };

        public static void CleanDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                if (context.Database.EnsureDeleted())
                {
                    Log.Information("Database is sucessfully deleted");
                }
            }
        }        

        public static void InitializeIdentityServerDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                //Add Clients
                foreach (var client in Clients)
                {
                    if (!context.Clients.Any(t => t.ClientId == client.ClientId))
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                }
                context.SaveChanges();

                //Add Identity Resource
                foreach (var resource in IdentityResources)
                {
                    if (!context.IdentityResources.Any(t => t.Name == resource.Name))
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                }
                context.SaveChanges();

                //Add API Scope
                foreach (var scope in ApiScopes)
                {
                    if (!context.ApiScopes.Any(t => t.Name == scope.Name))
                    {
                        context.ApiScopes.Add(scope.ToEntity());
                    }
                }
                context.SaveChanges();

                //Add API Resources
                foreach (var resource in ApiResources)
                {
                    if (!context.ApiResources.Any(t => t.Name == resource.Name))
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                }
                context.SaveChanges();
            }
        }

        private static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

        public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
        {
            new ApiScope("CodeGenerator.Sample.Api", "Sample API")
        };

        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("CodeGenerator.Sample.Api", "Sample API")
            {
                Scopes = { "CodeGenerator.Sample.Api" }
            }
        };

        private static IEnumerable<Client> Clients => new List<Client>
        {
            // machine to machine client
            new Client
            {
                ClientName = "Console App",
                ClientId = "CodeGenerator.Sample.Client.ConsoleApp",
                ClientSecrets = { new Secret("secret".Sha256()) },

                RequireClientSecret = true,

                AllowedGrantTypes = GrantTypes.ClientCredentials,

                AllowOfflineAccess = true,
                UpdateAccessTokenClaimsOnRefresh = true,

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "CodeGenerator.Sample.Api",
                }
            },

            // interactive ASP.NET Core MVC client
            new Client
            {
                ClientName = "MVC Web App",
                ClientId = "CodeGenerator.Sample.Client.MvcWebApp",
                ClientSecrets = { new Secret("secret".Sha256()) },

                RequireClientSecret = true,

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44333/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:44333/signout-callback-oidc" },
                AllowedCorsOrigins = { "https://localhost:44333" },

                AllowOfflineAccess = true,
                UpdateAccessTokenClaimsOnRefresh = true,

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "CodeGenerator.Sample.Api",
                }
            },

            // JavaScript Client
            new Client
            {
                ClientName = "JavaScript Web App",
                ClientId = "CodeGenerator.Sample.Client.JsWebApp",
                ClientSecrets = { new Secret("secret".Sha256()) },

                RequireClientSecret = false,

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44344/callback.html" },
                PostLogoutRedirectUris = { "https://localhost:44344/index.html" },
                AllowedCorsOrigins = { "https://localhost:44344" },

                AllowOfflineAccess = true,
                UpdateAccessTokenClaimsOnRefresh = true,

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "CodeGenerator.Sample.Api",
                }
            },

            //Blazor App
            new Client
            {
                ClientName = "Blazor Web App",
                ClientId = "CodeGenerator.Sample.Client.BlazorWebApp",
                ClientSecrets = { new Secret("secret".Sha256()) },

                RequireClientSecret = false,

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44355/authentication/login-callback" },
                PostLogoutRedirectUris = { "https://localhost:44355/authentication/logout-callback" },
                AllowedCorsOrigins = { "https://localhost:44355" },

                AllowOfflineAccess = true,
                UpdateAccessTokenClaimsOnRefresh = true,

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "CodeGenerator.Sample.Api",
                }
            },

            //Blazor Server App
            new Client
            {
                ClientName = "Blazor Server Web App",
                ClientId = "CodeGenerator.Sample.Server.BlazorWebApp",
                ClientSecrets = { new Secret("secret".Sha256()) },

                RequireClientSecret = false,

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:44366/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:44366/signout-callback-oidc" },
                AllowedCorsOrigins = { "https://localhost:44366" },

                AllowOfflineAccess = true,
                UpdateAccessTokenClaimsOnRefresh = true,

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "CodeGenerator.Sample.Api",
                }
            }
        };

        public static void InitializeIdentityDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
                AddRoles(roleManager);

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                AddUsers(userManager);
            }
        }

        private static void AddRoles(RoleManager<IdentityRole<int>> roleManager)
        {
            foreach (var rolename in Roles)
            {
                var role = roleManager.FindByNameAsync(rolename).Result;
                if (role == null)
                {
                    role = new IdentityRole<int>(rolename);
                    var result = roleManager.CreateAsync(role).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }
                }
                else
                {
                    Log.Debug($"{rolename} role already exists");
                }
            }
        }

        private static void AddUsers(UserManager<User> userManager)
        {
            for (int i = 1; i <= 10; i++)
            {
                var user = userManager.FindByNameAsync($"user{i}").Result;
                if (user == null)
                {
                    user = new User
                    {
                        FirstName = "User",
                        LastName = $"{i}",
                        UserName = $"user{i}",
                        Email = $"user{i}@email.com",
                        EmailConfirmed = true,
                    };

                    //Add User
                    var result = userManager.CreateAsync(user, user.UserName).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    //Add User Role
                    result = i == 1 ? userManager.AddToRolesAsync(user, Roles).Result
                                    : userManager.AddToRoleAsync(user, Roles[1]).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    Log.Debug($"user{i} is created");
                }
                else
                {
                    Log.Debug($"user{i} already exists");
                }
            }
        }
    }
}