using System;

namespace Brightsoft.GraphQL.Helpers.Interfaces
{
    /// <summary>
    /// Decorates models implementing <see cref="ISupportGraphQLModel"/> with GraphQl metadata Name and Description
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GraphQlModelMetadataAttribute : Attribute
    {
        public GraphQlModelMetadataAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
