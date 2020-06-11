using System.Collections.Generic;
using System.Linq;
using Brightsoft.Data.Entities;
using Brightsoft.GraphQL.Helpers.Interfaces;

namespace Brightsoft.JuneApp.Models
{
    public class UserModel : BaseModel, ISupportGraphQLModel
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Ssn { get; set; }
        public string Email { get; set; }
        public bool IsDisabled { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public UserModel() { }
        public UserModel(AppUser user)
        {
            var entity = user.Account;
            Id = user.Id;
            UserName = entity.UserName;
            Email = entity.Email;
            FullName = entity.FullName;
            //IsDisabled = entity.IsDisabled;
            Roles = entity.Roles != null ? entity.Roles.Where(x => x.Role != null).Select(x => x.Role.Name) : Enumerable.Empty<string>();
        }
    }
}
