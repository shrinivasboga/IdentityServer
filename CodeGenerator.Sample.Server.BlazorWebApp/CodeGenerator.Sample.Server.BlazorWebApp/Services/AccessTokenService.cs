using CodeGenerator.Sample.Server.BlazorWebApp.Models;
using CodeGenerator.Sample.Server.BlazorWebApp.Security;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CodeGenerator.Sample.Server.BlazorWebApp.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly HttpClient httpClient;

        public AccessTokenService(HttpClient httpClient,
                                  TokenProvider tokenProvider)
        {
            this.httpClient = httpClient;
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenProvider.AccessToken);
        }

        public async Task<IEnumerable<AccessTokenData>> GetAccessTokenDataAsync()
        {
            var response = await httpClient.GetAsync("/identity");
            if(!response.IsSuccessStatusCode)
            {
                return new List<AccessTokenData>();
            }
            else
            {
                return new List<AccessTokenData>();

                //return await response.Content.ReadAsAsync<AccessTokenData>();
            }
        }
    }
}
