using System.Collections.Generic;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Per-survivor input the RadiationSystem needs to compute one tick of exposure:
    /// the dose-rate of the zone they occupy, how much shelter shielding applies
    /// (0 when outside), optional Shelter aggregate reference, and gear currently worn.
    /// </summary>
    public sealed class ExposureContext
    {
        /// <summary>Ambient zone dose-rate (e.g. RadZoneProfile.radLevel / FalloutMap sample).</summary>
        public float ZoneRadLevel;

        /// <summary>Effective shelter shielding subtracted from ambient (0 when unsheltered).</summary>
        public float ShelterShielding;

        /// <summary>Optional Shelter aggregate reference.</summary>
        public Shelter.Shelter Shelter;

        /// <summary>Gear currently worn; may be null or empty.</summary>
        public List<WornGear> WornGear = new List<WornGear>();
    }
}
