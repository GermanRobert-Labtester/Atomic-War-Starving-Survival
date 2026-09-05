using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Content;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Task 8: Cross-Catalog Integrity and Orphan Detection (Sections 8.1–8.18).
    /// Pure validation of collectible definitions, physical item mappings,
    /// acquisition reachability, effect target resolvers, and deterministic diagnostics.
    /// </summary>
    public class CollectibleCatalogIntegrityTests
    {
        private static readonly string DataDir = FindDataDir();
        private static readonly IFileIO FileIO = new FileSystemIO();
        private static readonly IJsonSerializer Serializer = new SystemTextJsonSerializer();

        private static string FindDataDir()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            while (dir != null)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "collectibles.json");
                if (File.Exists(probe)) return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                dir = Path.GetDirectoryName(dir.TrimEnd(Path.DirectorySeparatorChar));
            }
            throw new DirectoryNotFoundException("data authority not found");
        }

        [Fact]
        public void Production_CollectibleCatalog_HasZeroFindings()
        {
            var findings = CollectibleCatalogIntegrityValidator.Validate(DataDir, FileIO, Serializer);
            Assert.Empty(findings);
        }

        [Fact]
        public void Test1_ProductionCollectibleCount_Is40()
        {
            var catalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);
            Assert.NotNull(catalog);
            Assert.Equal(40, catalog.Count);
        }

        [Fact]
        public void Test2_AllCollectibleDefinitions_ResolvePhysicalItems()
        {
            var catalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);
            var itemCatalog = ItemCatalogLoader.LoadCatalog(DataDir, FileIO, Serializer);
            Assert.NotNull(catalog);
            Assert.NotNull(itemCatalog);

            foreach (var kv in catalog.ByItemId)
            {
                Assert.True(itemCatalog.Contains(kv.Key), $"Item '{kv.Key}' in collectibles.json missing from items.json.");
            }
        }

        [Fact]
        public void Test3_AllCollectibleClassifiedItems_ResolveDefinitions()
        {
            var catalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);
            var itemsRaw = Serializer.Deserialize<ItemFileRootDto>(
                FileIO.ReadAllText(Path.Combine(DataDir, "items.json")));
            Assert.NotNull(catalog);
            Assert.NotNull(itemsRaw?.items);

            foreach (var it in itemsRaw.items)
            {
                if (it.id.StartsWith("item_collectible_", StringComparison.Ordinal))
                {
                    Assert.True(catalog.IsCollectible(it.id), $"Item '{it.id}' in items.json missing from collectibles.json.");
                }
            }
        }

        [Fact]
        public void Test4_All40Collectibles_HaveAtLeastOneAcquisitionSource()
        {
            var findings = CollectibleCatalogIntegrityValidator.Validate(DataDir, FileIO, Serializer);
            var missingSources = findings.Where(f => f.ErrorCode == "ERR_NO_ACQUISITION_SOURCE").ToList();
            Assert.Empty(missingSources);
        }

        [Fact]
        public void Test5_KnowledgeTargets_ResolveAgainstAuthority()
        {
            var rk = Serializer.Deserialize<ResearchKnowledgeCatalogContainer>(
                FileIO.ReadAllText(Path.Combine(DataDir, "research_knowledge.json")));
            var known = new HashSet<string>(rk!.KnowledgeNodes.Select(n => n.Id));

            var catalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);
            foreach (var def in catalog!.ByItemId.Values)
            {
                if (def.effect_type == "knowledge")
                {
                    Assert.False(string.IsNullOrEmpty(def.effect_target));
                    Assert.Contains(def.effect_target, known);
                }
            }
        }

        [Fact]
        public void Test6_LocationTargets_ResolveAgainstMapAuthority()
        {
            var map = WastelandMapCatalogLoader.Load(DataDir, FileIO, Serializer);
            var nodes = new HashSet<string>(map!.nodes.Select(n => n.Id));

            var catalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);
            foreach (var def in catalog!.ByItemId.Values)
            {
                if (def.effect_type == "location_clue")
                {
                    Assert.False(string.IsNullOrEmpty(def.effect_target));
                    Assert.Contains(def.effect_target, nodes);
                }
            }
        }

        [Fact]
        public void Test7_FactionInfoTargets_ResolveAgainstProseAuthority()
        {
            var prose = Serializer.Deserialize<JournalVoiceProseFileRaw>(
                FileIO.ReadAllText(Path.Combine(DataDir, "journal_voice_prose.json")));
            Assert.NotNull(prose?.prose_variants);

            var catalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);
            foreach (var def in catalog!.ByItemId.Values)
            {
                if (def.effect_type == "faction_info")
                {
                    Assert.False(string.IsNullOrEmpty(def.effect_target));
                    Assert.True(prose.prose_variants.ContainsKey(def.effect_target),
                        $"Faction info target '{def.effect_target}' not found in journal_voice_prose.json.");
                }
            }
        }

        [Fact]
        public void Test8_JournalTargets_ResolveAgainstProseAuthority()
        {
            var prose = Serializer.Deserialize<JournalVoiceProseFileRaw>(
                FileIO.ReadAllText(Path.Combine(DataDir, "journal_voice_prose.json")));
            Assert.NotNull(prose?.prose_variants);

            var catalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);
            foreach (var def in catalog!.ByItemId.Values)
            {
                if (def.effect_type == "journal_unlock")
                {
                    Assert.False(string.IsNullOrEmpty(def.effect_target));
                    Assert.True(prose.prose_variants.ContainsKey(def.effect_target),
                        $"Journal unlock target '{def.effect_target}' not found in journal_voice_prose.json.");
                }
            }
        }

        [Fact]
        public void Test9_AllScavengingReferences_ResolveValidItems()
        {
            var scav = Serializer.Deserialize<ScavengingTableCatalogContainer>(
                FileIO.ReadAllText(Path.Combine(DataDir, "scavenging_tables.json")));
            var itemCatalog = ItemCatalogLoader.LoadCatalog(DataDir, FileIO, Serializer);

            foreach (var table in scav!.tables)
            {
                foreach (var entry in table.entries)
                {
                    if (!string.IsNullOrEmpty(entry.item_id))
                    {
                        Assert.True(itemCatalog.Contains(entry.item_id),
                            $"Scavenging table '{table.id}' references item '{entry.item_id}' not found in ItemCatalog.");
                    }
                }
            }
        }

        [Fact]
        public void Test10_UniqueItems_ExcludedFromRecurringMerchantRestock()
        {
            var catalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);
            var uniques = catalog!.ByItemId.Values.Where(c => c.unique).Select(c => c.item_id).ToList();
            Assert.NotEmpty(uniques);

            // Caravan route export surpluses are recurring merchant stock; unique items must NEVER be in export surpluses
            string caravanPath = Path.Combine(DataDir, "caravan_routes.json");
            if (File.Exists(caravanPath))
            {
                string text = File.ReadAllText(caravanPath);
                foreach (var u in uniques)
                {
                    Assert.DoesNotContain($"\"{u}\"", text);
                }
            }
        }

        [Fact]
        public void Test11_AllRarities_BelongToLegalSet()
        {
            var catalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);
            foreach (var def in catalog!.ByItemId.Values)
            {
                Assert.Contains(def.rarity, CollectibleCatalogIntegrityValidator.ValidRarities);
            }
        }

        [Fact]
        public void Test12_All16Categories_AreRepresentedAndLegal()
        {
            var catalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);
            var categories = new HashSet<string>(StringComparer.Ordinal);
            foreach (var def in catalog!.ByItemId.Values)
            {
                Assert.Contains(def.category, CollectibleCatalogIntegrityValidator.ValidCategories);
                categories.Add(def.category);
            }
            Assert.Equal(16, categories.Count);
        }

        [Fact]
        public void Test13_EffectBearingDefinitions_RequireTarget()
        {
            var catalog = CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer);
            foreach (var def in catalog!.ByItemId.Values)
            {
                if (def.effect_type == "knowledge" ||
                    def.effect_type == "location_clue" ||
                    def.effect_type == "journal_unlock" ||
                    def.effect_type == "faction_info")
                {
                    Assert.False(string.IsNullOrWhiteSpace(def.effect_target),
                        $"Collectible '{def.item_id}' with effect '{def.effect_type}' has no effect_target.");
                }
            }
        }

        [Fact]
        public void Test14_BrokenFixture_CatchesAllIntendedFailures()
        {
            // Build a simulated in-memory broken file structure using a mock IFileIO
            var mockIO = new InMemoryFileIO();
            string mockDir = "/mock/data";

            var brokenCol = new CollectibleCatalogFileRaw
            {
                collectibles = new List<CollectibleDefinition>
                {
                    new CollectibleDefinition { item_id = "item_collectible_missing_item", category = "photograph", rarity = "common", effect_type = "none" },
                    new CollectibleDefinition { item_id = "item_collectible_bad_cat", category = "alien_artifact", rarity = "common", effect_type = "none" },
                    new CollectibleDefinition { item_id = "item_collectible_bad_rarity", category = "book", rarity = "mythic", effect_type = "none" },
                    new CollectibleDefinition { item_id = "item_collectible_bad_knowledge", category = "book", rarity = "rare", effect_type = "knowledge", effect_target = "knowledge_fake_target" },
                    new CollectibleDefinition { item_id = "item_collectible_bad_location", category = "map", rarity = "rare", effect_type = "location_clue", effect_target = "loc_fake_node" },
                    new CollectibleDefinition { item_id = "item_collectible_bad_journal", category = "book", rarity = "rare", effect_type = "journal_unlock", effect_target = "journal_fake_entry" },
                    new CollectibleDefinition { item_id = "item_collectible_bad_faction", category = "badge", rarity = "uncommon", effect_type = "faction_info", effect_target = "faction_fake_dossier" },
                    new CollectibleDefinition { item_id = "item_collectible_no_source", category = "toy", rarity = "common", effect_type = "none" }
                }
            };

            var mockItems = new ItemFileRootDto
            {
                items = new List<ItemHeaderDto>
                {
                    new ItemHeaderDto { id = "item_collectible_bad_cat" },
                    new ItemHeaderDto { id = "item_collectible_bad_rarity" },
                    new ItemHeaderDto { id = "item_collectible_bad_knowledge" },
                    new ItemHeaderDto { id = "item_collectible_bad_location" },
                    new ItemHeaderDto { id = "item_collectible_bad_journal" },
                    new ItemHeaderDto { id = "item_collectible_bad_faction" },
                    new ItemHeaderDto { id = "item_collectible_no_source" },
                    new ItemHeaderDto { id = "item_collectible_orphan_item" } // in items.json but not in collectibles.json
                }
            };

            var mockScav = new ScavengingTableCatalogContainer
            {
                tables = new List<ScavengingTableDef>
                {
                    new ScavengingTableDef
                    {
                        id = "table_mock_01",
                        entries = new List<ScavengingLootEntryDef>
                        {
                            new ScavengingLootEntryDef { item_id = "item_collectible_bad_cat" },
                            new ScavengingLootEntryDef { item_id = "item_nonexistent_scav_item" }
                        }
                    }
                }
            };

            mockIO.WriteAllText("/mock/data/collectibles.json", Serializer.Serialize(brokenCol));
            mockIO.WriteAllText("/mock/data/items.json", Serializer.Serialize(mockItems));
            mockIO.WriteAllText("/mock/data/scavenging_tables.json", Serializer.Serialize(mockScav));
            mockIO.WriteAllText("/mock/data/research_knowledge.json", "{\"schema_version\":1,\"collection_id\":\"c\",\"knowledge_nodes\":[]}");
            mockIO.WriteAllText("/mock/data/wasteland_map_v1.json", "{\"schema_version\":1,\"nodes\":[],\"routes\":[]}");
            mockIO.WriteAllText("/mock/data/journal_voice_prose.json", "{\"schema_version\":1,\"prose_variants\":{}}");

            var findings = CollectibleCatalogIntegrityValidator.Validate(mockDir, mockIO, Serializer);

            Assert.NotEmpty(findings);
            var codes = new HashSet<string>(findings.Select(f => f.ErrorCode));

            Assert.Contains("ERR_ITEM_NOT_FOUND", codes);
            Assert.Contains("ERR_INVALID_CATEGORY", codes);
            Assert.Contains("ERR_INVALID_RARITY", codes);
            Assert.Contains("ERR_KNOWLEDGE_TARGET_MISSING", codes);
            Assert.Contains("ERR_LOCATION_TARGET_MISSING", codes);
            Assert.Contains("ERR_JOURNAL_TARGET_MISSING", codes);
            Assert.Contains("ERR_FACTION_TARGET_MISSING", codes);
            Assert.Contains("ERR_NO_ACQUISITION_SOURCE", codes);
            Assert.Contains("ERR_ITEM_ORPHAN", codes);
            Assert.Contains("ERR_SCAV_ITEM_MISSING", codes);
        }

        [Fact]
        public void Test15_Diagnostics_AreDeterministicallyOrdered()
        {
            var mockIO = new InMemoryFileIO();
            string mockDir = "/mock/data";

            var brokenCol = new CollectibleCatalogFileRaw
            {
                collectibles = new List<CollectibleDefinition>
                {
                    new CollectibleDefinition { item_id = "item_z", category = "fake_z", rarity = "bad_z" },
                    new CollectibleDefinition { item_id = "item_a", category = "fake_a", rarity = "bad_a" }
                }
            };
            mockIO.WriteAllText("/mock/data/collectibles.json", Serializer.Serialize(brokenCol));
            mockIO.WriteAllText("/mock/data/items.json", "{\"items\":[]}");

            var run1 = CollectibleCatalogIntegrityValidator.Validate(mockDir, mockIO, Serializer);
            var run2 = CollectibleCatalogIntegrityValidator.Validate(mockDir, mockIO, Serializer);

            Assert.Equal(run1.Count, run2.Count);
            for (int i = 0; i < run1.Count; i++)
            {
                Assert.Equal(run1[i].ToString(), run2[i].ToString());
            }
        }

        private sealed class InMemoryFileIO : IFileIO
        {
            private readonly Dictionary<string, string> _files = new Dictionary<string, string>(StringComparer.Ordinal);

            public bool DirectoryExists(string path) => true;
            public bool FileExists(string path) => _files.ContainsKey(path);
            public string ReadAllText(string path) => _files.TryGetValue(path, out var s) ? s : string.Empty;
            public void WriteAllText(string path, string contents) => _files[path] = contents;
            public string Combine(params string[] parts) => string.Join("/", parts).Replace("//", "/");
        }
    }
}
