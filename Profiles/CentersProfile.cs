using AutoMapper;

using EventManager.API.Domain.Entities;
using EventManager.API.Models;

namespace EventManager.API.Profiles
{
    public class CentersProfile : Profile
    {
        public CentersProfile ()
        {
            CreateMap<Center, CenterDto> ();
            CreateMap<CenterForCreationDto, Center> ();
            CreateMap<CenterForUpdateDto, Center> ();
        }
    }
}
