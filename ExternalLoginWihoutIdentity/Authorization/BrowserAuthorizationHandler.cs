using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ExternalLoginWihoutIdentity.Authorization
{
    public class BrowserAuthorizationHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (var requirment in context.PendingRequirements)
            {
                var userAgentClaim = context.User.FindFirstValue("user-agent");
                if (string.IsNullOrWhiteSpace(userAgentClaim))
                    continue;

                if (requirment is ChromeRequirmenet)
                {                    
                    if(userAgentClaim.Contains("Chrome"))
                        context.Succeed(requirment);
                }
                else if(requirment is BrowserVersionRequirmenet)
                {
                    if(userAgentClaim.Contains("Edg/128.0.0.0"))
                        context.Succeed(requirment);
                }
            }

            return Task.CompletedTask;
        }
    }

    public class ChromeRequirmenet : IAuthorizationRequirement
    {

    }

    public class BrowserVersionRequirmenet : IAuthorizationRequirement
    {

    }
}
