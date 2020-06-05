using Brightsoft.GraphQL.Helpers.Data;

namespace Brightsoft.GraphQL.Helpers.Interfaces
{
    /// <summary>
    /// In target project, create schema builders implementing this interface and the query and mutations will automatically be added to your GraphQL endpoint.
    /// Leverages the use of reflection.
    /// </summary>
    public interface ISchemaBuilder
    {
        void BuildQuery(QueryRoot queryRoot);
        void BuildMutation(MutationRoot mutationRoot);
    }
}
