using AutoMapper;
using EventManager.API.Domain.Entities;
using EventManager.API.Models;

namespace EventManager.API.Profiles
{
    public class EventsProfile : Profile
    {
        public EventsProfile()
        {
            CreateMap<Event, EventDto>();
            CreateMap<EventForCreationDto, Event>();
            CreateMap<EventForUpdateDto, Event>();
        }
    }
}