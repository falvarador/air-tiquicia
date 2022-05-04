using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Security.Claims;

public class JWTAuthenticationProvider : AuthenticationStateProvider, ILoginService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    public static readonly string EXPIRATION_TOKEN_KEY = "EXPIRATION_TOKEN_KEY";
    public static readonly string TOKEN_KEY = "TOKEN_KEY";

    private AuthenticationState Anonymous =>
          new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    public JWTAuthenticationProvider(IJSRuntime jsRuntime, HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _jsRuntime.GetFromLocalStorage(TOKEN_KEY);

        if (string.IsNullOrEmpty(token))
            return Anonymous;

        return BuildAuthenticationState(token);
    }

    public AuthenticationState BuildAuthenticationState(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        return new AuthenticationState(new ClaimsPrincipal(
            new ClaimsIdentity(JwtHelper.ParseClaimsFromJwt(token), "jwt")));
    }

    public async Task Login(string token)
    {
        await _jsRuntime.SetInLocalStorage(TOKEN_KEY, token);

        var authState = BuildAuthenticationState(token);
        NotifyAuthenticationStateChanged(Task.FromResult(authState));
    }

    public async Task Logout()
    {
        await _jsRuntime.RemoveFromLocalStorage(TOKEN_KEY);

        _httpClient.DefaultRequestHeaders.Authorization = null;
        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
    }

    public Task<string> RenewToken(string token)
    {
        throw new NotImplementedException();
    }
}
