using FluentValidation;

namespace DriverGuide.Application.Requests;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        
    }
}
