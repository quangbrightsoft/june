using Brightsoft.Authentication.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Brightsoft.Core.Identity.Accounts;
using Brightsoft.Core.Identity.Roles;
using Brightsoft.Data.Data;

namespace Brightsoft.Authentication
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            // Configure Entity Framework Identity with default token providers
            services.AddIdentity<Account, Role>()
                .AddRoles<Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Configure Identity options
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // HttpContext settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            return services;
        }

        public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, IConfigurationSection jwtSettingsSection)
        {
            var secretKey = jwtSettingsSection.GetSection(nameof(JwtSettings.SecretKey)).Value;
            var issuer = jwtSettingsSection.GetSection(nameof(JwtSettings.Issuer)).Value;
            var audience = jwtSettingsSection.GetSection(nameof(JwtSettings.Audience)).Value;
            var expireInSeconds = Convert.ToUInt64(jwtSettingsSection.GetSection(nameof(JwtSettings.ExpireInSeconds)).Value);

            // Configure JwtSettings
            services.Configure<JwtSettings>(options =>
            {
                options.SecretKey = secretKey;
                options.Issuer = issuer;
                options.Audience = audience;
                options.ExpireInSeconds = expireInSeconds;
            });

            services.AddSingleton(x => x.GetRequiredService<IOptions<JwtSettings>>().Value);

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }
    }
}
