// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: Plan 116 — Deep Lore Locations Expansion (10 → 25 destinations).
// Validates catalog parsing, existing 10 parity, 15 new destinations, D3 degradation coupling,
// item reference resolution, procedural scavenging determinism, and cross-plan integration hooks.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.IO;
using Ashfall.Core.Maritime;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class DeepLoreLocationExpansionTests : CatalogTestBase
    {
        private static List<DeepLoreLocationEntry> LoadLocations()
        {
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            return DeepLoreLocationCatalogLoader.Load(DataDirectory, io, json);
        }

        [Fact]
        public void DeepLoreLocations_CatalogLoads_ExactlyTwentyFiveLocations()
        {
            var locations = LoadLocations();
            Assert.NotNull(locations);
            Assert.Equal(25, locations.Count);
        }

        [Fact]
        public void DeepLoreLocations_OriginalTenLocations_Preserved()
        {
            var locations = LoadLocations();
            var baselineIds = new[]
            {
                "location_municipal_library",
                "location_sunshine_daycare",
                "location_regional_blood_bank",
                "location_grand_cinema",
                "location_upland_logging_camp",
                "location_stadium_evacuation_center",
                "location_automated_abattoir",
                "location_central_postal_hub",
                "location_municipal_water_reservoir",
                "location_television_studio"
            };

            foreach (var id in baselineIds)
            {
                var loc = DeepLoreLocationCatalogLoader.FindById(locations, id);
                Assert.NotNull(loc);
                Assert.False(string.IsNullOrWhiteSpace(loc.displayName));
                Assert.True(loc.radiationUSv > 0f);
                Assert.True(loc.dangerLevel >= 3);
                Assert.True(loc.travelHours > 0f);
                Assert.True(loc.lootTable.Count >= 6);
            }
        }

        [Fact]
        public void DeepLoreLocations_FifteenNewLocations_ExistAndHaveLocationPrefix()
        {
            var locations = LoadLocations();
            var newIds = new[]
            {
                "location_apartment_block",
                "location_metro_station",
                "location_police_station",
                "location_chemical_plant",
                "location_steelworks",
                "location_power_substation",
                "location_ammunition_depot",
                "location_radar_site",
                "location_weather_station",
                "location_agricultural_research",
                "location_metro_tunnel",
                "location_drainage_network",
                "location_irradiated_forest",
                "location_frozen_wetland",
                "location_burned_woodland"
            };

            Assert.Equal(15, newIds.Length);

            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var loc in locations)
            {
                Assert.True(seen.Add(loc.id), $"Duplicate location id: {loc.id}");
                Assert.StartsWith("location_", loc.id);
            }

            foreach (var id in newIds)
            {
                var loc = DeepLoreLocationCatalogLoader.FindById(locations, id);
                Assert.NotNull(loc);
                Assert.False(string.IsNullOrWhiteSpace(loc.displayName));
            }
        }

        [Fact]
        public void DeepLoreLocations_DisplayNames_AreNonEmptyAndUnique()
        {
            var locations = LoadLocations();
            var seenNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var loc in locations)
            {
                Assert.False(string.IsNullOrWhiteSpace(loc.displayName), $"Empty display name on {loc.id}");
                Assert.True(seenNames.Add(loc.displayName), $"Duplicate display name: '{loc.displayName}' on {loc.id}");
            }
        }

        [Fact]
        public void DeepLoreLocations_RadiationAndDangerAndTravel_InValidRange()
        {
            var locations = LoadLocations();
            foreach (var loc in locations)
            {
                Assert.InRange(loc.radiationUSv, 5.0f, 35.0f);
                Assert.InRange(loc.dangerLevel, 1, 10);
                Assert.InRange(loc.travelHours, 1.0f, 6.0f);
            }
        }

        [Fact]
        public void DeepLoreLocations_AllLootTables_HaveValidEntryCounts()
        {
            var locations = LoadLocations();
            foreach (var loc in locations)
            {
                Assert.NotNull(loc.lootTable);
                Assert.InRange(loc.lootTable.Count, 4, 10);
            }
        }

        [Fact]
        public void DeepLoreLocations_AllLootItems_ResolveToAuthoredItems()
        {
            var locations = LoadLocations();
            var io = new FileSystemIO();
            var report = CatalogIntegrityValidator.Validate(DataDirectory, io);

            Assert.True(report.Clean, $"Catalog integrity report has errors: {string.Join("; ", report.Errors)}");

            foreach (var loc in locations)
            {
                foreach (var loot in loc.lootTable)
                {
                    Assert.False(string.IsNullOrWhiteSpace(loot.ItemId), $"Empty ItemId in location {loc.id}");
                }
            }
        }

        [Fact]
        public void DeepLoreLocations_DegradationCoupling_SatisfiesD3()
        {
            var locations = LoadLocations();
            foreach (var loc in locations)
            {
                foreach (var loot in loc.lootTable)
                {
                    if (loot.DegradationChance > 0f)
                    {
                        Assert.False(string.IsNullOrWhiteSpace(loot.DegradedItemId),
                            $"Loot node in {loc.id} for item '{loot.ItemId}' has degradationChance {loot.DegradationChance} but no degradedItemId");
                        Assert.NotEqual(loot.ItemId, loot.DegradedItemId);
                    }
                    else
                    {
                        Assert.True(string.IsNullOrEmpty(loot.DegradedItemId),
                            $"Loot node in {loc.id} for item '{loot.ItemId}' has degradedItemId '{loot.DegradedItemId}' but degradationChance <= 0");
                    }
                }
            }
        }

        [Fact]
        public void DeepLoreLocations_LootQuantityAndSpawnChances_AreValid()
        {
            var locations = LoadLocations();
            foreach (var loc in locations)
            {
                foreach (var loot in loc.lootTable)
                {
                    Assert.True(loot.MinQty >= 0, $"Negative MinQty in {loc.id} for {loot.ItemId}");
                    Assert.True(loot.MaxQty >= loot.MinQty, $"MaxQty < MinQty in {loc.id} for {loot.ItemId}");
                    Assert.True(loot.MaxQty > 0, $"MaxQty must be > 0 in {loc.id} for {loot.ItemId}");
                    Assert.InRange(loot.SpawnChance, 0.01f, 1.0f);
                    Assert.InRange(loot.DegradationChance, 0.0f, 1.0f);
                }
            }
        }

        [Fact]
        public void DeepLoreLocations_PrimaryLootIdentities_AreUniqueAcrossNewDestinations()
        {
            var locations = LoadLocations();
            var newLocationIds = new[]
            {
                "location_apartment_block",
                "location_metro_station",
                "location_police_station",
                "location_chemical_plant",
                "location_steelworks",
                "location_power_substation",
                "location_ammunition_depot",
                "location_radar_site",
                "location_weather_station",
                "location_agricultural_research",
                "location_metro_tunnel",
                "location_drainage_network",
                "location_irradiated_forest",
                "location_frozen_wetland",
                "location_burned_woodland"
            };

            var primaryItems = new Dictionary<string, string>(StringComparer.Ordinal);
            // Identify the primary signature/highest probability distinctive item per new location
            primaryItems["location_apartment_block"] = "cloth";
            primaryItems["location_metro_station"] = "paper_scrap";
            primaryItems["location_police_station"] = "ammo_9x19";
            primaryItems["location_chemical_plant"] = "chemicals";
            primaryItems["location_steelworks"] = "item_galvanized_rebar";
            primaryItems["location_power_substation"] = "battery";
            primaryItems["location_ammunition_depot"] = "ammo_762x54r_jhp_ap";
            primaryItems["location_radar_site"] = "item_faraday_mesh";
            primaryItems["location_weather_station"] = "item_radiosonde";
            primaryItems["location_agricultural_research"] = "item_seed_tuber";
            primaryItems["location_metro_tunnel"] = "item_rebreather_scrubber";
            primaryItems["location_drainage_network"] = "pipe_wrench";
            primaryItems["location_irradiated_forest"] = "wood_block";
            primaryItems["location_frozen_wetland"] = "item_fur_mittens";
            primaryItems["location_burned_woodland"] = "sawdust_block";

            var distinct = new HashSet<string>(primaryItems.Values, StringComparer.Ordinal);
            Assert.Equal(primaryItems.Count, distinct.Count);

            foreach (var kvp in primaryItems)
            {
                var loc = DeepLoreLocationCatalogLoader.FindById(locations, kvp.Key);
                Assert.NotNull(loc);
                Assert.Contains(loc.lootTable, node => node.ItemId == kvp.Value);
            }
        }

        [Fact]
        public void DeepLoreLocations_ProceduralScavenge_DeterministicWithFixedSeed()
        {
            var locations = LoadLocations();
            var loc = DeepLoreLocationCatalogLoader.FindById(locations, "location_apartment_block");
            Assert.NotNull(loc);

            var sysA = new ProceduralScavengeSystem(new SeededRng(4242));
            var sysB = new ProceduralScavengeSystem(new SeededRng(4242));

            sysA.SetCurrentDay(10);
            sysB.SetCurrentDay(10);

            var rollA = sysA.RollLootTable(loc.id, loc.lootTable, loc.radiationUSv, false);
            var rollB = sysB.RollLootTable(loc.id, loc.lootTable, loc.radiationUSv, false);

            Assert.Equal(rollA.Count, rollB.Count);
            for (int i = 0; i < rollA.Count; i++)
            {
                Assert.Equal(rollA[i].ItemId, rollB[i].ItemId);
                Assert.Equal(rollA[i].Quantity, rollB[i].Quantity);
                Assert.Equal(rollA[i].IsDegraded, rollB[i].IsDegraded);
                Assert.Equal(rollA[i].DegradedItemId, rollB[i].DegradedItemId);
                Assert.Equal(rollA[i].IsContaminated, rollB[i].IsContaminated);
            }
        }

        [Fact]
        public void DeepLoreLocations_ProceduralScavenge_DegradationAndContaminationTriggers()
        {
            var locations = LoadLocations();
            var forest = DeepLoreLocationCatalogLoader.FindById(locations, "location_irradiated_forest");
            Assert.NotNull(forest);

            // The forest has 28 uSv/h (> 15 uSv/h threshold), so any rolled loot must be marked contaminated
            var sys = new ProceduralScavengeSystem(new SeededRng(777));
            sys.SetCurrentDay(15);
            var rolls = sys.RollLootTable(forest.id, forest.lootTable, forest.radiationUSv, false);

            Assert.NotEmpty(rolls);
            foreach (var roll in rolls)
            {
                Assert.True(roll.IsContaminated, $"Roll {roll.ItemId} in {forest.id} should be contaminated at 28 uSv/h");
            }
        }

        [Fact]
        public void DeepLoreLocations_SimulationRolls_HaveAcceptableNoLootRates()
        {
            var locations = LoadLocations();
            foreach (var loc in locations)
            {
                // Theoretical no-loot probability: product of (1 - spawnChance) for all nodes
                double noLootProb = 1.0;
                foreach (var node in loc.lootTable)
                {
                    noLootProb *= (1.0 - node.SpawnChance);
                }

                // In ASHFALL, an expedition destination should reliably produce at least one item on > 90% of visits
                // (no-loot probability should be under 10%)
                Assert.True(noLootProb < 0.10,
                    $"Location {loc.id} has excessive theoretical no-loot probability: {noLootProb:P2}");

                // Empirical test: run 500 visits and confirm success rate
                var sys = new ProceduralScavengeSystem(new SeededRng(12345));
                sys.SetCurrentDay(5);
                int emptyCount = 0;
                const int iterations = 500;
                for (int i = 0; i < iterations; i++)
                {
                    var results = sys.RollLootTable(loc.id, loc.lootTable, loc.radiationUSv, false);
                    if (results.Count == 0) emptyCount++;
                }

                double observedEmptyRate = (double)emptyCount / iterations;
                Assert.True(observedEmptyRate < 0.15,
                    $"Observed no-loot rate in {loc.id} is too high: {observedEmptyRate:P2}");
            }
        }

        [Fact]
        public void DeepLoreLocations_CrossPlanIntegrations_AreReferenced()
        {
            var locations = LoadLocations();

            // Plan 112: 4 disease vector candidate locations
            var diseaseVectors = new[]
            {
                "location_drainage_network",
                "location_metro_tunnel",
                "location_chemical_plant",
                "location_irradiated_forest"
            };
            foreach (var id in diseaseVectors)
            {
                Assert.NotNull(DeepLoreLocationCatalogLoader.FindById(locations, id));
            }

            // Plan 48: 3 weather sensitive locations
            var weatherSites = new[]
            {
                "location_frozen_wetland",
                "location_burned_woodland",
                "location_weather_station"
            };
            foreach (var id in weatherSites)
            {
                Assert.NotNull(DeepLoreLocationCatalogLoader.FindById(locations, id));
            }

            // Plan 76: 3 primary technical expedition targets
            var expeditionTargets = new[]
            {
                "location_power_substation",
                "location_agricultural_research",
                "location_radar_site"
            };
            foreach (var id in expeditionTargets)
            {
                Assert.NotNull(DeepLoreLocationCatalogLoader.FindById(locations, id));
            }

            // Plan 113: 2 Verdict investigation sites
            var verdictSites = new[]
            {
                "location_weather_station",
                "location_radar_site"
            };
            foreach (var id in verdictSites)
            {
                Assert.NotNull(DeepLoreLocationCatalogLoader.FindById(locations, id));
            }
        }
    }
}
