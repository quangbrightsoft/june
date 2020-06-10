using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Brightsoft.Authentication.Jwt
{
    public class JwtGenerator : IJwtGenerator
    {
        public string GenerateToken(JwtSettings jwtSettings, JwtModel jwtModel)
        {
            if (jwtSettings == null)
            {
                throw new ArgumentNullException(nameof(jwtSettings));
            }

            if (jwtModel == null)
            {
                throw new ArgumentNullException(nameof(jwtModel));
            }

            if (string.IsNullOrEmpty(jwtSettings.SecretKey))
            {
                throw new ArgumentNullException("jwtSettings.SecretKey", "SecretKey cannot be null or empty.");
            }

            var claims = new List<Claim>
            {
                // JWT unique ID
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (!string.IsNullOrEmpty(jwtModel.Username))
            {
                // The subject of JWT (the user)
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, jwtModel.Username));
            }

            if (!string.IsNullOrEmpty(jwtModel.Email))
            {
                // The email of the user
                claims.Add(new Claim(JwtRegisteredClaimNames.Email, jwtModel.Email));
            }

            if (jwtModel.Roles != null)
            {
                // Add role to claims
                foreach (var role in jwtModel.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            claims.Add(new Claim(ClaimTypes.Name, jwtModel.Username));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));

            var token = new JwtSecurityToken(
                    issuer: jwtSettings.Issuer,
                    audience: jwtSettings.Audience,
                    expires: DateTime.UtcNow.AddSeconds(jwtSettings.ExpireInSeconds),
                    claims: claims,
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(JwtSettings jwtSettings, string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }

    public interface IJwtGenerator
    {
        string GenerateToken(JwtSettings jwtSettings, JwtModel jwtModel);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(JwtSettings jwtSettings, string token);
    }
}
