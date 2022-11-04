namespace Melody.Core.Entities;

public class Song
{
    public Song(long id, string name, string path, string authorName, int year, long genreId)
    {
        Id = id;
        Name = name;
        Path = path;
        AuthorName = authorName;
        Year = year;
        GenreId = genreId;
    }

    public long Id { get; }
    public string Name { get; }
    public string Path { get; }
    public string AuthorName { get; }
    public int Year { get; }
    public long GenreId { get; }
}
