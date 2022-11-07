using Melody.Core.Exceptions;

namespace Melody.Core.Entities;

public class Song
{
    public Song(string name, string path, string authorName, int year, long genreId, long id = -1)
    {
        Id = id;
        Name = name;
        Path = path;
        AuthorName = authorName;
        Year = year;
        GenreId = genreId;
    }

    private long _id;
    public long Id
    {
        get => _id < 0 ? throw new WrongIdException() : _id;
        set => _id = value;
    }
        
    public string Name { get; }
    public string Path { get; }
    public string AuthorName { get; }
    public int Year { get; }
    public long GenreId { get; }
}
