using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>Static definition of a scavenge location: danger, travel time, base radiation.
    ///
    /// Lives in its own file because Unity only links a MonoScript to an asset
    /// when the type's file name matches the class name. Declared inside
    /// LocationCatalogSO.cs it serialized with m_Script: {fileID: 0}, which made
    /// every generated location unresolvable by type.</summary>
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
