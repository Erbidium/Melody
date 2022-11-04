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

    public long Id { get; set; }
    public string Name { get; set; }
    public string Link { get; set; }
    public long AuthorId { get; set; }
}
