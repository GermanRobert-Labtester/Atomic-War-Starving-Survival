using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Phantom Memory System — when survivors scavenge everyday pre-war objects,
    /// background-specific memories trigger psychological breaks or motivation
    /// bursts. Tied to Survivor.PhantomBackgroundId.
    ///
    /// Plain C#, leaf assembly (no Core/Inventory refs). Host injects item-category
    /// lookup and notification callbacks.
    /// </summary>
    public class PhantomMemorySystem
    {
        // ── Constants ──────────────────────────────────────────────────
        public const float MotivationBoostDurationHours = 8f;
        public const float MotivationMoraleBoost = 15f;
        public const float MotivationWorkSpeedBonus = 0.20f;
        public const float BreakdownMoraleDrop = -20f;
        public const float BreakdownWorkRefusalHours = 4f;
        public const float BaseTriggerChance = 0.15f;

        // ── Events ─────────────────────────────────────────────────────
        public event Action<Survivor, string, bool> OnPhantomTrigger;
        // sv, itemId, isMotivation
        public event Action<Survivor, string> OnPhantomBreakdown;

        // ── State ──────────────────────────────────────────────────────
        public readonly Dictionary<string, List<PhantomTriggerRule>> RulesByBackground =
            new Dictionary<string, List<PhantomTriggerRule>>();

        // ── Host hooks ─────────────────────────────────────────────────
        public Func<Survivor, string> GetItemCategory;
        // itemId → category
        public Action<Survivor, float> ApplyMoraleDelta;
        public Action<Survivor, float> SetWorkEfficiencyMultiplier;
        public Action<Survivor, float> SetWorkRefusalHours;
        public System.Random Rng;

        /// <summary>
        /// Register a trigger rule: when a survivor with backgroundId finds an item
        /// of matching category, roll motivationChance for motivation vs breakdown.
        /// </summary>
        public void RegisterRule(string backgroundId, string itemCategory,
            float motivationChance, string descriptionKey,
            string motivationText = null, string breakdownText = null)
        {
            if (!RulesByBackground.TryGetValue(backgroundId, out var rules))
            {
                rules = new List<PhantomTriggerRule>();
                RulesByBackground[backgroundId] = rules;
            }
            rules.Add(new PhantomTriggerRule
            {
                ItemCategory = itemCategory,
                MotivationChance = motivationChance,
                DescriptionKey = descriptionKey,
                MotivationText = motivationText,
                BreakdownText = breakdownText
            });
        }

        /// <summary>
        /// Resolve vignette narrative text for a phantom trigger outcome.
        /// </summary>
        public string ResolveTriggerText(Survivor sv, string itemId, bool isMotivation)
        {
            if (sv == null) return string.Empty;
            string category = GetCategoryFromId(itemId);
            if (string.IsNullOrEmpty(category)) return string.Empty;
            if (!RulesByBackground.TryGetValue(sv.PhantomBackgroundId, out var rules))
                return string.Empty;

            for (int i = 0; i < rules.Count; i++)
            {
                var rule = rules[i];
                if (!string.Equals(rule.ItemCategory, category, StringComparison.OrdinalIgnoreCase))
                    continue;
                string template = isMotivation ? rule.MotivationText : rule.BreakdownText;
                if (string.IsNullOrEmpty(template)) return string.Empty;
                string name = string.IsNullOrEmpty(sv.DisplayName) ? "Someone" : sv.DisplayName;
                return template.Replace("{name}", name);
            }
            return string.Empty;
        }

        /// <summary>
        /// Called when a survivor discovers/acquires an item during scavenging.
        /// Checks for phantom memory triggers based on background + item category.
        /// </summary>
        public void OnItemScavenged(Survivor sv, string itemId)
        {
            if (sv == null || !sv.IsAlive || string.IsNullOrEmpty(sv.PhantomBackgroundId))
                return;
            if (string.IsNullOrEmpty(itemId)) return;

            string category = GetItemCategory?.Invoke(sv) ?? GetCategoryFromId(itemId);
            if (string.IsNullOrEmpty(category)) return;

            if (!RulesByBackground.TryGetValue(sv.PhantomBackgroundId, out var rules))
                return;

            for (int i = 0; i < rules.Count; i++)
            {
                var rule = rules[i];
                if (!string.Equals(rule.ItemCategory, category,
                    System.StringComparison.OrdinalIgnoreCase))
                    continue;

                float roll = (float)(Rng?.NextDouble() ?? 0.5);
                float adjustedChance = BaseTriggerChance *
                    (1f + sv.PhantomTriggersExperienced * 0.1f);
                if (roll >= adjustedChance) return;

                sv.PhantomTriggersExperienced++;

                if (roll < rule.MotivationChance * adjustedChance)
                {
                    // Motivation boost
                    sv.PhantomMotivationBoostHours = MotivationBoostDurationHours;
                    ApplyMoraleDelta?.Invoke(sv, MotivationMoraleBoost);
                    SetWorkEfficiencyMultiplier?.Invoke(sv, 1f + MotivationWorkSpeedBonus);
                    OnPhantomTrigger?.Invoke(sv, itemId, true);
                }
                else
                {
                    // Psychological breakdown
                    ApplyMoraleDelta?.Invoke(sv, BreakdownMoraleDrop);
                    SetWorkRefusalHours?.Invoke(sv, BreakdownWorkRefusalHours);
                    OnPhantomTrigger?.Invoke(sv, itemId, false);
                    OnPhantomBreakdown?.Invoke(sv, itemId);
                }
                return;
            }
        }

        /// <summary>
        /// Tick — decay motivation boost timer.
        /// </summary>
        public void Tick(Survivor sv, float gameHours)
        {
            if (sv == null || sv.PhantomMotivationBoostHours <= 0f) return;
            sv.PhantomMotivationBoostHours -= gameHours;
            if (sv.PhantomMotivationBoostHours <= 0f)
            {
                sv.PhantomMotivationBoostHours = 0f;
                SetWorkEfficiencyMultiplier?.Invoke(sv, 1f); // reset to normal
            }
        }

        private static string GetCategoryFromId(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return null;
            // Simple prefix-based category extraction
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

    /// <summary>One trigger rule mapping background + item category → outcome.</summary>
    [System.Serializable]
    public class PhantomTriggerRule
    {
        public string ItemCategory;
        public float MotivationChance;
        public string DescriptionKey;
        public string MotivationText;
        public string BreakdownText;
    }
}
