using GraphQL.Validation;
using System.Threading.Tasks;

namespace Brightsoft.GraphQL.Helpers
{
    public class InputValidationRule : IValidationRule
    {
        /// <summary>
        /// Contains validation rules for the GraphQl Api
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<INodeVisitor> ValidateAsync(ValidationContext context)
        {
            return Task.FromResult((INodeVisitor)new EnterLeaveListener(_ =>
            {
            }));
        }
    }
}
