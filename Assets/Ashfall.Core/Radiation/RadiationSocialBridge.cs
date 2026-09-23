// SPDX-License-Identifier: MIT
// Plan 146: Radiation → Social Bridge
// Pure domain authority connecting survivor radiation dose to social stigma, discrimination, and faction standing adjustments.

using System;
using System.Collections.Generic;
using Ashfall.Core;

namespace Ashfall.Core.Radiation
{
    [Serializable]
    public sealed class RadiationSocialBracket
    {
        public string bracket_id { get; set; } = string.Empty;
        public float min_dose_msv { get; set; }
        public float max_dose_msv { get; set; }
        public int social_penalty { get; set; }
        public float discrimination_chance { get; set; }
        public string status_title { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class SurvivorRadiationSocialProfile
    {
        public string survivorId { get; set; } = string.Empty;
        public float doseMsv { get; set; }
        public string bracketId { get; set; } = string.Empty;
        public int socialPenalty { get; set; }
        public float discriminationChance { get; set; }
        public string statusTitle { get; set; } = string.Empty;
        public bool isDiscriminated { get; set; }
        public List<string> quarantinedByFactions { get; set; } = new();
    }

    [Serializable]
    public sealed class RadiationSocialSaveState
    {
        public int schema_version { get; set; } = 1;
        public int totalDiscriminationIncidents { get; set; }
        public int totalFactionProtests { get; set; }
    }

    public interface IRadiationSocialSink
    {
        void OnSocialEvaluation(SurvivorRadiationSocialProfile profile);
    }

    public sealed class RadiationSocialBridge
    {
        private readonly List<RadiationSocialBracket> _brackets = new();
        private readonly Dictionary<string, FactionContaminationPolicy> _factionPolicies = new(StringComparer.OrdinalIgnoreCase);
        private int _totalDiscriminationIncidents;
        private int _totalFactionProtests;

        // Seams
        public Action<string, string, int>? OnRadiationSocialPenaltyAppliedSeam; // survivorId, bracketId, penalty
        public Action<string, string>? OnRadiationDiscriminationTriggeredSeam; // survivorId, context
        public Action<string, string, int>? OnRadiationFactionStandingAdjustedSeam; // survivorId, factionId, delta
        public IRadiationSocialSink? Sink { get; set; }

        public IReadOnlyList<RadiationSocialBracket> Brackets => _brackets;
        public int TotalDiscriminationIncidents => _totalDiscriminationIncidents;
        public int TotalFactionProtests => _totalFactionProtests;

        public RadiationSocialBridge()
        {
            LoadDefaultBrackets();
        }

        public void LoadCatalog(string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent)) return;

            _brackets.Clear();
            _factionPolicies.Clear();

            ParseBrackets(jsonContent);
            ParseFactionPolicies(jsonContent);
        }

        public void LoadCatalog(IFileIO fileIO, string path)
        {
            if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
            if (!fileIO.FileExists(path))
                throw new System.IO.FileNotFoundException($"Radiation social catalog not found at {path}");

            LoadCatalog(fileIO.ReadAllText(path));
        }

        public SurvivorRadiationSocialProfile EvaluateSocialStance(
            string survivorId,
            float doseMsv,
            ISeededRng? rng = null,
            string? encounteringFactionId = null)
        {
            doseMsv = Math.Max(0f, doseMsv);

            RadiationSocialBracket? matched = null;
            for (int i = 0; i < _brackets.Count; i++)
            {
                var b = _brackets[i];
                if (doseMsv >= b.min_dose_msv && doseMsv <= b.max_dose_msv)
                {
                    matched = b;
                    break;
                }
            }

            if (matched == null)
            {
                matched = _brackets.Count > 0 ? _brackets[_brackets.Count - 1] : new RadiationSocialBracket
                {
                    bracket_id = "unknown",
                    social_penalty = 0,
                    discrimination_chance = 0f,
                    status_title = "Unassayed"
                };
            }

            var profile = new SurvivorRadiationSocialProfile
            {
                survivorId = survivorId,
                doseMsv = doseMsv,
                bracketId = matched.bracket_id,
                socialPenalty = matched.social_penalty,
                discriminationChance = matched.discrimination_chance,
                statusTitle = matched.status_title
            };

            // Check discrimination roll
            if (rng != null && matched.discrimination_chance > 0f)
            {
                float roll = rng.NextFloat();
                if (roll < matched.discrimination_chance)
                {
                    profile.isDiscriminated = true;
                    _totalDiscriminationIncidents++;
                    OnRadiationDiscriminationTriggeredSeam?.Invoke(survivorId, $"Encountered prejudice due to {matched.status_title} status ({doseMsv:0.0} mSv)");
                }
            }

            // Check encountering faction policies
            if (!string.IsNullOrEmpty(encounteringFactionId) && _factionPolicies.TryGetValue(encounteringFactionId, out var policy))
            {
                if (policy.reject_irradiated_personnel && doseMsv > 25.0f)
                {
                    profile.quarantinedByFactions.Add(encounteringFactionId);
                    _totalFactionProtests++;
                    int penalty = -policy.standing_penalty_per_violation;
                    OnRadiationFactionStandingAdjustedSeam?.Invoke(survivorId, encounteringFactionId, penalty);
                }
            }

            if (profile.socialPenalty > 0)
            {
                OnRadiationSocialPenaltyAppliedSeam?.Invoke(survivorId, profile.bracketId, profile.socialPenalty);
            }

            Sink?.OnSocialEvaluation(profile);

            return profile;
        }

