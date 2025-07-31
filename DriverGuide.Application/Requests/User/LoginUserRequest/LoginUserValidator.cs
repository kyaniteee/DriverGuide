using FluentValidation;

namespace DriverGuide.Application.Requests;
public class LoginUserValidator : AbstractValidator<LoginUserRequest>
{
    public LoginUserValidator()
    {
        
    }
}
