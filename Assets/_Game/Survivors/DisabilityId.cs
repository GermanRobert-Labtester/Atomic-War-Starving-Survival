namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Canonical permanent-disability ids. These lived as bare string literals at the
    /// call sites (Survivor.MaxHealthCap, ExpeditionSystem.Ops, CraftingSystem) and as
    /// two separate copies of the same constants (DisabilitySO.Ids in Medical,
    /// PersonalQuestSystem.ScarredLungsId in Survivors), so a rename could silently
    /// disable a disability's effect in one system while leaving it live in another.
    ///
    /// This class is the single source of truth. It lives in the Survivors assembly
    /// because that is the only one every consumer can reference — Medical, Core,
    /// and Crafting all depend on Survivors, not the other way round.
    /// </summary>
    public static class DisabilityId
    {
        /// <summary>Leg injury — slows expedition travel.</summary>
        public const string Limp = "limp";

        /// <summary>Lung scarring — caps maximum health.</summary>
        public const string ScarredLungs = "scarred_lungs";

        /// <summary>Hand tremors — degrades crafting outcomes.</summary>
        public const string Tremors = "tremors";

        /// <summary>Lost eye — degrades ranged accuracy and search results.</summary>
        public const string OneEye = "one_eye";
    }
}
