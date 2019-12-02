using AutoMapper;

using EventManager.API.Domain.Entities;
using EventManager.API.Models;

namespace EventManager.API.Profiles
{
    public class UsersProfile : Profile
    {
        public UsersProfile ()
        {
            CreateMap<User, UserDto> ()
                .ForMember (
                    dest => dest.User,
                    opt => opt.MapFrom (src => $"{src.Firstname} {src.Lastname}"))
                .ForMember (
                    dest => dest.Email,
                    opt => opt.MapFrom (src => src.Email));
            CreateMap<User, UserForAuthenticationDto>()
                .ForMember(
                    dest => dest.User,
                    opt => opt.MapFrom (src => $"{src.Firstname} {src.Lastname}"));
            CreateMap<UserForCreationDto, User>();
        }
    }
}
