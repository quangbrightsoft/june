using System;
using System.Collections.Generic;
using System.Linq;

namespace Brightsoft.GraphQL.Helpers
{
    public static class TypeHelper
    {
        /// <summary>
        /// Gets all types assignable from <paramref name="query"/> in the AppDomain.CurrentDomain
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<Type> GetAllTypesAssignableFromInCurrentDomain(Type query)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => query.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                 .ToList();
        }
    }
}
