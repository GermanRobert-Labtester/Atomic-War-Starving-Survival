using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    /// <summary>
    /// Engine-agnostic phantom memory system (Antigravity #41, Master Plan §V).
    /// When survivors scavenge everyday pre-war objects, background-specific
    /// memories trigger psychological breaks or motivation bursts.
    ///
    /// Ported from Unity's Assets/_Game/Survivors/PhantomMemorySystem.cs.
    /// Replaces the Unity Survivor class with a PhantomSurvivorSnapshot POCO
    /// and stores all mutable state internally (no field injection on a
    /// Survivor object). Uses ISeededRng for deterministic rolls.
    /// Zero references to UnityEngine or Godot.
    /// </summary>

    // ── Snapshot types ───────────────────────────────────────────────

    /// <summary>Minimal survivor data the phantom memory system reads.</summary>
    public class PhantomSurvivorSnapshot
    {
        public string survivorId;
        public string displayName;
        public string backgroundId;
        public bool isAlive;
    }

    /// <summary>Mutable per-survivor phantom memory state.</summary>
    [Serializable]
    public class PhantomMemoryRecord
    {
        public string survivorId;
        public int triggersExperienced;
        public float motivationBoostHoursRemaining;
        public List<string> triggeredItemIds = new List<string>();
    }

    [Serializable]
    public class PhantomMemoryEngineState
    {
        public string systemId = PhantomMemoryEngine.SystemId;
        public List<PhantomMemoryRecord> records = new List<PhantomMemoryRecord>();
    }

    /// <summary>One trigger rule: background + item category → outcome.</summary>
    [Serializable]
    public class PhantomTriggerRule
    {
        public string itemCategory;
        public float motivationChance;
        public string descriptionKey;
        public string motivationText;
        public string breakdownText;
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
        public event Action<string, string, bool> OnPhantomTriggered;
        /// <summary>SurvivorId, itemId — a breakdown specifically.</summary>
        public event Action<string, string> OnPhantomBreakdown;
        public event Action<PhantomMemoryEngineState> OnStateChanged;

        // ── State ──────────────────────────────────────────────────────
        private readonly PhantomMemoryEngineState _state = new PhantomMemoryEngineState();
        private readonly Dictionary<string, PhantomMemoryRecord> _records = new Dictionary<string, PhantomMemoryRecord>();
        private readonly Dictionary<string, List<PhantomTriggerRule>> _rulesByBackground = new Dictionary<string, List<PhantomTriggerRule>>();

        public IReadOnlyList<PhantomTriggerRule> GetRules(string backgroundId) =>
            _rulesByBackground.TryGetValue(backgroundId, out var rules) ? rules : new List<PhantomTriggerRule>();

        public IReadOnlyList<PhantomMemoryRecord> Records => _state.records;

        // ── Rule registration ─────────────────────────────────────────

        public void RegisterRule(string backgroundId, string itemCategory,
            float motivationChance, string descriptionKey,
            string motivationText = null, string breakdownText = null)
        {
            if (!_rulesByBackground.TryGetValue(backgroundId, out var rules))
            {
                rules = new List<PhantomTriggerRule>();
                _rulesByBackground[backgroundId] = rules;
            }
            rules.Add(new PhantomTriggerRule
            {
                itemCategory = itemCategory,
                motivationChance = motivationChance,
                descriptionKey = descriptionKey,
                motivationText = motivationText,
                breakdownText = breakdownText
            });
        }

        // ── Core mechanic ─────────────────────────────────────────────

        /// <summary>
        /// Called when a survivor discovers an item. Rolls for a phantom memory
        /// trigger based on background + item category.
        /// </summary>
        public TriggerOutcome OnItemScavenged(PhantomSurvivorSnapshot sv, string itemId, ISeededRng rng)
        {
            if (sv == null || !sv.isAlive || string.IsNullOrEmpty(sv.backgroundId) || string.IsNullOrEmpty(itemId))
                return TriggerOutcome.None;

            string category = GetCategoryFromId(itemId);
            if (string.IsNullOrEmpty(category)) return TriggerOutcome.None;

            if (!_rulesByBackground.TryGetValue(sv.backgroundId, out var rules))
                return TriggerOutcome.None;

            for (int i = 0; i < rules.Count; i++)
            {
                var rule = rules[i];
                if (!string.Equals(rule.itemCategory, category, StringComparison.OrdinalIgnoreCase))
                    continue;

                var record = GetOrCreateRecord(sv.survivorId);
                double roll = (double)rng.NextFloat();
                float effectiveBase = TriggerChanceOverride >= 0f ? TriggerChanceOverride : BaseTriggerChance;
                double adjustedChance = effectiveBase * (1f + record.triggersExperienced * 0.1f);
                if (roll >= adjustedChance) return TriggerOutcome.None;

                record.triggersExperienced++;
                record.triggeredItemIds.Add(itemId);

                if (roll < rule.motivationChance * adjustedChance)
                {
                    record.motivationBoostHoursRemaining = MotivationBoostDurationHours;
                    OnPhantomTriggered?.Invoke(sv.survivorId, itemId, true);
                    RaiseChanged();
                    return TriggerOutcome.Motivation;
                }
                else
                {
                    record.motivationBoostHoursRemaining = 0f;
                    OnPhantomTriggered?.Invoke(sv.survivorId, itemId, false);
                    OnPhantomBreakdown?.Invoke(sv.survivorId, itemId);
                    RaiseChanged();
                    return TriggerOutcome.Breakdown;
                }
            }
            return TriggerOutcome.None;
        }

        /// <summary>Decay motivation boost timers.</summary>
        public void TickHour(string survivorId, float gameHours)
        {
            if (!_records.TryGetValue(survivorId, out var record)) return;
            if (record.motivationBoostHoursRemaining <= 0f) return;
            record.motivationBoostHoursRemaining -= gameHours;
            if (record.motivationBoostHoursRemaining <= 0f)
            {
                record.motivationBoostHoursRemaining = 0f;
                RaiseChanged();
            }
        }

        /// <summary>Resolve vignette text for a trigger outcome.</summary>
        public string ResolveTriggerText(PhantomSurvivorSnapshot sv, string itemId, bool isMotivation)
        {
            if (sv == null || string.IsNullOrEmpty(sv.backgroundId)) return string.Empty;
            string category = GetCategoryFromId(itemId);
            if (string.IsNullOrEmpty(category)) return string.Empty;
            if (!_rulesByBackground.TryGetValue(sv.backgroundId, out var rules)) return string.Empty;

            for (int i = 0; i < rules.Count; i++)
            {
                var rule = rules[i];
                if (!string.Equals(rule.itemCategory, category, StringComparison.OrdinalIgnoreCase))
                    continue;
                string template = isMotivation ? rule.motivationText : rule.breakdownText;
                if (string.IsNullOrEmpty(template)) return string.Empty;
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

        // ── Save / Load ───────────────────────────────────────────────

        public PhantomMemoryEngineState CaptureState()
        {
            // Fresh copy, ordinal-ordered: the live records dict must never be
            // returned to the envelope (aliasing), and dictionary iteration
            // order is not a cross-host guarantee, so the record list is
            // emitted sorted by survivor id.
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
                    triggeredItemIds = new List<string>(r.triggeredItemIds)
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
                        triggeredItemIds = r.triggeredItemIds != null
                            ? new List<string>(r.triggeredItemIds)
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

        // ── Category inference (mirrors Unity original) ───────────────

        private static string GetCategoryFromId(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return null;
            if (itemId.StartsWith("toy") || itemId.StartsWith("child")) return "childhood";
            if (itemId.Contains("photo") || itemId.Contains("album")) return "photograph";
            if (itemId.Contains("letter") || itemId.Contains("mail") || itemId.Contains("diary"))
                return "correspondence";
            if (itemId.Contains("ring") || itemId.Contains("watch") || itemId.Contains("heirloom"))
                return "personal_item";
            if (itemId.Contains("dog_tag") || itemId.Contains("military")) return "military";
            if (itemId.Contains("medical") || itemId.Contains("bandage") || itemId.Contains("pill"))
                return "medical";
            return "generic";
        }
    }

    public enum TriggerOutcome { None, Motivation, Breakdown }
}
