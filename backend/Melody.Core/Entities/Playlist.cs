using Ardalis.GuardClauses;
using Melody.SharedKernel;

namespace Melody.Core.Entities;

public class Playlist : EntityBase<long>
{
    public Playlist(string name, long authorId)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(Name));
        AuthorId = Guard.Against.Negative(authorId, nameof(AuthorId));
    }

    public List<FavouriteSong> Songs { get; set; } = new();
    public string Name { get; }
    public long AuthorId { get; }
}