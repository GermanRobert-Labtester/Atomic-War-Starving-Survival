namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Air-filtration unit: scrubs airborne fallout from incoming air. Degrades
    /// over time and consumes replaceable air filters; failure lets interior
    /// contamination rise. Save/load safe.
    /// </summary>
    [System.Serializable]
    public class AirFiltration
    {
        public float FilterHealth = 100f;
        public bool IsPowered;

        /// <summary>Current filtration efficiency (0..1) given filter health and power.</summary>
        public float FiltrationEfficiency => throw new System.NotImplementedException();

        /// <summary>Degrade the filter and update efficiency over elapsed game hours.</summary>
        public void Tick(float gameHours) => throw new System.NotImplementedException();

        /// <summary>Install a fresh air filter, restoring filter health.</summary>
        public void ReplaceFilter() => throw new System.NotImplementedException();
    }
}
