using System.Collections.Generic;
using System.Security.Claims;

namespace Brightsoft.GraphQL.Helpers
{
    public class GraphQLUserContext : Dictionary<string, object>
    {
            public ClaimsPrincipal User { get; set; }
    }
}
