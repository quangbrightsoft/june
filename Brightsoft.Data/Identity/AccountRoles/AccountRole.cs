using System;
using Brightsoft.Data.Identity.Accounts;
using Brightsoft.Data.Identity.Roles;
using Microsoft.AspNetCore.Identity;

namespace Brightsoft.Data.Identity.AccountRoles
{
    public class AccountRole : IdentityUserRole<Guid>
    {
        public Account Account { get; set; }

        public Role Role { get; set; }
    }
}
