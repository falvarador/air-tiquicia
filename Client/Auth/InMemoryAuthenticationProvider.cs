using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

public class InMemoryAuthenticationProvider : AuthenticationStateProvider
{

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var anonymousUser = new ClaimsIdentity(new List<Claim>() {
                new Claim(ClaimTypes.Name, "Administrador"),
                new Claim(ClaimTypes.Role, "admin")
            }, "Anonymous");

        return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(anonymousUser)));
    }
}
