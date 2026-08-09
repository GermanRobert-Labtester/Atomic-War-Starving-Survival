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
        public LocationDefinitionSO GetById(string id)
        {
            if (string.IsNullOrEmpty(id) || locations == null) return null;
            for (int i = 0; i < locations.Count; i++)
            {
                if (locations[i] != null && locations[i].id == id)
                    return locations[i];
            }
            return null;
        }
    }
}
