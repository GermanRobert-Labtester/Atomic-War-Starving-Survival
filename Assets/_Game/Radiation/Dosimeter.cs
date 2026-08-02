namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Personal dosimeter reading model: cumulative dose and current dose rate.
    /// This is the in-world device's data; the DosimeterHUD presents it.
    /// Save/load safe.
    /// </summary>
    [System.Serializable]
    public class Dosimeter
    {
        public float CumulativeDose { get; private set; }
        public float CurrentRate { get; private set; }

        /// <summary>Record exposure at a given rate over a number of hours.</summary>
        public void Record(float radsPerHour, float hours) => throw new System.NotImplementedException();

        /// <summary>Zero the cumulative reading (new dosimeter / reset).</summary>
        public void Reset() => throw new System.NotImplementedException();
    }
}
