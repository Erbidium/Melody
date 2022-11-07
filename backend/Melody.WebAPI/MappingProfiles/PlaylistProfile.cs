using AutoMapper;
using Melody.Core.Entities;
using Melody.WebAPI.DTO.User;

namespace Melody.WebAPI.MappingProfiles;

public class PlaylistProfile : Profile
{
    public PlaylistProfile()
    {
        CreateMap<NewUserDto, User>();
    }
}
