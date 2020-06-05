using System;
using System.Collections.Generic;
using Brightsoft.GraphQL.Helpers.Interfaces;
using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Brightsoft.GraphQL.Helpers.Data
{
    public class MutationRoot : ObjectGraphType<object>
    {
        public MutationRoot(IServiceProvider provider, List<Type> allQueryBuilders)
        {
            Name = "MutationRoot";

            foreach (Type queryBuilder in allQueryBuilders)
            {
                var instance = ActivatorUtilities.CreateInstance(provider, queryBuilder);

                if (instance is ISchemaBuilder builder)
                {
                    builder.BuildMutation(this);
                }
            }
        }
    }
}
