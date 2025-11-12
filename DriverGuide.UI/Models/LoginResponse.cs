namespace DriverGuide.UI.Models;

/// <summary>
/// Response model for login containing JWT token
/// </summary>
public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
}
