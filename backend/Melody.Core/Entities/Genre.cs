using Ardalis.GuardClauses;
using Melody.SharedKernel.Interfaces;

namespace Melody.Core.Entities;

public class Genre : EntityBase<long>
{
    public Genre(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(Name));
    }

    public string Name { get; }
}