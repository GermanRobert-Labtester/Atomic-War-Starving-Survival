using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Identifies a single survivor need, used for targeted modification and UI.
    /// </summary>
    public enum NeedKind
    {
        Hunger,
        Thirst,
        Fatigue,
        Warmth,
        Morale,
        Health
    }

    /// <summary>
    /// Physiological + psychological need values for one survivor, each clamped
    /// 0..100. Radiation dose accumulates separately (see AtomicWar._Game.Radiation).
    /// Save/load safe: primitives only.
    /// </summary>
    [System.Serializable]
    public class Needs
    {
        public float Hunger;
        public float Thirst;
        public float Fatigue;
        public float Warmth = 100f;
        public float Morale = 75f;
        public float Health = 100f;

        // Edge-detection for NeedsSystem.OnNeedCritical: lets the system fire the
        // event once on the transition into critical range instead of every tick.
        public bool WasHungerCritical;
        public bool WasThirstCritical;
        public bool WasWarmthCritical;

        /// <summary>Clamp every need into the valid 0..100 range.</summary>
        public void ClampAll()
        {
            Hunger = Mathf.Clamp(Hunger, 0f, 100f);
            Thirst = Mathf.Clamp(Thirst, 0f, 100f);
            Fatigue = Mathf.Clamp(Fatigue, 0f, 100f);
            Warmth = Mathf.Clamp(Warmth, 0f, 100f);
            Morale = Mathf.Clamp(Morale, 0f, 100f);
            Health = Mathf.Clamp(Health, 0f, 100f);
        }
    }
}
