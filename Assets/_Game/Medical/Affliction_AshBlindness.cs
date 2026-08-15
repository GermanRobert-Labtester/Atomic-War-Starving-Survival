using System;
using UnityEngine;

namespace AtomicWar._Game.Medical
{
    /// <summary>
    /// Expansion III — Grounded Pathology: Pterygium / Rad Cataracts.
    /// Prolonged surface exposure without snow_goggles. UV radiation bouncing
    /// off ash combined with ionizing radiation. Vision blurs, UI gets a grey
    /// vignette, scavenging accuracy drops 40%. Surgery has 30% chance of
    /// permanent Affliction_RadiationBlindness.
    /// </summary>
    public class Affliction_AshBlindness
    {
        public const string AfflictionId = "affliction_ash_blindness";
        public const string DisplayName = "Ash Blindness";
        public const string Description = "Pterygium / Rad Cataracts. The ash and the radiation " +
            "have eaten the survivor's vision. Everything is grey fog and pain.";

        public const float HealthDrainPerHour = 0.3f;
        public const float ProgressionHours = 120f; // 5 days to worsen
        public const string ProgressesToId = AfflictionSO.Ids.ImmuneCollapse;
        public const float ScavengingAccuracyPenalty = 0.40f; // -40% scavenging accuracy
        public const float SurgeryPermanentBlindnessChance = 0.30f;

        // ── Surgery requirements ──────────────────────────────────────
        public const string SurgeryItemId_Scalpel = "scalpel";
        public const string SurgeryItemId_Antiseptic = "antiseptic_1l_of_1l";
        public const float SurgeryHours = 3f;

        /// <summary>
        /// Create the AfflictionSO for ash blindness.
        /// </summary>
        public static AfflictionSO CreateDefinition()
        {
            var so = ScriptableObject.CreateInstance<AfflictionSO>();
            so.id = AfflictionId;
            so.displayName = DisplayName;
            so.description = Description;
            so.phase = AfflictionPhase.Phase2;
            so.healthDrainPerHour = HealthDrainPerHour;
            so.baseLethality = 0.8f;
            so.isInfection = false;
            so.progressionHours = ProgressionHours;
            so.progressesToId = ProgressesToId;
            so.emergencyHaltItemId = null;
            so.requiresMedicalBed = true; // Surgery required
            so.fatigueDrainPerHour = 0.5f;
            so.staminaCap = 60f; // Limited vision = limited stamina
            return so;
        }

        /// <summary>
        /// Attempt the surgical cure. 30% chance of permanent blindness.
        /// </summary>
        public static SurgeryResult AttemptSurgery(System.Random rng)
        {
            rng ??= new System.Random();
            bool permanentBlindness = rng.NextDouble() < SurgeryPermanentBlindnessChance;

            return new SurgeryResult
            {
                Success = !permanentBlindness,
                CausedPermanentBlindness = permanentBlindness,
                ResultAfflictionId = permanentBlindness
                    ? AfflictionSO.Ids.ImmuneCollapse // Closest existing: permanent damage
                    : null
            };
        }
    }

    [Serializable]
    public struct SurgeryResult
    {
        public bool Success;
        public bool CausedPermanentBlindness;
        public string ResultAfflictionId;
    }
}
