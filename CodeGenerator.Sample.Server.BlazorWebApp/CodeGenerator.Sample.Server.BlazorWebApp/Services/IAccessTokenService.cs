using CodeGenerator.Sample.Server.BlazorWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeGenerator.Sample.Server.BlazorWebApp.Services
{
    public interface IAccessTokenService
    {
        Task<IEnumerable<AccessTokenData>> GetAccessTokenDataAsync();
    }
}
