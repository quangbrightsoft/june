using System;
using System.Linq;
using System.Threading.Tasks;
using Brightsoft.Core.Identity.Accounts;
using Brightsoft.Core.Identity.Roles;
using Brightsoft.Data.Data;
using Brightsoft.Data.Entities;
using Brightsoft.JuneApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Brightsoft.JuneApp.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<Account> _userManager;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<Role> _roleManager;

        public UserService(UserManager<Account> userManager, IConfiguration config, ApplicationDbContext context, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _config = config;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<PagedData<UserModel>> GetUsersAsync(GetUsersModel model)
        {
            var query = _context.AppUsers.Include(i => i.Account).AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.Search))
            {
                query = query.Where(i => i.Account.UserName.Contains(model.Search));
            }
            switch (model.SortBy)
            {
                case "email":
                    query = model.Descending ? query.OrderByDescending(i => i.Account.Email) : query.OrderBy(i => i.Account.Email);
                    break;
                case "userName":
                    query = model.Descending ? query.OrderByDescending(i => i.Account.UserName) : query.OrderBy(i => i.Account.UserName);
                    break;
                case "id":
                    query = model.Descending ? query.OrderByDescending(i => i.Account.Id) : query.OrderBy(i => i.Account.Id);
                    break;
            }
            var count = await query.AsNoTracking().CountAsync();
            if (model.PageSize != default)
            {
                query = query.Skip((model.Page - 1) * model.PageSize);
            }

            if (model.PageSize != default)
            {
                query = query.Take(model.PageSize);
            }

            var users = await query.AsNoTracking().ToListAsync();

            return new PagedData<UserModel>
            {
                Items = users.Select(i => new UserModel(i)),
                TotalCount = count,
                Page = model.Page,
                PageSize = model.PageSize
            };
        }
        public async Task<UserModel> GetUserAsync(int id)
        {
            var user = await _context.AppUsers

                .FirstOrDefaultAsync(i => i.Id == id);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }
            return new UserModel(user);
        }

        public async Task<UserModel> CreateUserAsync(CreateUserModel model)
        {
            var user = new AppUser
            {
                Account = new Account
                {
                    UserName = model.Email,
                    Email = model.Email,
                }
            };
            var roles = _roleManager.Roles;

            var result = await _userManager.CreateAsync(user.Account, "Asdfgh1@3");

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user.Account, model.Roles);
                _context.Add(user);
                await _context.SaveChangesAsync();


                return new UserModel(user);
            }

            if (result.Errors.Any())
            {
                throw new Exception(result.Errors.First().Description);
            }
            else
            {
                throw new Exception("Cannot create user.");
            }
        }
        public async Task<bool> UpdateUserAsync(int id, CreateUserModel model)
        {
            var user = await _context.AppUsers
                .Include(i => i.Account)
                .FirstOrDefaultAsync(i => i.Id == id);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }
            user.Account.Email = model.Email;
            user.UpdatedAt = new DateTimeOffset();
            var updateUserResult = await _userManager.UpdateAsync(user.Account);
            var updateAccountResult = updateUserResult.Succeeded;
            var currentUser = new UserModel(user);
            if (!currentUser.Roles.SequenceEqual(model.Roles))
            {
                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user.Account, currentUser.Roles);
                var addToRolesResult = await _userManager.AddToRolesAsync(user.Account, model.Roles);
                updateAccountResult = updateAccountResult && addToRolesResult.Succeeded && removeRolesResult.Succeeded;
            }
            var saveResult = await _context.SaveChangesAsync() > 0;
            return saveResult || updateAccountResult;
        }

        public async Task<bool> DeleteUserAsync(int id, string currentUserName)
        {
            var user = await _context.AppUsers.Include(i => i.Account).FirstOrDefaultAsync(i => i.Id == id);
            if (user == null)
            {
                throw new Exception("User does not exist");
            }
            if (user.Account.UserName == currentUserName)
            {
                throw new Exception("You cannot delete yourself");
            }
            await _userManager.DeleteAsync(user.Account);
            _context.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
