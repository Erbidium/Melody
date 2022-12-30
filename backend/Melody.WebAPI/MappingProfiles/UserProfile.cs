﻿using AutoMapper;
using Melody.Core.Entities;
using Melody.WebAPI.DTO.RecommendationsPreferences;
using Melody.WebAPI.DTO.User;

namespace Melody.WebAPI.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserForAdminDto>();
        CreateMap<CreateRecommendationsPreferencesDto, RecommendationsPreferences>();
        CreateMap<RecommendationsPreferences, RecommendationsPreferencesDto>();
    }
}