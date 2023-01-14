using AutoMapper;
using Melody.Core.Entities;
using Melody.WebAPI.DTO.Genre;

namespace Melody.WebAPI.MappingProfiles;

public class GenreProfile : Profile
{
    public GenreProfile()
    {
        CreateMap<Genre, GenreDto>();
    }
}