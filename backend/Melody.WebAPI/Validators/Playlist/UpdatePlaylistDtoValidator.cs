using FluentValidation;
using Melody.WebAPI.DTO.Playlist;

namespace Melody.WebAPI.Validators.Playlist;

public class UpdatePlaylistDtoValidator : AbstractValidator<UpdatePlaylistDto>
{
    public UpdatePlaylistDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(p => p.Link)
            .NotEmpty()
            .MaximumLength(50);
    }
}