using System.Collections.Generic;

namespace EventManager.API.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination> ();
        bool ValidMappingExistsFor<TSource, TDestination> (string fields);
    }
}