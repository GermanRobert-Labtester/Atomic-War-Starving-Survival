using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    public class ScavengingTableCatalogTests
    {
        private readonly string _dataDir;
        private readonly ScavengingTableCatalog _catalog;
        private readonly HashSet<string> _damagedMapFragmentIds;

        public ScavengingTableCatalogTests()
        {
            // Resolve path to StreamingAssets/Data
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _dataDir = Path.GetFullPath(Path.Combine(baseDir, "../../../../Assets/StreamingAssets/Data"));
            if (!Directory.Exists(_dataDir))
            {
                // Fallback for different test runner working dirs
                _dataDir = Path.GetFullPath(Path.Combine(baseDir, "../../../Assets/StreamingAssets/Data"));
            }

            var fileIO = new FileSystemIO();
            _catalog = ScavengingTableCatalog.LoadFromDirectory(_dataDir, fileIO);

            // Plan 85 — live damaged-map fragment ids for the fragment-entry gate.
            var (damagedZones, damagedErrors) = Ashfall.Core.World.DamagedMapCatalogLoader.LoadWithValidation(
                _dataDir, fileIO, new SystemTextJsonSerializer());
            Assert.Empty(damagedErrors);
            _damagedMapFragmentIds = damagedZones
                .SelectMany(z => z.Fragments)
                .Select(f => f.fragment_id)
                .ToHashSet(StringComparer.Ordinal);
        }

        [Fact]
        public void LoadCatalog_LoadsAll20AuthoredTables()
        {
            Assert.True(_catalog.TableCount >= 20, $"Expected at least 20 tables, found {_catalog.TableCount}");

            string[] expectedLocationTypes =
            {
                "hospital", "rail_yard", "school", "military_depot", "apartment_block",
                "fire_station", "metro_station", "police_station", "industrial_district", "shopping_center",
                "power_substation", "chemical_plant", "warehouse", "farm", "forestry_compound",
                "hunting_cabin", "monastery", "clinic", "observatory", "greenhouse"
            };

            foreach (var locType in expectedLocationTypes)
            {
                bool found = _catalog.TryGetTableByLocationType(locType, out var table);
                Assert.True(found, $"Table for location_type '{locType}' not found in catalog");
                Assert.NotNull(table);
                Assert.StartsWith("table_loot_", table.id);
            }
        }

        [Fact]
        public void TableIntegrity_AllTablesHaveValidWeightsAndQuantities()
        {
            foreach (var table in _catalog.Tables)
            {
                Assert.False(string.IsNullOrEmpty(table.id), "Table ID must not be empty");
                Assert.True(table.TotalWeight > 0, $"Table {table.id} has non-positive TotalWeight");
                Assert.NotEmpty(table.entries);
                Assert.True(table.entries.Count >= 6, $"Table {table.id} has fewer than 6 entries");

                foreach (var entry in table.entries)
                {
                    // Plan 85 — fragment-only entries (empty item_id) are legal
                    // when the entry carries a damaged-map fragment token that
                    // resolves in the live catalog. Anything else with an empty
                    // item_id remains a defect.
                    if (string.IsNullOrEmpty(entry.item_id))
                    {
                        Assert.False(string.IsNullOrEmpty(entry.map_fragment_id),
                            $"Entry in {table.id} has empty item_id and no map_fragment_id");
                        Assert.Contains(entry.map_fragment_id, _damagedMapFragmentIds);
                    }
                    Assert.True(entry.weight > 0, $"Entry {entry.item_id} in {table.id} has non-positive weight");
                    Assert.True(entry.min_quantity >= 1, $"Entry {entry.item_id} in {table.id} has min_quantity < 1");
                    Assert.True(entry.max_quantity >= entry.min_quantity, $"Entry {entry.item_id} in {table.id} has max_quantity < min_quantity");
                    Assert.True(entry.hazard_chance >= 0f && entry.hazard_chance <= 1f, $"Hazard chance out of bounds in {table.id}");
                }
            }
        }

        [Fact]
        public void SeededDeterminism_SameSeedSameTable_ProducesIdenticalOutcomes()
        {
            string tableId = "table_loot_hospital";
            int seed = 123456789;

            var rngA = new SeededRng(seed);
            var rngB = new SeededRng(seed);

            for (int i = 0; i < 50; i++)
            {
                var rollA = _catalog.RollLoot(tableId, rngA);
                var rollB = _catalog.RollLoot(tableId, rngB);

                Assert.NotNull(rollA);
                Assert.NotNull(rollB);
                Assert.Equal(rollA.ItemId, rollB.ItemId);
                Assert.Equal(rollA.Quantity, rollB.Quantity);
                Assert.Equal(rollA.HazardTriggered, rollB.HazardTriggered);
                Assert.Equal(rollA.HazardType, rollB.HazardType);
            }
        }

        [Fact]
        public void DistributionSimulation_10000Rolls_ReflectsRelativeWeights()
        {
            string tableId = "table_loot_hospital";
            Assert.True(_catalog.TryGetTable(tableId, out var table));

            var rng = new SeededRng(987654321);
            int totalRolls = 10000;
            var counts = new Dictionary<string, int>();

            for (int i = 0; i < totalRolls; i++)
            {
                var result = _catalog.RollLoot(tableId, rng);
                Assert.NotNull(result);
                counts[result.ItemId] = counts.GetValueOrDefault(result.ItemId) + 1;
            }

            int totalWeight = table.TotalWeight;
            foreach (var entry in table.entries)
            {
                double expectedFraction = (double)entry.weight / totalWeight;
                double observedFraction = (double)counts.GetValueOrDefault(entry.item_id) / totalRolls;

                // Tolerance within +/- 3.5%
                Assert.True(Math.Abs(expectedFraction - observedFraction) < 0.035,
                    $"Item {entry.item_id} observed fraction {observedFraction:P2} drifted from expected {expectedFraction:P2}");
            }
        }

        [Fact]
        public void HazardTriggering_RespectsTableHazardChances()
        {
            string tableId = "table_loot_chemical_plant";
            Assert.True(_catalog.TryGetTable(tableId, out var table));
            Assert.Equal(0.30f, table.base_hazard_chance);
            Assert.Equal("chemical", table.primary_hazard_type);

            var rng = new SeededRng(55555555);
            int totalRolls = 5000;
            int hazardCount = 0;

            for (int i = 0; i < totalRolls; i++)
            {
                var result = _catalog.RollLoot(tableId, rng);
                Assert.NotNull(result);
                if (result.HazardTriggered)
                {
                    hazardCount++;
                    Assert.Equal("chemical", result.HazardType);
                }
            }

            double observedRate = (double)hazardCount / totalRolls;
            Assert.True(Math.Abs(observedRate - 0.30) < 0.035,
                $"Observed hazard rate {observedRate:P2} drifted from expected 30%");
        }

        [Fact]
        public void CodexUnlockIds_PreservedOnRelevantRolls()
        {
            string tableId = "table_loot_hospital";
            Assert.True(_catalog.TryGetTable(tableId, out var table));

            var chiselEntry = table.entries.FirstOrDefault(e => e.item_id == "item_surgical_bone_chisel");
            Assert.NotNull(chiselEntry);
            Assert.Equal("codex_surgical_log", chiselEntry.codex_unlock_id);
        }

        [Fact]
        public void ExpeditionSystem_WithScavengingCatalog_RollsLootFromTable()
        {
            var system = new ExpeditionSystem();
            system.ScavengingCatalog = _catalog;

            var def = new ExpeditionDefinition
            {
                id = "exp_test_hospital",
                displayName = "Test Field Hospital",
                distanceTicks = 1,
                dangerLevel = 1,
                scavenging_table_id = "table_loot_hospital"
            };
            ExpeditionDefinitionRegistry.Register(def);

            bool started = system.Start(def, "surv_tester_01", 1);
            Assert.True(started);

            var state = system.Active["surv_tester_01"];
            state.phase = (int)ExpeditionPhase.Looting;
            state.maxLootCapacityKg = 100f;

            var rng = new SeededRng(42);

            // Trigger looting ticks
            for (int i = 0; i < 10; i++)
            {
                system.TickHours(1.0f, rng);
            }

            Assert.NotEmpty(state.loot);
            // Verify loot items came from hospital table
            Assert.True(_catalog.TryGetTable("table_loot_hospital", out var hospTable));
            var validItems = hospTable.entries.Select(e => e.item_id).ToHashSet();

            foreach (var lootEntry in state.loot)
            {
                Assert.Contains(lootEntry.itemId, validItems);
                Assert.True(lootEntry.quantity >= 1);
            }
        }
    }
}
