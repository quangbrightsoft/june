using System;
using System.Collections.Generic;
using Brightsoft.Core.Identity.AccountRoles;
using Microsoft.AspNetCore.Identity;

namespace Brightsoft.Core.Identity.Accounts
{
    public class Account : IdentityUser<Guid>
    {
        public ICollection<AccountRole> Roles { get; set; }
    }
}
