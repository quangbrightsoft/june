using Brightsoft.GraphQL.Helpers.Interfaces;
using GraphQL.Types;
using System;

namespace Brightsoft.GraphQL.Helpers.Data
{
    public class RootSchema : Schema
    {
        public RootSchema(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            var allQueryBuilders = TypeHelper.GetAllTypesAssignableFromInCurrentDomain(typeof(ISchemaBuilder));

            Query = new QueryRoot(serviceProvider, allQueryBuilders);
            Mutation = new MutationRoot(serviceProvider, allQueryBuilders);
        }
    }
}
