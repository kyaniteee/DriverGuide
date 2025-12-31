using Microsoft.JSInterop;

namespace DriverGuide.UI.Services;

public interface INavbarService
{
    Task CloseNavbarAsync();
    Task InitNavbarAsync();
}

public class NavbarService : INavbarService
{
    private readonly IJSRuntime _jsRuntime;

    public NavbarService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task CloseNavbarAsync()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("navbarHelper.closeNavbar");
        }
        catch (JSException)
        {
        }
    }

    public async Task InitNavbarAsync()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("navbarHelper.initNavbar");
        }
        catch (JSException)
        {
        }
    }
}
