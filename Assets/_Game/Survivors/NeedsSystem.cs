namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Pure-C# system that decays and restores survivor needs over game time and
    /// raises threshold events (starving, dehydrated, exhausted, broken) on the
    /// EventBus. Reads the clock from the TimeSystem; writes only via Needs.
    /// </summary>
    public class NeedsSystem
    {
        /// <summary>Advance need decay/recovery for all survivors over elapsed game hours.</summary>
        public void Tick(float gameHours) => throw new System.NotImplementedException();

        /// <summary>Apply a clamped delta to a single need of a survivor.</summary>
        public void Modify(Survivor survivor, NeedKind need, float delta) => throw new System.NotImplementedException();
    }
}