        public RadiationSocialSaveState CaptureState()
        {
            return new RadiationSocialSaveState
            {
                schema_version = 1,
                totalDiscriminationIncidents = _totalDiscriminationIncidents,
                totalFactionProtests = _totalFactionProtests
            };
        }

        public void RestoreState(RadiationSocialSaveState? state)
        {
            if (state == null)
            {
                _totalDiscriminationIncidents = 0;
                _totalFactionProtests = 0;
                return;
            }

            _totalDiscriminationIncidents = state.totalDiscriminationIncidents;
            _totalFactionProtests = state.totalFactionProtests;
        }

        private void LoadDefaultBrackets()
        {
            _brackets.Add(new RadiationSocialBracket { bracket_id = "safe", min_dose_msv = 0.0f, max_dose_msv = 20.0f, social_penalty = 0, discrimination_chance = 0.0f, status_title = "Clean", description = "No visible hazard." });
            _brackets.Add(new RadiationSocialBracket { bracket_id = "moderate", min_dose_msv = 20.1f, max_dose_msv = 50.0f, social_penalty = 10, discrimination_chance = 0.05f, status_title = "Exposed", description = "Mild fatigue." });
            _brackets.Add(new RadiationSocialBracket { bracket_id = "high", min_dose_msv = 50.1f, max_dose_msv = 100.0f, social_penalty = 25, discrimination_chance = 0.15f, status_title = "Hot", description = "Hair loss and erythema." });
            _brackets.Add(new RadiationSocialBracket { bracket_id = "severe", min_dose_msv = 100.1f, max_dose_msv = 1000.0f, social_penalty = 50, discrimination_chance = 0.35f, status_title = "Anointed Outcast", description = "Severe sickness." });

            _factionPolicies["faction_garrison"] = new FactionContaminationPolicy { faction_id = "faction_garrison", max_acceptable_contamination = 25, reject_irradiated_personnel = true, standing_penalty_per_violation = 5, policy_notes = "Military protocol." };
            _factionPolicies["faction_hydro_barons"] = new FactionContaminationPolicy { faction_id = "faction_hydro_barons", max_acceptable_contamination = 10, reject_irradiated_personnel = true, standing_penalty_per_violation = 10, policy_notes = "Water purifiers zero-tolerance." };
            _factionPolicies["faction_rebels"] = new FactionContaminationPolicy { faction_id = "faction_rebels", max_acceptable_contamination = 65, reject_irradiated_personnel = false, standing_penalty_per_violation = 2, policy_notes = "Guerillas accept contaminated goods." };
            _factionPolicies["faction_scavengers"] = new FactionContaminationPolicy { faction_id = "faction_scavengers", max_acceptable_contamination = 85, reject_irradiated_personnel = false, standing_penalty_per_violation = 0, policy_notes = "Waste scavengers trade anything." };
            _factionPolicies["faction_cult_of_ash"] = new FactionContaminationPolicy { faction_id = "faction_cult_of_ash", max_acceptable_contamination = 100, reject_irradiated_personnel = false, standing_penalty_per_violation = 0, policy_notes = "Ash zealots venerate irradiated survivors." };
        }

        private void ParseBrackets(string json)
        {
            int idx = json.IndexOf("\"social_brackets\"", StringComparison.Ordinal);
            if (idx < 0) return;
            int arrStart = json.IndexOf('[', idx);
            if (arrStart < 0) return;
            int arrEnd = json.IndexOf(']', arrStart);
            if (arrEnd < 0) return;

            string slice = json.Substring(arrStart + 1, arrEnd - arrStart - 1);
            var blocks = SplitObjects(slice);
            foreach (var b in blocks)
            {
                var br = new RadiationSocialBracket
                {
                    bracket_id = ExtractString(b, "bracket_id"),
                    min_dose_msv = ExtractFloat(b, "min_dose_msv", 0f),
                    max_dose_msv = ExtractFloat(b, "max_dose_msv", 1000f),
                    social_penalty = ExtractInt(b, "social_penalty", 0),
                    discrimination_chance = ExtractFloat(b, "discrimination_chance", 0f),
                    status_title = ExtractString(b, "status_title"),
                    description = ExtractString(b, "description")
                };
                if (!string.IsNullOrEmpty(br.bracket_id))
                {
                    _brackets.Add(br);
                }
            }
        }

