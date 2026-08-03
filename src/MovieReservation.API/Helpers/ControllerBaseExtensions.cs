using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace MovieReservation.API;

public static class ControllerBaseExtensions
{
    public static long GetCurrentUserId(this ControllerBase controller)
    {
        var idClaim = controller.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return long.Parse(idClaim!);
    }

    public static bool IsAdmin(this ControllerBase controller) =>
        controller.User.IsInRole("Admin");
}
