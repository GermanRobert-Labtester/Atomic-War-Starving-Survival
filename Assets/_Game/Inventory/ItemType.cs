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
        Weapon,
        /// <summary>Dead body occupying bunker storage (Internal Horror — corpse management).</summary>
        Corpse,
        /// <summary>Food spoiled by humidity/rust; eating risks botulism.</summary>
        ContaminatedFood,
        /// <summary>
        /// Expansion IV: pre-war cultural artefact (cassette tapes, photo albums, vinyl).
        /// No consumption mechanics; Bunker-Born survivors treat them as sacred objects.
        /// Dismantling triggers ArtifactReverenceBrawlEvent.
        /// </summary>
        Relic
    }
}
