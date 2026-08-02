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
        public float Warmth;
        public float Morale;
        public float Health;

        /// <summary>Clamp every need into the valid 0..100 range.</summary>
        public void ClampAll() => throw new System.NotImplementedException();
    }
}
