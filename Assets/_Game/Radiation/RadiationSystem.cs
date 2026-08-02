using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Accumulates radiation dose on survivors from the environment and
    /// contaminated items/zones, applies shelter shielding / filtration
    /// mitigation, and triggers chronic-illness effects at high cumulative dose.
    /// </summary>
    public class RadiationSystem
    {
        /// <summary>Advance dose accumulation and chronic effects over elapsed game hours.</summary>
        public void Tick(float gameHours) => throw new System.NotImplementedException();

        /// <summary>Expose a survivor to a dose rate for a number of hours.</summary>
        public void Expose(Survivor survivor, float radsPerHour, float hours) => throw new System.NotImplementedException();

        /// <summary>Administer iodine pills to blunt thyroid uptake for a window of time.</summary>
        public void AdministerIodine(Survivor survivor) => throw new System.NotImplementedException();

        /// <summary>Administer anti-rad medication to reduce cumulative dose.</summary>
        public void AdministerAntiRad(Survivor survivor, float radsRemoved) => throw new System.NotImplementedException();
    }
}
