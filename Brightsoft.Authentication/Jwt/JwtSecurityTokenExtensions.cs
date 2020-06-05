using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Brightsoft.Authentication.Jwt
{
    public static class JwtSecurityTokenExtensions
    {
        private static string GetClaimValueByName(this JwtSecurityToken token, string claimName)
        {
            if (token == null)
            {
                return null;
            }

            var claim = token.Claims.FirstOrDefault(x => x.Type == claimName);

            if (claim == null)
            {
                return null;
            }

            return claim.Value;
        }

        public static string GetUsername(this JwtSecurityToken token)
        {
            return token.GetClaimValueByName(JwtRegisteredClaimNames.Sub);
        }
    }
}
