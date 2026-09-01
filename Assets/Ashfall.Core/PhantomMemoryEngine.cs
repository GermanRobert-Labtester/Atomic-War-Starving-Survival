using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>
    /// Engine-agnostic phantom memory system (Antigravity #41, Master Plan §V, Plan 21).
    /// When survivors scavenge everyday pre-war objects, background-specific
    /// memories trigger psychological breaks or motivation bursts.
    ///
    /// Extended in Plan 21 with one-shot idempotence, rich category taxonomy,
    /// affinity amplification, and lore-only/payload classification.
    /// Zero references to UnityEngine or Godot.
    /// </summary>

    // ── Snapshot types ───────────────────────────────────────────────

    /// <summary>Minimal survivor data the phantom memory system reads.</summary>
    public class PhantomSurvivorSnapshot
    {
        public string survivorId = string.Empty;
        public string displayName = string.Empty;
        public string backgroundId = string.Empty;
        public List<string> traitIds = new List<string>();
        public bool isAlive = true;
    }

    /// <summary>Mutable per-survivor phantom memory state.</summary>
    [Serializable]
    public class PhantomMemoryRecord
    {
        public string survivorId = string.Empty;
        public int triggersExperienced;
        public float motivationBoostHoursRemaining;
        public float breakdownRefusalHoursRemaining;
        public List<string> triggeredItemIds = new List<string>();
        public List<string> triggeredTriggerIds = new List<string>();
    }

    [Serializable]
    public class PhantomMemoryEngineState
    {
        public string systemId = PhantomMemoryEngine.SystemId;
        public List<PhantomMemoryRecord> records = new List<PhantomMemoryRecord>();
    }

    /// <summary>One trigger rule: background + item category/id → outcome.</summary>
    [Serializable]
    public class PhantomTriggerRule
    {
        public string triggerId = string.Empty;
        public string itemCategory = string.Empty;
        public string itemId = string.Empty;
        public float motivationChance;
        public string descriptionKey = string.Empty;
        public string motivationText = string.Empty;
        public string breakdownText = string.Empty;
        public string affinityTrait = string.Empty;
        public bool loreOnly;
        public float moralePayload;
        public float guiltPayload;
        public string gatingFlag = string.Empty;
        public bool repeatable;
    }

    // ── System ───────────────────────────────────────────────────────

    public class PhantomMemoryEngine
    {
        public const string SystemId = "phantom_memory_engine";

        // ── Constants ──────────────────────────────────────────────────
        public const float MotivationBoostDurationHours = 8f;
        public const float MotivationMoraleBoost = 15f;
        public const float MotivationWorkSpeedBonus = 0.20f;
        public const float BreakdownMoraleDrop = -20f;
        public const float BreakdownWorkRefusalHours = 4f;
        public const float BaseTriggerChance = 0.15f;

        /// <summary>Override for tests. Set to 1.0 to force a trigger every time.</summary>
        public float TriggerChanceOverride = -1f;

        // ── Events ─────────────────────────────────────────────────────
        /// <summary>SurvivorId, itemId, isMotivation.</summary>
        public event Action<string, string, bool>? OnPhantomTriggered;
        /// <summary>SurvivorId, itemId — a breakdown specifically.</summary>
        public event Action<string, string>? OnPhantomBreakdown;
        /// <summary>SurvivorId, itemId, isMotivation, moraleDelta, guiltDelta.</summary>
        public event Action<string, string, bool, float, float>? OnPhantomMemoryResolved;
        public event Action<PhantomMemoryEngineState>? OnStateChanged;

        // ── State ──────────────────────────────────────────────────────
        private readonly PhantomMemoryEngineState _state = new PhantomMemoryEngineState();
        private readonly Dictionary<string, PhantomMemoryRecord> _records = new Dictionary<string, PhantomMemoryRecord>(StringComparer.Ordinal);
        private readonly Dictionary<string, List<PhantomTriggerRule>> _rulesByBackground = new Dictionary<string, List<PhantomTriggerRule>>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<PhantomTriggerRule> GetRules(string backgroundId) =>
            _rulesByBackground.TryGetValue(backgroundId, out var rules) ? rules : new List<PhantomTriggerRule>();

        public IReadOnlyList<PhantomMemoryRecord> Records => _state.records;

        // ── Rule registration ─────────────────────────────────────────

        public void RegisterRule(
            string backgroundId,
            string itemCategory,
            float motivationChance,
            string descriptionKey,
            string? motivationText = null,
            string? breakdownText = null)
        {
            RegisterRuleDetailed(new PhantomTriggerRule
            {
                triggerId = $"rule_{backgroundId}_{itemCategory}",
                itemCategory = itemCategory,
                motivationChance = motivationChance,
                descriptionKey = descriptionKey ?? string.Empty,
                motivationText = motivationText ?? string.Empty,
                breakdownText = breakdownText ?? string.Empty,
                repeatable = true
            }, backgroundId);
        }

        public void RegisterRuleDetailed(PhantomTriggerRule rule, string backgroundId)
        {
            if (rule == null || string.IsNullOrEmpty(backgroundId)) return;
            if (!_rulesByBackground.TryGetValue(backgroundId, out var rules))
            {
                rules = new List<PhantomTriggerRule>();
                _rulesByBackground[backgroundId] = rules;
            }
            rules.Add(rule);
        }

        // ── Core mechanic ─────────────────────────────────────────────

        /// <summary>
        /// Called when a survivor discovers an item. Rolls for a phantom memory
        /// trigger based on background + item category/id.
        /// </summary>
        public TriggerOutcome OnItemScavenged(PhantomSurvivorSnapshot sv, string itemId, ISeededRng rng)
        {
            if (sv == null || !sv.isAlive || string.IsNullOrEmpty(sv.backgroundId) || string.IsNullOrEmpty(itemId))
                return TriggerOutcome.None;

            string category = GetCategoryFromId(itemId)!;
            if (string.IsNullOrEmpty(category)) return TriggerOutcome.None;

            var matchingRules = FindMatchingRules(sv, itemId, category);
            if (matchingRules.Count == 0) return TriggerOutcome.None;

            var record = GetOrCreateRecord(sv.survivorId);

            for (int i = 0; i < matchingRules.Count; i++)
            {
                var rule = matchingRules[i];

                // One-shot idempotence check
                if (!rule.repeatable && !string.IsNullOrEmpty(rule.triggerId) && record.triggeredTriggerIds.Contains(rule.triggerId))
                    continue;

                double roll = (double)rng.NextFloat();
                float effectiveBase = TriggerChanceOverride >= 0f ? TriggerChanceOverride : BaseTriggerChance;
                double adjustedChance = effectiveBase * (1f + record.triggersExperienced * 0.1f);
                if (roll >= adjustedChance) return TriggerOutcome.None;

                record.triggersExperienced++;
                if (!record.triggeredItemIds.Contains(itemId))
                    record.triggeredItemIds.Add(itemId);
                if (!string.IsNullOrEmpty(rule.triggerId) && !record.triggeredTriggerIds.Contains(rule.triggerId))
                    record.triggeredTriggerIds.Add(rule.triggerId);

                // Check affinity amplification
                float motivationChance = rule.motivationChance;
                if (!string.IsNullOrEmpty(rule.affinityTrait) && sv.traitIds != null && sv.traitIds.Contains(rule.affinityTrait))
                {
                    motivationChance = Math.Min(1.0f, motivationChance + 0.20f);
                }

                if (rule.loreOnly)
                {
                    OnPhantomMemoryResolved?.Invoke(sv.survivorId, itemId, false, 0f, 0f);
                    RaiseChanged();
                    return TriggerOutcome.None;
                }

                if (roll < motivationChance * adjustedChance)
                {
                    float moraleDelta = rule.loreOnly ? 0f : (rule.moralePayload != 0f ? rule.moralePayload : MotivationMoraleBoost);
                    if (!rule.loreOnly)
                    {
                        record.motivationBoostHoursRemaining = MotivationBoostDurationHours;
                    }
                    OnPhantomTriggered?.Invoke(sv.survivorId, itemId, true);
                    OnPhantomMemoryResolved?.Invoke(sv.survivorId, itemId, true, moraleDelta, 0f);
                    RaiseChanged();
                    return TriggerOutcome.Motivation;
                }
                else
                {
                    float moraleDrop = rule.loreOnly ? 0f : (rule.moralePayload != 0f ? -Math.Abs(rule.moralePayload) : BreakdownMoraleDrop);
                    float guiltDelta = rule.loreOnly ? 0f : rule.guiltPayload;
                    if (!rule.loreOnly)
                    {
                        record.motivationBoostHoursRemaining = 0f;
                        record.breakdownRefusalHoursRemaining = BreakdownWorkRefusalHours;
                        OnPhantomBreakdown?.Invoke(sv.survivorId, itemId);
                    }
                    OnPhantomTriggered?.Invoke(sv.survivorId, itemId, false);
                    OnPhantomMemoryResolved?.Invoke(sv.survivorId, itemId, false, moraleDrop, guiltDelta);
                    RaiseChanged();
                    return TriggerOutcome.Breakdown;
                }
            }

            return TriggerOutcome.None;
        }

        private List<PhantomTriggerRule> FindMatchingRules(PhantomSurvivorSnapshot sv, string itemId, string category)
        {
            var results = new List<PhantomTriggerRule>();

            // 1. Check primary background rules
            if (_rulesByBackground.TryGetValue(sv.backgroundId, out var bgRules))
            {
                for (int i = 0; i < bgRules.Count; i++)
                {
                    var r = bgRules[i];
                    if (MatchesRule(r, itemId, category))
                        results.Add(r);
                }
            }

            // 2. Fallback to generic rules if no primary background rules matched
            if (results.Count == 0 && !string.Equals(sv.backgroundId, "generic", StringComparison.OrdinalIgnoreCase))
            {
                if (_rulesByBackground.TryGetValue("generic", out var genRules))
                {
                    for (int i = 0; i < genRules.Count; i++)
                    {
                        var r = genRules[i];
                        if (MatchesRule(r, itemId, category))
                            results.Add(r);
                    }
                }
            }

            return results;
        }

        private static bool MatchesRule(PhantomTriggerRule rule, string itemId, string category)
        {
            if (!string.IsNullOrEmpty(rule.itemId) && string.Equals(rule.itemId, itemId, StringComparison.OrdinalIgnoreCase))
                return true;
            if (!string.IsNullOrEmpty(rule.itemCategory) && string.Equals(rule.itemCategory, category, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }

        /// <summary>Decay motivation boost and breakdown refusal timers.</summary>
        public void TickHour(string survivorId, float gameHours)
        {
            if (!_records.TryGetValue(survivorId, out var record)) return;

            bool changed = false;
            if (record.motivationBoostHoursRemaining > 0f)
            {
                record.motivationBoostHoursRemaining -= gameHours;
                if (record.motivationBoostHoursRemaining <= 0f)
                {
                    record.motivationBoostHoursRemaining = 0f;
                    changed = true;
                }
            }
            if (record.breakdownRefusalHoursRemaining > 0f)
            {
                record.breakdownRefusalHoursRemaining -= gameHours;
                if (record.breakdownRefusalHoursRemaining <= 0f)
                {
                    record.breakdownRefusalHoursRemaining = 0f;
                    changed = true;
                }
            }
            if (changed) RaiseChanged();
        }

        /// <summary>Resolve vignette text for a trigger outcome.</summary>
        public string ResolveTriggerText(PhantomSurvivorSnapshot sv, string itemId, bool isMotivation)
        {
            if (sv == null || string.IsNullOrEmpty(sv.backgroundId)) return string.Empty;
            string category = GetCategoryFromId(itemId) ?? "generic";

            var rules = FindMatchingRules(sv, itemId, category);
            for (int i = 0; i < rules.Count; i++)
            {
                var rule = rules[i];
                string template = isMotivation ? rule.motivationText : rule.breakdownText;
                if (string.IsNullOrEmpty(template)) continue;
                string name = string.IsNullOrEmpty(sv.displayName) ? "Someone" : sv.displayName;
                return template.Replace("{name}", name);
            }
            return string.Empty;
        }

        // ── Queries ───────────────────────────────────────────────────

        public bool HasMotivationBoost(string survivorId) =>
            _records.TryGetValue(survivorId, out var r) && r.motivationBoostHoursRemaining > 0f;

        public int GetTriggersExperienced(string survivorId) =>
            _records.TryGetValue(survivorId, out var r) ? r.triggersExperienced : 0;

        public float GetWorkEfficiencyMultiplier(string survivorId)
        {
            if (!_records.TryGetValue(survivorId, out var r)) return 1f;
            return r.motivationBoostHoursRemaining > 0f
                ? 1f + MotivationWorkSpeedBonus : 1f;
        }

        public float GetWorkRefusalHours(string survivorId)
        {
            return _records.TryGetValue(survivorId, out var r)
                ? r.breakdownRefusalHoursRemaining : 0f;
        }

        public bool HasExperiencedItem(string survivorId, string itemId)
        {
            return _records.TryGetValue(survivorId, out var r) && r.triggeredItemIds.Contains(itemId);
        }

        // ── Save / Load ───────────────────────────────────────────────

        public PhantomMemoryEngineState CaptureState()
        {
            var copy = new PhantomMemoryEngineState { systemId = _state.systemId };
            var keys = new List<string>(_records.Count);
            foreach (var kv in _records) keys.Add(kv.Key);
            keys.Sort(string.CompareOrdinal);
            for (int i = 0; i < keys.Count; i++)
            {
                var r = _records[keys[i]];
                copy.records.Add(new PhantomMemoryRecord
                {
                    survivorId = r.survivorId,
                    triggersExperienced = r.triggersExperienced,
                    motivationBoostHoursRemaining = r.motivationBoostHoursRemaining,
                    breakdownRefusalHoursRemaining = r.breakdownRefusalHoursRemaining,
                    triggeredItemIds = new List<string>(r.triggeredItemIds),
                    triggeredTriggerIds = new List<string>(r.triggeredTriggerIds)
                });
            }
            return copy;
        }

        public void RestoreState(PhantomMemoryEngineState saved)
        {
            if (saved == null) return;
            _state.systemId = SystemId;
            _records.Clear();
            _state.records.Clear();
            if (saved.records != null)
            {
                foreach (var r in saved.records)
                {
                    if (r == null || string.IsNullOrEmpty(r.survivorId)) continue;
                    var copy = new PhantomMemoryRecord
                    {
                        survivorId = r.survivorId,
                        triggersExperienced = r.triggersExperienced,
                        motivationBoostHoursRemaining = r.motivationBoostHoursRemaining,
                        breakdownRefusalHoursRemaining = r.breakdownRefusalHoursRemaining,
                        triggeredItemIds = r.triggeredItemIds != null
                            ? new List<string>(r.triggeredItemIds)
                            : new List<string>(),
                        triggeredTriggerIds = r.triggeredTriggerIds != null
                            ? new List<string>(r.triggeredTriggerIds)
                            : new List<string>()
                    };
                    _records[r.survivorId] = copy;
                    _state.records.Add(copy);
                }
            }
            RaiseChanged();
        }

        private PhantomMemoryRecord GetOrCreateRecord(string survivorId)
        {
            if (_records.TryGetValue(survivorId, out var existing)) return existing;
            var record = new PhantomMemoryRecord { survivorId = survivorId };
            _records[survivorId] = record;
            _state.records.Add(record);
            return record;
        }

        private void RaiseChanged() => OnStateChanged?.Invoke(_state);

        // ── Category inference (mirrors Unity original + Plan 21 taxonomy) ─

        public static string? GetCategoryFromId(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return null;
            string lower = itemId.ToLowerInvariant();

            if (lower.StartsWith("toy") || lower.StartsWith("child") || lower.Contains("drawing") || lower.Contains("mitten"))
                return "childhood";
            if (lower.Contains("photo") || lower.Contains("album") || lower.Contains("portrait"))
                return "photograph";
            if (lower.Contains("letter") || lower.Contains("mail") || lower.Contains("diary") || lower.Contains("notebook") || lower.Contains("ledger") || lower.Contains("logbook") || lower.Contains("chart"))
                return "correspondence";
            if (lower.Contains("ring") || lower.Contains("watch") || lower.Contains("heirloom") || lower.Contains("lighter") || lower.Contains("medal") || lower.Contains("scarf") || lower.Contains("key"))
                return "personal_item";
            if (lower.Contains("dog_tag") || lower.Contains("military") || lower.Contains("canteen") || lower.Contains("insignia") || lower.Contains("patch"))
                return "military";
            if (lower.Contains("medical") || lower.Contains("bandage") || lower.Contains("pill") || lower.Contains("stethoscope") || lower.Contains("scalpel") || lower.Contains("satchel") || lower.Contains("suture"))
                return "medical";
            if (lower.Contains("whistle") || lower.Contains("caliper") || lower.Contains("micrometer") || lower.Contains("punch") || lower.Contains("slide_rule") || lower.Contains("stamp") || lower.Contains("gloves") || lower.Contains("tag") || lower.Contains("tester"))
                return "work_tool";
            if (lower.Contains("ticket") || lower.Contains("receipt") || lower.Contains("mug") || lower.Contains("comb") || lower.Contains("matchbook") || lower.Contains("charm") || lower.Contains("shopping_list"))
                return "ordinary_object";

            return "generic";
        }
    }

    public enum TriggerOutcome { None, Motivation, Breakdown }
}
