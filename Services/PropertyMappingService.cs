using System;
using System.Collections.Generic;
using System.Linq;
using EventManager.API.Domain.Entities;
using EventManager.API.Models;

namespace EventManager.API.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private readonly Dictionary<string, PropertyMappingValue> _centerPropertyMapping = new
        Dictionary<string, PropertyMappingValue> (StringComparer.OrdinalIgnoreCase)
        { 
            { "CenterId", new PropertyMappingValue (new List<string> () { "CenterId" }) } 
            ,{ "Name", new PropertyMappingValue (new List<string> () { "Name" }) }
            ,{ "HallCapacity", new PropertyMappingValue (new List<string> () { "HallCapacity" }) }
            ,{ "Location", new PropertyMappingValue (new List<string> () { "Location" }) } 
            ,{ "Price", new PropertyMappingValue (new List<string> () { "Price" }) }
        };

        private readonly Dictionary<string, PropertyMappingValue> _eventPropertyMapping = new
        Dictionary<string, PropertyMappingValue> (StringComparer.OrdinalIgnoreCase)
        { 
            { "EventId", new PropertyMappingValue (new List<string> () { "EventId" }) }, 
            { "Name", new PropertyMappingValue (new List<string> () { "Name" }) }, 
            { "Purpose", new PropertyMappingValue (new List<string> () { "Purpose" }) }, 
            { "Note", new PropertyMappingValue (new List<string> () { "Note" }) }, 
        };

        private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping> ();

        public PropertyMappingService ()
        {
            _propertyMappings.Add (new PropertyMapping<CenterDto, Center> (_centerPropertyMapping));
            _propertyMappings.Add(new PropertyMapping<EventDto, Event>(_eventPropertyMapping));
        }

        public bool ValidMappingExistsFor<TSource, TDestination> (string fields)
        {
            var propertyMapping = GetPropertyMapping<TSource, TDestination> ();

            if (string.IsNullOrWhiteSpace (fields))
            {
                return true;
            }

            // the string is separated by ",", so we split it.
            var fieldsAfterSplit = fields.Split (',');

            // run through the fields clauses
            foreach (var field in fieldsAfterSplit)
            {
                // trim
                var trimmedField = field.Trim ();

                // remove everything after the first " " - if the fields 
                // are coming from an orderBy string, this part must be 
                // ignored
                var indexOfFirstSpace = trimmedField.IndexOf (" ");
                var propertyName = indexOfFirstSpace == -1 ?
                    trimmedField : trimmedField.Remove (indexOfFirstSpace);

                // find the matching property
                if (!propertyMapping.ContainsKey (propertyName))
                {
                    return false;
                }
            }
            return true;
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination> ()
            {
                // get matching mapping
                var matchingMapping = _propertyMappings
                    .OfType<PropertyMapping<TSource, TDestination>> ();

                if (matchingMapping.Count () == 1)
                {
                    return matchingMapping.First ().MappingDictionary;
                }

                throw new Exception ($"Cannot find exact property mapping instance " +
                    $"for <{typeof(TSource)},{typeof(TDestination)}");
            }
    }
}
  