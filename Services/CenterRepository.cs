using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using EventManager.API.Data;
using EventManager.API.Domain.Entities;
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
        public void AddCenterAsync (Center center)
        {
            throw new NotImplementedException ();
        }

        public bool CenterExists (Guid centerId)
        {
            throw new NotImplementedException ();
        }

        public Task<bool> DeleteCenterAsync (Center center)
        {
            throw new NotImplementedException ();
        }

        public Task<Center> GetCenterByIdAsync (Guid centerId)
        {
            throw new NotImplementedException ();
        }

        public async Task<IEnumerable<Center>> GetCentersAsync ()
        {
            return await _dataContext.Centers.ToListAsync();
        }

        public async Task<bool> SaveChangesAsync ()
        {
            return (await _dataContext.SaveChangesAsync() > 0);
        }

        public Task<Center> UpdateCenterAsync (Center center)
        {
            throw new NotImplementedException ();
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
