namespace Ashfall.Core.Economy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.Json;

    /// <summary>
    /// Loads, validates, and converts a Hardcore Economy Tuning JSON document
    /// into the engine-agnostic <see cref="HardcoreEconomyTuningBundle"/>.
    /// Validation rules mirror GoodsCatalogLoader: required fields, enum parsing,
    /// non-negative durations, positive multipliers, no duplicate ids.
    /// </summary>
    public static class HardcoreEconomyTuningLoader
    {
        private static readonly JsonSerializerOptions _opts = new()
        {
            // Field-based snake_case DTOs (JSON-authority convention): without
            // IncludeFields every list binds empty (binding-parity defect).
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        /// <summary>
        /// Load from a JSON string. Returns a failure result with per-field errors
        /// when the document is malformed; never throws.
        /// </summary>
        public static HardcoreEconomyTuningLoadResult Load(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return HardcoreEconomyTuningLoadResult.Failure(new[] { "JSON document is empty." });

            HardcoreEconomyTuningDto? dto;
            try
            {
                dto = JsonSerializer.Deserialize<HardcoreEconomyTuningDto>(json, _opts);
            }
            catch (JsonException ex)
            {
                return HardcoreEconomyTuningLoadResult.Failure(new[] { $"JSON parse error: {ex.Message}" });
            }

            if (dto is null)
                return HardcoreEconomyTuningLoadResult.Failure(new[] { "Deserialized document is null." });

            if (dto.Version != 1)
                return HardcoreEconomyTuningLoadResult.Failure(new[] { $"Unsupported tuning version {dto.Version}; expected 1." });

            var errors = new List<string>();
            var scarcity = new List<ScarcityEntry>();
            var factions = new List<FactionTradePreference>();
            var shocks = new List<PriceShockRule>();

            // ── Validate scarcity tiers ──────────────────────────────────
            if (dto.ScarcityTiers is not null)
            {
                for (int i = 0; i < dto.ScarcityTiers.Count; i++)
                {
                    var s = dto.ScarcityTiers[i];
                    var entryErrors = new List<string>();
                    if (!Enum.TryParse<ScarcityTier>(s.Tier, true, out var tier))
                        entryErrors.Add($"[scarcity[{i}] invalid tier \"{s.Tier}\"");
                    if (s.Multiplier <= 0f) entryErrors.Add($"[scarcity[{i}] multiplier must be > 0, got {s.Multiplier}");
                    if (string.IsNullOrWhiteSpace(s.DayRangeLabel)) entryErrors.Add($"[scarcity[{i}] DayRangeLabel is required");
                    if (s.AffectedItemIds is null || s.AffectedItemIds.Count == 0)
                        entryErrors.Add($"[scarcity[{i}] AffectedItemIds must contain at least one item id");
                    if (string.IsNullOrWhiteSpace(s.Rationale)) entryErrors.Add($"[scarcity[{i}] Rationale is required");

                    if (entryErrors.Count > 0)
                    {
                        errors.AddRange(entryErrors);
                    }
                    else
                    {
                        scarcity.Add(new ScarcityEntry(tier, s.Multiplier, s.DayRangeLabel, s.AffectedItemIds, s.Rationale));
                    }
                }
            }

            // ── Validate faction preferences ─────────────────────────────
            if (dto.FactionPreferences is not null)
            {
                var seenFactions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < dto.FactionPreferences.Count; i++)
                {
                    var f = dto.FactionPreferences[i];
                    var entryErrors = new List<string>();
                    if (string.IsNullOrWhiteSpace(f.FactionId))
                        entryErrors.Add($"[faction[{i}] FactionId is required");
                    else if (!seenFactions.Add(f.FactionId))
                        entryErrors.Add($"[faction[{i}] duplicate FactionId \"{f.FactionId}\"");
                    if (f.BuysAtPremium is null || f.BuysAtPremium.Count == 0)
                        entryErrors.Add($"[faction[{i}] BuysAtPremium must contain at least one item id prefix");
                    if (f.Refuses is null || f.Refuses.Count == 0)
                        entryErrors.Add($"[faction[{i}] Refuses must contain at least one item id prefix");
                    if (string.IsNullOrWhiteSpace(f.TradeCurrency))
                        entryErrors.Add($"[faction[{i}] TradeCurrency is required");

                    if (entryErrors.Count > 0)
                    {
                        errors.AddRange(entryErrors);
                    }
                    else
                    {
                        factions.Add(new FactionTradePreference(f.FactionId, f.BuysAtPremium, f.Refuses, f.TradeCurrency));
                    }
                }
            }

            // ── Validate price shocks ────────────────────────────────────
            if (dto.PriceShockRules is not null)
            {
                for (int i = 0; i < dto.PriceShockRules.Count; i++)
                {
                    var p = dto.PriceShockRules[i];
                    var entryErrors = new List<string>();
                    if (!Enum.TryParse<PriceShockKind>(p.Kind, true, out var kind))
                        entryErrors.Add($"[shock[{i}] invalid kind \"{p.Kind}\"");
                    if (p.Multiplier <= 0f) entryErrors.Add($"[shock[{i}] multiplier must be > 0, got {p.Multiplier}");
                    if (p.DurationDays < 0) entryErrors.Add($"[shock[{i}] DurationDays must be >= 0, got {p.DurationDays}");
                    if (p.AffectedItemIds is null || p.AffectedItemIds.Count == 0)
                        entryErrors.Add($"[shock[{i}] AffectedItemIds must contain at least one item id or \"*\"");
                    if (string.IsNullOrWhiteSpace(p.Trigger)) entryErrors.Add($"[shock[{i}] Trigger is required");

                    if (entryErrors.Count > 0)
                    {
                        errors.AddRange(entryErrors);
                    }
                    else
                    {
                        shocks.Add(new PriceShockRule(kind, p.Multiplier, p.DurationDays, p.AffectedItemIds, p.Trigger));
                    }
                }
            }

            if (errors.Count > 0)
                return HardcoreEconomyTuningLoadResult.Failure(errors);

            var bundle = new HardcoreEconomyTuningBundle(scarcity, factions, shocks);
            return HardcoreEconomyTuningLoadResult.Success(bundle);
        }
    }
}
