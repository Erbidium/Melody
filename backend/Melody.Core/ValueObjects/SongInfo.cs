using Melody.SharedKernel;

namespace Melody.Core.ValueObjects;

public class SongInfo : ValueObject
{
    public string Name { get; private set; }
    public string Path { get; private set; }
    public string AuthorName { get; private set; }
    public int Year { get; private set; }
    public int GenreId { get; private set; }

    public SongInfo(string name, string path, string authorName, int year, int genreId)
    {
        Name = name;
        Path = path;
        AuthorName = authorName;
        Year = year;
        GenreId = genreId;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Path;
        yield return AuthorName;
        yield return Year;
        yield return GenreId;
    }
}
