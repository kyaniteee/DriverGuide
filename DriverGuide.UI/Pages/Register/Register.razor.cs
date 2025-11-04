using System.ComponentModel.DataAnnotations;

namespace DriverGuide.UI.Pages.Register
{
    public partial class Register
    {
        private RegisterRequest registerModel = new RegisterRequest();
        private string? message;

        private async Task HandleRegister()
        {
            if (registerModel.Password != registerModel.ConfirmPassword)
            {
                message = "Hasła nie są zgodne.";
                return;
            }

            var response = await Http.PostAsJsonAsync("/User/register", registerModel);
            if (response.IsSuccessStatusCode)
            {
                message = "Rejestracja zakończona sukcesem. Przekierowanie do logowania...";
                await Task.Delay(1500);
                Navigation.NavigateTo("/");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                message = $"Wystąpił nieoczekiwany problem podczas rejestracji: {error}";
            }
        }

        public class RegisterRequest
        {
            [Required(ErrorMessage = "Login jest wymagany")]
            public string? Login { get; set; }

            [Required(ErrorMessage = "Imię jest wymagane")]
            public string? FirstName { get; set; }

            [Required(ErrorMessage = "Nazwisko jest wymagane")]
            public string? LastName { get; set; }

            [Required(ErrorMessage = "Data urodzenia jest wymagana")]
            public DateOnly? BirthDate { get; set; }

            [Required(ErrorMessage = "Email jest wymagany")]
            [EmailAddress(ErrorMessage = "Nieprawidłowy adres email")]
            public string? Email { get; set; }

            [Required(ErrorMessage = "Hasło jest wymagane")]
            [MinLength(6, ErrorMessage = "Hasło musi mieć co najmniej 6 znaków")]
            public string? Password { get; set; }

            [System.Text.Json.Serialization.JsonIgnore]
            [Required(ErrorMessage = "Powtórz hasło jest wymagane")]
            [Compare(nameof(Password), ErrorMessage = "Hasła nie są zgodne")]
            public string? ConfirmPassword { get; set; }
        }
    }
}
