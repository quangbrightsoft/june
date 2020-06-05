using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Brightsoft.Core.Identity.Accounts;
using Brightsoft.Core.Identity.Roles;
using Brightsoft.Data.Data;
using Brightsoft.Data.Entities;
using Microsoft.AspNetCore.Identity;
using RandomNameGeneratorLibrary;

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

    }
}