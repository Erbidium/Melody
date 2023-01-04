using FluentValidation;
using Melody.WebAPI.DTO.Auth.Models;

namespace Melody.WebAPI.Validation.Validators.User;

public class UserLoginValidator : AbstractValidator<UserLogin>
{
    public UserLoginValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .MaximumLength(50)
            .Must(e => e.IsValidEmail());

        RuleFor(u => u.Password)
            .NotEmpty()
            .MaximumLength(50)
            .Must(p => p.IsValidPassword());
    }
}