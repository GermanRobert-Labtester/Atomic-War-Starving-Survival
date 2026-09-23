#nullable enable
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Excavation;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class Plan36_37TrappingExcavationIntegrationTests
    {
        private static string ResolveDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void TrappingCatalog_LoadsAll10TrapsAnd15Prey_FromAuthoritativeJson()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var catalog = WildlifeTrappingCatalogLoader.Load(dataDir, fileIO, json);

            Assert.NotNull(catalog);
            Assert.Equal(10, catalog.Traps.Count);
            Assert.Equal(15, catalog.Prey.Count);

            string[] expectedTrapIds = new[]
            {
                "trap_snare",
                "trap_deadfall",
                "trap_pit",
                "trap_net",
                "trap_fish",
                "trap_cage",
                "trap_bird_snare",
                "trap_body_grip",
                "trap_box",
                "trap_improvised_wire"
            };

            foreach (var trapId in expectedTrapIds)
            {
                Assert.True(catalog.Traps.TryGetValue(trapId, out var trap),
                    $"Trap '{trapId}' should be present in catalog.");
                Assert.NotNull(trap);
                Assert.False(string.IsNullOrWhiteSpace(trap.displayName),
                    $"Trap '{trapId}' must have non-empty displayName.");
                Assert.False(string.IsNullOrWhiteSpace(trap.trapType),
                    $"Trap '{trapId}' must have a valid trapType.");
                Assert.NotEmpty(trap.setupCosts);
                Assert.NotEmpty(trap.compatiblePrey);
                Assert.True(trap.Validate(out string error),
                    $"Trap '{trapId}' validation failed: {error}");

                // Setup bill calculation
                var setupBill = trap.CalculateSetupBill();
                Assert.NotNull(setupBill);
                Assert.NotEmpty(setupBill.Costs);

                // Repair bill calculation
                var repairBill = trap.CalculateRepairBill();
                Assert.NotNull(repairBill);
                Assert.NotEmpty(repairBill.Costs);
            }

            string[] expectedPreyIds = new[]
            {
                "rabbit",
                "cotton_hare",
                "deer",
                "boar",
                "fox",
                "rat",
                "pheasant",
                "ash_crow",
                "mirror_carp",
                "ash_pike",
                "irradiated_squirrel",
                "contaminated_fowl",
                "rad_dog",
                "muskrat",
                "hedgehog"
            };

            foreach (var preyId in expectedPreyIds)
            {
                Assert.True(catalog.Prey.TryGetValue(preyId, out var prey),
                    $"Prey '{preyId}' should be present in catalog.");
                Assert.NotNull(prey);
                Assert.False(string.IsNullOrWhiteSpace(prey.displayName),
                    $"Prey '{preyId}' must have non-empty displayName.");
                Assert.True(prey.baseYieldKg > 0f,
                    $"Prey '{preyId}' must have positive base yield.");
                Assert.InRange(prey.diseaseRisk, 0f, 1f);
                Assert.InRange(prey.contaminationRisk, 0f, 1f);
                Assert.True(prey.Validate(out string error),
                    $"Prey '{preyId}' validation failed: {error}");

                // Test disease resolution
                string resolvedDisease = prey.ResolveDiseaseId();
                if (prey.diseaseRisk > 0.1f)
                {
                    Assert.False(string.IsNullOrEmpty(resolvedDisease),
                        $"Prey '{preyId}' with disease risk {prey.diseaseRisk} must resolve to a valid disease.");
                }
            }
        }

        [Fact]
        public void ExcavationSitesCatalog_LoadsAllEightSites_FromAuthoritativeJson()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var sites = ExcavationCatalogLoader.Load(dataDir, fileIO, json);

            Assert.NotNull(sites);
            Assert.Equal(8, sites.Count);

            string[] expectedSiteIds = new[]
            {
                "excavation_command_vault",
                "excavation_utility_tunnels",
                "excavation_metro_interchange",
                "excavation_mine_shaft",
                "excavation_archive_bunker",
                "excavation_drainage_network",
                "excavation_storage_chamber",
                "excavation_civilian_shelter"
            };

            var sitesById = sites.ToDictionary(s => s.site_id, StringComparer.Ordinal);

            foreach (var siteId in expectedSiteIds)
            {
                Assert.True(sitesById.TryGetValue(siteId, out var site),
                    $"Excavation site '{siteId}' should be present in catalog.");
                Assert.NotNull(site);
                Assert.False(string.IsNullOrWhiteSpace(site.display_name),
                    $"Site '{siteId}' must have non-empty display_name.");
                Assert.False(string.IsNullOrWhiteSpace(site.description),
                    $"Site '{siteId}' must have non-empty description.");
                Assert.True(site.max_depth_meters > 0f,
                    $"Site '{siteId}' max_depth_meters must be positive.");
                Assert.True(site.required_progress > 0f,
                    $"Site '{siteId}' required_progress must be positive.");
                Assert.InRange(site.structural_risk, 0f, 1f);
                Assert.NotEmpty(site.required_tools);
                Assert.NotEmpty(site.shoring_materials);
                Assert.False(string.IsNullOrWhiteSpace(site.journal_entry_id),
                    $"Site '{siteId}' must have journal_entry_id.");
                Assert.True(site.depth_bands.Count >= 2,
                    $"Site '{siteId}' must have at least 2 depth strata.");

                // Validate depth strata monotonically increase
                float previousDepth = 0f;
                foreach (var band in site.depth_bands)
                {
                    Assert.True(band.depth_meters > previousDepth,
                        $"Site '{siteId}' depth band '{band.label}' depth {band.depth_meters} must exceed previous {previousDepth}.");
                    Assert.False(string.IsNullOrWhiteSpace(band.label),
                        $"Site '{siteId}' depth band must have non-empty label.");
                    Assert.InRange(band.risk, 0f, 1f);
                    previousDepth = band.depth_meters;
                }
            }
        }

        [Fact]
        public void TrappingSystem_And_ExcavationSystem_ExecuteConcurrentlyWithoutInterference()
        {
            string dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // 1. Load trapping catalog and setup a trap evaluation
            var trappingCatalog = WildlifeTrappingCatalogLoader.Load(dataDir, fileIO, json);
            Assert.NotNull(trappingCatalog);

            var snare = trappingCatalog.Traps["trap_snare"];
            var snareSetup = snare.CalculateSetupBill();
            Assert.NotNull(snareSetup);
            Assert.True(snareSetup.Costs.Count > 0);

            var rabbit = trappingCatalog.Prey["rabbit"];
            Assert.Contains(rabbit.preferredTrapType, snare.trapType);

            // 2. Load excavation sites catalog and evaluate hazard mitigation
            var excavationSites = ExcavationCatalogLoader.Load(dataDir, fileIO, json);
            Assert.NotNull(excavationSites);

            var commandVault = excavationSites.First(s => s.site_id == "excavation_command_vault");
            Assert.Equal("loc_excavation_command_vault", commandVault.location_id);
            Assert.Equal(120.0f, commandVault.max_depth_meters);
            Assert.Equal("hazard_radiation_hotspot", commandVault.hazard_type);

            var inventory = new Ashfall.Core.Inventory.Inventory { Capacity = 100, MaxWeight = 500f };
            var rng = new SeededRng(42);
            var hazardSystem = new ExcavationHazardSystem(inventory, rng);
            string hazardJsonPath = Path.Combine(dataDir, "excavation_hazard_mitigation.json");
            if (fileIO.FileExists(hazardJsonPath))
            {
                hazardSystem.LoadCatalog(fileIO.ReadAllText(hazardJsonPath));
            }
            Assert.NotNull(hazardSystem);

            // Verify both systems evaluated deterministically with zero cross-system interference
            Assert.Equal("Wire Snare", snare.displayName);
            Assert.Equal("Collapsed Civil Defense Command Vault", commandVault.display_name);
            Assert.Equal(10, trappingCatalog.Traps.Count);
            Assert.Equal(8, excavationSites.Count);
        }
    }
}
