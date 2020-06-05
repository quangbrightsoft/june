using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Brightsoft.Authentication.Jwt
{
    public class JwtGenerator
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
    }
}
