using Brightsoft.Core.Identity.AccountRoles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Brightsoft.Data.Configuration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<AccountRole>
    {
        public void Configure(EntityTypeBuilder<AccountRole> builder)
        {
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });

            builder.HasOne(x => x.Role)
                .WithMany(x => x.AccountRoles)
                .HasForeignKey(x => x.RoleId)
                .IsRequired();

            builder.HasOne(x => x.Account)
                .WithMany(x => x.Roles)
                .HasForeignKey(x => x.UserId)
                .IsRequired();
        }
    }
}
