using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Expansion III — Dead Hand trait constants and mechanics.
    /// Traits are checked via Survivor.HasTrait(string).
    ///
    /// trait_tinnitus:         Permanent ringing. Immune to audio-triggered
    ///                         hallucinations, but -30% scavenging speed in
    ///                         Biome_Rust (cannot hear sensors winding up).
    /// trait_faraday_paranoia: Refuses to sleep unless room has Faraday mesh.
    ///                         Mesh degradation → Insomnia + PanicAttack.
    /// trait_uxo_instinct:     Subtle UI highlight of safe paths in UXO nodes.
    ///                         Reduces probe time by 40%.
    /// trait_magnetism_phobia: Refuses expeditions to magnetic anomaly nodes.
    /// </summary>
    public static class DeadHandTraits
    {
        public const string Tinnitus = "trait_tinnitus";
        public const string FaradayParanoia = "trait_faraday_paranoia";
        public const string UxoInstinct = "trait_uxo_instinct";
        public const string MagnetismPhobia = "trait_magnetism_phobia";

        /// <summary>Locations that trigger magnetism phobia refusal.</summary>
        public static readonly HashSet<string> MagneticAnomalyLocations = new HashSet<string>
        {
            "location_magnetic_anomaly_crater",
            "location_radar_array_spire"
        };

        /// <summary>Scavenging speed multiplier for tinnitus survivors in Biome_Rust.</summary>
        public const float TinnitusScavengeSpeedMult = 0.7f;

        /// <summary>Check if a location triggers magnetism phobia.</summary>
        public static bool IsMagneticAnomalyLocation(string locationId)
        {
            return !string.IsNullOrEmpty(locationId) && MagneticAnomalyLocations.Contains(locationId);
        }
    }
}
