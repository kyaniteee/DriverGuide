using Blazored.LocalStorage;

namespace DriverGuide.UI.Services;

public interface IThemeService
{
    event EventHandler<bool>? ThemeChanged;
    Task<bool> GetIsDarkModeAsync();
    Task ToggleThemeAsync();
    Task SetThemeAsync(bool isDarkMode);
}

public class ThemeService : IThemeService
{
    private readonly ILocalStorageService _localStorage;
    private const string ThemeKey = "isDarkMode";
    private bool _isDarkMode;

    public event EventHandler<bool>? ThemeChanged;

    public ThemeService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<bool> GetIsDarkModeAsync()
    {
        try
        {
            _isDarkMode = await _localStorage.GetItemAsync<bool>(ThemeKey);
            return _isDarkMode;
        }
        catch
        {
            return false;
        }
    }

    public async Task ToggleThemeAsync()
    {
        _isDarkMode = !_isDarkMode;
        await _localStorage.SetItemAsync(ThemeKey, _isDarkMode);
        ThemeChanged?.Invoke(this, _isDarkMode);
    }

    public async Task SetThemeAsync(bool isDarkMode)
    {
        _isDarkMode = isDarkMode;
        await _localStorage.SetItemAsync(ThemeKey, isDarkMode);
        ThemeChanged?.Invoke(this, isDarkMode);
    }
}
