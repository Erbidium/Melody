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
                configurationExpression => configurationExpression.MapFrom(source => source.Songs.Select(s => s.AuthorName).ToList())
            );
    }
}