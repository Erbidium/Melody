﻿using Melody.Core.Entities;

namespace Melody.Core.Interfaces;

public interface IPlaylistRepository
{
    Task<bool> Create(CreatePlaylist playlist);
    Task<IReadOnlyCollection<FavouritePlaylist>> GetPlaylistsCreatedByUser(long userId);
    Task<FavouritePlaylist?> GetById(long id, long userId);
    Task<IReadOnlyCollection<Playlist>> GetAll();
    Task<bool> AddSongs(long id, long[] songIds);
    Task Delete(long id);
    Task DeleteSong(long id, long songId);
    Task CreateFavouritePlaylist(long id, long userId);
    Task DeleteFavouritePlaylist(long id, long userId);
}