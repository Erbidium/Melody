using Melody.SharedKernel.Interfaces;

namespace Melody.Core.Entities;

public class Genre : EntityBase<long>
{
    public Genre(string name)
    {
        Name = name;
    }

    public string Name { get; }
}