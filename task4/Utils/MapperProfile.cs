using AutoMapper;
using task4.Entities;
using task4.Models;
using task4.Utils;

namespace task4.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<RegisterViewModel, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => AccountUtils.HashPassword(src.Password)))
                .ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<LoginViewModel, User>();
        }
    }
}