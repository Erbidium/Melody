using FluentValidation;
using Melody.WebAPI.DTO.Auth.Models;

namespace Melody.WebAPI.Validators.User;

public class UserRegisterValidator : AbstractValidator<UserRegister>
{
    public UserRegisterValidator()
    {
        RuleFor(u => u.UserName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(u => u.Email)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(u => u.PhoneNumber)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(u => u.Password)
            .NotEmpty()
            .MaximumLength(50);
    }
}