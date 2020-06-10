using Brightsoft.GraphQL.Helpers.Interfaces;

namespace Brightsoft.JuneApp.Models
{
    public class RefreshTokenResultModel : ISupportGraphQLModel
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
