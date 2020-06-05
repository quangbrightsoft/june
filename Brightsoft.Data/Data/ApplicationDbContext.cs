using System;
using Brightsoft.Core.Identity.AccountRoles;
using Brightsoft.Core.Identity.Accounts;
using Brightsoft.Core.Identity.Roles;
using Brightsoft.Data.Configuration;
using Brightsoft.Data.Entities;
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
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Account>().ToTable("AspNetUsers");
            builder.Entity<AppUser>();
            builder.ApplyConfiguration(new UserRoleConfiguration());
        }
    }
}
