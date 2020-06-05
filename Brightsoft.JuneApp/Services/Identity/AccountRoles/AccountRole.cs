using System;
using Brightsoft.Core.Identity.Accounts;
using Brightsoft.JuneApp.Services.Identity.Roles;
using Microsoft.AspNetCore.Identity;

namespace Brightsoft.JuneApp.Services.Identity.AccountRoles
{
    public class AccountRole : IdentityUserRole<Guid>
    {
        public Account Account { get; set; }

        public Role Role { get; set; }
    }
}
