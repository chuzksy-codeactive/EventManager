using System;
using System.Threading.Tasks;

using EventManager.API.Domain.Entities;
using EventManager.API.Helpers;
using EventManager.API.ResourceParameters;

namespace EventManager.API.Services
{
    public interface IEventRepository
    {
        PagedList<Event> GetEvents (EventsResourceParameters eventsResourceParameters);
        void AddEvent (Event eventToAdd);
        Task<Event> GetEventByIdAsync (Guid eventId);
        Task<bool> CheckIfEventExistForCenterAsync(Guid centerId, DateTimeOffset eventDate); 
        Task<bool> CheckIfEventExistForCenterAsync(Guid centerId, Guid eventId, DateTimeOffset eventDate); 
        void UpdateEvent (Event eventToUpdate);
        void DeleteEvent (Event eventToDelete);
        Task<bool> SaveChangesAsync ();
    }
}
