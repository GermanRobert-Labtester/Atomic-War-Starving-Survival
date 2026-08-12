using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Expansion II — Black Aquifer trait constants and mechanics.
    /// Traits are checked via Survivor.HasTrait(string).
    ///
    /// trait_thalassophobia:     -40% scavenging speed + 20% fatigue drain
    ///                           in flooded/deep-water locations.
    /// trait_spore_carrier:      Asymptomatic. No health penalty, but
    ///                           sleeping room accumulates +5% spore/day.
    /// trait_dark_acclimated:    No morale penalty for unlit rooms.
    ///                           50% less battery for flashlights.
    /// trait_claustrophobia:     Sub-Level 3+ or cave-in → MentalBreak_PanicAttack.
    ///                           Attempts to open outer hatch.
    /// trait_rot_immunity:       Immune to SporeLung and FoodPoisoning
    ///                           from mold. -10% morale for roommates.
    /// </summary>
    public static class BlackAquiferTraits
    {
        public const string Thalassophobia = "trait_thalassophobia";
        public const string SporeCarrier = "trait_spore_carrier";
        public const string DarkAcclimated = "trait_dark_acclimated";
        public const string Claustrophobia = "trait_claustrophobia";
        public const string RotImmunity = "trait_rot_immunity";

        /// <summary>Locations that trigger thalassophobia penalties.</summary>
        public static readonly HashSet<string> FloodedLocations = new HashSet<string>
        {
            "location_flooded_subway_depot",
            "location_submerged_data_center",
            "location_the_sump_cathedral"
        };

        /// <summary>Scavenging speed multiplier for thalassophobic survivors in flooded locations.</summary>
        public const float ThalassophobiaScavengeSpeedMult = 0.6f;

        /// <summary>Extra fatigue drain multiplier for thalassophobic survivors.</summary>
        public const float ThalassophobiaFatigueMult = 1.2f;

        /// <summary>Flashlight battery draw multiplier for dark-acclimated survivors.</summary>
        public const float DarkAcclimatedBatteryMult = 0.5f;

        /// <summary>Morale penalty per hour for roommates of rot-immune survivors.</summary>
        public const string RotImmunityMoralePenaltyReason = "rot_immunity_roommate_smell";

        /// <summary>Check if a location triggers thalassophobia.</summary>
        public static bool IsFloodedLocation(string locationId)
        {
            return !string.IsNullOrEmpty(locationId) && FloodedLocations.Contains(locationId);
        }
    }
}
