using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Brightsoft.Authentication.Jwt;
using Brightsoft.Core.Identity.Accounts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Brightsoft.Data;
using Brightsoft.Data.Data;
using Brightsoft.Data.Entities;
using JuneApp.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace JuneApp.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> logger;

        private readonly JwtSettings jwtSettings;

        private readonly SignInManager<Account> signInManager;

        private readonly UserManager<Account> userManager;

        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;

        public AccountService(ILogger<AccountService> logger,
            JwtSettings jwtSettings,
            SignInManager<Account> signInManager,
            UserManager<Account> userManager,
            ApplicationDbContext context,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.jwtSettings = jwtSettings;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;
            this.context = context;
        }

        private async Task<string> GenerateToken(JwtSettings jwtSettings, Account user)
        {
            // Get roles of the user to add to the claims later
            var userRoles = await this.userManager.GetRolesAsync(user);

            var jwtModel = new JwtModel
            {
                Username = user.UserName,
                Roles = userRoles
            };

            var jwtGenerator = new JwtGenerator();

            return jwtGenerator.GenerateToken(jwtSettings, jwtModel);
        }

        #region Login & Register

        public async Task<LoginResultModel> LoginAsync(LoginModel model)
        {
            // Check user existing
            var user = await this.userManager.FindByNameAsync(model.UserName);

            if (user == null)
            {
                throw new Exception("Incorrect username.");
            }

            // Signin using username and password provided
            var loginResult = await this.signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (loginResult.IsLockedOut)
            {
                throw new Exception("Your account was locked out.");
            }

            if (!loginResult.Succeeded)
            {
                throw new Exception("Invalid username or password.");
            }

            return new LoginResultModel
            {
                UserName = user.UserName,
                AccessToken = await this.GenerateToken(jwtSettings, user)
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

            var result = await this.userManager.CreateAsync(user.Account, model.Password);
            if (!result.Succeeded)
            {
                var errorMessage = result.Errors.Any() ? result.Errors.First().Description : "Cannot create a new user.";
                throw new Exception(errorMessage);
            }
            await context.AddAsync(user);
            await context.SaveChangesAsync();
            return await this.LoginAsync(new LoginModel { UserName = model.UserName, Password = model.Password });
        }

        public async Task<bool> ForgotPassword(ForgotPasswordModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new Exception("Cannot find the email");
            }

            var code = await userManager.GeneratePasswordResetTokenAsync(user);

            var apiKey = configuration["SendGrid:ApiKey"];
            var from = new EmailAddress(configuration["SendGrid:FromMail"], configuration["SendGrid:FromName"]);
            var client = new SendGridClient(apiKey);

            var host = configuration["WebDomain"];
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
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new Exception("Cannot find the email");
            }
            var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            return result.Succeeded;
        }

        #endregion
    }
}
