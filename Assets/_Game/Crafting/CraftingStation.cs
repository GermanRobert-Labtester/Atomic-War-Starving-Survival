namespace AtomicWar._Game.Crafting
{
    /// <summary>
    /// A craft station (workbench, water still, chemistry set) that gates which
    /// recipes can run and may degrade with use. Save/load safe.
    /// </summary>
    [System.Serializable]
    public class CraftingStation
    {
        public string id;
        public string displayName;
        public float Condition = 100f;

        public bool IsOperational => Condition > 0f;

        /// <summary>Reduce station condition by an amount (wear from crafting).</summary>
        public void Degrade(float amount) => throw new System.NotImplementedException();
    }
}
