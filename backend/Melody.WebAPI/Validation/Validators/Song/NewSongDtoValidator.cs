using FluentValidation;
using Melody.WebAPI.DTO.Song;

namespace Melody.WebAPI.Validation.Validators.Song;

public class NewSongDtoValidator : AbstractValidator<NewSongDto>
{
    public NewSongDtoValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(s => s.AuthorName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(s => s.Year)
            .GreaterThan(0);
    }
}