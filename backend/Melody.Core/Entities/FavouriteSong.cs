using Ardalis.GuardClauses;
using Melody.SharedKernel;

namespace Melody.Core.Entities;

public class FavouriteSong : EntityBase<long>
{
    public FavouriteSong(string name, string authorName, long genreId,
        DateTime uploadedAt, TimeSpan duration, bool isFavourite)
    {
        Name = Guard.Against.NullOrWhiteSpace(name, nameof(Name));
        AuthorName = Guard.Against.NullOrWhiteSpace(authorName, nameof(AuthorName));
        GenreId = Guard.Against.Negative(genreId, nameof(GenreId));
        UploadedAt = Guard.Against.Default(uploadedAt, nameof(UploadedAt));
        Duration = Guard.Against.NegativeOrZero(duration, nameof(Duration));
        IsFavourite = isFavourite;
    }

    public string Name { get; }
    public string AuthorName { get; }
    public TimeSpan Duration { get; }
    public DateTime UploadedAt { get; }
    public long GenreId { get; }
    public Genre? Genre { get; set; }
    public bool IsFavourite { get; }
}