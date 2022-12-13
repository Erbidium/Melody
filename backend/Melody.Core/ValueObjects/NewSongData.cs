using Ardalis.GuardClauses;
using Melody.Core.Constants;
using Melody.Core.Exceptions;
using Melody.SharedKernel;

namespace Melody.Core.ValueObjects;

public class NewSongData : ValueObject
{
    public NewSongData(long userId, string name, string authorName, int year, long genreId, string extension)
    {
        if (extension != SongConstants.SoundExtension) throw new WrongExtensionException();

        UserId = Guard.Against.Negative(userId, nameof(UserId));
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(Name));
        AuthorName = Guard.Against.NullOrWhiteSpace(authorName, nameof(AuthorName));
        Year = Guard.Against.Negative(year, nameof(Year));
        GenreId = Guard.Against.Negative(genreId, nameof(GenreId));
        Extension = Guard.Against.NullOrWhiteSpace(extension, nameof(extension));
    }

    public long UserId { get; }
    public string Name { get; }
    public string AuthorName { get; }
    public int Year { get; }
    public long GenreId { get; }
    public string Extension { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return UserId;
        yield return Name;
        yield return AuthorName;
        yield return Year;
        yield return GenreId;
        yield return Extension;
    }
}