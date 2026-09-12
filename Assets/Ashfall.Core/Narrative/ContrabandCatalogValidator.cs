// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// PLAN 147 — structural validation for the bunker contraband barter catalog
    /// (narrative/bunker_contraband_barter.json).
    ///
    /// The typed <see cref="ContrabandEntry"/> loader silently drops mechanics keys
    /// it does not bind, which is precisely the "JSON says it works" hazard this
    /// plan closes: an authored catalog can promise a mechanic no code reads. This
    /// validator therefore works over the RAW JSON document, not the typed shape,
    /// and enforces:
    ///   1. required, non-empty string fields and id conventions (contraband_ prefix, unique);
    ///   2. tier in [1,3] and strictly positive market_price_scrip;
    ///   3. every mechanics key appears on the reviewed key allowlist — adding a
    ///      new pseudo-mechanic requires a conscious disposition decision, never a
    ///      silent drop;
    ///   4. all numeric mechanics values are finite (no NaN/Infinity, parse-level
    ///      and value-level) and within their field-class sanity ranges
    ///      (probabilities in [0,1], multipliers positive, non-negative consumables);
    ///   5. tags are a non-empty list of non-empty strings.
    ///
    /// Engine-agnostic by construction: pure string in, report out.
    /// </summary>
    public static class ContrabandCatalogValidator
    {
        /// <summary>
        /// Every mechanics key observed in the 20-entry authored corpus, plus the
        /// typed bindings. This is the reviewed allowlist: an unknown key is an
        /// error, not a silently ignored promise. Update this set ONLY together
        /// with a disposition decision recorded in
        /// docs/plans/CONTRABAND_MECHANICS_AUTHORITY_MATRIX.md.
        /// </summary>
        public static readonly HashSet<string> KnownMechanicsKeys = new HashSet<string>(StringComparer.Ordinal)
        {
            // Typed bindings (ContrabandMechanics)
            "morale_delta", "trade_value_bonus", "tribunal_suspicion_rate",
            "requires_clean_water_liters", "radio_range_boost_km", "emp_shielded",
            "scrip_purchasing_falsification", "blackout_illumination_hours",
            "calorie_surplus_kcal", "hunger_restore_value", "instant_pain_relief_hp",
            "chemical_dependency_risk", "blast_door_override_success_rate",
            "agricultural_yield_multiplier",
            // Authored-but-untyped keys (current disposition: PRESENTATION-ONLY / DESCRIPTIVE)
            "acid_spill_hazard", "air_flow_ease_factor", "alarm_trigger_chance",
            "alcohol_proof_precision", "barter_brawl_chance", "battery_recharge_slots",
            "century_tree_viability", "diesel_theft_liters_per_use",
            "distillation_yield_bonus", "dweller_loyalty_boost",
            "early_storm_warning_minutes", "emergency_power_storage_kwh",
            "faction_influence_rebuilder", "faction_influence_tempest",
            "fatigue_reduction_hours", "fermentation_accelerator",
            "fuel_sabotage_potential", "genetic_integrity_score",
            "infection_risk_percentage", "noise_generation_db",
            "rad_detection_precision_multiplier", "rad_resistance_delta",
            "ration_chit_forgery_success_rate", "recharge_cycle_durability",
            "scrip_gambling_win_rate_boost", "shift_absence_concealment",
            "trade_multiplier", "trauma_suppression_duration_days",
            "vending_machine_jam_chance", "waterproof_boot_grease_uses"
        };

        /// <summary>Probability-class keys: value must lie in [0,1].</summary>
        private static readonly HashSet<string> ProbabilityKeys = new(StringComparer.Ordinal)
        {
            "tribunal_suspicion_rate", "chemical_dependency_risk", "alarm_trigger_chance",
            "barter_brawl_chance", "vending_machine_jam_chance", "infection_risk_percentage",
            "fuel_sabotage_potential", "acid_spill_hazard", "distillation_yield_bonus",
            "alcohol_proof_precision", "genetic_integrity_score",
            "scrip_gambling_win_rate_boost", "ration_chit_forgery_success_rate",
            "blast_door_override_success_rate"
        };

        /// <summary>Multiplier/factor-class keys: value must be finite and positive.</summary>
        private static readonly HashSet<string> MultiplierKeys = new(StringComparer.Ordinal)
        {
            "trade_multiplier", "air_flow_ease_factor", "rad_detection_precision_multiplier",
            "agricultural_yield_multiplier"
        };

        /// <summary>Signed keys where negative values are meaningful flavor (risk deltas, morale).</summary>
        private static readonly HashSet<string> SignedKeys = new(StringComparer.Ordinal)
        {
            "morale_delta", "faction_influence_rebuilder", "faction_influence_tempest",
            "rad_resistance_delta"
        };

        public static ContrabandCatalogValidationReport ValidateJson(string json)
        {
            var report = new ContrabandCatalogValidationReport();
            if (string.IsNullOrWhiteSpace(json))
            {
                report.Errors.Add("catalog_json_empty: catalog payload is null or empty");
                return report;
            }

            JsonDocument doc;
            try
            {
                doc = JsonDocument.Parse(json);
            }
            catch (JsonException ex)
            {
                // NaN/Infinity literals and other malformed numbers are rejected at parse level.
                report.Errors.Add("catalog_json_unparseable: " + ex.Message);
                return report;
            }

            using (doc)
            {
                var root = doc.RootElement;
                if (root.ValueKind != JsonValueKind.Object || !root.TryGetProperty("items", out var itemsEl) || itemsEl.ValueKind != JsonValueKind.Array)
                {
                    report.Errors.Add("catalog_root_invalid: expected root object with an 'items' array");
                    return report;
                }

                var seenIds = new HashSet<string>(StringComparer.Ordinal);
                int index = 0;
                foreach (var entry in itemsEl.EnumerateArray())
                {
                    ValidateEntry(entry, index, seenIds, report);
                    index++;
                }

                report.EntryCount = index;
                if (index == 0)
                    report.Errors.Add("catalog_empty: no entries found");
            }

            return report;
        }

        private static void ValidateEntry(JsonElement entry, int index, HashSet<string> seenIds, ContrabandCatalogValidationReport report)
        {
            string path = $"items[{index}]";

            if (entry.ValueKind != JsonValueKind.Object)
            {
                report.Errors.Add($"{path}: entry must be an object");
                return;
            }

            string id = RequireString(entry, "id", path, report);
            if (id.Length == 0) return;

            if (!id.StartsWith("contraband_", StringComparison.Ordinal))
                report.Errors.Add($"{path}.id: '{id}' must start with the contraband_ prefix");
            if (!seenIds.Add(id))
                report.Errors.Add($"{path}.id: duplicate contraband id '{id}'");

            RequireString(entry, "title", path, report);
            RequireString(entry, "category", path, report);
            RequireString(entry, "risk_profile", path, report);
            RequireString(entry, "hidden_stash_location", path, report);
            RequireString(entry, "prose", path, report);

            if (entry.TryGetProperty("contraband_tier", out var tierEl))
            {
                if (tierEl.ValueKind != JsonValueKind.Number || !tierEl.TryGetInt32(out int tier) || tier < 1 || tier > 3)
                    report.Errors.Add($"{path}.contraband_tier: tier must be an integer in [1,3]");
            }
            else
            {
                report.Errors.Add($"{path}: missing contraband_tier");
            }

            if (entry.TryGetProperty("market_price_scrip", out var priceEl))
            {
                if (priceEl.ValueKind != JsonValueKind.Number || !priceEl.TryGetInt32(out int price) || price < 0)
                    report.Errors.Add($"{path}.market_price_scrip: price must be a non-negative integer");
            }
            else
            {
                report.Errors.Add($"{path}: missing market_price_scrip");
            }

            if (entry.TryGetProperty("tags", out var tagsEl))
            {
                if (tagsEl.ValueKind != JsonValueKind.Array || tagsEl.GetArrayLength() == 0)
                {
                    report.Errors.Add($"{path}.tags: must be a non-empty array");
                }
                else
                {
                    foreach (var tag in tagsEl.EnumerateArray())
                    {
                        if (tag.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(tag.GetString()))
                        {
                            report.Errors.Add($"{path}.tags: every tag must be a non-empty string");
                            break;
                        }
                    }
                }
            }
            else
            {
                report.Errors.Add($"{path}: missing tags");
            }

            if (entry.TryGetProperty("mechanics", out var mechEl))
            {
                if (mechEl.ValueKind == JsonValueKind.Object)
                    ValidateMechanics(mechEl, path, report);
                else
                    report.Errors.Add($"{path}.mechanics: must be an object when present");
            }
        }

        private static void ValidateMechanics(JsonElement mech, string path, ContrabandCatalogValidationReport report)
        {
            foreach (var prop in mech.EnumerateObject())
            {
                string key = prop.Name;
                string keyPath = $"{path}.mechanics.{key}";

                if (!KnownMechanicsKeys.Contains(key))
                {
                    report.Errors.Add(
                        $"{keyPath}: unknown mechanics key — not on the reviewed allowlist; " +
                        "a new pseudo-mechanic requires a recorded disposition in CONTRABAND_MECHANICS_AUTHORITY_MATRIX.md");
                    continue;
                }

                var value = prop.Value;
                if (value.ValueKind == JsonValueKind.True || value.ValueKind == JsonValueKind.False)
                    continue; // booleans carry no range contract

                if (value.ValueKind != JsonValueKind.Number)
                {
                    report.Errors.Add($"{keyPath}: expected a number or boolean");
                    continue;
                }

                if (!value.TryGetDouble(out double d) || double.IsNaN(d) || double.IsInfinity(d))
                {
                    report.Errors.Add($"{keyPath}: value must be a finite number (NaN/Infinity rejected)");
                    continue;
                }

                if (ProbabilityKeys.Contains(key) && (d < 0d || d > 1d))
                {
                    report.Errors.Add($"{keyPath}: probability-class value {d} outside [0,1]");
                }
                else if (MultiplierKeys.Contains(key) && d <= 0d)
                {
                    report.Errors.Add($"{keyPath}: multiplier-class value {d} must be positive");
                }
                else if (!ProbabilityKeys.Contains(key) && !MultiplierKeys.Contains(key))
                {
                    // Quantity/int-class keys: non-negative unless explicitly signed.
                    if (!SignedKeys.Contains(key) && d < 0d)
                        report.Errors.Add($"{keyPath}: value {d} must be non-negative");
                    else if (SignedKeys.Contains(key) && key == "rad_resistance_delta" && (d < -1d || d > 1d))
                        report.Errors.Add($"{keyPath}: rad_resistance_delta {d} outside [-1,1]");
                }
            }
        }

        private static string RequireString(JsonElement entry, string property, string path, ContrabandCatalogValidationReport report)
        {
            if (!entry.TryGetProperty(property, out var el))
            {
                report.Errors.Add($"{path}: missing '{property}'");
                return string.Empty;
            }
            if (el.ValueKind != JsonValueKind.String || string.IsNullOrWhiteSpace(el.GetString()))
            {
                report.Errors.Add($"{path}.{property}: must be a non-empty string");
                return string.Empty;
            }
            return el.GetString()!;
        }
    }

    public sealed class ContrabandCatalogValidationReport
    {
        public List<string> Errors { get; } = new List<string>();
        public List<string> Warnings { get; } = new List<string>();
        public int EntryCount { get; internal set; }

        public bool IsValid => Errors.Count == 0;
    }
}
