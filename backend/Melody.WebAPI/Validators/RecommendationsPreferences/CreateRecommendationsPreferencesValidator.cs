using FluentValidation;
using Melody.WebAPI.DTO.RecommendationsPreferences;

namespace Melody.WebAPI.Validators.RecommendationsPreferences;

public class CreateRecommendationsPreferencesValidator : AbstractValidator<CreateRecommendationsPreferencesDto>
{
    public CreateRecommendationsPreferencesValidator()
    {
        RuleFor(p => p.EndYear)
            .Must((d, _) => d.StartYear <= d.EndYear)
            .When((d, _) => d.StartYear.HasValue && d.EndYear.HasValue)
            .WithMessage("Start year should be smaller or equal than end year");

        RuleFor(p => p.EndYear)
            .GreaterThan(0)
            .When(p=> p.EndYear.HasValue)
            .WithMessage("End year should be positive");

        RuleFor(p => p.StartYear)
            .GreaterThan(0)
            .When(p=> p.StartYear.HasValue)
            .WithMessage("Start year should be positive");
        
        RuleFor(p => p.AverageDurationInMinutes)
            .GreaterThan(0)
            .When(p=> p.AverageDurationInMinutes.HasValue)
            .WithMessage("Average duration should be positive");
    }
}