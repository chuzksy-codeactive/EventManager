using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using EventManager.API.Domain.Entities;
using EventManager.API.Helpers;
using EventManager.API.ResourceParameters;

namespace EventManager.API.Services
{
    public interface ICenterRepository
    {
        PagedList<Center> GetCenters (CentersResourceParameters centersResourceParameters);
        void AddCenter (Center center);
        Task<Center> GetCenterByIdAsync (Guid centerId);
        void UpdateCenter (Center center);
        void DeleteCenter (Center center);
        bool CenterExists (string centerName);
        Task<bool> SaveChangesAsync ();
    }
}
