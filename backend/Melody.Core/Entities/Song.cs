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

    public long Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string AuthorName { get; set; }
    public int Year { get; set; }
    public long GenreId { get; set; }
}
