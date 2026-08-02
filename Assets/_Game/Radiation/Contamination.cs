using UnityEngine;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Radioactive contamination carried on a surface, zone, item, or survivor:
    /// a current dose-rate, natural decay, and whether it is still actively shedding
    /// fallout. Items picked up in hot zones carry this; bringing contaminated items
    /// into the bunker adds their AmbientContribution to the bunker's ambient dose
    /// until they decay or are decontaminated (DecontaminationStation). Save/load safe.
    /// </summary>
    [System.Serializable]
    public class Contamination
    {
        public float RadsPerHour;
        public float DecayPerHour;
        public bool IsActive;

        /// <summary>Natural decay over elapsed game hours; deactivates once the rate hits zero.</summary>
        public void Decay(float gameHours)
        {
            if (gameHours <= 0f)
            {
                return;
            }
            RadsPerHour = Mathf.Max(0f, RadsPerHour - DecayPerHour * gameHours);
            IsActive = RadsPerHour > 0f;
        }

        /// <summary>Dose-rate this contamination adds to a space it is brought into (0 when inactive).</summary>
        public float AmbientContribution()
        {
            return IsActive ? Mathf.Max(0f, RadsPerHour) : 0f;
        }
    }
}
