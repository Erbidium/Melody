namespace Melody.Core.Entities;

public class FavouritePlaylist : Playlist
{
    public FavouritePlaylist(string name, long authorId, bool isFavourite)
        : base(name, authorId)
    {
        IsFavourite = isFavourite;
    }

    public bool IsFavourite { get; }
}