        private void ParseFactionPolicies(string json)
        {
            int idx = json.IndexOf("\"faction_contamination_policies\"", StringComparison.Ordinal);
            if (idx < 0) return;
            int arrStart = json.IndexOf('[', idx);
            if (arrStart < 0) return;
            int arrEnd = json.IndexOf(']', arrStart);
            if (arrEnd < 0) return;

            string slice = json.Substring(arrStart + 1, arrEnd - arrStart - 1);
            var blocks = SplitObjects(slice);
            foreach (var b in blocks)
            {
                var p = new FactionContaminationPolicy
                {
                    faction_id = ExtractString(b, "faction_id"),
                    max_acceptable_contamination = ExtractInt(b, "max_acceptable_contamination", 100),
                    reject_irradiated_personnel = ExtractBool(b, "reject_irradiated_personnel", false),
                    standing_penalty_per_violation = ExtractInt(b, "standing_penalty_per_violation", 0),
                    policy_notes = ExtractString(b, "policy_notes")
                };
                if (!string.IsNullOrEmpty(p.faction_id))
                {
                    _factionPolicies[p.faction_id] = p;
                }
            }
        }

        private static List<string> SplitObjects(string content)
        {
            var results = new List<string>();
            int depth = 0, start = -1;
            bool inString = false;
            for (int i = 0; i < content.Length; i++)
            {
                char c = content[i];
                if (c == '"' && (i == 0 || content[i - 1] != '\\')) inString = !inString;
                else if (!inString)
                {
                    if (c == '{')
                    {
                        if (depth == 0) start = i;
                        depth++;
                    }
                    else if (c == '}')
                    {
                        depth--;
                        if (depth == 0 && start >= 0)
                        {
                            results.Add(content.Substring(start, i - start + 1));
                            start = -1;
                        }
                    }
                }
            }
            return results;
        }

        private static string ExtractString(string json, string key)
        {
            string p = $"\"{key}\"";
            int idx = json.IndexOf(p, StringComparison.Ordinal);
            if (idx < 0) return string.Empty;
            int colon = json.IndexOf(':', idx + p.Length);
            if (colon < 0) return string.Empty;
            int q1 = json.IndexOf('"', colon + 1);
            if (q1 < 0) return string.Empty;
            int q2 = json.IndexOf('"', q1 + 1);
            while (q2 < json.Length && json[q2 - 1] == '\\') q2 = json.IndexOf('"', q2 + 1);
            if (q2 < 0) return string.Empty;
            return json.Substring(q1 + 1, q2 - q1 - 1).Trim();
        }

        private static int ExtractInt(string json, string key, int def)
        {
            string p = $"\"{key}\"";
            int idx = json.IndexOf(p, StringComparison.Ordinal);
            if (idx < 0) return def;
            int colon = json.IndexOf(':', idx + p.Length);
            if (colon < 0) return def;
            int s = colon + 1;
            while (s < json.Length && char.IsWhiteSpace(json[s])) s++;
            int e = s;
            while (e < json.Length && (char.IsDigit(json[e]) || json[e] == '-')) e++;
            if (e > s && int.TryParse(json.Substring(s, e - s), out int val)) return val;
            return def;
        }

        private static float ExtractFloat(string json, string key, float def)
        {
            string p = $"\"{key}\"";
            int idx = json.IndexOf(p, StringComparison.Ordinal);
            if (idx < 0) return def;
            int colon = json.IndexOf(':', idx + p.Length);
            if (colon < 0) return def;
            int s = colon + 1;
            while (s < json.Length && char.IsWhiteSpace(json[s])) s++;
            int e = s;
            while (e < json.Length && (char.IsDigit(json[e]) || json[e] == '.' || json[e] == '-')) e++;
            if (e > s && float.TryParse(json.Substring(s, e - s), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float val)) return val;
            return def;
        }

        private static bool ExtractBool(string json, string key, bool def)
        {
            string p = $"\"{key}\"";
            int idx = json.IndexOf(p, StringComparison.Ordinal);
            if (idx < 0) return def;
            int colon = json.IndexOf(':', idx + p.Length);
            if (colon < 0) return def;
            int s = colon + 1;
            while (s < json.Length && char.IsWhiteSpace(json[s])) s++;
            if (s + 4 <= json.Length && json.Substring(s, 4).ToLowerInvariant() == "true") return true;
            if (s + 5 <= json.Length && json.Substring(s, 5).ToLowerInvariant() == "false") return false;
            return def;
        }
    }
}
