using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Economy
{
    /// <summary>
    /// "The Ash Gets Deeper" content batch (Prompts #326–#330) — Hardcore
    /// Survival Mode economy tuning. Pure-data static helper. The host
    /// (DynamicEconomySystem) reads the multipliers + price-shock list at
    /// boot and applies them to the existing trade pipeline. No new
    /// economy system is created; this is a tuning overlay.
    ///
    /// Wire-up is opt-in. The default <see cref="DynamicEconomySystem"/>
    /// is unchanged unless a host reads <see cref="ScarcityMultipliers"/>
    /// or registers <see cref="PriceShockEvents"/>.
    /// </summary>
    public static class HardcoreEconomyTuning
    {
        public enum ScarcityTier { Critical, High, Moderate, Low }
        public enum PriceShock { PlumePassing, ConvoyAmbush, FactionWar, WinterDeepens }

        // ── Scarcity-tier multipliers (Section V) ───────────────────────
        public readonly struct ScarcityEntry
        {
            public readonly ScarcityTier Tier;
            public readonly float Multiplier;
            public readonly string DayRangeLabel;
            public readonly string AffectedItemIds; // comma-separated
            public readonly string Rationale;

            public ScarcityEntry(ScarcityTier tier, float mult, string dayRange, string items, string rationale)
            { Tier = tier; Multiplier = mult; DayRangeLabel = dayRange; AffectedItemIds = items; Rationale = rationale; }
        }

        public static readonly IReadOnlyList<ScarcityEntry> ScarcityMultipliers = new List<ScarcityEntry>
        {
            new ScarcityEntry(
                ScarcityTier.Critical, 2.5f, "Days 1-15",
                "clean_water, iodine_pills, anti_rad, air_filter",
                "Immediate survival. Everyone needs them. Nobody has enough."),
            new ScarcityEntry(
                ScarcityTier.High, 2.0f, "Days 15-40",
                "antibiotics, medical_kit, fuel, water_filter",
                "Infections set in. Filters clog. Fuel runs low."),
            new ScarcityEntry(
                ScarcityTier.Moderate, 1.5f, "Days 40-80",
                "bandage, canned_food, cloth, scrap_metal",
                "Sustainable but finite. The scavenging radius is shrinking."),
            new ScarcityEntry(
                ScarcityTier.Low, 0.5f, "Days 80+",
                "jewelry, currency, book, playing_cards_worn",
                "Nobody cares about gold when they're starving. Comfort items lose value."),
        };

        // ── Faction trade preferences (Section V) ────────────────────────
        public readonly struct FactionTradePreference
        {
            public readonly string FactionId;
            public readonly string BuysAtPremium; // comma-separated item id prefixes
            public readonly string Refuses;        // comma-separated item id prefixes
            public readonly string TradeCurrency;
            public FactionTradePreference(string faction, string buys, string refuses, string currency)
            { FactionId = faction; BuysAtPremium = buys; Refuses = refuses; TradeCurrency = currency; }
        }

        public static readonly IReadOnlyList<FactionTradePreference> FactionTradePreferences = new List<FactionTradePreference>
        {
            new FactionTradePreference(
                "central_garrison_remnants",
                "ammo_*, body_armour_military, fuel, mre_military",
                "jewelry, book, cigarette",
                "Fuel, ammunition, obedience"),
            new FactionTradePreference(
                "upland_militia",
                "seed_*, fertilizer, antibiotics, water_filter",
                "weapon_hmg, grenade_military",
                "Grain, livestock, local knowledge"),
            new FactionTradePreference(
                "cultists_of_the_glow",
                "iodine_pills, candle_tallow, incense",
                "anti_rad",
                "Devotion, recruits, territory access"),
            new FactionTradePreference(
                "scavenger_warlords",
                "alcohol, cigarettes, weapon_*, morphine_ampoule",
                "medical_kit",
                "Protection, information, captives"),
            new FactionTradePreference(
                "safe_haven_rebuilders",
                "water_purification_tablet, seed_*, book, circuit_board",
                "weapon_*",
                "Clean water, labor, shelter access"),
        };

        // ── Dynamic price-shock events (Section V) ───────────────────────
        public readonly struct PriceShockRule
        {
            public readonly PriceShock Kind;
            public readonly float Multiplier;
            public readonly int DurationDays;
            public readonly string AffectedItemIds; // comma-separated; "*" = all
            public readonly string Trigger;         // free-form host-side condition

            public PriceShockRule(PriceShock kind, float mult, int days, string items, string trigger)
            { Kind = kind; Multiplier = mult; DurationDays = days; AffectedItemIds = items; Trigger = trigger; }
        }

        public static readonly IReadOnlyList<PriceShockRule> PriceShockEvents = new List<PriceShockRule>
        {
            new PriceShockRule(PriceShock.PlumePassing, 1.8f, 3, "*", "fallout storm crosses a trade route"),
            new PriceShockRule(PriceShock.ConvoyAmbush, 2.0f, 7, "*", "trade convoy destroyed (specific items from manifest)"),
            new PriceShockRule(PriceShock.FactionWar,   3.0f, 14, "ammo_*, medical_*, food",
                "two factions actively fighting (food dumps, weapons/meds spike)"),
            new PriceShockRule(PriceShock.WinterDeepens, 2.2f, 30, "fuel, candle_tallow, hand_warmer_packet",
                "after Day 60 (water drops, fuel/heating spikes)"),
        };

        // ── Helper API (host-side) ───────────────────────────────────────
        /// <summary>Returns the scarcity multiplier for the given day, or 1.0 if none applies.</summary>
        public static float GetScarcityMultiplierForDay(int currentDay, string itemId = null)
        {
            if (string.IsNullOrEmpty(itemId)) return 1.0f;
            // Quick path: check each entry for a day-range match.
            if (currentDay >= 1 && currentDay <= 15) return GetItemMatch(ScarcityTier.Critical, 2.5f, itemId);
            if (currentDay > 15 && currentDay <= 40) return GetItemMatch(ScarcityTier.High, 2.0f, itemId);
            if (currentDay > 40 && currentDay <= 80) return GetItemMatch(ScarcityTier.Moderate, 1.5f, itemId);
            if (currentDay > 80) return GetItemMatch(ScarcityTier.Low, 0.5f, itemId);
            return 1.0f;
        }

        private static float GetItemMatch(ScarcityTier tier, float mult, string itemId)
        {
            // For now, we trust the tier's multiplier for items in its bucket.
            // A finer-grained item-id match lives in the host's existing
            // DynamicEconomySystem.GetEffectiveValue; the catalog exposes
            // the multiplier as a hint the host may apply on top.
            for (int i = 0; i < ScarcityMultipliers.Count; i++)
            {
                if (ScarcityMultipliers[i].Tier != tier) continue;
                if (ScarcityMultipliers[i].AffectedItemIds.IndexOf(itemId, System.StringComparison.OrdinalIgnoreCase) >= 0)
                    return mult;
            }
            return 1.0f;
        }
    }
}
