// SPDX-License-Identifier: MIT
// Plan 146: Radiation → Economy Bridge
// Pure domain authority connecting radiological contamination to trade restrictions, price modifiers, and faction policies.

using System;
using System.Collections.Generic;
using Ashfall.Core;

namespace Ashfall.Core.Radiation
{
    [Serializable]
    public sealed class ContaminationTradeRule
    {
        public string category { get; set; } = string.Empty;
        public int min_contamination { get; set; }
        public int max_contamination { get; set; }
        public float price_multiplier { get; set; } = 1.0f;
        public bool trade_blocked { get; set; }
        public string restriction_label { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class FactionContaminationPolicy
    {
        public string faction_id { get; set; } = string.Empty;
        public int max_acceptable_contamination { get; set; } = 100;
        public bool reject_irradiated_personnel { get; set; }
        public int standing_penalty_per_violation { get; set; }
        public string policy_notes { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class TradeEvaluation
    {
        public string itemId { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
        public int contaminationLevel { get; set; }
        public float basePrice { get; set; }
        public float finalPrice { get; set; }
        public float priceMultiplier { get; set; } = 1.0f;
        public bool isBlocked { get; set; }
        public string blockReason { get; set; } = string.Empty;
        public string targetFactionId { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class RadiationEconomySaveState
    {
        public int schema_version { get; set; } = 1;
        public int totalEvaluations { get; set; }
        public int totalBlockedTrades { get; set; }
    }

    public interface IRadiationEconomySink
    {
        void OnTradeEvaluated(TradeEvaluation evaluation);
    }

    public sealed class RadiationEconomyBridge
    {
        private readonly List<ContaminationTradeRule> _rules = new();
        private readonly Dictionary<string, FactionContaminationPolicy> _factionPolicies = new(StringComparer.OrdinalIgnoreCase);
        private int _totalEvaluations;
        private int _totalBlockedTrades;

        // Seams
        public Action<string, string, float>? OnRadiationPriceModifiedSeam; // itemId, factionId, multiplier
        public Action<string, string, string>? OnContaminatedTradeBlockedSeam; // itemId, factionId, reason
        public IRadiationEconomySink? Sink { get; set; }

        public IReadOnlyList<ContaminationTradeRule> Rules => _rules;
        public int TotalEvaluations => _totalEvaluations;
        public int TotalBlockedTrades => _totalBlockedTrades;

        public RadiationEconomyBridge()
        {
            LoadDefaultRules();
        }

        public void LoadCatalog(string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent)) return;

            _rules.Clear();
            _factionPolicies.Clear();

            // Extract economy_rules
            ParseEconomyRules(jsonContent);
            ParseFactionPolicies(jsonContent);
        }

        public void LoadCatalog(IFileIO fileIO, string path)
        {
            if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
            if (!fileIO.FileExists(path))
                throw new System.IO.FileNotFoundException($"Radiation economy catalog not found at {path}");

            LoadCatalog(fileIO.ReadAllText(path));
        }

        public TradeEvaluation EvaluateTrade(string itemId, string category, int contaminationLevel, float basePrice, string? targetFactionId = null)
        {
            _totalEvaluations++;
            contaminationLevel = Math.Max(0, Math.Min(100, contaminationLevel));

            var evaluation = new TradeEvaluation
            {
                itemId = itemId,
                category = category,
                contaminationLevel = contaminationLevel,
                basePrice = basePrice,
                targetFactionId = targetFactionId ?? string.Empty
            };

            // 1. Check general category rule
            ContaminationTradeRule? matchedRule = null;
            for (int i = 0; i < _rules.Count; i++)
            {
                var r = _rules[i];
                if (string.Equals(r.category, category, StringComparison.OrdinalIgnoreCase) &&
                    contaminationLevel >= r.min_contamination &&
                    contaminationLevel <= r.max_contamination)
                {
                    matchedRule = r;
                    break;
                }
            }

            if (matchedRule != null)
            {
                evaluation.priceMultiplier = matchedRule.price_multiplier;
                evaluation.isBlocked = matchedRule.trade_blocked;
                evaluation.blockReason = matchedRule.restriction_label;
            }
            else
            {
                evaluation.priceMultiplier = 1.0f;
                evaluation.isBlocked = false;
                evaluation.blockReason = string.Empty;
            }

            // 2. Check faction policy override
            if (!string.IsNullOrEmpty(targetFactionId) && _factionPolicies.TryGetValue(targetFactionId, out var policy))
            {
                if (contaminationLevel > policy.max_acceptable_contamination)
                {
                    evaluation.isBlocked = true;
                    evaluation.blockReason = $"Contamination ({contaminationLevel}%) exceeds {policy.faction_id} tolerance limit ({policy.max_acceptable_contamination}%). {policy.policy_notes}";
                }
            }

            evaluation.finalPrice = (float)Math.Round(basePrice * evaluation.priceMultiplier, 2);

            if (evaluation.isBlocked)
            {
                _totalBlockedTrades++;
                OnContaminatedTradeBlockedSeam?.Invoke(itemId, targetFactionId ?? "unknown", evaluation.blockReason);
            }
            else if (evaluation.priceMultiplier != 1.0f)
            {
                OnRadiationPriceModifiedSeam?.Invoke(itemId, targetFactionId ?? "unknown", evaluation.priceMultiplier);
            }

            Sink?.OnTradeEvaluated(evaluation);

            return evaluation;
        }

        public RadiationEconomySaveState CaptureState()
        {
            return new RadiationEconomySaveState
            {
                schema_version = 1,
                totalEvaluations = _totalEvaluations,
                totalBlockedTrades = _totalBlockedTrades
            };
        }

        public void RestoreState(RadiationEconomySaveState? state)
        {
            if (state == null)
            {
                _totalEvaluations = 0;
                _totalBlockedTrades = 0;
                return;
            }

            _totalEvaluations = state.totalEvaluations;
            _totalBlockedTrades = state.totalBlockedTrades;
        }

        private void LoadDefaultRules()
        {
            _rules.Add(new ContaminationTradeRule { category = "food_water", min_contamination = 1, max_contamination = 30, price_multiplier = 0.70f, trade_blocked = false, restriction_label = "Tainted provisions: 30% discount." });
            _rules.Add(new ContaminationTradeRule { category = "food_water", min_contamination = 31, max_contamination = 60, price_multiplier = 0.40f, trade_blocked = false, restriction_label = "Hot provisions: 60% discount." });
            _rules.Add(new ContaminationTradeRule { category = "food_water", min_contamination = 61, max_contamination = 100, price_multiplier = 0.10f, trade_blocked = true, restriction_label = "Lethal dose goods: strictly quarantined." });
            _rules.Add(new ContaminationTradeRule { category = "equipment", min_contamination = 1, max_contamination = 40, price_multiplier = 0.80f, trade_blocked = false, restriction_label = "Irradiated salvage: 20% discount." });
            _rules.Add(new ContaminationTradeRule { category = "equipment", min_contamination = 41, max_contamination = 80, price_multiplier = 0.50f, trade_blocked = false, restriction_label = "Heavy contamination: 50% discount." });
            _rules.Add(new ContaminationTradeRule { category = "equipment", min_contamination = 81, max_contamination = 100, price_multiplier = 0.20f, trade_blocked = true, restriction_label = "Gamma-emitting gear: refused by station guards." });
            _rules.Add(new ContaminationTradeRule { category = "medical_supplies", min_contamination = 1, max_contamination = 100, price_multiplier = 0.05f, trade_blocked = true, restriction_label = "Contaminated medical supplies: biohazard block." });
            _rules.Add(new ContaminationTradeRule { category = "hot_zone_scarcity", min_contamination = 0, max_contamination = 0, price_multiplier = 1.50f, trade_blocked = false, restriction_label = "Certified pristine hot-zone relic: +50% premium." });

            _factionPolicies["faction_garrison"] = new FactionContaminationPolicy { faction_id = "faction_garrison", max_acceptable_contamination = 25, reject_irradiated_personnel = true, standing_penalty_per_violation = 5, policy_notes = "Military protocol requires clean supplies." };
            _factionPolicies["faction_hydro_barons"] = new FactionContaminationPolicy { faction_id = "faction_hydro_barons", max_acceptable_contamination = 10, reject_irradiated_personnel = true, standing_penalty_per_violation = 10, policy_notes = "Water purifiers strictly enforce zero-tolerance." };
            _factionPolicies["faction_rebels"] = new FactionContaminationPolicy { faction_id = "faction_rebels", max_acceptable_contamination = 65, reject_irradiated_personnel = false, standing_penalty_per_violation = 2, policy_notes = "Desperate guerillas trade for contaminated goods." };
            _factionPolicies["faction_scavengers"] = new FactionContaminationPolicy { faction_id = "faction_scavengers", max_acceptable_contamination = 85, reject_irradiated_personnel = false, standing_penalty_per_violation = 0, policy_notes = "Waste scavengers trade anything holding value." };
            _factionPolicies["faction_cult_of_ash"] = new FactionContaminationPolicy { faction_id = "faction_cult_of_ash", max_acceptable_contamination = 100, reject_irradiated_personnel = false, standing_penalty_per_violation = 0, policy_notes = "The ash zealots venerate irradiated relics." };
        }

        private void ParseEconomyRules(string json)
        {
            int idx = json.IndexOf("\"economy_rules\"", StringComparison.Ordinal);
            if (idx < 0) return;
            int arrStart = json.IndexOf('[', idx);
            if (arrStart < 0) return;
            int arrEnd = json.IndexOf(']', arrStart);
            if (arrEnd < 0) return;

            string slice = json.Substring(arrStart + 1, arrEnd - arrStart - 1);
            var blocks = SplitObjects(slice);
            foreach (var b in blocks)
            {
                var r = new ContaminationTradeRule
                {
                    category = ExtractString(b, "category"),
                    min_contamination = ExtractInt(b, "min_contamination", 0),
                    max_contamination = ExtractInt(b, "max_contamination", 100),
                    price_multiplier = ExtractFloat(b, "price_multiplier", 1.0f),
                    trade_blocked = ExtractBool(b, "trade_blocked", false),
                    restriction_label = ExtractString(b, "restriction_label")
                };
                if (!string.IsNullOrEmpty(r.category))
                {
                    _rules.Add(r);
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
