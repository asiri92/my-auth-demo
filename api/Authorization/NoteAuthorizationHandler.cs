using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using my_auth_api_demo.Models;

namespace my_auth_api_demo.Authorization
{
    public class NoteAuthorizationHandler : AuthorizationHandler<SameOwnerRequirement, Note>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            SameOwnerRequirement requirement,
            Note resource)
        {
            // Admins bypass ownership entirely
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            var userId = context.User.FindFirst(ClaimConstants.ObjectId)?.Value;

            if (!string.IsNullOrEmpty(userId) && resource.OwnerId == userId)
            {
                context.Succeed(requirement);
            }

            // No explicit Fail() call needed — if Succeed() is never called,
            // the requirement is simply unmet and authorization fails by default.

            return Task.CompletedTask;
        }
    }
}