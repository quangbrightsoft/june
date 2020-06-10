using System;
using Brightsoft.Data.Configuration;
using Brightsoft.Data.Entities;
using Brightsoft.Data.Identity.AccountRoles;
using Brightsoft.Data.Identity.Accounts;
using Brightsoft.Data.Identity.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Brightsoft.Data.Data
{
    public class ApplicationDbContext : IdentityDbContext<Account, Role, Guid, IdentityUserClaim<Guid>, AccountRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public override DbSet<Role> Roles { get; set; }
        public override DbSet<AccountRole> UserRoles { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new UserRoleConfiguration());
        }
    }
}
