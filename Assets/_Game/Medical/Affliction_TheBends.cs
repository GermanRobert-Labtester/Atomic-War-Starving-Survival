using System;
using UnityEngine;

namespace AtomicWar._Game.Medical
{
    /// <summary>
    /// Expansion III — Grounded Pathology: Decompression Sickness.
    /// Digging too deep into StratigraphySystem Layer 3 without equalizing
    /// pressure in the airlock. Joint pain, paralysis, bubbles in the blood.
    /// Treatment: improvised hyperbaric chamber (ShelterModule_Autoclave).
    /// Requires massive fuel and oxygen.
    /// </summary>
    public class Affliction_TheBends
    {
        public const string AfflictionId = "affliction_the_bends";
        public const string DisplayName = "The Bends";
        public const string Description = "Decompression Sickness. Nitrogen bubbles in the " +
            "blood. The joints scream. The fingers go numb. The body was not made for this depth.";

        public const float HealthDrainPerHour = 3f;
        public const float ProgressionHours = 12f;
        public const string ProgressesToId = AfflictionSO.Ids.Coma;
        public const float JointPainFatigueDrain = 4f;

        // ── Treatment: improvised hyperbaric chamber ──────────────────
        public const string TreatmentModuleId = "shelter_module_autoclave";
        public const float TreatmentFuelCost = 20f;  // liters of fuel
        public const float TreatmentOxygenCost = 10f; // oxygen units
        public const float TreatmentHours = 8f;

        // ── Trigger: digging below Layer 2 without pressure equalization
        public const float DepthTriggerThreshold = 20f; // meters (Layer 3)

        public static AfflictionSO CreateDefinition()
        {
            var so = ScriptableObject.CreateInstance<AfflictionSO>();
            so.id = AfflictionId;
            so.displayName = DisplayName;
            so.description = Description;
            so.phase = AfflictionPhase.Phase2;
            so.healthDrainPerHour = HealthDrainPerHour;
            so.baseLethality = 1.2f;
            so.isInfection = false;
            so.progressionHours = ProgressionHours;
            so.progressesToId = ProgressesToId;
            so.emergencyHaltItemId = null;
            so.requiresMedicalBed = true;
            so.fatigueDrainPerHour = JointPainFatigueDrain;
            so.staminaCap = 40f;
            return so;
        }
    }

    /// <summary>
    /// Expansion III — Grounded Pathology: Radiation-Induced Sarcoma.
    /// Cumulative dose > 2,500 mSv. Immune system collapse. Purple lesions
    /// on the skin. -5 Health per day. High infection risk. Terminal.
    /// Only morphine_ampoule to ease the passing.
    /// </summary>
    public class Affliction_Kaposi
    {
        public const string AfflictionId = "affliction_kaposi";
        public const string DisplayName = "Kaposi's Sarcoma";
        public const string Description = "Radiation-Induced Sarcoma. Purple lesions spread " +
            "across the skin like a map of every hot zone they ever walked through.";

        public const float HealthDrainPerHour = 0.21f; // ~5/day
        public const float ProgressionHours = 0f; // Terminal — no progression
        public const float Lethality = 1.5f;
        public const float CumulativeDoseTrigger = 2500f; // mSv lifetime

        // Palliative only
        public const string PalliativeItemId = "morphine_ampoule";

        public static AfflictionSO CreateDefinition()
        {
            var so = ScriptableObject.CreateInstance<AfflictionSO>();
            so.id = AfflictionId;
            so.displayName = DisplayName;
            so.description = Description;
            so.phase = AfflictionPhase.Phase2;
            so.healthDrainPerHour = HealthDrainPerHour;
            so.baseLethality = Lethality;
            so.isInfection = false;
            so.progressionHours = 0f;
            so.progressesToId = null;
            so.emergencyHaltItemId = PalliativeItemId;
            so.requiresMedicalBed = true;
            so.fatigueDrainPerHour = 2f;
            so.staminaCap = 30f;
            return so;
        }

        /// <summary>Check if a survivor's lifetime dose triggers Kaposi's.</summary>
        public static bool ShouldTrigger(float lifetimeDose)
        {
            return lifetimeDose >= CumulativeDoseTrigger;
        }
    }
}
