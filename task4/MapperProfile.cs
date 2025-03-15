using AutoMapper;
using Isopoh.Cryptography.Argon2;
using task4.Entities;
using task4.Models;
using task4.Utils;

namespace task4
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<RegisterViewModel, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => Argon2.Hash(src.Password, 2, 16384, 2, Argon2Type.HybridAddressing, 32, null)))
                .ForMember(dest => dest.LastLogin, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<LoginViewModel, User>();
        }
    }
}