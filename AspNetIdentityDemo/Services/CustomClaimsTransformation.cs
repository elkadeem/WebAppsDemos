using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace AspNetIdentityDemo.Services
{
    public class CustomClaimsTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            string userName = principal.Identity.Name;

            ((ClaimsIdentity)principal.Identity).AddClaim(new Claim("FullName", "John Doe"));
            // Check if user is employee
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            var claimType = "IsEmployee";
            if (!principal.HasClaim(claim => claim.Type == claimType))
            {
                claimsIdentity.AddClaim(new Claim(claimType, true.ToString()));
            }

            principal.AddIdentity(claimsIdentity);
            return Task.FromResult(principal);
        }
    }
}
