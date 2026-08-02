using UnityEngine;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Designer-authored radiation profile for a map tile or scavenging location.
    /// radLevel is an abstract dose-rate in the same units as ProtectiveGear.radProtection
    /// and shelter shielding (think mSv/hr-style), on a 0..1000 scale.
    /// </summary>
    [CreateAssetMenu(fileName = "NewRadZoneProfile", menuName = "ASHFALL/Radiation/Rad Zone Profile")]
    public class RadZoneProfile : ScriptableObject
    {
        public string id;
        public string displayName;
        [TextArea(2, 4)] public string description;

        /// <summary>Ambient dose-rate of the zone, 0..1000 abstract units.</summary>
        [Range(0f, 1000f)] public float radLevel;
    }
}
