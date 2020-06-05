using System;
using System.Collections.Generic;
using Brightsoft.Core.Identity.AccountRoles;
using Microsoft.AspNetCore.Identity;

namespace Brightsoft.Core.Identity.Roles
{
    public class Role : IdentityRole<Guid>
    {
        public ICollection<AccountRole> AccountRoles { get; set; }
    }
}
