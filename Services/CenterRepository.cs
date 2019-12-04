using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EventManager.API.Data;
using EventManager.API.Domain.Entities;
using EventManager.API.Helpers;
using EventManager.API.ResourceParameters;

using Microsoft.EntityFrameworkCore;

namespace EventManager.API.Services
{
    public class CenterRepository : ICenterRepository
    {
        private DataContext _dataContext;

        public CenterRepository (DataContext dataContext)
        {
            _dataContext = dataContext ??
                throw new ArgumentNullException (nameof (dataContext));
        }
        public void AddCenter (Center centerToAdd)
        {
            if (centerToAdd == null)
            {
                throw new ArgumentNullException (nameof (centerToAdd));
            }

            _dataContext.Add (centerToAdd);
        }

        public bool CenterExists (string centerName)
        {
            return _dataContext.Centers.Any (x => x.Name == centerName);
        }

        public void DeleteCenter (Center center)
        {
            _dataContext.Remove (center);
        }

        public async Task<Center> GetCenterByIdAsync (Guid centerId)
        {
            return await _dataContext.Centers.FindAsync (centerId);
        }

        public async Task<IEnumerable<Center>> GetCentersAsync ()
        {
            return await _dataContext.Centers.ToListAsync ();
        }

        public PagedList<Center> GetCenters (CentersResourceParameters centersResourceParameters)
        {
            var centers = _dataContext.Centers as IQueryable<Center>;

            if (!string.IsNullOrWhiteSpace (centersResourceParameters.Name))
            {
                var name = centersResourceParameters.Name.Trim ();
                centers = centers.Where (x => x.Name == name);
            }

            if (!string.IsNullOrWhiteSpace (centersResourceParameters.SearchQuery))
            {
                var searchQuery = centersResourceParameters.SearchQuery.Trim ();
                centers = centers.Where (x => x.Name.Contains (searchQuery) ||
                    x.Location.Contains (searchQuery));
            }

            if(!string.IsNullOrWhiteSpace(centersResourceParameters.OrderBy))
            {
                if(centersResourceParameters.OrderBy.ToLowerInvariant() == "name")
                {
                    centers = centers.OrderBy(x => x.Name).ThenBy(x => x.Location);
                }
            }

            return PagedList<Center>.Create(centers, centersResourceParameters.PageNumber, centersResourceParameters.PageSize);
        }

        public async Task<bool> SaveChangesAsync ()
        {
            return (await _dataContext.SaveChangesAsync () > 0);
        }

        public void UpdateCenter (Center center)
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
