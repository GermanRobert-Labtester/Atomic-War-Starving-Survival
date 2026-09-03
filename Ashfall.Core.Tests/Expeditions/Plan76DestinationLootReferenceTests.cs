using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    /// <summary>
    /// Plan 76 — expedition destination loot-reference gate.
    ///
    /// Every <c>lootCategories</c> string on an authored <c>expeditions.json</c>
    /// destination is handed straight to <see cref="ExpeditionSystem"/> loot
    /// rolls (PerformLootRoll → PickLootCategory → inventory itemId), so each
    /// value must resolve against the merged item catalog. Every
    /// <c>scavenging_table_id</c> must resolve against the Plan 46
    /// <c>scavenging_tables.json</c> authority. This pins the Plan 76 reference
    /// repairs (bandages→bandage, food_rations→dried_rations,
    /// copper_wire→copper_wire_10m_of_10m) against regression.
    /// </summary>
    public sealed class Plan76DestinationLootReferenceTests : IDisposable
    {
        private readonly string _dataDir;
        private readonly IFileIO _fileIO;
        private readonly IJsonSerializer _serializer;

        public Plan76DestinationLootReferenceTests()
        {
            _dataDir = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (!Directory.Exists(_dataDir))
            {
                _dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data");
            }

            _fileIO = new FileSystemIO();
            _serializer = new SystemTextJsonSerializer();
            ExpeditionDefinitionRegistry.Clear();
        }

        public void Dispose()
        {
            ExpeditionDefinitionRegistry.Clear();
        }

        private sealed class AuthoredDestinationDto
        {
            public string id { get; set; } = string.Empty;
            public string displayName { get; set; } = string.Empty;
            public int distanceTicks { get; set; }
            public float travelHours { get; set; }
            public int dangerLevel { get; set; }
            public float encounterChancePerTick { get; set; }
            public float baseStaminaDrainPerHour { get; set; }
            public List<string>? lootCategories { get; set; }
            public string? scavenging_table_id { get; set; }
        }

        /// <summary>Authored scope only: parse expeditions.json directly
        /// (the Plan 32 test pattern) rather than the merged five-file
        /// loader surface.</summary>
        private List<AuthoredDestinationDto> LoadAuthoredDestinations()
        {
            string primaryPath = _fileIO.Combine(_dataDir, ExpeditionCatalogLoader.PrimaryFileName);
            string raw = _fileIO.ReadAllText(primaryPath);
            var dtos = CatalogLocator.LoadWrappedList<AuthoredDestinationDto>(raw, SystemTextJsonSerializer.Options);
            var list = new List<AuthoredDestinationDto>();
            foreach (var dto in dtos)
            {
                if (dto != null && !string.IsNullOrEmpty(dto.id)) list.Add(dto);
            }

            return list;
        }

        private HashSet<string> LoadAllItemIds()
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var file in Directory.GetFiles(_dataDir, "*items*.json"))
            {
                CollectIds(_fileIO.ReadAllText(file), ids);
            }

            return ids;
        }

        private static void CollectIds(string json, HashSet<string> ids)
        {
            // Lightweight structural scan: pull every "id": "..." string value.
            // The scan survives arbitrary wrapper shapes across the many item
            // catalogs (items.json, holdfast_items.json, ...).
            var i = 0;
            const string needle = "\"id\"";
            while ((i = json.IndexOf(needle, i, StringComparison.Ordinal)) >= 0)
            {
                var colon = json.IndexOf(':', i + needle.Length);
                if (colon < 0) break;
                var quote = json.IndexOf('"', colon + 1);
                if (quote < 0) break;
                var end = json.IndexOf('"', quote + 1);
                if (end < 0) break;
                var value = json.Substring(quote + 1, end - quote - 1);
                if (!string.IsNullOrEmpty(value) && value.IndexOf(' ') < 0 && !value.Contains('/'))
                {
                    ids.Add(value);
                }

                i = end;
            }
        }

        [Fact]
        public void AuthoredCatalog_LoadsAndKeepsOriginalTwoParity()
        {
            var defs = LoadAuthoredDestinations();
            Assert.Equal(53, defs.Count);
            Assert.Contains(defs, d => d.id == "loc_the_allotments");
            Assert.Contains(defs, d => d.id == "loc_denial_cut_substation");
        }

        [Fact]
        public void AllLootCategories_ResolveAgainstMergedItemCatalog()
        {
            var itemIds = LoadAllItemIds();
            Assert.True(itemIds.Count > 0, "merged item catalog must not be empty");
            Assert.Contains("bandage", itemIds);
            Assert.Contains("dried_rations", itemIds);
            Assert.Contains("copper_wire_10m_of_10m", itemIds);

            var defs = LoadAuthoredDestinations();
            var broken = new List<string>();
            foreach (var def in defs)
            {
                foreach (var category in def.lootCategories ?? new List<string>())
                {
                    if (!itemIds.Contains(category))
                    {
                        broken.Add($"{def.id}: '{category}'");
                    }
                }
            }

            Assert.True(broken.Count == 0,
                "lootCategories must resolve to item ids (Plan 76 §31). Unresolved:\n" + string.Join("\n", broken));
        }

        [Fact]
        public void AllScavengingTableReferences_ResolveAgainstPlan46Authority()
        {
            string raw = _fileIO.ReadAllText(Path.Combine(_dataDir, "scavenging_tables.json"));
            var catalog = ScavengingTableCatalog.LoadFromJson(raw, _serializer);
            Assert.True(catalog.Tables.Count > 0, "scavenging_tables.json must define tables");

            var known = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var table in catalog.Tables)
            {
                known.Add(table.id);
            }

            var defs = LoadAuthoredDestinations();
            var broken = new List<string>();
            int bound = 0;
            foreach (var def in defs)
            {
                if (string.IsNullOrEmpty(def.scavenging_table_id)) continue;
                bound++;
                if (!known.Contains(def.scavenging_table_id))
                {
                    broken.Add($"{def.id}: '{def.scavenging_table_id}'");
                }
            }

            Assert.True(broken.Count == 0,
                "scavenging_table_id must resolve to a Plan 46 table. Unresolved:\n" + string.Join("\n", broken));
            Assert.Equal(53, bound); // Plan 76.1 complete: every authored destination carries a Plan 46 table binding
        }

        [Fact]
        public void AuthoredCatalog_FullyMigrated_NoDestinationLeftUnbound()
        {
            var defs = LoadAuthoredDestinations();
            var unbound = defs.Where(d => string.IsNullOrEmpty(d.scavenging_table_id)).Select(d => d.id).ToList();
            Assert.True(unbound.Count == 0, "destinations still on lootCategories fallback:\n" + string.Join("\n", unbound));
        }

        [Fact]
        public void RepairedReferences_NeverRegress()
        {
            var defs = LoadAuthoredDestinations();
            var forbidden = new HashSet<string> { "bandages", "food_rations", "copper_wire" };
            var offenders = new List<string>();
            foreach (var def in defs)
            {
                foreach (var category in def.lootCategories ?? new List<string>())
                {
                    if (forbidden.Contains(category))
                    {
                        offenders.Add($"{def.id}: '{category}'");
                    }
                }
            }

            Assert.True(offenders.Count == 0,
                "Plan 76 repaired refs regressed:\n" + string.Join("\n", offenders));
        }
    }
}
