﻿using FluentValidation;
using Melody.WebAPI.DTO.Playlist;

namespace Melody.WebAPI.Validation.Validators.Playlist;

public class NewPlaylistDtoValidator : AbstractValidator<NewPlaylistDto>
{
    public NewPlaylistDtoValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(30);
    }
}