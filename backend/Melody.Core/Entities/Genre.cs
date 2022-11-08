using Melody.SharedKernel.Interfaces;

namespace Melody.Core.Entities;

public class Genre : IEntityBase<long>
{
    public Genre(long id, string name)
    {
        Id = id;
        Name = name;
    }
    public long Id { get; }
    public string Name { get; }
}
