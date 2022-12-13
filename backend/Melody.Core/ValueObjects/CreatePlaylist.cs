using Ardalis.GuardClauses;
using Melody.SharedKernel;

namespace Melody.Core.ValueObjects;

public class CreatePlaylist : ValueObject
{
    public CreatePlaylist(string name, long authorId, long[] songIds)
    {
        foreach (var songId in songIds) Guard.Against.Negative(songId);
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(Name));
        AuthorId = Guard.Against.Negative(authorId, nameof(AuthorId));
        SongIds = songIds;
    }

    public string Name { get; }
    public long AuthorId { get; }
    public long[] SongIds { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return AuthorId;
        foreach (var role in SongIds.OrderBy(id => id)) yield return role;
    }
}