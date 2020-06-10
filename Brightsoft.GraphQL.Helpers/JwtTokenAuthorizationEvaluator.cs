using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Brightsoft.GraphQL.Helpers
{
    public class JwtTokenAuthorizationEvaluator : IAuthorizationEvaluator
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthorizationSettings _authorizationSettings;

        public JwtTokenAuthorizationEvaluator(IHttpContextAccessor httpContextAccessor,
            AuthorizationSettings authorizationSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _authorizationSettings = authorizationSettings;
        }

        public async Task<AuthorizationResult> Evaluate(ClaimsPrincipal principal, object userContext, IDictionary<string, object> arguments, IEnumerable<string> requiredPolicies)
        {
            var cxt = userContext as GraphQLUserContext;
            ClaimsPrincipal claimsPrincipal = _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated ? _httpContextAccessor.HttpContext.User : null;
            if (claimsPrincipal == null)
            {
                var response = await _httpContextAccessor.HttpContext.AuthenticateAsync();
                if (response.Succeeded)
                {
                    claimsPrincipal = response.Principal;
                    _httpContextAccessor.HttpContext.User = response.Principal;
                }
                else
                {
                    return AuthorizationResult.Fail(new []{response.Failure.Message });
                }
            }

            var context = new AuthorizationContext();
            context.User = claimsPrincipal;
            context.UserContext = userContext;
            context.InputVariables = arguments;

            var authPolicies = _authorizationSettings.GetPolicies(requiredPolicies);
            var tasks = new List<Task>();
            authPolicies.Apply(p =>
            {
                p.Requirements.Apply(r =>
                {
                    var task = r.Authorize(context);
                    tasks.Add(task);
                });
            });

            await Task.WhenAll(tasks.ToArray());
            return !context.HasErrors
            ? AuthorizationResult.Success()
            : AuthorizationResult.Fail(context.Errors);
        }
    }
}
