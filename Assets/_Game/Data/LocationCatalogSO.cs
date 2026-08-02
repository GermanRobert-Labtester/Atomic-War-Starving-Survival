using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// ScriptableObject catalog of scavenge locations; imported from
    /// StreamingAssets/Data/locations.json.
    /// </summary>
    [CreateAssetMenu(fileName = "NewLocationCatalog", menuName = "ASHFALL/Data/Location Catalog")]
    public class LocationCatalogSO : ScriptableObject
    {
        public List<LocationDefinitionSO> locations = new List<LocationDefinitionSO>();

        /// <summary>Look up a location by its snake_case id.</summary>
        public LocationDefinitionSO GetById(string id) => throw new System.NotImplementedException();
    }

    /// <summary>Static definition of a scavenge location: danger, travel time, base radiation.</summary>
    [CreateAssetMenu(fileName = "NewLocation", menuName = "ASHFALL/Data/Location")]
    public class LocationDefinitionSO : ScriptableObject
    {
        public string id;
        public string displayName;
        [TextArea(2, 4)] public string description;
        public float dangerLevel;
        public float travelHours;
        public float baseRadsPerHour;
    }
}
