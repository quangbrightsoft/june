using System;
using System.Collections.Generic;
using Brightsoft.GraphQL.Helpers.Interfaces;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Brightsoft.GraphQL.Helpers.Data
{
    public class QueryRoot : ObjectGraphType<object>
    {
        public QueryRoot(IServiceProvider provider, List<Type> allQueryBuilders)
        {
            Name = "QueryRoot";

            foreach (Type queryBuilder in allQueryBuilders)
            {
                var instance = ActivatorUtilities.CreateInstance(provider, queryBuilder);

                if (instance is ISchemaBuilder builder)
                {
                    builder.BuildQuery(this);
                }
            }
        }
    }
}
