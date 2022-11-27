using Melody.Core.Exceptions;
using Melody.SharedKernel.Interfaces;

namespace Melody.Core.Entities;

public class Playlist : EntityBase<long>
{
    public Playlist(string name, string link, long authorId)
    {
        Name = name;
        Link = link;
        AuthorId = authorId;
    }

    public string Name { get; }
    public string Link { get; }
    public long AuthorId { get; }
}