using UnityEngine;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Decontamination station: reduces the contamination on items (or a survivor)
    /// over time so they can be brought into the bunker without raising its ambient
    /// dose. Stub-level system — not yet wired into the inventory/scene; operates
    /// directly on a Contamination model.
    /// </summary>
    public class DecontaminationStation
    {
        /// <summary>Contamination dose-rate (RadsPerHour) scrubbed per hour of operation.</summary>
        public float DeconRatePerHour = 10f;

        /// <summary>Whether the station is powered/supplied and able to run.</summary>
        public bool IsOperational = true;

        /// <summary>Reduce a contamination over elapsed game hours; deactivates it once clean.</summary>
        public void Decontaminate(Contamination contamination, float gameHours)
        {
            if (!IsOperational || contamination == null || gameHours <= 0f)
            {
                return;
            }

            contamination.RadsPerHour = Mathf.Max(0f, contamination.RadsPerHour - DeconRatePerHour * gameHours);
            contamination.IsActive = contamination.RadsPerHour > 0f;
        }
    }
}
