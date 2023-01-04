using FluentValidation;
using Melody.WebAPI.DTO.Auth.Models;

namespace Melody.WebAPI.Validation.Validators.User;

public class UserRegisterValidator : AbstractValidator<UserRegister>
{
    public UserRegisterValidator()
    {
        RuleFor(u => u.UserName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(30);

        RuleFor(u => u.Email)
            .NotEmpty()
            .MaximumLength(50)
            .Must(e => e.IsValidEmail());

        RuleFor(u => u.PhoneNumber)
            .NotEmpty()
            .Must(e => e.IsValidPhoneNumber());

        RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(30)
            .Must(p => p.IsValidPassword());
    }
}