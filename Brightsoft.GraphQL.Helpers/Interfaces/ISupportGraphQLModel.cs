namespace Brightsoft.GraphQL.Helpers.Interfaces
{
    /// <summary>
    /// All classes implementing this interface can be used in the Schema builder in this syntax:
    /// <code>
    ///  queryRoot.Field<ListGraphType<AutoRegisteringObjectGraphType<ExamModel>>>(
    ///   ...
    ///  );
    /// </code>
    /// </summary>
    public interface ISupportGraphQLModel
    {
    }
}