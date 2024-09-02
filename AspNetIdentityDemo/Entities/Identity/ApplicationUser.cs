using Microsoft.AspNetCore.Identity;

namespace AspNetIdentityDemo.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
