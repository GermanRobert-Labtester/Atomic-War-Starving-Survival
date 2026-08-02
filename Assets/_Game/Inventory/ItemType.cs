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
        Medical,
        RadProtection,
        Tool,
        Material,
        Fuel,
        Filter,
        Comfort,
        Quest
    }
}
