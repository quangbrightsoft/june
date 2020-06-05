using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using GraphQL.Validation;

namespace Brightsoft.GraphQL.Helpers
{
    public class GraphQLSettings
    {
        public PathString Path { get; set; } = "/api/graphql";

        public Func<HttpContext, IDictionary<string, object>> BuildUserContext { get; set; }

        public bool EnableMetrics { get; set; }

        public object Root { get; set; }
        public IEnumerable<IValidationRule> ValidationRules { get; set; }
    }
}
