using System.Linq;
using System.Threading.Tasks;
using Brightsoft.Core.Identity.Accounts;
using Brightsoft.Core.Identity.Roles;
using Brightsoft.Data.Data;
using Brightsoft.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace Brightsoft.Data
{
    public class DbInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<Role> _roleManager;
        public DbInitializer(ApplicationDbContext context, UserManager<Account> userManager, RoleManager<Role> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedData()
        {
            await _context.Database.EnsureCreatedAsync();

            // Look for any users.
            if (_context.Users.Any())
            {
                return;   // DB has been seeded
            }

            var roles = new Role[]
            {
                new Role() { Name = StaticRoles.Admin },
                new Role() { Name = StaticRoles.PowerUser },
                new Role() { Name = StaticRoles.Patient },
                new Role() { Name = StaticRoles.Doctor },
                new Role() { Name = StaticRoles.BmaSkill },
            };
            foreach (var role in roles)
            {
                await _roleManager.CreateAsync(role);
            }

            var owner = new AppUser
            {
                Account = new Account
                {
                    UserName = "developer.brightsoft@gmail.com",
                    Email = "developer.brightsoft@gmail.com",
                }
            };
            var result = await _userManager.CreateAsync(owner.Account, "Asdfgh1@3");
            await _userManager.AddToRoleAsync(owner.Account, StaticRoles.Admin);
            await _context.AddAsync(owner);


            await _context.SaveChangesAsync();
        }


    }
}