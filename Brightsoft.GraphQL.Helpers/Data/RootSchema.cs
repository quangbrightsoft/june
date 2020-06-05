using Brightsoft.GraphQL.Helpers.Interfaces;
using GraphQL.Types;
using System;

namespace Brightsoft.GraphQL.Helpers.Data
{
    public class RootSchema : Schema
    {
        public RootSchema(IServiceProvider provider)
            : base(provider)
        {
            var allQueryBuilders = TypeHelper.GetAllTypesAssignableFromInCurrentDomain(typeof(ISchemaBuilder));

            Query = new QueryRoot(provider, allQueryBuilders);
            Mutation = new MutationRoot(provider, allQueryBuilders);
        }
    }
}
