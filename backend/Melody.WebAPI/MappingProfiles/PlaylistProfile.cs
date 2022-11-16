using AutoMapper;
using Melody.Core.Entities;
using Melody.WebAPI.DTO.Playlist;

namespace Melody.WebAPI.MappingProfiles;

public class PlaylistProfile : Profile
{
    public PlaylistProfile()
    {
        CreateMap<NewPlaylistDto, Playlist>();
        CreateMap<UpdatePlaylistDto, Playlist>();
    }
}