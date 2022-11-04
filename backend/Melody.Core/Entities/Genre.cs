namespace Melody.Core.Entities;

public class Genre
{
    public Genre(long id, string name)
    {
        Id = id;
        Name = name;
    }

    public long Id { get; set; }
    public string Name { get; set; }
}
