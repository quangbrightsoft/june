using Brightsoft.GraphQL.Helpers.Interfaces;

namespace Brightsoft.JuneApp.Models
{
    public class LoginResultModel : ISupportGraphQLModel
    {
        public string UserName { get; set; }

        public string AccessToken { get; set; }
    }
}
