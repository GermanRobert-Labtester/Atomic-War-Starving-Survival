namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Save-safe snapshot of WeatherSystem's runtime state: enough to resume the
    /// exact same deterministic transition sequence after a load. WeatherSystem
    /// reseeds fresh per roll from Seed + RollCount rather than serializing
    /// System.Random's opaque internal state, so this snapshot only needs plain
    /// primitives.
    /// </summary>
    [System.Serializable]
    public class WeatherState
    {
        public WeatherKind Current = WeatherKind.Clear;
        public int Seed;
        public int RollCount;
        public float HoursUntilNextCheck;
        public float TotalElapsedHours;
    }
}
