using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class Plan81DoseLocationsExpansionTests : CatalogTestBase
    {
        private static string FindDataDir()
        {
            string dataDir = string.Empty;
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) { dataDir = candidate; break; }
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return dataDir;
        }

        private static DoseContentCatalog LoadCatalog()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Data directory not found.");
            return DoseContentCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
        }

        [Fact]
        public void Load_FindsAtLeastTwelveLocations_ExactlyFourteenAuthored()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog.locations);
            Assert.True(catalog.locations.Count >= 12, $"Expected >= 12 dose locations, found {catalog.locations.Count}");
            Assert.Equal(14, catalog.locations.Count);
        }

        [Fact]
        public void PreservesAllOriginalFiveBunkerLocations()
        {
            var catalog = LoadCatalog();
            var byId = catalog.locations.ToDictionary(l => l.id);

            Assert.True(byId.ContainsKey("loc_the_dose_room"));
            Assert.True(byId.ContainsKey("loc_the_calibration_bench"));
            Assert.True(byId.ContainsKey("loc_the_childrens_baseline_board"));
            Assert.True(byId.ContainsKey("loc_the_register_hall"));
            Assert.True(byId.ContainsKey("loc_the_screening_station"));

            Assert.Equal("bunker", byId["loc_the_dose_room"].sector);
            Assert.Equal("bunker", byId["loc_the_calibration_bench"].sector);
            Assert.Equal("bunker", byId["loc_the_childrens_baseline_board"].sector);
            Assert.Equal("bunker", byId["loc_the_register_hall"].sector);
            Assert.Equal("bunker", byId["loc_the_screening_station"].sector);
        }

        [Fact]
        public void VerifiesAllNineNewLocationsExistWithCorrectSectors()
        {
            var catalog = LoadCatalog();
            var byId = catalog.locations.ToDictionary(l => l.id);

            // Surface (3)
            Assert.True(byId.ContainsKey("loc_shelter_exterior_approach"));
            Assert.Equal("surface", byId["loc_shelter_exterior_approach"].sector);

            Assert.True(byId.ContainsKey("loc_surface_observation_post"));
            Assert.Equal("surface", byId["loc_surface_observation_post"].sector);

            Assert.True(byId.ContainsKey("loc_contaminated_water_access"));
            Assert.Equal("surface", byId["loc_contaminated_water_access"].sector);

            // Expedition (3)
            Assert.True(byId.ContainsKey("loc_irradiated_forest_edge"));
            Assert.Equal("expedition", byId["loc_irradiated_forest_edge"].sector);

            Assert.True(byId.ContainsKey("loc_ruined_hospital_grounds"));
            Assert.Equal("expedition", byId["loc_ruined_hospital_grounds"].sector);

            Assert.True(byId.ContainsKey("loc_military_depot_perimeter"));
            Assert.Equal("expedition", byId["loc_military_depot_perimeter"].sector);

            // External (2)
            Assert.True(byId.ContainsKey("loc_frozen_wetland_crossing"));
            Assert.Equal("external", byId["loc_frozen_wetland_crossing"].sector);

            Assert.True(byId.ContainsKey("loc_burned_woodland_ridge"));
            Assert.Equal("external", byId["loc_burned_woodland_ridge"].sector);

            // Faction (1)
            Assert.True(byId.ContainsKey("loc_garrison_checkpoint_gamma_exterior"));
            Assert.Equal("faction", byId["loc_garrison_checkpoint_gamma_exterior"].sector);
        }

        [Fact]
        public void HasExpectedSectorDistributionAcrossFiveSectors()
        {
            var catalog = LoadCatalog();
            var sectorCounts = catalog.locations
                .GroupBy(l => l.sector)
                .ToDictionary(g => g.Key, g => g.Count());

            Assert.Equal(5, sectorCounts.Count); // bunker, surface, expedition, external, faction
            Assert.Equal(5, sectorCounts["bunker"]);
            Assert.Equal(3, sectorCounts["surface"]);
            Assert.Equal(3, sectorCounts["expedition"]);
            Assert.Equal(2, sectorCounts["external"]);
            Assert.Equal(1, sectorCounts["faction"]);
        }

        [Fact]
        public void AllLocationIdsAreUniqueAndFollowCanonicalPrefix()
        {
            var catalog = LoadCatalog();
            var seen = new HashSet<string>(StringComparer.Ordinal);

            foreach (var loc in catalog.locations)
            {
                Assert.False(string.IsNullOrWhiteSpace(loc.id), "Location ID cannot be null or empty.");
                Assert.StartsWith("loc_", loc.id);
                Assert.DoesNotContain(" ", loc.id);
                Assert.True(seen.Add(loc.id), $"Duplicate location ID detected: {loc.id}");
            }
        }

        [Fact]
        public void RiskLevelsWithinValidZeroToEightRange()
        {
            var catalog = LoadCatalog();
            foreach (var loc in catalog.locations)
            {
                Assert.InRange(loc.riskLevel, 0, 8);
            }
        }

        [Fact]
        public void RadiationDoseIsFinitePositiveAndBounded()
        {
            var catalog = LoadCatalog();
            foreach (var loc in catalog.locations)
            {
                Assert.False(float.IsNaN(loc.radiationUsv), $"NaN radiation value on {loc.id}");
                Assert.False(float.IsInfinity(loc.radiationUsv), $"Infinite radiation value on {loc.id}");
                Assert.InRange(loc.radiationUsv, 0.01f, 80.0f);
            }
        }

        [Fact]
        public void AllLocationsHaveNonEmptyDisplayNamesAndEnvironmentalDescriptions()
        {
            var catalog = LoadCatalog();
            foreach (var loc in catalog.locations)
            {
                Assert.False(string.IsNullOrWhiteSpace(loc.displayName), $"Display name missing on {loc.id}");
                Assert.False(string.IsNullOrWhiteSpace(loc.description), $"Description missing on {loc.id}");
                Assert.True(loc.description.Length >= 20, $"Description suspiciously short on {loc.id}");
            }
        }

        [Fact]
        public void RiskLevelAndDoseCorrelationCoherent()
        {
            var catalog = LoadCatalog();
            // Bunker risk 0 should have the lowest doses (<= 0.05)
            var bunkerLocs = catalog.locations.Where(l => l.sector == "bunker");
            foreach (var b in bunkerLocs)
            {
                Assert.Equal(0, b.riskLevel);
                Assert.True(b.radiationUsv <= 0.05f);
            }

            // High risk (>= 5) should have substantial doses (>= 10 uSv/h)
            var highRiskLocs = catalog.locations.Where(l => l.riskLevel >= 5);
            foreach (var hr in highRiskLocs)
            {
                Assert.True(hr.radiationUsv >= 10.0f, $"Expected high risk {hr.id} to have dose >= 10, got {hr.radiationUsv}");
            }
        }

        [Fact]
        public void DoseLedgerReadingAttributionWorksForNewLocations()
        {
            var ledger = new DoseLedgerSystem();
            ledger.AssignDosimeter("survivor_auditor", "tag_gamma_01");

            var rng = new SeededRng(42);

            // Plan 100 — the register books lifetime totals. Location visits
            // accrue the survivor's lifetime burden; each bench reading books
            // the increment since the last one.
            var resSurface = ledger.BookReadingFromLifetime(
                "survivor_auditor",
                day: 10,
                lifetimeNowMsv: 0.00085f, // ~0.85 uSv converted to mSv
                source: "loc_shelter_exterior_approach",
                highEnergyEvent: false,
                rng: rng);

            Assert.Equal(DoseBandResult.Green, resSurface);

            // Book a reading acquired at the military depot perimeter (45 uSv/h * 4h = 180 uSv = 0.18 mSv)
            var resDepot = ledger.BookReadingFromLifetime(
                "survivor_auditor",
                day: 12,
                lifetimeNowMsv: 0.00085f + 0.18f,
                source: "loc_military_depot_perimeter",
                highEnergyEvent: false,
                rng: rng);

            Assert.Equal(DoseBandResult.Green, resDepot);

            var entry = ledger.Entries.First(e => e.survivorId == "survivor_auditor");
            Assert.Equal(2, entry.readingsHistory.Count);
            Assert.Equal("loc_shelter_exterior_approach", entry.readingsHistory[0].source);
            Assert.Equal("loc_military_depot_perimeter", entry.readingsHistory[1].source);
            Assert.True(entry.cumulativeMsv > 0.18f);
        }

        [Fact]
        public void DoseLedgerStateCaptureAndRestoreRoundTripPreservesLocationAttribution()
        {
            var ledger = new DoseLedgerSystem();
            ledger.AssignDosimeter("survivor_scout", "tag_scout_02");
            var rng = new SeededRng(101);

            ledger.BookReadingFromLifetime(
                "survivor_scout",
                day: 15,
                lifetimeNowMsv: 0.028f,
                source: "loc_ruined_hospital_grounds",
                highEnergyEvent: false,
                rng: rng);

            // Capture state
            var state = ledger.State;
            Assert.NotNull(state);

            // Restore into fresh ledger
            var freshLedger = new DoseLedgerSystem();
            freshLedger.AssignDosimeter("survivor_scout", "tag_scout_02");
            var restoredEntry = state.entries.FirstOrDefault(e => e.survivorId == "survivor_scout");
            Assert.NotNull(restoredEntry);
            Assert.Single(restoredEntry.readingsHistory);
            Assert.Equal("loc_ruined_hospital_grounds", restoredEntry.readingsHistory[0].source);
            Assert.Equal(0.028f, restoredEntry.readingsHistory[0].nominalMsv);
        }
    }
}
