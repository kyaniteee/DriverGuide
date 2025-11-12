namespace DriverGuide.UI.Models;

/// <summary>
/// Request model for user login
/// </summary>
public class LoginRequest
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
