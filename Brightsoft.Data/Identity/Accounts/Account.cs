using System;
using System.Collections.Generic;
using Brightsoft.Data.Identity.AccountRoles;
using Microsoft.AspNetCore.Identity;

namespace Brightsoft.Data.Identity.Accounts
{
    public class Account : IdentityUser<Guid>
    {
        public ICollection<AccountRole> Roles { get; set; }
    }
}
