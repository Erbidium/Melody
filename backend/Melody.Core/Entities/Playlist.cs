using Melody.SharedKernel.Interfaces;

namespace Melody.Core.Entities;

public class Playlist : EntityBase<long>
{
    public Playlist(string name, long authorId)
    {
        Name = name;
        AuthorId = authorId;
    }

    public List<FavouriteSong> Songs { get; set; } = new();

    public string Name { get; }
    public long AuthorId { get; }
}