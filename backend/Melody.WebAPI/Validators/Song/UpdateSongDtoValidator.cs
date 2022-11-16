using FluentValidation;
using Melody.WebAPI.DTO.Song;

namespace Melody.WebAPI.Validators.Song;

public class UpdateSongDtoValidator : AbstractValidator<UpdateSongDto>
{
    public UpdateSongDtoValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(s => s.Path)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(s => s.AuthorName)
            .NotEmpty()
            .MaximumLength(50);
    }
}