using System;
using System.Collections.Generic;
using JuneApp.Services.Identity.AccountRoles;
using Microsoft.AspNetCore.Identity;

namespace JuneApp.Services.Identity.Roles
{
    public class Role : IdentityRole<Guid>
    {
        public ICollection<AccountRole> AccountRoles { get; set; }
    }
}
