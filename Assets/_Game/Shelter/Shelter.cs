namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// The bunker aggregate: owns the shielding and air-filtration sub-systems
    /// and exposes the effective interior radiation level for occupants given
    /// the exterior dose. Save/load safe.
    /// </summary>
    [System.Serializable]
    public class Shelter
    {
        public Shielding Shielding { get; } = new Shielding();
        public AirFiltration AirFiltration { get; } = new AirFiltration();

        /// <summary>Effective interior dose rate given an exterior dose rate.</summary>
        public float GetInteriorRadsPerHour(float exteriorRads) => throw new System.NotImplementedException();

        /// <summary>Advance shelter sub-systems over elapsed game hours.</summary>
        public void Tick(float gameHours) => throw new System.NotImplementedException();
    }
}
