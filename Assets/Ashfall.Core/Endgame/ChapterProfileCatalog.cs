// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Endgame
{
    [Serializable]
    public sealed class ChapterProfileDef
    {
        [JsonPropertyName("profile_id")]
        public string profile_id { get; set; } = string.Empty;

        [JsonPropertyName("family")]
        public string family { get; set; } = "legacy";

        [JsonPropertyName("display_name")]
        public string display_name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string description { get; set; } = string.Empty;

        [JsonPropertyName("knowing_day")]
        public int knowing_day { get; set; } = 160;

        [JsonPropertyName("culpable_day")]
        public int culpable_day { get; set; } = 210;

        [JsonPropertyName("counted_day")]
        public int counted_day { get; set; } = 240;

        [JsonPropertyName("reading_day")]
        public int reading_day { get; set; } = 360;

        [JsonPropertyName("reckoning_offset")]
        public int reckoning_offset { get; set; } = 0;

        [JsonPropertyName("year_one_ending_source")]
        public string year_one_ending_source { get; set; } = "legacy_context";

        [JsonPropertyName("close_rule")]
        public string close_rule { get; set; } = "fixed_day";

        [JsonPropertyName("settle_window_days")]
        public int settle_window_days { get; set; } = 0;

        [JsonPropertyName("floor_day")]
        public int floor_day { get; set; } = 360;

        [JsonPropertyName("ceiling_day")]
        public int ceiling_day { get; set; } = 360;

        [JsonPropertyName("waive_evidence_gate_after_day")]
        public int waive_evidence_gate_after_day { get; set; } = -1;

        [JsonPropertyName("default_standing_modifier")]
        public string default_standing_modifier { get; set; } = "standing_base_neutral";

        public int EvaluateCloseDay(bool endingResolved, int endingResolvedDay)
        {
            if (string.Equals(close_rule, "ending_resolved", StringComparison.OrdinalIgnoreCase))
            {
                if (endingResolved)
                {
                    int candidate = endingResolvedDay + settle_window_days;
                    return Math.Max(floor_day, Math.Min(ceiling_day, candidate));
                }
                return ceiling_day > 0 ? ceiling_day : reading_day;
            }
            return reading_day;
        }

        public bool IsDayPastClose(int currentDay, bool endingResolved, int endingResolvedDay)
        {
            return currentDay >= EvaluateCloseDay(endingResolved, endingResolvedDay);
        }
    }

    [Serializable]
    public sealed class StandingModifierDef
    {
        [JsonPropertyName("modifier_id")]
        public string modifier_id { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string display_name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string description { get; set; } = string.Empty;

        [JsonPropertyName("weight")]
        public int weight { get; set; }
    }

    [Serializable]
    public sealed class ChapterProfilesData
    {
        [JsonPropertyName("schema_version")]
        public int schema_version { get; set; } = 1;

        [JsonPropertyName("title")]
        public string title { get; set; } = string.Empty;

        [JsonPropertyName("profiles")]
        public List<ChapterProfileDef> profiles { get; set; } = new();

        [JsonPropertyName("standing_modifiers")]
        public Dictionary<string, StandingModifierDef> standing_modifiers { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        [JsonPropertyName("branch_ending_modifiers")]
        public Dictionary<string, string> branch_ending_modifiers { get; set; } = new(StringComparer.OrdinalIgnoreCase);
    }

    public readonly struct ChapterProfileValidationReport
    {
        public bool IsValid { get; }
        public IReadOnlyList<string> Errors { get; }

        public ChapterProfileValidationReport(bool isValid, IReadOnlyList<string> errors)
        {
            IsValid = isValid;
            Errors = errors ?? Array.Empty<string>();
        }
    }

    public sealed class ChapterProfileCatalog
    {
        public const string DefaultProfileId = "profile_base_v1";

        private readonly Dictionary<string, ChapterProfileDef> _profiles = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, StandingModifierDef> _standingModifiers = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _branchEndingModifiers = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<string> _validationErrors = new();

        public IReadOnlyDictionary<string, ChapterProfileDef> Profiles => _profiles;
        public IReadOnlyDictionary<string, StandingModifierDef> StandingModifiers => _standingModifiers;
        public IReadOnlyDictionary<string, string> BranchEndingModifiers => _branchEndingModifiers;
        public IReadOnlyList<string> ValidationErrors => _validationErrors;
        public bool IsValid => _validationErrors.Count == 0 && _profiles.Count > 0;

        public ChapterProfileCatalog(ChapterProfilesData? data = null)
        {
            if (data != null)
            {
                if (data.profiles != null)
                {
                    foreach (var p in data.profiles)
                    {
                        if (p != null && !string.IsNullOrWhiteSpace(p.profile_id))
                            _profiles[p.profile_id] = p;
                    }
                }

                if (data.standing_modifiers != null)
                {
                    foreach (var kvp in data.standing_modifiers)
                    {
                        if (kvp.Value != null)
                            _standingModifiers[kvp.Key] = kvp.Value;
                    }
                }

                if (data.branch_ending_modifiers != null)
                {
                    foreach (var kvp in data.branch_ending_modifiers)
                    {
                        if (!string.IsNullOrWhiteSpace(kvp.Value))
                            _branchEndingModifiers[kvp.Key] = kvp.Value;
                    }
                }
            }

            Validate();
        }

        public static ChapterProfileCatalog LoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                var empty = new ChapterProfileCatalog();
                empty._validationErrors.Add("ChapterProfileCatalog JSON is empty or null.");
                return empty;
            }

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<ChapterProfilesData>(json, options);
                if (data == null)
                {
                    var invalid = new ChapterProfileCatalog();
                    invalid._validationErrors.Add("Failed to deserialize ChapterProfilesData.");
                    return invalid;
                }
                return new ChapterProfileCatalog(data);
            }
            catch (Exception ex)
            {
                var failed = new ChapterProfileCatalog();
                failed._validationErrors.Add($"Deserialization exception: {ex.Message}");
                return failed;
            }
        }

        public bool TryGetProfile(string profileId, out ChapterProfileDef? profile)
        {
            if (string.IsNullOrWhiteSpace(profileId))
                profileId = DefaultProfileId;
            return _profiles.TryGetValue(profileId, out profile);
        }

        public ChapterProfileDef GetProfileOrDefault(string profileId)
        {
            if (TryGetProfile(profileId, out var profile) && profile != null)
                return profile;
            if (_profiles.TryGetValue(DefaultProfileId, out var fallback) && fallback != null)
                return fallback;
            return CreateDefaultLegacyProfile();
        }

        public string ResolveStandingModifier(string endingId, string? profileId = null)
        {
            if (!string.IsNullOrWhiteSpace(endingId))
            {
                if (_branchEndingModifiers.TryGetValue(endingId, out var specificMod))
                    return specificMod;
            }

            if (!string.IsNullOrWhiteSpace(profileId) && _profiles.TryGetValue(profileId, out var profile))
            {
                if (!string.IsNullOrWhiteSpace(profile.default_standing_modifier))
                    return profile.default_standing_modifier;
            }

            // Infer from ending id prefix if recognized
            if (!string.IsNullOrEmpty(endingId))
            {
                if (endingId.StartsWith("ending_mil_", StringComparison.OrdinalIgnoreCase))
                    return "modifier_military_command";
                if (endingId.StartsWith("ending_reb_", StringComparison.OrdinalIgnoreCase))
                    return "modifier_rebel_autonomy";
                if (endingId.StartsWith("ending_ind_", StringComparison.OrdinalIgnoreCase))
                    return "modifier_independent_trade";
                if (endingId.StartsWith("ending_muster_", StringComparison.OrdinalIgnoreCase))
                    return "modifier_frontier_muster";
                if (endingId.StartsWith("ending_verdict_", StringComparison.OrdinalIgnoreCase))
                    return "modifier_standing_d_late_call";
            }

            return "standing_base_neutral";
        }

        public ChapterProfileValidationReport Validate(IEnumerable<string>? branchEndingIds = null)
        {
            _validationErrors.Clear();

            if (_profiles.Count == 0)
            {
                _validationErrors.Add("No chapter profiles defined.");
                return new ChapterProfileValidationReport(IsValid, _validationErrors);
            }

            if (!_profiles.ContainsKey(DefaultProfileId))
            {
                _validationErrors.Add($"Default profile '{DefaultProfileId}' missing from catalog.");
            }

            foreach (var kvp in _profiles)
            {
                var p = kvp.Value;
                if (string.IsNullOrWhiteSpace(p.profile_id))
                    _validationErrors.Add("Profile has empty profile_id.");

                if (p.knowing_day > p.culpable_day)
                    _validationErrors.Add($"Profile '{p.profile_id}' has knowing_day ({p.knowing_day}) > culpable_day ({p.culpable_day}).");

                if (p.floor_day > p.ceiling_day)
                    _validationErrors.Add($"Profile '{p.profile_id}' has floor_day ({p.floor_day}) > ceiling_day ({p.ceiling_day}).");

                if (p.reading_day <= 0)
                    _validationErrors.Add($"Profile '{p.profile_id}' has invalid reading_day ({p.reading_day}).");

                if (!string.Equals(p.close_rule, "fixed_day", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(p.close_rule, "ending_resolved", StringComparison.OrdinalIgnoreCase))
                    _validationErrors.Add($"Profile '{p.profile_id}' has unsupported close_rule '{p.close_rule}'.");

                if (!string.Equals(p.year_one_ending_source, "legacy_context", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(p.year_one_ending_source, "faction_branch", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(p.year_one_ending_source, "muster_approach", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(p.year_one_ending_source, "verdict_late_call", StringComparison.OrdinalIgnoreCase))
                    _validationErrors.Add($"Profile '{p.profile_id}' has unsupported year_one_ending_source '{p.year_one_ending_source}'.");

                if (!string.IsNullOrEmpty(p.default_standing_modifier) &&
                    _standingModifiers.Count > 0 &&
                    !_standingModifiers.ContainsKey(p.default_standing_modifier))
                {
                    _validationErrors.Add($"Profile '{p.profile_id}' references unknown default standing modifier '{p.default_standing_modifier}'.");
                }
            }

            // Verify all supplied branch endings resolve to a non-empty modifier
            if (branchEndingIds != null)
            {
                foreach (var endingId in branchEndingIds)
                {
                    if (string.IsNullOrWhiteSpace(endingId)) continue;
                    string resolved = ResolveStandingModifier(endingId);
                    if (string.IsNullOrWhiteSpace(resolved))
                    {
                        _validationErrors.Add($"Branch ending '{endingId}' could not be resolved to any Standing modifier.");
                    }
                }
            }

            return new ChapterProfileValidationReport(IsValid, _validationErrors);
        }

        public static ChapterProfileDef CreateDefaultLegacyProfile()
        {
            return new ChapterProfileDef
            {
                profile_id = DefaultProfileId,
                family = "legacy",
                display_name = "Base Holdfast (Legacy)",
                knowing_day = 160,
                culpable_day = 210,
                counted_day = 240,
                reading_day = 360,
                reckoning_offset = 0,
                year_one_ending_source = "legacy_context",
                close_rule = "fixed_day",
                settle_window_days = 0,
                floor_day = 360,
                ceiling_day = 360,
                waive_evidence_gate_after_day = -1,
                default_standing_modifier = "standing_base_neutral"
            };
        }
    }
}
