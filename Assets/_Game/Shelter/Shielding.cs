namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Radiation shielding level of the bunker; attenuates incoming fallout.
    /// Upgradeable with materials. Save/load safe.
    /// </summary>
    [System.Serializable]
    public class Shielding
    {
        public int Level = 1;

        /// <summary>Fraction of exterior radiation blocked (0..1) at the current level.</summary>
        public float AttenuationFactor => throw new System.NotImplementedException();

        /// <summary>Increase the shielding level by one.</summary>
        public void Upgrade() => throw new System.NotImplementedException();
    }
}
