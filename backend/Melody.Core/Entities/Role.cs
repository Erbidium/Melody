namespace Melody.Core.Entities;

public class Role
{
    public Role(long id, string name)
    {
        Id = id;
        Name = name;
    }

    public long Id { get; }
    public string Name { get; }
}
