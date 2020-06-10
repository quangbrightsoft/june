using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Brightsoft.Authentication.Jwt;
using Brightsoft.Data.Data;
using Brightsoft.Data.Entities;
using Brightsoft.Data.Identity.Accounts;
using Brightsoft.JuneApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Brightsoft.JuneApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly SignInManager<Account> _signInManager;
        private readonly UserManager<Account> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly ApplicationDbContext _context;

        public AccountService(ILogger<AccountService> logger,
            JwtSettings jwtSettings,
            SignInManager<Account> signInManager,
            UserManager<Account> userManager,
            ApplicationDbContext context,
            IConfiguration configuration,
            IJwtGenerator jwtGenerator)
        {
            _jwtSettings = jwtSettings;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _jwtGenerator = jwtGenerator;
            _context = context;
        }

        private async Task<string> GenerateToken(JwtSettings jwtSettings, Account user)
        {
            // Get roles of the user to add to the claims later
            var userRoles = await _userManager.GetRolesAsync(user);

            var jwtModel = new JwtModel
            {
                Username = user.UserName,
                Roles = userRoles
            };

            return _jwtGenerator.GenerateToken(jwtSettings, jwtModel);
        }

        #region Login & Register

        public async Task<LoginResultModel> LoginAsync(LoginModel model)
        {
            // Check user existing
            var user = await _userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                throw new Exception("Incorrect username.");
            }

            // Signin using username and password provided
            var loginResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (loginResult.IsLockedOut)
            {
                throw new Exception("Your account was locked out.");
            }

            if (!loginResult.Succeeded)
            {
                throw new Exception("Invalid username or password.");
            }
            var newRefreshToken = _jwtGenerator.GenerateRefreshToken();
            var userRefreshToken = _context.UserRefreshTokens.FirstOrDefault(urt => urt.Username == user.UserName);
            if (userRefreshToken != null)
            {
                userRefreshToken.RefreshToken = newRefreshToken;
            }
            else
            {
                _context.UserRefreshTokens.Add(new UserRefreshToken
                {
                    Username = user.UserName,
                    RefreshToken = newRefreshToken
                });
            }

            await _context.SaveChangesAsync();

            return new LoginResultModel
            {
                UserName = user.UserName,
                AccessToken = await GenerateToken(_jwtSettings, user),
                RefreshToken = newRefreshToken
            };
        }

        public async Task<LoginResultModel> RegisterAsync(RegisterModel model)
        {
            var user = new AppUser
            {
                Account = new Account
                {
                    UserName = model.UserName,
                    Email = model.Email,
                }
            };

            var result = await _userManager.CreateAsync(user.Account, model.Password);
            if (!result.Succeeded)
            {
                var errorMessage = result.Errors.Any() ? result.Errors.First().Description : "Cannot create a new user.";
                throw new Exception(errorMessage);
            }

            AddRefreshToken(user);


            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return await LoginAsync(new LoginModel { UserName = model.UserName, Password = model.Password });
        }

        private void AddRefreshToken(AppUser user)
        {
            var refreshUser = _context.UserRefreshTokens.SingleOrDefault(u => u.Username == user.Account.UserName);
            if (refreshUser != null) throw new Exception("User refresh token already exist");

            _context.UserRefreshTokens.Add(new UserRefreshToken
            {
                Username = user.Account.UserName
            });
        }

        public async Task<RefreshTokenResultModel> RefreshToken(string authenticationToken, string refreshToken)
        {
            var principal = _jwtGenerator.GetPrincipalFromExpiredToken(_jwtSettings, authenticationToken);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default

            var user = _context.UserRefreshTokens.SingleOrDefault(u => u.Username == username);
            if (user == null || user.RefreshToken != refreshToken) throw new Exception("wrong refresh token");

            var account = await _userManager.FindByNameAsync(principal.Identity.Name);
            var userRoles = await _userManager.GetRolesAsync(account);

            var newJwtToken = _jwtGenerator.GenerateToken(_jwtSettings, new JwtModel
            {
                Username = account.UserName,
                Roles = userRoles
            });
            var newRefreshToken = _jwtGenerator.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _context.SaveChangesAsync();

            return new RefreshTokenResultModel
            {
                AccessToken = newJwtToken,
                RefreshToken = newRefreshToken
            };
        }

        public async Task<bool> ForgotPassword(ForgotPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new Exception("Cannot find the email");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);

            var apiKey = _configuration["SendGrid:ApiKey"];
            var from = new EmailAddress(_configuration["SendGrid:FromMail"], _configuration["SendGrid:FromName"]);
            var client = new SendGridClient(apiKey);

            var host = _configuration["WebDomain"];
            var email = WebUtility.UrlEncode(user.Email);
            var token = WebUtility.UrlEncode(code);
            var callbackUrl = $"{host}/resetPassword?email={email}&token={token}";
            var content = "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>";
            var msg = MailHelper.CreateSingleEmail(from, new EmailAddress(user.Email, user.UserName), "Reset Password", null, content);
            var result = await client.SendEmailAsync(msg);
            if ((int)result.StatusCode < 200 || (int)result.StatusCode >= 300)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SetPassword(PasswordRecoverModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new Exception("Cannot find the email");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            return result.Succeeded;
        }

        #endregion
    }
}
