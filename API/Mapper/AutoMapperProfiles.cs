using AutoMapper;
using Chanchas.DTOs;
using Microsoft.AspNetCore.Identity;

namespace Chanchas.Mapper
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDto, IdentityUser>();
        }

    }
}
