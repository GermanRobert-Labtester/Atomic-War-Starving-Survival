namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Coarse activity state driving need decay rates and AI availability.
    /// </summary>
    public enum SurvivorState
    {
        Idle,
        Working,
        Resting,
        Sick,
        Incapacitated,
        Dead
    }

    /// <summary>
    /// Runtime model for a single survivor: identity, activity state, and current
    /// need values. Save/load safe (primitives only); behaviour lives in
    /// NeedsSystem and the Utility AI.
    /// </summary>
    [System.Serializable]
    public class Survivor
    {
        public string Id;
        public string DisplayName;
        public SurvivorState State { get; set; } = SurvivorState.Idle;

        public bool IsAlive => State != SurvivorState.Dead;

        public Needs Needs { get; } = new Needs();
    }
}
