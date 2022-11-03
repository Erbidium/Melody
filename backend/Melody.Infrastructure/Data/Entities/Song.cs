namespace Melody.Infrastructure.Data.Entities;

public class Song
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Path { get; set; }
    public string AuthorName { get; set; }
    public int Year { get; set; }
    public string Genre { get; set; }
    public bool IsDeleted { get; set; }
}
