// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// C2[6] 23C — authored cascade rule. A rule declares the facts it requires,
    /// its warning window, the effect tags it predicts, and its recovery off-ramps.
    /// Rules are facts-only: the coordinator never writes subsystem state.
    /// </summary>
    [Serializable]
    public sealed class CascadeRule
    {
        [JsonPropertyName("id")] public string id { get; set; } = string.Empty;
        [JsonPropertyName("display_name")] public string display_name { get; set; } = string.Empty;
        [JsonPropertyName("requires_all")] public List<string> requires_all { get; set; } = new List<string>();
        [JsonPropertyName("warning_hours")] public float warning_hours { get; set; } = 24f;
        [JsonPropertyName("effect_tags")] public List<string> effect_tags { get; set; } = new List<string>();
        [JsonPropertyName("off_ramps")] public List<string> off_ramps { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class CascadeRuleCatalog
    {
        [JsonPropertyName("schema_version")] public int schema_version { get; set; }
        [JsonPropertyName("minimum_warning_hours")] public float minimum_warning_hours { get; set; } = 6f;
        [JsonPropertyName("rules")] public List<CascadeRule> rules { get; set; } = new List<CascadeRule>();
    }

    /// <summary>
    /// C2[6] 23C — loader + structural validation for <c>cascade_rules.json</c>.
    /// Validation is the integrity gate the plan requires: no rule may be
    /// reachable without at least one declared off-ramp, no severe rule may
    /// fall below the authored minimum warning floor, and every condition /
    /// off-ramp key must be a known vocabulary term.
    /// </summary>
    public static class CascadeRuleCatalogLoader
    {
        public const int SupportedSchemaVersion = 1;
        public const string FileName = "cascade_rules.json";

        /// <summary>
        /// Load <c>cascade_rules.json</c>. Returns null when absent or malformed so
        /// the host can degrade to "no cascade layer" without failing boot; a loaded
        /// catalog is always structurally validated before use.
        /// </summary>
        public static CascadeRuleCatalog? TryLoad(string dataDir, IFileIO fileIo)
        {
            if (string.IsNullOrWhiteSpace(dataDir)) return null;
            string path = Path.Combine(dataDir, FileName);
            if (!fileIo.FileExists(path)) return null;
            try
            {
                string json = fileIo.ReadAllText(path);
                return JsonSerializer.Deserialize<CascadeRuleCatalog>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (Exception ex)
            {
                // Loader diagnostic context (catch-policy gate): a cascade
                // catalog that fails to parse must say why — legacy contract
                // preserved, the caller still receives null.
                Console.Error.WriteLine($"CascadeRuleCatalogLoader: {path} failed to load: {ex.Message}");
                return null;
            }
        }

        /// <summary>Conditions a rule may require; each maps to a fact field.</summary>
        public static readonly HashSet<string> KnownConditions = new HashSet<string>(StringComparer.Ordinal)
        {
            "power_deficit", "critical_deficit", "pumping_unserved", "sump_rising",
            "heating_unserved", "filtration_unserved", "fallout_storm", "outbreak_active",
            "fire_active", "hatch_unsealed", "cold_storage_unpowered", "darkness"
        };

        /// <summary>Recovery paths a rule may declare.</summary>
        public static readonly HashSet<string> KnownOffRamps = new HashSet<string>(StringComparer.Ordinal)
        {
            "restore_power", "manual_pump", "shed_other_load", "seal_hatch",
            "treat_outbreak", "decontaminate", "service_heating", "clear_fire", "extinguish_fire"
        };

        /// <summary>
        /// Validates a catalog. Returns false with a specific, named error — never a
        /// silent drop. Deterministic: rules are checked in ordinal id order.
        /// </summary>
        public static bool Validate(CascadeRuleCatalog? catalog, out string error)
        {
            error = string.Empty;
            if (catalog == null) { error = "cascade_rules.json: catalog is null."; return false; }
            if (catalog.schema_version != SupportedSchemaVersion)
            {
                error = $"cascade_rules.json: unsupported schema_version {catalog.schema_version} (expected {SupportedSchemaVersion}).";
                return false;
            }
            if (catalog.minimum_warning_hours < 0f)
            {
                error = "cascade_rules.json: minimum_warning_hours must be non-negative.";
                return false;
            }
            if (catalog.rules == null || catalog.rules.Count == 0)
            {
                error = "cascade_rules.json: no rules defined.";
                return false;
            }

            var ids = new HashSet<string>(StringComparer.Ordinal);
            var ordered = new List<CascadeRule>(catalog.rules);
            ordered.Sort(static (a, b) => string.CompareOrdinal(a?.id ?? string.Empty, b?.id ?? string.Empty));

            foreach (var rule in ordered)
            {
                if (rule == null || string.IsNullOrWhiteSpace(rule.id))
                {
                    error = "cascade_rules.json: rule with empty id.";
                    return false;
                }
                if (!ids.Add(rule.id))
                {
                    error = $"cascade_rules.json: duplicate rule id '{rule.id}'.";
                    return false;
                }
                if (string.IsNullOrWhiteSpace(rule.display_name))
                {
                    error = $"cascade_rules.json: rule '{rule.id}' has empty display_name.";
                    return false;
                }
                if (rule.requires_all == null || rule.requires_all.Count == 0)
                {
                    error = $"cascade_rules.json: rule '{rule.id}' requires no facts (would fire unconditionally).";
                    return false;
                }
                foreach (string condition in rule.requires_all)
                {
                    if (!KnownConditions.Contains(condition))
                    {
                        error = $"cascade_rules.json: rule '{rule.id}' uses unknown condition '{condition}'.";
                        return false;
                    }
                }
                if (rule.off_ramps == null || rule.off_ramps.Count == 0)
                {
                    error = $"cascade_rules.json: rule '{rule.id}' has no off-ramp — every reachable cascade must be recoverable.";
                    return false;
                }
                foreach (string offRamp in rule.off_ramps)
                {
                    if (!KnownOffRamps.Contains(offRamp))
                    {
                        error = $"cascade_rules.json: rule '{rule.id}' declares unknown off-ramp '{offRamp}'.";
                        return false;
                    }
                }
                if (rule.warning_hours < catalog.minimum_warning_hours)
                {
                    error = $"cascade_rules.json: rule '{rule.id}' warning_hours {rule.warning_hours} is below the minimum floor {catalog.minimum_warning_hours}.";
                    return false;
                }
            }
            return true;
        }
    }
}