using Melody.SharedKernel.Interfaces;

namespace Melody.Core.Entities;

public class FavouriteSong : EntityBase<long>
{
    public FavouriteSong(string name, string authorName, long genreId,
        DateTime uploadedAt, TimeSpan duration, bool isFavourite)
    {
        Name = name;
        AuthorName = authorName;
        GenreId = genreId;
        UploadedAt = uploadedAt;
        Duration = duration;
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