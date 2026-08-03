using UnityEngine;

namespace AtomicWar._Game.Medical
{
    /// <summary>
    /// Data-driven medical state (gunshot, dysentery, rad burns, sepsis, …).
    /// Health is no longer a free-floating bar: active afflictions drain it.
    /// Untreated afflictions can progress to a worse state after ProgressionHours.
    /// </summary>
    [CreateAssetMenu(fileName = "Affliction", menuName = "ASHFALL/Medical/Affliction")]
    public class AfflictionSO : ScriptableObject
    {
        [Header("Identity")]
        public string id;
        public string displayName;
        [TextArea(2, 4)] public string description;
        public AfflictionPhase phase = AfflictionPhase.Phase1;

        [Header("Health pressure")]
        [Tooltip("Health lost per game-hour while this affliction is active (before lethality mult).")]
        public float healthDrainPerHour = 1f;

        [Tooltip("Base lethality scale. Infections with high LatentDamage use ImmuneCollapseFactor.")]
        public float baseLethality = 1f;

        [Tooltip("If true, LatentDamage from the prognosis pipeline multiplies lethality (immune collapse).")]
        public bool isInfection;

        [Header("Progression")]
        [Tooltip("Game-hours until this progresses if not halted/treated. 0 = never auto-progresses.")]
        public float progressionHours = 48f;

        [Tooltip("snake_case id of the next AfflictionSO (e.g. sepsis). Empty = no progression.")]
        public string progressesToId;

        [Tooltip("Item id that can emergency-halt progression (e.g. bandage for gunshot_wound).")]
        public string emergencyHaltItemId;

        [Header("Treatment gates")]
        [Tooltip("Complex treatments require an operational medical_bed module.")]
        public bool requiresMedicalBed;

        /// <summary>Canonical ids used in code and StreamingAssets.</summary>
        public static class Ids
        {
            // Phase 1
            public const string Bleeding = "bleeding";
            public const string GunshotWound = "gunshot_wound";
            public const string BrokenBone = "broken_bone";
            public const string Dysentery = "dysentery";
            public const string BacterialInfection = "bacterial_infection";
            public const string Sepsis = "sepsis";

            // Phase 2
            public const string RadBurns = "rad_burns";
            public const string ImmuneCollapse = "immune_collapse";
            public const string HeavyMetalPoisoning = "heavy_metal_poisoning";
        }
    }
}
