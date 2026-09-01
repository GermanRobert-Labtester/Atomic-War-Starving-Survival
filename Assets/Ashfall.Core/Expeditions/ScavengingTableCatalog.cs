using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ashfall.Core.Expeditions
{
    /// <summary>
    /// Root container for scavenging_tables.json deserialization.
    /// </summary>
    [Serializable]
    public sealed class ScavengingTableCatalogContainer
    {
        public int schema_version { get; set; } = 1;
        public string collection_id { get; set; } = "scavenging_tables_catalog";
        public List<ScavengingTableDef> tables { get; set; } = new List<ScavengingTableDef>();
    }

    /// <summary>
    /// Authoritative definition of a location-specific weighted scavenging loot table (Plan 46).
    /// </summary>
    [Serializable]
    public sealed class ScavengingTableDef
    {
        public string id { get; set; } = string.Empty;
        public string location_type { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string depletion_model { get; set; } = "finite"; // finite, renewable, one_time, slow_regeneration
        public float base_hazard_chance { get; set; } = 0.0f;
        public string primary_hazard_type { get; set; } = string.Empty;
        public List<ScavengingLootEntryDef> entries { get; set; } = new List<ScavengingLootEntryDef>();

        public int TotalWeight => entries?.Sum(e => Math.Max(0, e.weight)) ?? 0;
    }

    /// <summary>
    /// A single weighted entry in a scavenging loot table.
    /// </summary>
    [Serializable]
    public sealed class ScavengingLootEntryDef
    {
        public string item_id { get; set; } = string.Empty;
        public int weight { get; set; } = 10;
        public int min_quantity { get; set; } = 1;
        public int max_quantity { get; set; } = 1;
        public string rarity_tier { get; set; } = "common"; // common, uncommon, rare, unique
        public float hazard_chance { get; set; } = 0.0f;
        public string hazard_type { get; set; } = string.Empty;
        public string codex_unlock_id { get; set; } = string.Empty;
    }

    /// <summary>
    /// Result outcome from resolving a weighted scavenging roll.
    /// </summary>
    public sealed class ScavengingRollResult
    {
        public string TableId { get; set; } = string.Empty;
        public string ItemId { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
        public string RarityTier { get; set; } = "common";
        public bool HazardTriggered { get; set; }
        public string HazardType { get; set; } = string.Empty;
        public string CodexUnlockId { get; set; } = string.Empty;
    }

    /// <summary>
    /// Engine-agnostic domain catalog for 20 location-specific scavenging loot tables (Plan 46).
    /// </summary>
    public sealed class ScavengingTableCatalog
    {
        public const string DefaultFileName = "scavenging_tables.json";

        private readonly List<ScavengingTableDef> _tables;
        private readonly Dictionary<string, ScavengingTableDef> _tablesById;
        private readonly Dictionary<string, ScavengingTableDef> _tablesByLocationType;

        public IReadOnlyList<ScavengingTableDef> Tables => _tables;
        public int TableCount => _tables.Count;

        public ScavengingTableCatalog(IEnumerable<ScavengingTableDef> tables)
        {
            _tables = tables?.Where(t => t != null && !string.IsNullOrEmpty(t.id)).ToList() ?? new List<ScavengingTableDef>();
            _tablesById = new Dictionary<string, ScavengingTableDef>(StringComparer.OrdinalIgnoreCase);
            _tablesByLocationType = new Dictionary<string, ScavengingTableDef>(StringComparer.OrdinalIgnoreCase);

            foreach (var table in _tables)
            {
                _tablesById[table.id] = table;
                if (!string.IsNullOrEmpty(table.location_type))
                {
                    _tablesByLocationType[table.location_type] = table;
                }
            }
        }

        public bool TryGetTable(string tableId, out ScavengingTableDef table)
        {
            return _tablesById.TryGetValue(tableId, out table!);
        }

        public bool TryGetTableByLocationType(string locationType, out ScavengingTableDef table)
        {
            return _tablesByLocationType.TryGetValue(locationType, out table!);
        }

        /// <summary>
        /// Deterministically rolls an item from the specified scavenging table using the provided ISeededRng.
        /// </summary>
        public ScavengingRollResult? RollLoot(string tableId, ISeededRng rng)
        {
            if (rng == null || string.IsNullOrEmpty(tableId)) return null;
            if (!TryGetTable(tableId, out var table) || table.entries == null || table.entries.Count == 0)
                return null;

            int totalWeight = table.TotalWeight;
            if (totalWeight <= 0) return null;

            int roll = rng.Next(0, totalWeight);
            int cumulative = 0;
            ScavengingLootEntryDef? selected = null;

            for (int i = 0; i < table.entries.Count; i++)
            {
                var entry = table.entries[i];
                if (entry.weight <= 0) continue;
                cumulative += entry.weight;
                if (roll < cumulative)
                {
                    selected = entry;
                    break;
                }
            }

            if (selected == null)
            {
                selected = table.entries.FirstOrDefault(e => e.weight > 0) ?? table.entries[0];
            }

            int quantity = selected.min_quantity;
            if (selected.max_quantity > selected.min_quantity)
            {
                quantity = rng.Next(selected.min_quantity, selected.max_quantity + 1);
            }

            bool hazardTriggered = false;
            string hazardType = string.Empty;

            float hazardRoll = (float)rng.NextDouble();
            float effectiveHazardChance = selected.hazard_chance > 0 ? selected.hazard_chance : table.base_hazard_chance;

            if (effectiveHazardChance > 0 && hazardRoll < effectiveHazardChance)
            {
                hazardTriggered = true;
                hazardType = !string.IsNullOrEmpty(selected.hazard_type) ? selected.hazard_type : table.primary_hazard_type;
            }

            return new ScavengingRollResult
            {
                TableId = table.id,
                ItemId = selected.item_id,
                Quantity = Math.Max(1, quantity),
                RarityTier = selected.rarity_tier,
                HazardTriggered = hazardTriggered,
                HazardType = hazardType,
                CodexUnlockId = selected.codex_unlock_id ?? string.Empty
            };
        }

        public static ScavengingTableCatalog LoadFromDirectory(string dataDirectory, IFileIO fileIO, IJsonSerializer? jsonSerializer = null)
        {
            if (fileIO == null) throw new ArgumentNullException(nameof(fileIO));
            string path = Path.Combine(dataDirectory, DefaultFileName);
            if (!fileIO.FileExists(path))
            {
                return new ScavengingTableCatalog(Enumerable.Empty<ScavengingTableDef>());
            }

            string json = fileIO.ReadAllText(path);
            return LoadFromJson(json, jsonSerializer);
        }

        public static ScavengingTableCatalog LoadFromJson(string json, IJsonSerializer? jsonSerializer = null)
        {
            var serializer = jsonSerializer ?? new SystemTextJsonSerializer();
            var container = serializer.Deserialize<ScavengingTableCatalogContainer>(json);
            if (container == null)
            {
                return new ScavengingTableCatalog(Enumerable.Empty<ScavengingTableDef>());
            }

            return new ScavengingTableCatalog(container.tables);
        }
    }
}
