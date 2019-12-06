using AutoMapper;

using EventManager.API.Domain.Entities;
using EventManager.API.Helpers;
using EventManager.API.Models;

namespace EventManager.API.Profiles
{
    public class CentersProfile : Profile
    {
        public CentersProfile ()
        {
            CreateMap<Center, CenterDto> ()
                .ForMember (
                    dest => dest.Type,
                    opt => opt.MapFrom (src => src.Type.GetDescription ()));
            CreateMap<CenterForCreationDto, Center> ();
            CreateMap<CenterForUpdateDto, Center> ();
            CreateMap<Center, CenterForUpdateDto> ();
        }
    }
}
