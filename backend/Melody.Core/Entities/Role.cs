using Melody.SharedKernel.Interfaces;

namespace Melody.Core.Entities;

public class Role : IEntityBase<long>
{
    public Role(long id, string name)
    {
        Id = id;
        Name = name;
    }

    public long Id { get; }
    public string Name { get; }
}
