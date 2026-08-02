using UnityEngine;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Personal dosimeter read-model the UI (DosimeterHUD) presents: the current
    /// dose-rate, a decaying "recent exposure" window, and the lifetime dose mirrored
    /// from Survivor.LifetimeRadiationExposure (the authoritative, save/load-safe
    /// value). Public fields, not properties, so JsonUtility can serialize it.
    /// </summary>
    [System.Serializable]
    public class Dosimeter
    {
        /// <summary>Survivor this reading belongs to.</summary>
        public string SurvivorId;

        /// <summary>Instantaneous dose-rate for the latest tick (units/hour).</summary>
        public float CurrentRate;

        /// <summary>Decaying sum of recent exposure (a short rolling window for the UI).</summary>
        public float RecentExposure;

        /// <summary>Lifetime dose mirrored from the survivor; the survivor field is authoritative.</summary>
        public float LifetimeDose;

        /// <summary>Fraction of RecentExposure retained each recorded tick (the rest decays away).</summary>
        public const float RecentRetention = 0.5f;

        /// <summary>Record one tick of exposure: refresh the rate and fold it into the recent window.</summary>
        public void Record(float exposureThisTick, float gameHours)
        {
            float clamped = Mathf.Max(0f, exposureThisTick);
            CurrentRate = gameHours > 0f ? clamped / gameHours : 0f;
            RecentExposure = Mathf.Max(0f, RecentExposure * RecentRetention + clamped);
        }

        /// <summary>Zero the reading (new dosimeter / reset).</summary>
        public void Reset()
        {
            CurrentRate = 0f;
            RecentExposure = 0f;
            LifetimeDose = 0f;
        }
    }
}
