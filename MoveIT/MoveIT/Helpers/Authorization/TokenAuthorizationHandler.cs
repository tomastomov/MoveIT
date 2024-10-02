using Microsoft.AspNetCore.Authorization;
using MoveIT.Common.Contracts;

namespace MoveIT.Helpers.Authorization
{
    public class TokenAuthorizationHandler : AuthorizationHandler<HasTokenRequirement>
    {
        private readonly ITokenManager _tokenManager;

        public TokenAuthorizationHandler(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasTokenRequirement requirement)
        {
            if (_tokenManager.Token is not null && _tokenManager.Token.ExpirationDate > DateTime.UtcNow)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
