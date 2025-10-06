using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace DriverGuide.UI.Auth;

public class ServerAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private ClaimsPrincipal _currentUser = new ClaimsPrincipal(new ClaimsIdentity());

    public ServerAuthenticationStateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        if (keyValuePairs != null)
        {
            claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString() ?? string.Empty)));
        }

        return claims;
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // W przyszłości można tu dodać faktyczną logikę uwierzytelniania
        // np. sprawdzając token JWT w localStorage i weryfikując go
        return Task.FromResult(new AuthenticationState(_currentUser)); 
    }

    // Metoda do używania, gdy użytkownik się zaloguje
    public void NotifyUserAuthentication(string token)
    {
        var authenticatedUser = new ClaimsPrincipal(
            new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));

        _currentUser = authenticatedUser;
        SetAuthHeader(token);

        var authState = Task.FromResult(new AuthenticationState(_currentUser));
        NotifyAuthenticationStateChanged(authState);
    }
    public void SetAuthHeader(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
    // Metoda do używania, gdy użytkownik się wyloguje
    public void NotifyUserLogout()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        _currentUser = anonymousUser;

        var authState = Task.FromResult(new AuthenticationState(_currentUser));
        NotifyAuthenticationStateChanged(authState);
    }
}