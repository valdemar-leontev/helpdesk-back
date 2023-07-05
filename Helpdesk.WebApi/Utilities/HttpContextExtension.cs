using Helpdesk.WebApi.Models;

namespace Helpdesk.WebApi.Utilities;

public static class HttpContextExtension
{
    public static int GetUserId(this HttpContext context)
    {
        var claim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.UserId);

        return claim is not null
            ? int.Parse(claim.Value)
            : default;
    }

    public static int? GetProfileId(this HttpContext context)
    {
        var claim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.ProfileId);

        return claim is not null
            ? int.Parse(claim.Value)
            : null;
    }
}