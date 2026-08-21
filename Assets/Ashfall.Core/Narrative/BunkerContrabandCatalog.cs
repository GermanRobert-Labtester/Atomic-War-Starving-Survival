using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class ContrabandMechanics
    {
        [JsonPropertyName("morale_delta")]
        public int MoraleDelta { get; set; }

        [JsonPropertyName("trade_value_bonus")]
        public int TradeValueBonus { get; set; }

        [JsonPropertyName("tribunal_suspicion_rate")]
        public float TribunalSuspicionRate { get; set; }

        [JsonPropertyName("requires_clean_water_liters")]
        public int RequiresCleanWaterLiters { get; set; }

        [JsonPropertyName("radio_range_boost_km")]
        public int RadioRangeBoostKm { get; set; }

        [JsonPropertyName("emp_shielded")]
        public bool EmpShielded { get; set; }

        [JsonPropertyName("scrip_purchasing_falsification")]
        public int ScripPurchasingFalsification { get; set; }

        [JsonPropertyName("blackout_illumination_hours")]
        public int BlackoutIlluminationHours { get; set; }

        [JsonPropertyName("calorie_surplus_kcal")]
        public int CalorieSurplusKcal { get; set; }

        [JsonPropertyName("hunger_restore_value")]
        public int HungerRestoreValue { get; set; }

        [JsonPropertyName("instant_pain_relief_hp")]
        public int InstantPainReliefHp { get; set; }

        [JsonPropertyName("chemical_dependency_risk")]
        public float ChemicalDependencyRisk { get; set; }

        [JsonPropertyName("blast_door_override_success_rate")]
        public float BlastDoorOverrideSuccessRate { get; set; }

        [JsonPropertyName("agricultural_yield_multiplier")]
        public float AgriculturalYieldMultiplier { get; set; }
    }

    public sealed class ContrabandEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("contraband_tier")]
        public int ContrabandTier { get; set; } = 1;

        [JsonPropertyName("risk_profile")]
        public string RiskProfile { get; set; } = string.Empty;

        [JsonPropertyName("market_price_scrip")]
        public int MarketPriceScrip { get; set; }

        [JsonPropertyName("hidden_stash_location")]
        public string HiddenStashLocation { get; set; } = string.Empty;

        [JsonPropertyName("mechanics")]
        public ContrabandMechanics Mechanics { get; set; } = new ContrabandMechanics();

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class BunkerContrabandCatalog
    {
        private readonly Dictionary<string, ContrabandEntry> _entriesById =
            new Dictionary<string, ContrabandEntry>(StringComparer.OrdinalIgnoreCase);
        private readonly List<ContrabandEntry> _allEntries = new List<ContrabandEntry>();

        public int Count => _allEntries.Count;
        public IReadOnlyList<ContrabandEntry> All => _allEntries;

        public static BunkerContrabandCatalog LoadFromJson(string json)
        {
            var catalog = new BunkerContrabandCatalog();
            if (string.IsNullOrWhiteSpace(json)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            var list = JsonSerializer.Deserialize<List<ContrabandEntry>>(json, options);
            if (list != null)
            {
                foreach (var entry in list)
                {
                    if (entry == null || string.IsNullOrWhiteSpace(entry.Id)) continue;
                    catalog._entriesById[entry.Id] = entry;
                    catalog._allEntries.Add(entry);
                }
            }

            return catalog;
        }

        public static BunkerContrabandCatalog LoadFromFile(string path)
        {
            if (!File.Exists(path)) return new BunkerContrabandCatalog();
            string json = File.ReadAllText(path);
            return LoadFromJson(json);
        }

        public ContrabandEntry? GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            _entriesById.TryGetValue(id, out var entry);
            return entry;
        }

        public List<ContrabandEntry> GetByCategory(string category)
        {
            var result = new List<ContrabandEntry>();
            if (string.IsNullOrEmpty(category)) return result;

            for (int i = 0; i < _allEntries.Count; i++)
            {
                if (string.Equals(_allEntries[i].Category, category, StringComparison.OrdinalIgnoreCase))
                    result.Add(_allEntries[i]);
            }
            return result;
        }

        public List<ContrabandEntry> GetByTier(int tier)
        {
            var result = new List<ContrabandEntry>();
            for (int i = 0; i < _allEntries.Count; i++)
            {
                if (_allEntries[i].ContrabandTier == tier)
                    result.Add(_allEntries[i]);
            }
            return result;
        }

        public List<ContrabandEntry> GetByTag(string tag)
        {
            var result = new List<ContrabandEntry>();
            if (string.IsNullOrEmpty(tag)) return result;

            for (int i = 0; i < _allEntries.Count; i++)
            {
                var entry = _allEntries[i];
                if (entry.Tags != null)
                {
                    for (int t = 0; t < entry.Tags.Count; t++)
                    {
                        if (string.Equals(entry.Tags[t], tag, StringComparison.OrdinalIgnoreCase))
                        {
                            result.Add(entry);
                            break;
                        }
                    }
                }
            }
            return result;
        }
    }
}
