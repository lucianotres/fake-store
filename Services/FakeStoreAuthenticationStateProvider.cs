using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FakeProduct.Services;

public class FakeStoreAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly FakeStoreUsersService _usersService;
    private readonly ClaimsPrincipal anonymousUser;

    public FakeStoreAuthenticationStateProvider(
        ILocalStorageService localStorageService, 
        FakeStoreUsersService fakeStoreUsersService)
    {
        _localStorage = localStorageService;
        _usersService = fakeStoreUsersService;

        anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var userJwt = await _localStorage.GetItemAsStringAsync("user-token");

        if (string.IsNullOrEmpty(userJwt))
            return new AuthenticationState(anonymousUser);

        var handler = new JwtSecurityTokenHandler();
        var clainsPrincipal = handler.ValidateToken(userJwt, new TokenValidationParameters
        {
            NameClaimType = "user",
            ValidateIssuerSigningKey = false,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero,
            SignatureValidator = (token, parameters) => new JwtSecurityToken(token)
        }, out SecurityToken validatedToken);

        if (clainsPrincipal == null)
            return new AuthenticationState(anonymousUser);

        return new AuthenticationState(clainsPrincipal);
    }

    public async Task SetUserAuthenticated(string token)
    {
        await _localStorage.SetItemAsStringAsync("user-token", token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task RemoveAuthenticatedUser()
    {
        await _localStorage.RemoveItemAsync("user-token");
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }   
}
