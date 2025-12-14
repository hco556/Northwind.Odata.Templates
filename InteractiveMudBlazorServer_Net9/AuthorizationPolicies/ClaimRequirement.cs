using Microsoft.AspNetCore.Authorization;

namespace InteractiveMudBlazorServer_Net9Authorization.Policies
{
    public class ClaimRequirement : IAuthorizationRequirement
    {
        public string ClaimName { get; set; }

        public ClaimRequirement(string claimName)
        {
            ClaimName = claimName;
        }
    }
}
