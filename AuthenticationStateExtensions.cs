using FakeProduct.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace FakeProduct;

public static class AuthenticationStateExtensions
{
    public static int? GetUserId(this AuthenticationState state)
    {
        var claim = state.User.Claims.FirstOrDefault(w => w.Type == ClaimTypes.NameIdentifier);
        if (claim != null && int.TryParse(claim.Value, out int id))
            return id;
        else
            return null;
    }
}
