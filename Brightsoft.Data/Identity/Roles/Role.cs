using System;
using System.Collections.Generic;
using Brightsoft.Data.Identity.AccountRoles;
using Microsoft.AspNetCore.Identity;

namespace Brightsoft.Data.Identity.Roles
{
    public class Role : IdentityRole<Guid>
    {
        public ICollection<AccountRole> AccountRoles { get; set; }
    }
}
