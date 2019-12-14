using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EventManager.API.Data;
using EventManager.API.Domain.Entities;
using EventManager.API.Helpers;
using EventManager.API.Models;
using EventManager.API.ResourceParameters;

using Microsoft.EntityFrameworkCore;

namespace EventManager.API.Services
{
    public class EventRepository : IEventRepository
    {
        private DataContext _dataContext;
        private readonly IPropertyMappingService _propertyMappingService;

        public EventRepository (DataContext dataContext, IPropertyMappingService propertyMappingService)
        {
            _dataContext = dataContext ??
                throw new ArgumentNullException (nameof (dataContext));
            _propertyMappingService = propertyMappingService ??
                throw new ArgumentNullException (nameof (propertyMappingService));
        }
        public void AddEvent (Event eventToAdd)
        {
            if (eventToAdd == null)
            {
                throw new ArgumentNullException (nameof (eventToAdd));
            }

            _dataContext.Add (eventToAdd);
        }

        public PagedList<Event> GetEvents (EventsResourceParameters eventsResourceParameters)
        {
            if (eventsResourceParameters == null)
            {
                throw new ArgumentNullException (nameof (eventsResourceParameters));
            }

            IQueryable<Event> events = _dataContext.Events as IQueryable<Event>;

            if (!string.IsNullOrWhiteSpace (eventsResourceParameters.Name))
            {
                var name = eventsResourceParameters.Name.Trim ();
                events = events.Where (x => x.Name == name);
            }

            if (!string.IsNullOrWhiteSpace (eventsResourceParameters.SearchQuery))
            {
                var searchQuery = eventsResourceParameters.SearchQuery.Trim ();
                events = events.Where (x => x.Name.Contains (searchQuery) ||
                    x.Purpose.Contains (searchQuery) ||
                    x.Note.Contains (searchQuery));
            }

            if (!string.IsNullOrWhiteSpace (eventsResourceParameters.OrderBy))
            {
                var eventPropertyMappingDictionary = _propertyMappingService.GetPropertyMapping<EventDto, Event> ();
                events = events.ApplySort (eventsResourceParameters.OrderBy, eventPropertyMappingDictionary);
            }

            return PagedList<Event>.Create (events, eventsResourceParameters.PageNumber, eventsResourceParameters.PageSize);
        }

        public async Task<Event> GetEventByIdAsync (Guid eventId)
        {
            return await _dataContext.Events.FindAsync (eventId);
        }

        public async Task<IEnumerable<Event>> GetEventsAsync ()
        {
            return await _dataContext.Events.ToListAsync ();
        }

        public async Task<bool> SaveChangesAsync ()
        {
            return (await _dataContext.SaveChangesAsync () > 0);
        }

        public async Task<bool> CheckIfEventExistForCenterAsync(Guid centerId, DateTimeOffset eventDate)
        {
            return await _dataContext.Events.AnyAsync(x => x.CenterId == centerId && x.ScheduledDate == eventDate.Date.ToShortDateString());
        }

        public async Task<bool> CheckIfEventExistForCenterAsync(Guid centerId, Guid eventId, DateTimeOffset eventDate)
        {
            return await _dataContext.Events.AnyAsync(x => x.CenterId == centerId 
                && x.ScheduledDate == eventDate.Date.ToShortDateString()
                && x.Id != eventId);
        }

        public void DeleteEvent (Event eventToDelete)
        {
            throw new NotImplementedException ();
        }

        public void UpdateEvent (Event eventToUpdate)
        {
            // no code in this implementation
        }

        public void Dispose ()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }

        protected virtual void Dispose (bool disposing)
        {
            if (disposing)
            {
                if (_dataContext != null)
                {
                    _dataContext.Dispose ();
                    _dataContext = null;
                }
            }
        }
    }
}
