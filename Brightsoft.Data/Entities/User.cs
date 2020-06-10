using Brightsoft.Data.Identity.Accounts;
using Brightsoft.GraphQL.Helpers.Interfaces;

namespace Brightsoft.Data.Entities
{
    public class AppUser: Entity, ISupportGraphQLModel
    {
        public Account Account { get; set; }
    }
}