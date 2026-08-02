namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Tracks radioactive contamination on a surface, zone, item, or survivor:
    /// current dose rate, natural decay, and whether it is actively shedding
    /// fallout. Save/load safe.
    /// </summary>
    [System.Serializable]
    public class Contamination
    {
        public float RadsPerHour;
        public float DecayPerHour;
        public bool IsActive;

        /// <summary>Decay the contamination over elapsed game hours.</summary>
        public void Decay(float hours) => throw new System.NotImplementedException();
    }
}
