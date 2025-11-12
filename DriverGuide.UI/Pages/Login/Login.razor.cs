using Blazored.LocalStorage;
using DriverGuide.UI.Auth;
using DriverGuide.UI.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Text.Json;

namespace DriverGuide.UI.Pages.Login
{
    public partial class Login
    {
        [Inject] private HttpClient Http { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
        [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;

        private LoginRequest loginModel = new();
        private Dictionary<string, List<string>> validationErrors = new();
        private string? generalMessage;
        private bool isLoading = false;
        private bool isSuccess = false;

        private async Task HandleLogin()
        {
            // Reset poprzednich b³êdów
            validationErrors.Clear();
            generalMessage = null;

            isLoading = true;
            StateHasChanged();

            try
            {
                var response = await Http.PostAsJsonAsync("/User/login", loginModel);

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    
                    if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                    {
                        // Zapisz token w localStorage
                        await LocalStorage.SetItemAsync("authToken", loginResponse.Token);

                        // Powiadom AuthenticationStateProvider o logowaniu
                        ((ServerAuthenticationStateProvider)AuthStateProvider).NotifyUserAuthentication(loginResponse.Token);

                        isSuccess = true;
                        generalMessage = "Logowanie zakoñczone sukcesem. Przekierowanie...";
                        StateHasChanged();
                        await Task.Delay(1500);
                        Navigation.NavigateTo("/profile", true);
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    // Obs³uga b³êdów walidacji
                    var errorContent = await response.Content.ReadAsStringAsync();
                    
                    try
                    {
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };
                        
                        var validationResponse = JsonSerializer.Deserialize<ValidationErrorResponse>(errorContent, options);
                        
                        if (validationResponse?.Errors != null && validationResponse.Errors.Any())
                        {
                            validationErrors = validationResponse.Errors;
                            generalMessage = validationResponse.Message ?? "Wyst¹pi³y b³êdy walidacji";
                        }
                        else
                        {
                            generalMessage = "Wyst¹pi³ b³¹d podczas logowania. Spróbuj ponownie.";
                        }
                    }
                    catch
                    {
                        generalMessage = errorContent;
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    generalMessage = "Nieprawid³owy login lub has³o";
                }
                else
                {
                    generalMessage = "Wyst¹pi³ nieoczekiwany problem podczas logowania. Spróbuj ponownie póŸniej.";
                }
            }
            catch (Exception ex)
            {
                generalMessage = $"Wyst¹pi³ b³¹d po³¹czenia: {ex.Message}";
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private string GetFieldErrors(string fieldName)
        {
            if (validationErrors.TryGetValue(fieldName, out var errors) && errors.Any())
            {
                return string.Join(", ", errors);
            }
            return string.Empty;
        }

        private bool HasFieldErrors(string fieldName)
        {
            return validationErrors.ContainsKey(fieldName) && validationErrors[fieldName].Any();
        }

        private string GetCssClass(string fieldName)
        {
            return HasFieldErrors(fieldName) ? "form-control is-invalid" : "form-control";
        }

        private string GetButtonCssClass()
        {
            return isLoading ? "btn btn-primary btn-login btn-loading" : "btn btn-primary btn-login";
        }

        private string GetAlertCssClass()
        {
            return isSuccess ? "alert alert-success mt-3" : "alert alert-danger mt-3";
        }
    }
}
