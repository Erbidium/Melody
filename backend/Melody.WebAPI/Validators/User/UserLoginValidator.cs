using FluentValidation;
using Melody.WebAPI.DTO.Auth.Models;

namespace Melody.WebAPI.Validators.User;

public class UserLoginValidator : AbstractValidator<UserLogin>
{
    public UserLoginValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(u => u.Password)
            .NotEmpty()
            .MaximumLength(50);
    }
}