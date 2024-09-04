using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ExternalLoginWihoutIdentity.Authorization
{
    public class MinimumAgeAuhorizationHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
           var claim = context.User.FindFirst(ClaimTypes.DateOfBirth);
            if(claim != null && DateTime.TryParse(claim.Value, out DateTime birthDate))
            {
                if((DateTime.Today - birthDate).TotalDays >= requirement.MinimumAge * 365)
                {
                    context.Succeed(requirement);                    
                }
            }

            return Task.CompletedTask;
        }
    }

    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public MinimumAgeRequirement(int minimumAge) {
            MinimumAge = minimumAge;
        }

        public int MinimumAge { get; }
    }
}
