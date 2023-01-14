using AutoMapper;
using Melody.Core.Entities;
using Melody.WebAPI.DTO.Playlist;

namespace Melody.WebAPI.MappingProfiles;

public class PlaylistProfile : Profile
{
    public PlaylistProfile()
    {
        CreateMap<FavouritePlaylist, FavouritePlaylistDto>();
        CreateMap<Playlist, PlaylistDto>();
        CreateMap<FavouritePlaylist, FavouritePlaylistWithPerformersDto>()
            .ForMember(
                destination => destination.PerformersNames,
                options => options.MapFrom(source => source.Songs.Select(s => s.AuthorName).ToList())
            );
        CreateMap<Playlist, FavouritePlaylistWithPerformersDto>()
            .ForMember(
                destination => destination.PerformersNames,
                options => options.MapFrom(source => source.Songs.Select(s => s.AuthorName).ToList())
            )
            .ForMember(
                destination => destination.IsFavourite,
                options => options.MapFrom(source => true)
            );
    }
}