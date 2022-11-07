using Melody.Core.Exceptions;

namespace Melody.Core.Entities;

public class Playlist
{
    public Playlist(string name, string link, long authorId, long id = -1)
    {
        Id = id;
        Name = name;
        Link = link;
        AuthorId = authorId;
    }

    private long _id;
    public long Id
    {
        get => _id < 0 ? throw new WrongIdException() : _id;
        set => _id = value;
    }
    public string Name { get; }
    public string Link { get; }
    public long AuthorId { get; }
}
