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

        [Tooltip("Ceiling on available stamina (0..100, 100 = fresh) while this " +
                 "affliction is active. -1 = no cap. Applied to Needs.Fatigue — " +
                 "which runs the opposite way — as a floor of 100 - staminaCap, so " +
                 "a LOWER value is a HARSHER affliction. blood_loss sets this to 30 " +
                 "(fatigue never below 70): functional, but never fully rested for " +
                 "the duration of the cure window.")]
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

            // ── Expansion III: The Ghosts in the Concrete ─────────────
            /// <summary>Pterygium / Rad Cataracts. Ash + UV + ionizing radiation.</summary>
            public const string AshBlindness = "affliction_ash_blindness";
            /// <summary>Hypersensitivity Pneumonitis. Recycled air, black mold, concrete dust.</summary>
            public const string BunkerCough = "affliction_bunker_cough";
            /// <summary>Fatal mold colonization of lungs. Terminal progression of Bunker Cough.</summary>
            public const string SporeLung = "affliction_spore_lung";
            /// <summary>Heavy Metal Psychosis. Lead from car batteries/solar cells.</summary>
            public const string LeadMadness = "affliction_lead_madness";
            /// <summary>Calcium loss from chelation treatment for lead poisoning.</summary>
            public const string BrittleBones = "affliction_brittle_bones";
            /// <summary>Decompression Sickness. Digging too deep without pressure equalization.</summary>
            public const string TheBends = "affliction_the_bends";
            /// <summary>Radiation-Induced Sarcoma. Cumulative dose > 2,500 mSv. Terminal.</summary>
            public const string Kaposi = "affliction_kaposi";
            /// <summary>Asbestos exposure from 1962 bunker layer excavation.</summary>
            public const string Mesothelioma = "affliction_mesothelioma";
            /// <summary>Severe food poisoning from 40-year-old MREs.</summary>
            public const string FoodPoisoningSevere = "affliction_food_poisoning_severe";

            // ── Expansion IV: The Logistics of Ruin ───────────────────
            /// <summary>Ash Delirium. Whiteout exposure with low morale. Wandering into the grey.</summary>
            public const string AshDelirium = "affliction_ash_delirium";
            /// <summary>Ash Exhaustion. Sinking in ash without snowshoes. 300% fatigue burn.</summary>
            public const string AshExhaustion = "affliction_ash_exhaustion";
            /// <summary>Corpse Thief Shame. Wearing clothes stripped from a named corpse.</summary>
            public const string CorpseThiefShame = "affliction_corpse_thief_shame";

            // ── Expansion VI: The Architecture of Paranoia ────────────
            /// <summary>Claustrophobia. Assigned to unlit bunker rooms.</summary>
            public const string Claustrophobia = "affliction_claustrophobia";
            /// <summary>Stigmatized. Targeted by gossip and scapegoating.</summary>
            public const string Stigmatized = "affliction_stigmatized";
            /// <summary>Paranoia. From phantom knocks, contraband, and sleep deprivation.</summary>
            public const string Paranoia = "affliction_paranoia";

            // ── Expansion VII: The Marrow & The Mythology ─────────────
            /// <summary>Psychosomatic Nausea. Autopsy room adjacent to kitchen.</summary>
            public const string PsychosomaticNausea = "affliction_psychosomatic_nausea";
            /// <summary>Chronic Fatigue. Generator hum adjacent to bunkhouse.</summary>
            public const string ChronicFatigue = "affliction_chronic_fatigue";
            /// <summary>Night Terrors. Hearing screams through ventilation.</summary>
            public const string NightTerrors = "affliction_night_terrors";
            /// <summary>Phantom Limb. Feeling missing appendage after amputation.</summary>
            public const string PhantomLimb = "affliction_phantom_limb";
            /// <summary>Nerve Damage. From crude prosthetics or toxic anesthetic.</summary>
            public const string NerveDamage = "affliction_nerve_damage";
            /// <summary>Dysentery Outbreak. BioLatrine adjacent to water supply.</summary>
            public const string DysenteryOutbreak = "affliction_dysentery_outbreak";

            // ── Expansion II Addendum: The Black Aquifer & Myco-Necrosis ─
            /// <summary>Chemical Toxicity. Liver failure and blindness from Black Aquifer sludge in drinking water.</summary>
            public const string ChemicalToxicity = "affliction_chemical_toxicity";
            /// <summary>Myco-Hallucinations. High spore density causes Utility AI breakdown; survivors attack shadows.</summary>
            public const string MycoHallucinations = "affliction_myco_hallucinations";

            // ── Expansion III: The Dead Hand & The Oxide Wastes ──────
            /// <summary>Traumatic Amputation. Failed UXO probe detonation. Permanent limb loss.</summary>
            public const string TraumaticAmputation = "affliction_traumatic_amputation";
            /// <summary>EMP Phantom Blip. Corrupted electronics inject false data into perception.</summary>
            public const string EMPPhantomBlip = "affliction_emp_phantom_blip";
            /// <summary>Logic Gate Failure. Device corruption causes lethal automated errors.</summary>
            public const string LogicGateFailure = "affliction_logic_gate_failure";
        }
    }
}
