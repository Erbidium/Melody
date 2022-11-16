using FluentValidation;
using Melody.WebAPI.DTO.User;

namespace Melody.WebAPI.Validators.User;

public class NewUserDtoValidator : AbstractValidator<NewUserDto>
{
    public NewUserDtoValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(u => u.Email)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(u => u.PhoneNumber)
            .NotEmpty()
            .MaximumLength(50);
    }
}