using System;
using UnityEngine;

namespace AtomicWar._Game.Medical
{
    /// <summary>
    /// Expansion III — Grounded Pathology: Heavy Metal Psychosis.
    /// Scavenging car_battery or solar_cell without protective_rubber_gloves.
    /// Acid and lead absorb through micro-cuts. Paranoia, refusal to sleep,
    /// accusations of theft. Triggers Dynamic_Scapegoat events.
    /// Treatment: chelation using anti_rad (strips calcium → Affliction_BrittleBones)
    /// or milk (if hydroponics has it).
    /// </summary>
    public class Affliction_LeadMadness
    {
        public const string AfflictionId = "affliction_lead_madness";
        public const string DisplayName = "Lead Madness";
        public const string Description = "Heavy Metal Psychosis. The lead has crossed the " +
            "blood-brain barrier. The survivor sees conspiracies in every shadow.";

        public const float HealthDrainPerHour = 0.5f;
        public const float ProgressionHours = 96f; // 4 days
        public const string ProgressesToId = AfflictionSO.Ids.HeavyMetalPoisoning;
        public const float MoraleDrainPerHour = 2f;
        public const float ParanoiaThreshold = 50f; // Hours active before paranoia kicks in

        // ── Chelation treatment ───────────────────────────────────────
        public const string ChelationItem_AntiRad = "anti_rad";
        public const string ChelationItem_Milk = "milk";
        public const float ChelationHours = 8f;

        // Side effect: anti-rad chelation strips calcium
        public const string SideEffectAfflictionId = AfflictionSO.Ids.BrittleBones != null
            ? "affliction_brittle_bones" : null;

        // ── Trigger conditions ────────────────────────────────────────
        public static readonly string[] TriggerItems = { "car_battery", "solar_cell" };
        public const string RequiredProtection = "protective_rubber_gloves";

        public static AfflictionSO CreateDefinition()
        {
            var so = ScriptableObject.CreateInstance<AfflictionSO>();
            so.id = AfflictionId;
            so.displayName = DisplayName;
            so.description = Description;
            so.phase = AfflictionPhase.Phase1;
            so.healthDrainPerHour = HealthDrainPerHour;
            so.baseLethality = 0.7f;
            so.isInfection = false;
            so.progressionHours = ProgressionHours;
            so.progressesToId = ProgressesToId;
            so.emergencyHaltItemId = ChelationItem_AntiRad;
            so.requiresMedicalBed = false;
            so.fatigueDrainPerHour = 1f;
            so.staminaCap = -1f;
            return so;
        }

        /// <summary>True if the survivor is paranoid (accuses others of stealing).</summary>
        public static bool IsParanoid(float hoursActive)
        {
            return hoursActive >= ParanoiaThreshold;
        }
    }

    /// <summary>
    /// Expansion III — Grounded Pathology: Affliction_BrittleBones.
    /// Side effect of anti-rad chelation for lead poisoning. The calcium
    /// stripped from the bones makes fractures more likely and slower to heal.
    /// </summary>
    public class Affliction_BrittleBones
    {
        public const string AfflictionId = "affliction_brittle_bones";
        public const string DisplayName = "Brittle Bones";
        public const string Description = "The chelation worked — the lead is gone. But so " +
            "is the calcium. Every step is a gamble with gravity.";

        public const float HealthDrainPerHour = 0.2f;
        public const float ProgressionHours = 0f; // Chronic — no progression
        public const float FractureChanceMultiplier = 3.0f; // 3× more likely to break bones

        public static AfflictionSO CreateDefinition()
        {
            var so = ScriptableObject.CreateInstance<AfflictionSO>();
            so.id = AfflictionId;
            so.displayName = DisplayName;
            so.description = Description;
            so.phase = AfflictionPhase.Phase1;
            so.healthDrainPerHour = HealthDrainPerHour;
            so.baseLethality = 0.3f;
            so.isInfection = false;
            so.progressionHours = 0f;
            so.progressesToId = null;
            so.emergencyHaltItemId = null;
            so.requiresMedicalBed = false;
            so.fatigueDrainPerHour = 0.5f;
            so.staminaCap = 70f;
            return so;
        }
    }
}
