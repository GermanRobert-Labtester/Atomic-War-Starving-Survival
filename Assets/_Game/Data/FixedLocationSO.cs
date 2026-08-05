using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Data
{
    /// <summary>
    /// ScriptableObject defining a permanent, fixed location on the 2D world map.
    /// Prompt #319: Locations are fixed on the map, but their physical state and occupants change every run.
    /// </summary>
    [CreateAssetMenu(fileName = "NewFixedLocation", menuName = "ASHFALL/Data/Fixed Location")]
    public class FixedLocationSO : ScriptableObject
    {
        public string id; // snake_case id
        public string displayName;
        [TextArea(2, 4)] public string description;
        public Vector2 mapCoordinates;
        public float baseDangerLevel;
        public float travelHours;
        public float baseRadsPerHour;
        public List<string> possibleVisitorCardIds = new List<string>();
    }
}
