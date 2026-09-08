using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class FactionWarLocationOverridesExpansionTests : CatalogTestBase
    {
        private static FactionWarContentCatalog LoadCatalog()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new FactionWarContentCatalogLoader(files, json);
            return loader.Load(DataDirectory);
        }

        private static HashSet<string> LoadAllKnownLocationIds()
        {
            var known = new HashSet<string>(StringComparer.Ordinal);
            string dataDir = DataDirectory;

            string[] candidateFiles =
            {
                "locations.json",
                "deep_lore_locations.json",
                "crossing_locations.json",
                "year_of_ash_locations.json",
                "verdict_locations.json",
                "duty_roster_locations.json",
                "holdfast_locations.json",
                "locations_expansion3.json",
                "dose_locations.json",
                "micro_locations.json",
                "settlements.json"
            };

            foreach (var filename in candidateFiles)
            {
                string path = Path.Combine(dataDir, filename);
                if (!File.Exists(path)) continue;

                string text = File.ReadAllText(path);
                try
                {
                    var doc = System.Text.Json.JsonDocument.Parse(text);
                    if (doc.RootElement.TryGetProperty("locations", out var locs) && locs.ValueKind == System.Text.Json.JsonValueKind.Array)
                    {
                        foreach (var el in locs.EnumerateArray())
                        {
                            if (el.TryGetProperty("id", out var idProp) && idProp.ValueKind == System.Text.Json.JsonValueKind.String)
                                known.Add(idProp.GetString()!);
                            else if (el.TryGetProperty("locationId", out var lidProp) && lidProp.ValueKind == System.Text.Json.JsonValueKind.String)
                                known.Add(lidProp.GetString()!);
                        }
                    }
                    if (doc.RootElement.TryGetProperty("settlements", out var settlements) && settlements.ValueKind == System.Text.Json.JsonValueKind.Array)
                    {
                        foreach (var el in settlements.EnumerateArray())
                        {
                            if (el.TryGetProperty("id", out var idProp) && idProp.ValueKind == System.Text.Json.JsonValueKind.String)
                                known.Add(idProp.GetString()!);
                        }
                    }
                }
                catch
                {
                    // Fallback parse error suppression
                }
            }

            return known;
        }

        [Fact]
        public void Catalog_LoadsExact20LocationOverrides()
        {
            var catalog = LoadCatalog();
            Assert.Equal(20, catalog.LocationOverrideCount);
            Assert.Equal(20, catalog.LocationOverrides.Count);
        }

        [Fact]
        public void BaselineNine_PreservedWithByteForByteParity()
        {
            var catalog = LoadCatalog();
            var overrides = catalog.LocationOverrides;

            // 0: loc_override_almshouse_pre_strike
            Assert.Equal("loc_override_almshouse_pre_strike", overrides[0].id);
            Assert.Equal("loc_st_brigids_almshouse", overrides[0].locationId);
            Assert.Equal("pre_strike", overrides[0].overrideType);
            Assert.Equal(515, overrides[0].activeFromDay);
            Assert.Equal(516, overrides[0].activeUntilDay);
            Assert.Equal("St Brigid's Almshouse", overrides[0].displayName);

            // 1: loc_override_almshouse_post_strike
            Assert.Equal("loc_override_almshouse_post_strike", overrides[1].id);
            Assert.Equal("loc_st_brigids_almshouse", overrides[1].locationId);
            Assert.Equal("post_strike", overrides[1].overrideType);
            Assert.Equal(517, overrides[1].activeFromDay);
            Assert.Equal(0, overrides[1].activeUntilDay);

            // 2: loc_override_ration_plaza_pre_strike
            Assert.Equal("loc_override_ration_plaza_pre_strike", overrides[2].id);
            Assert.Equal("loc_ration_queue_plaza", overrides[2].locationId);
            Assert.Equal("pre_strike", overrides[2].overrideType);
            Assert.Equal(541, overrides[2].activeFromDay);
            Assert.Equal(544, overrides[2].activeUntilDay);

            // 3: loc_override_ration_plaza_post_strike
            Assert.Equal("loc_override_ration_plaza_post_strike", overrides[3].id);
            Assert.Equal("loc_ration_queue_plaza", overrides[3].locationId);
            Assert.Equal("post_strike", overrides[3].overrideType);
            Assert.Equal(545, overrides[3].activeFromDay);
            Assert.Equal(0, overrides[3].activeUntilDay);

            // 4: loc_override_ash_sign_shrine_pre_strike
            Assert.Equal("loc_override_ash_sign_shrine_pre_strike", overrides[4].id);
            Assert.Equal("loc_ash_sign_shrine", overrides[4].locationId);
            Assert.Equal("pre_strike", overrides[4].overrideType);
            Assert.Equal(576, overrides[4].activeFromDay);
            Assert.Equal(577, overrides[4].activeUntilDay);

            // 5: loc_override_ash_sign_shrine_post_strike
            Assert.Equal("loc_override_ash_sign_shrine_post_strike", overrides[5].id);
            Assert.Equal("loc_ash_sign_shrine", overrides[5].locationId);
            Assert.Equal("post_strike", overrides[5].overrideType);
            Assert.Equal(578, overrides[5].activeFromDay);
            Assert.Equal(0, overrides[5].activeUntilDay);

            // 6: loc_override_span44_ambient_crater
            Assert.Equal("loc_override_span44_ambient_crater", overrides[6].id);
            Assert.Equal("loc_railway_span_44_alpha", overrides[6].locationId);
            Assert.Equal("ambient_addendum", overrides[6].overrideType);
            Assert.Equal(495, overrides[6].activeFromDay);
            Assert.Equal(0, overrides[6].activeUntilDay);

            // 7: loc_override_forward_roster_camp_ambient
            Assert.Equal("loc_override_forward_roster_camp_ambient", overrides[7].id);
            Assert.Equal("loc_forward_roster_camp", overrides[7].locationId);
            Assert.Equal("ambient_addendum", overrides[7].overrideType);
            Assert.Equal(572, overrides[7].activeFromDay);
            Assert.Equal(0, overrides[7].activeUntilDay);

            // 8: loc_override_understory_transmitter_ambient
            Assert.Equal("loc_override_understory_transmitter_ambient", overrides[8].id);
            Assert.Equal("loc_understory_transmitter", overrides[8].locationId);
            Assert.Equal("ambient_addendum", overrides[8].overrideType);
            Assert.Equal(586, overrides[8].activeFromDay);
            Assert.Equal(0, overrides[8].activeUntilDay);
        }

        [Fact]
        public void ElevenNewOverrides_HaveUniqueIdsAndValidFields()
        {
            var catalog = LoadCatalog();
            var allIds = new HashSet<string>(StringComparer.Ordinal);

            for (int i = 0; i < 20; i++)
            {
                var o = catalog.LocationOverrides[i];
                Assert.True(allIds.Add(o.id), $"Duplicate override ID: {o.id}");
                Assert.StartsWith("loc_override_", o.id);
                Assert.False(string.IsNullOrWhiteSpace(o.displayName), $"{o.id} displayName required");
                Assert.False(string.IsNullOrWhiteSpace(o.description), $"{o.id} description required");
                Assert.True(o.activeFromDay > 0, $"{o.id} activeFromDay must be positive");
            }

            // Verify specific 11 new override IDs exist in positions 9..19
            string[] expectedNewIds =
            {
                "loc_override_checkpoint_occupied",
                "loc_override_granary_burned",
                "loc_override_well_contaminated",
                "loc_override_rail_yard_fortified",
                "loc_override_village_abandoned",
                "loc_override_factory_occupied",
                "loc_override_bridge_destroyed",
                "loc_override_roadblock_liberated",
                "loc_override_camp_overrun",
                "loc_override_station_reclaimed",
                "loc_override_field_scorched"
            };

            for (int i = 0; i < expectedNewIds.Length; i++)
            {
                Assert.Equal(expectedNewIds[i], catalog.LocationOverrides[9 + i].id);
            }
        }

        [Fact]
        public void AllTwentyLocationIds_ResolveAgainstCanonicalLocationCatalogs()
        {
            var catalog = LoadCatalog();
            var known = LoadAllKnownLocationIds();
            Assert.NotEmpty(known);

            foreach (var o in catalog.LocationOverrides)
            {
                Assert.True(known.Contains(o.locationId),
                    $"Override {o.id} references unresolved locationId '{o.locationId}'");
            }
        }

        [Fact]
        public void AllElevenNewDayWindows_AreStrictlyOrdered_AndWithinRange()
        {
            var catalog = LoadCatalog();
            for (int i = 9; i < 20; i++)
            {
                var o = catalog.LocationOverrides[i];
                Assert.True(o.activeUntilDay > 0, $"{o.id} must have bounded activeUntilDay");
                Assert.True(o.activeFromDay < o.activeUntilDay, $"{o.id} activeFromDay < activeUntilDay");
                Assert.InRange(o.activeFromDay, 200, 360);
                Assert.InRange(o.activeUntilDay, 250, 400);
            }
        }

        [Fact]
        public void OverrideTypeVocabulary_ContainsExpectedDistribution()
        {
            var catalog = LoadCatalog();
            var counts = catalog.LocationOverrides
                .GroupBy(o => o.overrideType)
                .ToDictionary(g => g.Key, g => g.Count());

            Assert.Equal(3, counts["pre_strike"]);
            Assert.Equal(6, counts["post_strike"]);
            Assert.Equal(3, counts["ambient_addendum"]);
            Assert.Equal(2, counts["occupied"]);
            Assert.Equal(2, counts["abandoned"]);
            Assert.Equal(1, counts["fortified"]);
            Assert.Equal(1, counts["liberated"]);
            Assert.Equal(1, counts["reclaimed"]);
            Assert.Equal(1, counts["contaminated"]);
        }

        [Fact]
        public void DeepLoreHandoff_FourLocationsResolveAndProjectCorrectly()
        {
            var catalog = LoadCatalog();
            string[] deepLoreLocations =
            {
                "location_municipal_water_reservoir",
                "location_chemical_plant",
                "location_metro_station",
                "location_burned_woodland"
            };

            foreach (var locId in deepLoreLocations)
            {
                var matches = catalog.LocationOverrides.Where(o => o.locationId == locId).ToList();
                Assert.Single(matches);
            }

            // Chemical plant active from day 300 to 350
            var chemBefore = catalog.GetActiveLocationOverride("location_chemical_plant", 299);
            var chemActive = catalog.GetActiveLocationOverride("location_chemical_plant", 325);
            var chemAfter = catalog.GetActiveLocationOverride("location_chemical_plant", 351);

            Assert.Null(chemBefore);
            Assert.NotNull(chemActive);
            Assert.Equal("Occupied Chemical Works", chemActive!.displayName);
            Assert.Null(chemAfter);
        }

        [Fact]
        public void YearOfAshHandoff_ThreeLocationsResolveAndProjectCorrectly()
        {
            var catalog = LoadCatalog();
            string[] yoaLocations =
            {
                "loc_garrison_checkpoint_gamma",
                "loc_sector_4_rail_switchyard",
                "loc_ash_militia_deadfall_barrier"
            };

            foreach (var locId in yoaLocations)
            {
                var matches = catalog.LocationOverrides.Where(o => o.locationId == locId).ToList();
                Assert.Single(matches);
            }

            // Checkpoint Gamma active from day 200 to 250
            var gammaBefore = catalog.GetActiveLocationOverride("loc_garrison_checkpoint_gamma", 199);
            var gammaActive = catalog.GetActiveLocationOverride("loc_garrison_checkpoint_gamma", 225);
            var gammaAfter = catalog.GetActiveLocationOverride("loc_garrison_checkpoint_gamma", 251);

            Assert.Null(gammaBefore);
            Assert.NotNull(gammaActive);
            Assert.Equal("Garrison-Held Checkpoint Gamma", gammaActive!.displayName);
            Assert.Null(gammaAfter);
        }

        [Fact]
        public void CrossingHandoff_TwoLocationsResolveAndProjectCorrectly()
        {
            var catalog = LoadCatalog();
            string[] crossingLocations =
            {
                "loc_crossing_granary_pledge",
                "loc_crossing_petition_tent"
            };

            foreach (var locId in crossingLocations)
            {
                var matches = catalog.LocationOverrides.Where(o => o.locationId == locId).ToList();
                Assert.Single(matches);
            }

            // Granary active from day 220 to 260
            var granaryActive = catalog.GetActiveLocationOverride("loc_crossing_granary_pledge", 240);
            Assert.NotNull(granaryActive);
            Assert.Equal("Burned Pledged Granary", granaryActive!.displayName);
        }

        [Fact]
        public void DeterministicSelection_Harness_VerifiesPrecedenceAndBoundaries()
        {
            var catalog = LoadCatalog();

            // Check boundaries for all 11 new overrides
            for (int i = 9; i < 20; i++)
            {
                var o = catalog.LocationOverrides[i];
                string loc = o.locationId;
                int from = o.activeFromDay;
                int until = o.activeUntilDay;

                // Day before start -> returns null (or baseline state if shared, but these 11 are unique)
                var before = catalog.GetActiveLocationOverride(loc, from - 1);
                Assert.Null(before);

                // First active day -> active
                var atStart = catalog.GetActiveLocationOverride(loc, from);
                Assert.NotNull(atStart);
                Assert.Equal(o.id, atStart!.id);

                // Mid-window -> active
                int mid = (from + until) / 2;
                var atMid = catalog.GetActiveLocationOverride(loc, mid);
                Assert.NotNull(atMid);
                Assert.Equal(o.id, atMid!.id);

                // Final active day -> active
                var atEnd = catalog.GetActiveLocationOverride(loc, until);
                Assert.NotNull(atEnd);
                Assert.Equal(o.id, atEnd!.id);

                // Day after expiry -> returns null (base state restored)
                var after = catalog.GetActiveLocationOverride(loc, until + 1);
                Assert.Null(after);
            }
        }

        [Fact]
        public void SequentialHandoff_Almshouse_PreStrikeToPostStrike()
        {
            var catalog = LoadCatalog();
            const string loc = "loc_st_brigids_almshouse";

            Assert.Null(catalog.GetActiveLocationOverride(loc, 514));

            var pre = catalog.GetActiveLocationOverride(loc, 515);
            Assert.NotNull(pre);
            Assert.Equal("loc_override_almshouse_pre_strike", pre!.id);

            var preEnd = catalog.GetActiveLocationOverride(loc, 516);
            Assert.NotNull(preEnd);
            Assert.Equal("loc_override_almshouse_pre_strike", preEnd!.id);

            var post = catalog.GetActiveLocationOverride(loc, 517);
            Assert.NotNull(post);
            Assert.Equal("loc_override_almshouse_post_strike", post!.id);

            var postLater = catalog.GetActiveLocationOverride(loc, 700);
            Assert.NotNull(postLater);
            Assert.Equal("loc_override_almshouse_post_strike", postLater!.id);
        }

        [Fact]
        public void MultiLocationConcurrentActivation_AtDay345()
        {
            var catalog = LoadCatalog();

            // At Day 345, the following overrides are simultaneously active on distinct locations:
            // 1. loc_override_village_abandoned (280-340: NO, ended at 340)
            // 2. loc_override_factory_occupied (300-350: YES)
            // 3. loc_override_bridge_destroyed (310-360: YES)
            // 4. loc_override_roadblock_liberated (330-370: YES)
            // 5. loc_override_camp_overrun (340-380: YES)
            var factory = catalog.GetActiveLocationOverride("location_chemical_plant", 345);
            var bridge = catalog.GetActiveLocationOverride("loc_bridge_seven", 345);
            var roadblock = catalog.GetActiveLocationOverride("loc_ash_militia_deadfall_barrier", 345);
            var camp = catalog.GetActiveLocationOverride("loc_crossing_petition_tent", 345);

            Assert.NotNull(factory);
            Assert.Equal("loc_override_factory_occupied", factory!.id);

            Assert.NotNull(bridge);
            Assert.Equal("loc_override_bridge_destroyed", bridge!.id);

            Assert.NotNull(roadblock);
            Assert.Equal("loc_override_roadblock_liberated", roadblock!.id);

            Assert.NotNull(camp);
            Assert.Equal("loc_override_camp_overrun", camp!.id);
        }

        [Fact]
        public void BaseRestoration_AfterWindowExpiry_ReturnsNull()
        {
            var catalog = LoadCatalog();

            // Bridge Seven active 310-360
            Assert.NotNull(catalog.GetActiveLocationOverride("loc_bridge_seven", 350));
            Assert.Null(catalog.GetActiveLocationOverride("loc_bridge_seven", 361));

            // Iron Siding active 280-340
            Assert.NotNull(catalog.GetActiveLocationOverride("loc_settlement_iron_siding", 300));
            Assert.Null(catalog.GetActiveLocationOverride("loc_settlement_iron_siding", 341));
        }

        [Fact]
        public void SafetyAudit_NoProhibitedInstructionsInDescriptions()
        {
            var catalog = LoadCatalog();

            string[] prohibitedPatterns =
            {
                "defuse", "disarm", "wire cutter", "tactical lane", "detonator assembly",
                "mix with water to cure", "bleach solution", "ingest charcoal",
                "demolition charge placement"
            };

            foreach (var o in catalog.LocationOverrides)
            {
                string descLower = o.description.ToLowerInvariant();
                foreach (var pattern in prohibitedPatterns)
                {
                    Assert.DoesNotContain(pattern, descLower);
                }
            }
        }
    }
}
