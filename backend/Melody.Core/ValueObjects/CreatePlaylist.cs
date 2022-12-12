using Melody.SharedKernel;

namespace Melody.Core.ValueObjects;

public class CreatePlaylist : ValueObject
{
    public CreatePlaylist(string name, long authorId, long[] songIds)
    {
        Name = name;
        AuthorId = authorId;
        SongIds = songIds;
    }
    public string Name { get; }
    public long AuthorId { get; }
    public long[] SongIds { get; }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return AuthorId;
        foreach (var role in SongIds.OrderBy(id => id))
        {
            yield return role;
        }
    }
}