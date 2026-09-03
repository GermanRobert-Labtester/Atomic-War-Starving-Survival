using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class TravelEncounterChoice
    {
        [JsonPropertyName("choice_id")]
        public string ChoiceId { get; set; } = string.Empty;

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        [JsonPropertyName("is_nonviolent")]
        public bool IsNonviolent { get; set; } = true;

        [JsonPropertyName("is_avoidance")]
        public bool IsAvoidance { get; set; } = false;

        [JsonPropertyName("morale_delta")]
        public int MoraleDelta { get; set; } = 0;

        [JsonPropertyName("guilt_delta")]
        public int GuiltDelta { get; set; } = 0;

        [JsonPropertyName("unlocks_field_guide_id")]
        public string UnlocksFieldGuideId { get; set; } = string.Empty;

        [JsonPropertyName("advances_chain_stage")]
        public int AdvancesChainStage { get; set; } = 0;

        // Plan 45 — Faction patrol extensions (backward-compatible defaults)
        [JsonPropertyName("faction_id")]
        public string FactionId { get; set; } = string.Empty;

        [JsonPropertyName("faction_standing_delta")]
        public int FactionStandingDelta { get; set; } = 0;

        [JsonPropertyName("cost_items")]
        public List<string> CostItems { get; set; } = new List<string>();

        [JsonPropertyName("required_item_id")]
        public string RequiredItemId { get; set; } = string.Empty;

        [JsonPropertyName("required_item_quantity")]
        public int RequiredItemQuantity { get; set; } = 0;

        // Plan 51 — Document flag integration (backward-compatible default)
        /// <summary>
        /// World flag that must be set for this choice to be available.
        /// Used to gate patrol encounter choices on document discovery.
        /// Empty = always available. Plan 51 integration point.
        /// </summary>
        [JsonPropertyName("required_flag")]
        public string RequiredFlag { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class TravelEncounterDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty; // "Creature", "Human", "Environmental", "Chained"

        /// <summary>
        /// Plan 45 phase 2 — wildlife combat binding for Creature encounters.
        /// Maps to a fauna/mutant combatant id via
        /// <see cref="Ashfall.Core.Combat.EnemyCompositionSelector.SelectWildlifeComposition"/>
        /// (pack_canine / swarm / lurker / spore_predator / charger / apex).
        /// Empty on every non-Creature row: hostile choices on Human
        /// encounters route through the raid/ambush composition instead.
        /// </summary>
        [JsonPropertyName("combatant_tag")]
        public string CombatantTag { get; set; } = string.Empty;

        [JsonPropertyName("chain_id")]
        public string ChainId { get; set; } = string.Empty;

        [JsonPropertyName("chain_stage")]
        public int ChainStage { get; set; } = 0;

        [JsonPropertyName("prereq_chain_stage")]
        public int PrereqChainStage { get; set; } = 0;

        [JsonPropertyName("region_tags")]
        public List<string> RegionTags { get; set; } = new List<string>();

        [JsonPropertyName("min_danger_level")]
        public float MinDangerLevel { get; set; } = 0.0f;

        [JsonPropertyName("max_danger_level")]
        public float MaxDangerLevel { get; set; } = 5.0f;

        [JsonPropertyName("base_weight")]
        public float BaseWeight { get; set; } = 1.0f;

        [JsonPropertyName("stance_weights")]
        public Dictionary<string, float> StanceWeights { get; set; } = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);

        [JsonPropertyName("season_tags")]
        public List<string> SeasonTags { get; set; } = new List<string>();

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("choices")]
        public List<TravelEncounterChoice> Choices { get; set; } = new List<TravelEncounterChoice>();

        // Plan 45 — Faction patrol extensions (backward-compatible defaults)
        [JsonPropertyName("faction_id")]
        public string FactionId { get; set; } = string.Empty;

        [JsonPropertyName("territory_state")]
        public string TerritoryState { get; set; } = string.Empty; // "controlled", "contested", "border"
    }

    [Serializable]
    public sealed class TravelEncounterCatalogData
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("collection_id")]
        public string CollectionId { get; set; } = string.Empty;

        [JsonPropertyName("encounters")]
        public List<TravelEncounterDefinition> Encounters { get; set; } = new List<TravelEncounterDefinition>();
    }

    [Serializable]
    public sealed class TravelEncounterState
    {
        [JsonPropertyName("chain_stages")]
        public Dictionary<string, int> ChainStages { get; set; } = new Dictionary<string, int>();

        [JsonPropertyName("encounter_cooldowns")]
        public Dictionary<string, int> EncounterAvailableDay { get; set; } = new Dictionary<string, int>();
    }

    public sealed class TravelEncounterCatalog
    {
        private readonly Dictionary<string, TravelEncounterDefinition> _encountersById = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<TravelEncounterDefinition> _allEncounters = new();

        public int Count => _allEncounters.Count;
        public IReadOnlyList<TravelEncounterDefinition> Encounters => _allEncounters;

        public static TravelEncounterCatalog LoadFromJson(string json)
        {
            var catalog = new TravelEncounterCatalog();
            if (string.IsNullOrWhiteSpace(json)) return catalog;

            var data = JsonSerializer.Deserialize<TravelEncounterCatalogData>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (data?.Encounters != null)
            {
                foreach (var encounter in data.Encounters)
                {
                    if (string.IsNullOrEmpty(encounter.Id)) continue;
                    catalog._encountersById[encounter.Id] = encounter;
                    catalog._allEncounters.Add(encounter);
                }
            }

            return catalog;
        }

        public static TravelEncounterCatalog LoadFromDirectory(string directoryPath, IFileIO fileIO)
        {
            if (string.IsNullOrEmpty(directoryPath) || fileIO == null) return new TravelEncounterCatalog();
            string path = Path.Combine(directoryPath, "travel_encounters.json");
            if (!fileIO.FileExists(path)) return new TravelEncounterCatalog();
            return LoadFromJson(fileIO.ReadAllText(path));
        }

        public bool TryGetEncounter(string id, out TravelEncounterDefinition encounter)
        {
            if (string.IsNullOrEmpty(id))
            {
                encounter = null!;
                return false;
            }
            return _encountersById.TryGetValue(id, out encounter!);
        }

        public TravelEncounterDefinition? GetEncounter(string id)
        {
            TryGetEncounter(id, out var enc);
            return enc;
        }
    }
}
