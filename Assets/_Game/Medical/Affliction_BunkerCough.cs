using System;
using UnityEngine;

namespace AtomicWar._Game.Medical
{
    /// <summary>
    /// Expansion III — Grounded Pathology: Hypersensitivity Pneumonitis.
    /// Breathing recycled air with AirFiltration below 20%. Black mold and
    /// concrete dust. Coughing fits wake the bunker, -10 Warmth, spreads
    /// to adjacent beds. Untreated → Affliction_SporeLung (fatal).
    /// </summary>
    public class Affliction_BunkerCough
    {
        public const string AfflictionId = "affliction_bunker_cough";
        public const string DisplayName = "Bunker Cough";
        public const string Description = "Hypersensitivity Pneumonitis. The recycled air is " +
            "killing them slowly. Black mold. Concrete dust. Every breath is a gamble.";

        public const float HealthDrainPerHour = 0.8f;
        public const float ProgressionHours = 72f; // 3 days to worsen
        public const string ProgressesToId = "affliction_spore_lung";
        public const float WarmthPenalty = -10f;
        public const float SpreadChance = 0.15f; // per hour, to adjacent beds

        // Treatment: sealed heated room + herbal_pills (steroids)
        public const string TreatmentItemId = "herbal_pills";
        public const float TreatmentHours = 6f;

        // Trigger: AirFiltration below this threshold
        public const float AirFiltrationTriggerThreshold = 0.20f;

        public static AfflictionSO CreateDefinition()
        {
            var so = ScriptableObject.CreateInstance<AfflictionSO>();
            so.id = AfflictionId;
            so.displayName = DisplayName;
            so.description = Description;
            so.phase = AfflictionPhase.Phase1;
            so.healthDrainPerHour = HealthDrainPerHour;
            so.baseLethality = 0.9f;
            so.isInfection = true; // Can spread
            so.progressionHours = ProgressionHours;
            so.progressesToId = ProgressesToId;
            so.emergencyHaltItemId = TreatmentItemId;
            so.requiresMedicalBed = false;
            so.fatigueDrainPerHour = 1.5f;
            so.staminaCap = 70f;
            return so;
        }
    }

    /// <summary>
    /// Expansion III — Grounded Pathology: Affliction_SporeLung.
    /// Fatal progression of untreated Bunker Cough. The lungs fill with
    /// mold spores. There is no cure — only time.
    /// </summary>
    public class Affliction_SporeLung
    {
        public const string AfflictionId = "affliction_spore_lung";
        public const string DisplayName = "Spore Lung";
        public const string Description = "The lungs are colonized. Every breath spreads " +
            "the spores deeper. There is no cure. Only time.";

        public const float HealthDrainPerHour = 4f;
        public const float ProgressionHours = 0f; // Terminal — no progression
        public const float Lethality = 2.0f;

        public static AfflictionSO CreateDefinition()
        {
            var so = ScriptableObject.CreateInstance<AfflictionSO>();
            so.id = AfflictionId;
            so.displayName = DisplayName;
            so.description = Description;
            so.phase = AfflictionPhase.Phase2;
            so.healthDrainPerHour = HealthDrainPerHour;
            so.baseLethality = Lethality;
            so.isInfection = true;
            so.progressionHours = 0f;
            so.progressesToId = null;
            so.emergencyHaltItemId = null;
            so.requiresMedicalBed = true;
            so.fatigueDrainPerHour = 3f;
            so.staminaCap = 30f;
            return so;
        }
    }
}
