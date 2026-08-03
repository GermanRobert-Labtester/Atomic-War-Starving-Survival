namespace AtomicWar._Game.Inventory
{
    /// <summary>
    /// Canonical item categories. snake_case ids in the JSON data map to these
    /// via the editor importer.
    /// </summary>
    public enum ItemType
    {
        Food,
        Water,
        IrradiatedWater,
        Medical,
        AntiRad,
        Iodine,
        Protective,
        Tool,
        Fuel,
        Filter,
        Material,
        Trade,
        Comfort,
        Quest,
        Device,
        /// <summary>Firearms, melee, ammo, ballistic gear used in hatch defense.</summary>
        Weapon
    }
}
