using Brightsoft.Authentication.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Brightsoft.Data.Data;
using Brightsoft.Data.Identity.Accounts;
using Brightsoft.Data.Identity.Roles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace Brightsoft.Authentication
{
    public static class TokenHelpers
    {

        public static string GetCurrentUsername(HttpContext httpContext)
        {
            try
            {
                return httpContext.GetAccessToken().GetAwaiter().GetResult().GetUsername();
            }
            catch
            {
                return null;
            }
        }

        public static async Task<JwtSecurityToken> GetAccessToken(this HttpContext httpContext)
        {
            // Get access token from the request
            var accessToken = await httpContext.GetTokenAsync("access_token");

            if (accessToken == null)
            {
                return null;
            }

            // Decode token
            var tokenHandler = new JwtSecurityTokenHandler();

            return tokenHandler.ReadJwtToken(accessToken);
        }
    }
}
