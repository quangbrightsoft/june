using System;
using Brightsoft.Core.Identity.Accounts;
using Brightsoft.Core.Identity.Roles;
using Microsoft.AspNetCore.Identity;

namespace Brightsoft.Core.Identity.AccountRoles
{
    public class AccountRole : IdentityUserRole<Guid>
    {
        public Account Account { get; set; }

        public Role Role { get; set; }
    }
}
