namespace Melody.Core.Entities;

public class Playlist
{
    public Playlist(long id, string name, string link, long authorId)
    {
        Id = id;
        Name = name;
        Link = link;
        AuthorId = authorId;
    }

    public long Id { get; }
    public string Name { get; }
    public string Link { get; }
    public long AuthorId { get; }
}
