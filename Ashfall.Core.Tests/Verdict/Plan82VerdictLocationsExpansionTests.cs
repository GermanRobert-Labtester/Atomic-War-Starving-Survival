using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests.Verdict
{
    public class Plan82VerdictLocationsExpansionTests : CatalogTestBase
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            return string.Empty;
        }

        private static List<VerdictCatalogLoader.VerdictLocationEntry> LoadLocations()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Data directory not found.");
            return VerdictCatalogLoader.LoadLocations(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        [Fact]
        public void LoadLocations_ReturnsExactlyFifteenSites()
        {
            var locs = LoadLocations();
            Assert.NotNull(locs);
            Assert.Equal(15, locs.Count);
        }

        [Fact]
        public void PreservesAllFourOriginalTempestArraySites()
        {
            var locs = LoadLocations();
            var byId = locs.ToDictionary(l => l.id);

            Assert.True(byId.ContainsKey("loc_geophone_pit_1"));
            Assert.True(byId.ContainsKey("loc_twelve_gauge_array"));
            Assert.True(byId.ContainsKey("loc_network_fuse_bunker"));
            Assert.True(byId.ContainsKey("loc_archive_tape_silo"));

            Assert.Equal("The First Geophone Pit", byId["loc_geophone_pit_1"].displayName);
            Assert.Equal("The Twelve-Gauge Array", byId["loc_twelve_gauge_array"].displayName);
            Assert.Equal("The Fuse World", byId["loc_network_fuse_bunker"].displayName);
            Assert.Equal("The Archive Tape-Silo", byId["loc_archive_tape_silo"].displayName);

            Assert.Equal(6, byId["loc_geophone_pit_1"].dangerLevel);
            Assert.Equal(7, byId["loc_twelve_gauge_array"].dangerLevel);
            Assert.Equal(8, byId["loc_network_fuse_bunker"].dangerLevel);
            Assert.Equal(9, byId["loc_archive_tape_silo"].dangerLevel);
        }

        [Fact]
        public void VerifiesAllFourInvestigationArcsPresent()
        {
            var locs = LoadLocations();
            var byId = locs.ToDictionary(l => l.id);

            // Arc 1 — Tempest Array (4)
            string[] arc1 = { "loc_geophone_pit_1", "loc_twelve_gauge_array", "loc_network_fuse_bunker", "loc_archive_tape_silo" };
            foreach (var id in arc1) Assert.True(byId.ContainsKey(id), $"Arc 1 missing {id}");

            // Arc 2 — The Coastal Survey (4)
            string[] arc2 = { "loc_abandoned_tide_gauge", "loc_coastal_meteorological_station", "loc_clifftop_observation_bunker", "loc_sealed_marine_laboratory" };
            foreach (var id in arc2) Assert.True(byId.ContainsKey(id), $"Arc 2 missing {id}");

            // Arc 3 — The Interior Caches (4)
            string[] arc3 = { "loc_forestry_survey_post", "loc_geological_core_vault", "loc_river_gauging_station", "loc_abandoned_agricultural_station" };
            foreach (var id in arc3) Assert.True(byId.ContainsKey(id), $"Arc 3 missing {id}");

            // Arc 4 — The Border Wire (3)
            string[] arc4 = { "loc_decommissioned_signal_relay", "loc_border_checkpoint_ruins", "loc_minefield_observation_tower" };
            foreach (var id in arc4) Assert.True(byId.ContainsKey(id), $"Arc 4 missing {id}");
        }

        [Fact]
        public void AllLocationIdsAreUniqueAndFollowCanonicalPrefix()
        {
            var locs = LoadLocations();
            var seen = new HashSet<string>(StringComparer.Ordinal);

            foreach (var loc in locs)
            {
                Assert.False(string.IsNullOrWhiteSpace(loc.id));
                Assert.StartsWith("loc_", loc.id);
                Assert.Matches("^[a-z0-9_]+$", loc.id);
                Assert.True(seen.Add(loc.id), $"Duplicate location ID {loc.id}");
            }
        }

        [Fact]
        public void DangerLevelsWithinValidThreeToTenRange()
        {
            var locs = LoadLocations();
            foreach (var loc in locs)
            {
                Assert.InRange(loc.dangerLevel, 3, 10);
            }
        }

        [Fact]
        public void TravelHoursWithinValidThreeToTwelveRange()
        {
            var locs = LoadLocations();
            foreach (var loc in locs)
            {
                Assert.InRange(loc.travelHours, 3.0f, 12.0f);
            }
        }

        [Fact]
        public void BaseRadsPerHourWithinValidTwentyToSixtyRange()
        {
            var locs = LoadLocations();
            foreach (var loc in locs)
            {
                Assert.InRange(loc.baseRadsPerHour, 20.0f, 60.0f);
            }
        }

        [Fact]
        public void AllDescriptionsMeetHighQualityDensityStandards()
        {
            var locs = LoadLocations();
            foreach (var loc in locs)
            {
                Assert.False(string.IsNullOrWhiteSpace(loc.displayName));
                Assert.False(string.IsNullOrWhiteSpace(loc.description));
                Assert.True(loc.description.Length >= 150, $"Description too short on {loc.id}: {loc.description.Length} chars");

                // Sentence count heuristic (period followed by space or end)
                int sentenceCount = loc.description.Split(new[] { ". ", "! ", "? " }, StringSplitOptions.RemoveEmptyEntries).Length;
                Assert.True(sentenceCount >= 3, $"Expected >= 3 sentences on {loc.id}, found {sentenceCount}");
            }
        }

        [Fact]
        public void NoForbiddenSupernaturalOrGenericTropesInDescriptions()
        {
            var locs = LoadLocations();
            string[] forbidden = { "mysterious energy", "secret lab", "alien", "magic", "sorcery", "portal", "cursed" };

            foreach (var loc in locs)
            {
                string descLower = loc.description.ToLowerInvariant();
                foreach (var f in forbidden)
                {
                    Assert.DoesNotContain(f, descLower);
                }
            }
        }

        [Fact]
        public void LocationsRoundTripSerialization_PreservesAllFields()
        {
            var locs = LoadLocations();
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(locs);
            var deserialized = serializer.Deserialize<List<VerdictCatalogLoader.VerdictLocationEntry>>(json);

            Assert.NotNull(deserialized);
            Assert.Equal(15, deserialized.Count);
            for (int i = 0; i < locs.Count; i++)
            {
                Assert.Equal(locs[i].id, deserialized[i].id);
                Assert.Equal(locs[i].displayName, deserialized[i].displayName);
                Assert.Equal(locs[i].description, deserialized[i].description);
                Assert.Equal(locs[i].dangerLevel, deserialized[i].dangerLevel);
                Assert.Equal(locs[i].travelHours, deserialized[i].travelHours);
                Assert.Equal(locs[i].baseRadsPerHour, deserialized[i].baseRadsPerHour);
            }
        }
    }
}
