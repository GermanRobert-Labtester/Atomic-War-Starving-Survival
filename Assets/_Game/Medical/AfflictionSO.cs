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

        // -----------------------------------------------------------------
        // Prompt #47 — biological trade side effects. Bleeding a survivor
        // for a faction convoy drops more than health: the body that just
        // lost a pint of blood cannot carry the same workload. These two
        // fields let a single AfflictionSO model the full consequence
        // profile (Health + Fatigue + Stamina cap) without the EventRunner
        // needing a separate per-effect side channel.
        // -----------------------------------------------------------------

        [Tooltip("Extra fatigue lost per game-hour while this affliction is " +
                 "active. 0 = no fatigue drain. Used by blood_loss and other " +
                 "biological-trade consequences (Prompt #47).")]
        public float fatigueDrainPerHour = 0f;

        [Tooltip("Hard cap on Needs.Fatigue (0..100) while this affliction is " +
                 "active. -1 = no cap. The donor cannot recover above this " +
                 "value until the affliction is cured. blood_loss sets this " +
                 "to 30 — the survivor is functional but never fully rested " +
                 "for the duration of the cure window.")]
        public float staminaCap = -1f;

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
            /// <summary>Prompt #222 — Juggernaut is immune to lacerations.</summary>
            public const string Laceration = "laceration";
            public const string Dysentery = "dysentery";
            public const string BacterialInfection = "bacterial_infection";
            public const string Sepsis = "sepsis";

            // Phase 2
            public const string RadBurns = "rad_burns";
            public const string ImmuneCollapse = "immune_collapse";
            public const string HeavyMetalPoisoning = "heavy_metal_poisoning";

            // Phase 3 / Prompt #47 — biological trade consequences.
            // BloodLoss is inflicted when a faction trades a pint of blood
            // for water/iodine; the cure window is ~7 days of high food
            // and water. Carries infection risk from the dirty needle.
            public const string BloodLoss = "blood_loss";

            // Internal Horror — triage / pantry
            /// <summary>Bedridden: cannot self-feed; requires Caregive or dies of neglect.</summary>
            public const string Coma = "coma";
            /// <summary>Foodborne toxin; paralyzes respiratory drive (Phase 1).</summary>
            public const string Botulism = "botulism";

            /// <summary>Prompt #13 — sabotaged medical cache (rat poison in iodine foil).</summary>
            public const string PoisonIngestion = "poison_ingestion";

            // Prompt #55 — incompatible blood transfusion.
            public const string AnaphylacticShock = "anaphylactic_shock";

            // Prompt #56 — surgical aftermath.
            public const string PhantomPain = "phantom_pain";

            // Prompt #57 — dietary deficiency.
            public const string Toothache = "toothache";
            public const string Scurvy = "scurvy";

            // Prompt #60 — radiation mutagenesis stages.
            public const string HairLoss = "hair_loss";
            public const string CellularBreakdown = "cellular_breakdown";
        }
    }
}
