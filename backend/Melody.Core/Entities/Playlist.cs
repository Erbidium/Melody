using Melody.Core.Exceptions;
using Melody.SharedKernel.Interfaces;

namespace Melody.Core.Entities;

public class Playlist : IEntityBase<long>
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
        private set => _id = value;
    }
    public string Name { get; }
    public string Link { get; }
    public long AuthorId { get; }
}
