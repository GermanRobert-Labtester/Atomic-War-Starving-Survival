namespace AtomicWar._Game.Inventory
{
    /// <summary>
    /// Equipment slot a wearable item occupies. A survivor can wear one item per
    /// slot; equipping into an occupied slot returns the previous item to storage.
    /// </summary>
    public enum EquipSlot
    {
        None,
        Body,
        Head,
        Face,
        Hands,
        Tool,
        Weapon
    }
}
