// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Ashfall.Core.Achievements
{
    /// <summary>
    /// Plan 149: In-Campaign Achievement & Milestone System.
    /// Pure domain engine for evaluating in-campaign survival milestones, tracking run-local
    /// completion state, and emitting once-only unlock facts for presentation and cross-run handoff.
    /// Engine-neutral, deterministic, and save/load persistent.
    /// </summary>
    public sealed class AchievementDefinition
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = "survival";
        public string Description { get; set; } = string.Empty;
        public string ConditionType { get; set; } = string.Empty;
        public float TargetThreshold { get; set; } = 1.0f;
        public string? EpilogueTag { get; set; }
    }

    public sealed class AchievementState
    {
        public int SchemaVersion { get; set; } = 1;
        public HashSet<string> CompletedAchievementIds { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, float> ProgressFacts { get; set; } = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);
        public List<string> EmittedEventIds { get; set; } = new List<string>();
    }

    public sealed class AchievementCatalog
    {
        private readonly Dictionary<string, AchievementDefinition> _definitions = new Dictionary<string, AchievementDefinition>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyCollection<AchievementDefinition> All => _definitions.Values;

        public bool TryGet(string id, out AchievementDefinition definition)
        {
            return _definitions.TryGetValue(id, out definition!);
        }

        public IEnumerable<AchievementDefinition> GetByCategory(string category)
        {
            return _definitions.Values.Where(d => string.Equals(d.Category, category, StringComparison.OrdinalIgnoreCase));
        }

        public static AchievementCatalog LoadFromJson(string json)
        {
            var catalog = new AchievementCatalog();
            if (string.IsNullOrWhiteSpace(json))
                return catalog;

            int arrStart = json.IndexOf("\"achievements\"", StringComparison.Ordinal);
            if (arrStart < 0) return catalog;
            arrStart = json.IndexOf('[', arrStart);
            if (arrStart < 0) return catalog;
            int arrEnd = json.LastIndexOf(']');
            if (arrEnd <= arrStart) return catalog;

            string arrayContent = json.Substring(arrStart + 1, arrEnd - arrStart - 1);
            int idx = 0;
            while (idx < arrayContent.Length)
            {
                int objStart = arrayContent.IndexOf('{', idx);
                if (objStart < 0) break;
                int objEnd = FindClosingBrace(arrayContent, objStart);
                if (objEnd < 0) break;

                string objText = arrayContent.Substring(objStart, objEnd - objStart + 1);
                var def = ParseDefinition(objText);
                if (!string.IsNullOrWhiteSpace(def.Id))
                {
                    catalog._definitions[def.Id] = def;
                }
                idx = objEnd + 1;
            }

            return catalog;
        }

        private static int FindClosingBrace(string text, int openPos)
        {
            int depth = 0;
            for (int i = openPos; i < text.Length; i++)
            {
                if (text[i] == '{') depth++;
                else if (text[i] == '}')
                {
                    depth--;
                    if (depth == 0) return i;
                }
            }
            return -1;
        }

        private static AchievementDefinition ParseDefinition(string json)
        {
            var def = new AchievementDefinition();
            def.Id = ExtractString(json, "id");
            def.Name = ExtractString(json, "name");
            def.Category = ExtractString(json, "category");
            if (string.IsNullOrEmpty(def.Category)) def.Category = "survival";
            def.Description = ExtractString(json, "description");
            def.ConditionType = ExtractString(json, "condition_type");
            def.TargetThreshold = ExtractFloat(json, "target_threshold", 1.0f);
            def.EpilogueTag = ExtractString(json, "epilogue_tag");
            if (string.IsNullOrEmpty(def.EpilogueTag)) def.EpilogueTag = null;
            return def;
        }

        private static string ExtractString(string json, string key)
        {
            string pattern = $"\"{key}\"";
            int keyIdx = json.IndexOf(pattern, StringComparison.Ordinal);
            if (keyIdx < 0) return string.Empty;
            int colon = json.IndexOf(':', keyIdx + pattern.Length);
            if (colon < 0) return string.Empty;
            int quoteStart = json.IndexOf('"', colon + 1);
            if (quoteStart < 0) return string.Empty;
            int quoteEnd = json.IndexOf('"', quoteStart + 1);
            if (quoteEnd < 0) return string.Empty;
            return json.Substring(quoteStart + 1, quoteEnd - quoteStart - 1).Trim();
        }

        private static float ExtractFloat(string json, string key, float defaultValue)
        {
            string pattern = $"\"{key}\"";
            int keyIdx = json.IndexOf(pattern, StringComparison.Ordinal);
            if (keyIdx < 0) return defaultValue;
            int colon = json.IndexOf(':', keyIdx + pattern.Length);
            if (colon < 0) return defaultValue;
            int start = colon + 1;
            while (start < json.Length && (char.IsWhiteSpace(json[start]) || json[start] == '"')) start++;
            int end = start;
            while (end < json.Length && (char.IsDigit(json[end]) || json[end] == '.' || json[end] == '-')) end++;
            if (end > start && float.TryParse(json.Substring(start, end - start), NumberStyles.Float, CultureInfo.InvariantCulture, out float val))
            {
                return val;
            }
            return defaultValue;
        }
    }

    public sealed class AchievementSystem
    {
        public delegate void AchievementUnlockedDelegate(string achievementId, string campaignId);

        public event AchievementUnlockedDelegate? OnAchievementUnlockedSeam;

        private readonly AchievementCatalog _catalog;
        private AchievementState _state;
        private readonly string _campaignId;

        public AchievementCatalog Catalog => _catalog;
        public AchievementState State => _state;
        public string CampaignId => _campaignId;

        public AchievementSystem(AchievementCatalog catalog, string campaignId = "default_campaign", AchievementState? initialState = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _campaignId = campaignId;
            _state = initialState ?? new AchievementState();
        }

        public bool IsUnlocked(string achievementId)
        {
            return _state.CompletedAchievementIds.Contains(achievementId);
        }

        public IReadOnlyCollection<string> GetCompletedAchievementIds()
        {
            return _state.CompletedAchievementIds;
        }

        public float GetProgressFact(string factKey)
        {
            return _state.ProgressFacts.TryGetValue(factKey, out float v) ? v : 0f;
        }

        public void SetFact(string factKey, float value)
        {
            _state.ProgressFacts[factKey] = value;
            EvaluateCondition(factKey, value);
        }

        public void IncrementFact(string factKey, float delta)
        {
            float current = GetProgressFact(factKey);
            float next = current + delta;
            _state.ProgressFacts[factKey] = next;
            EvaluateCondition(factKey, next);
        }

        public bool EvaluateCondition(string conditionTypeOrId, float currentValue)
        {
            bool anyUnlocked = false;

            // Check if conditionTypeOrId directly matches an achievement ID
            if (_catalog.TryGet(conditionTypeOrId, out var directDef))
            {
                if (CheckAndUnlock(directDef, currentValue))
                {
                    anyUnlocked = true;
                }
            }

            // Also check all definitions where ConditionType matches
            foreach (var def in _catalog.All)
            {
                if (string.Equals(def.ConditionType, conditionTypeOrId, StringComparison.OrdinalIgnoreCase))
                {
                    if (CheckAndUnlock(def, currentValue))
                    {
                        anyUnlocked = true;
                    }
                }
            }

            return anyUnlocked;
        }

        private bool CheckAndUnlock(AchievementDefinition def, float currentValue)
        {
            if (IsUnlocked(def.Id))
                return false;

            bool conditionMet = false;

            switch (def.ConditionType.ToLowerInvariant())
            {
                case "max_avg_dose":
                    // Lower is better: currentValue must be <= target threshold
                    conditionMet = currentValue <= def.TargetThreshold;
                    break;
                case "all_survivors_alive":
                case "flawless_defense":
                    // Binary or ratio condition: >= threshold
                    conditionMet = currentValue >= def.TargetThreshold;
                    break;
                case "survive_days":
                case "min_avg_health":
                case "min_avg_morale":
                case "raids_repelled":
                case "disputes_resolved":
                case "routes_mapped":
                case "expedition_distance":
                case "trades_completed":
                case "food_reserves":
                case "merciful_choices":
                case "memorials_erected":
                default:
                    conditionMet = currentValue >= def.TargetThreshold;
                    break;
            }

            if (conditionMet)
            {
                Unlock(def.Id);
                return true;
            }

            return false;
        }

        public void Unlock(string achievementId)
        {
            if (_state.CompletedAchievementIds.Contains(achievementId))
                return;

            _state.CompletedAchievementIds.Add(achievementId);
            _state.EmittedEventIds.Add($"unlock_{achievementId}");

            OnAchievementUnlockedSeam?.Invoke(achievementId, _campaignId);
        }

        /// <summary>
        /// Evaluates active campaign metrics and roster state against all achievement milestones.
        /// Replaces the hardcoded panel derivations with pure data-driven checks.
        /// </summary>
        public void EvaluateRosterSnapshot(int simDay, int totalRoster, int aliveCount, float avgHealth, float avgDose, float avgMorale)
        {
            SetFact("survive_days", simDay);

            if (totalRoster > 0)
            {
                float allAlive = (aliveCount == totalRoster) ? 1.0f : 0.0f;
                SetFact("all_survivors_alive", allAlive);
                SetFact("min_avg_health", avgHealth);
                SetFact("max_avg_dose", avgDose);
                SetFact("min_avg_morale", avgMorale);
            }
        }

        public string CaptureState()
        {
            var completedList = string.Join(",", _state.CompletedAchievementIds.Select(id => $"\"{id}\""));
            var emittedList = string.Join(",", _state.EmittedEventIds.Select(id => $"\"{id}\""));

            var factEntries = new List<string>();
            foreach (var kvp in _state.ProgressFacts)
            {
                factEntries.Add($"\"{kvp.Key}\":{kvp.Value.ToString("F2", CultureInfo.InvariantCulture)}");
            }
            string factsObj = string.Join(",", factEntries);

            return $"{{\"schema_version\":{_state.SchemaVersion},\"completed\":[{completedList}],\"facts\":{{{factsObj}}},\"emitted\":[{emittedList}]}}";
        }

        public void RestoreState(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return;

            var newState = new AchievementState();

            // Extract schema_version
            int svIdx = json.IndexOf("\"schema_version\"", StringComparison.Ordinal);
            if (svIdx >= 0)
            {
                int colon = json.IndexOf(':', svIdx);
                int end = colon + 1;
                while (end < json.Length && (char.IsDigit(json[end]) || char.IsWhiteSpace(json[end]))) end++;
                if (int.TryParse(json.Substring(colon + 1, end - colon - 1).Trim(), out int sv))
                {
                    newState.SchemaVersion = sv;
                }
            }

            // Extract completed array
            int compIdx = json.IndexOf("\"completed\"", StringComparison.Ordinal);
            if (compIdx >= 0)
            {
                int startArr = json.IndexOf('[', compIdx);
                int endArr = json.IndexOf(']', startArr);
                if (startArr >= 0 && endArr > startArr)
                {
                    string items = json.Substring(startArr + 1, endArr - startArr - 1);
                    string[] split = items.Split(',');
                    foreach (var s in split)
                    {
                        string trimmed = s.Trim().Trim('"');
                        if (!string.IsNullOrEmpty(trimmed))
                        {
                            newState.CompletedAchievementIds.Add(trimmed);
                        }
                    }
                }
            }

            // Extract facts object
            int factsIdx = json.IndexOf("\"facts\"", StringComparison.Ordinal);
            if (factsIdx >= 0)
            {
                int startObj = json.IndexOf('{', factsIdx);
                int endObj = json.IndexOf('}', startObj);
                if (startObj >= 0 && endObj > startObj)
                {
                    string pairs = json.Substring(startObj + 1, endObj - startObj - 1);
                    string[] split = pairs.Split(',');
                    foreach (var p in split)
                    {
                        int colon = p.IndexOf(':');
                        if (colon > 0)
                        {
                            string key = p.Substring(0, colon).Trim().Trim('"');
                            string valStr = p.Substring(colon + 1).Trim();
                            if (float.TryParse(valStr, NumberStyles.Float, CultureInfo.InvariantCulture, out float val))
                            {
                                newState.ProgressFacts[key] = val;
                            }
                        }
                    }
                }
            }

            // Extract emitted array
            int emitIdx = json.IndexOf("\"emitted\"", StringComparison.Ordinal);
            if (emitIdx >= 0)
            {
                int startArr = json.IndexOf('[', emitIdx);
                int endArr = json.IndexOf(']', startArr);
                if (startArr >= 0 && endArr > startArr)
                {
                    string items = json.Substring(startArr + 1, endArr - startArr - 1);
                    string[] split = items.Split(',');
                    foreach (var s in split)
                    {
                        string trimmed = s.Trim().Trim('"');
                        if (!string.IsNullOrEmpty(trimmed))
                        {
                            newState.EmittedEventIds.Add(trimmed);
                        }
                    }
                }
            }

            _state = newState;
        }
    }
}
