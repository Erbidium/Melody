using AutoMapper;
using Melody.Core.Entities;
using Melody.WebAPI.DTO.Song;

namespace Melody.WebAPI.MappingProfiles;

public class SongProfile : Profile
{
    public SongProfile()
    {
        CreateMap<NewSongDto, Song>();
        CreateMap<UpdateSongDto, Song>();
        CreateMap<Song, SongDto>();
    }
}