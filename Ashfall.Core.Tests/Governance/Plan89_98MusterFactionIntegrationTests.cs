// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Muster;
using Xunit;

namespace Ashfall.Core.Tests.Governance
{
    /// <summary>
    /// Cross-system integration tests for Wave 40 Batch 3:
    /// - Plan 89 (DEC-259): Muster Epilogues Expansion (12 -> 25 campaign-ending epilogues)
    /// - Plan 98 (DEC-260): Standing Record Factions Expansion (1 -> 8 factions)
    ///
    /// Validates referential integrity, deterministic ending matrix evaluation,
    /// standing record faction catalog loader binding, and cross-system alignment
    /// between faction territorial/economic authority and terminal campaign outcomes.
    /// </summary>
    public sealed class Plan89_98MusterFactionIntegrationTests
    {
        private static string ResolveDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(candidate)) return candidate;

            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }

            string current = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(current, out string found))
                return found;

            throw new DirectoryNotFoundException("Could not locate Assets/StreamingAssets/Data directory.");
        }

        [Fact]
        public void Plan89_MusterEpiloguesCatalog_LoadsAll25Outcomes_WithValidProseAndTitles()
        {
            string dataDir = ResolveDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            List<EndingDefinition> epilogues = EpilogueMatrixLoader.LoadEpilogues(dataDir, io, json);

            Assert.NotNull(epilogues);
            Assert.Equal(25, epilogues.Count);

            var seenKeys = new HashSet<string>(StringComparer.Ordinal);
            foreach (var epilogue in epilogues)
            {
                Assert.False(string.IsNullOrWhiteSpace(epilogue.endingKey), "Ending key must not be empty.");
                Assert.False(string.IsNullOrWhiteSpace(epilogue.title), $"Title for {epilogue.endingKey} must not be empty.");
                Assert.False(string.IsNullOrWhiteSpace(epilogue.prose), $"Prose for {epilogue.endingKey} must not be empty.");
                Assert.True(epilogue.prose.Length >= 20, $"Prose for {epilogue.endingKey} must be substantial narrative text.");
                Assert.True(seenKeys.Add(epilogue.endingKey), $"Duplicate ending key: {epilogue.endingKey}");
            }

            // Verify all keys declared in EpilogueMatrix.AllKeys are present in the catalog
            foreach (string expectedKey in EpilogueMatrix.AllKeys)
            {
                Assert.Contains(expectedKey, seenKeys);
            }
        }

        [Fact]
        public void Plan98_StandingRecordCatalogLoader_LoadsAll8Factions_WithCompleteDossiers()
        {
            string dataDir = ResolveDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new StandingRecordCatalogLoader(io, json);

            StandingRecordCatalog catalog = loader.Load(dataDir);

            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.Factions.Count);

            string[] expectedFactions =
            {
                "faction_the_overlay",
                "faction_the_scale",
                "faction_the_compact",
                "faction_the_underwrite",
                "faction_the_cutters",
                "faction_the_fleet",
                "faction_the_rebuilders",
                "faction_the_garrison"
            };

            foreach (string expectedId in expectedFactions)
            {
                var faction = catalog.GetFaction(expectedId);
                Assert.NotNull(faction);
                Assert.Equal(expectedId, faction.id);
                Assert.False(string.IsNullOrWhiteSpace(faction.display_name));
                Assert.False(string.IsNullOrWhiteSpace(faction.alignment));
                Assert.False(string.IsNullOrWhiteSpace(faction.home_region));
                Assert.False(string.IsNullOrWhiteSpace(faction.signature_quote));
                Assert.False(string.IsNullOrWhiteSpace(faction.access_rule));
                Assert.NotEmpty(faction.wants);
                Assert.NotEmpty(faction.offers);
                Assert.True(faction.is_active);
            }

            // Verify baseline faction "The Overlay" invariants
            var overlay = catalog.GetFaction("faction_the_overlay");
            Assert.NotNull(overlay);
            Assert.Equal("The Overlay", overlay.display_name);
            Assert.Equal("conditional", overlay.alignment);
            Assert.Equal("all_regions", overlay.home_region);
            Assert.Contains("cadastral_keys", overlay.offers);
        }

        [Fact]
        public void CrossSystem_FactionAuthority_MapsDeterministicallyToEpilogueOutcomes()
        {
            string dataDir = ResolveDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new StandingRecordCatalogLoader(io, json);

            StandingRecordCatalog catalog = loader.Load(dataDir);
            List<EndingDefinition> epilogues = EpilogueMatrixLoader.LoadEpilogues(dataDir, io, json);
            var epilogueMap = epilogues.ToDictionary(e => e.endingKey, e => e);

            // 1. Central Garrison integration outcome
            var garrison = catalog.GetFaction("faction_the_garrison");
            Assert.NotNull(garrison);
            Assert.Equal("ash_flats", garrison.home_region);
            var garrisonEnding = EpilogueMatrix.Evaluate(new EpilogueMatrixInput
            {
                FactionOutcome = FactionTerminalOutcome.GarrisonAbsorbed
            });
            Assert.Equal(EpilogueMatrix.GarrisonAbsorbsCoalition, garrisonEnding);
            Assert.True(epilogueMap.ContainsKey(garrisonEnding));
            Assert.Contains("Garrison", epilogueMap[garrisonEnding].prose);

            // 2. Rebuilders cooperative outcome
            var rebuilders = catalog.GetFaction("faction_the_rebuilders");
            Assert.NotNull(rebuilders);
            Assert.Equal("ash_flats", rebuilders.home_region);
            var rebuildersEnding = EpilogueMatrix.Evaluate(new EpilogueMatrixInput
            {
                FactionOutcome = FactionTerminalOutcome.RebuildersJoined
            });
            Assert.Equal(EpilogueMatrix.RebuildersJoined, rebuildersEnding);
            Assert.True(epilogueMap.ContainsKey(rebuildersEnding));
            Assert.Contains("reconstruction", epilogueMap[rebuildersEnding].prose);

            // 3. Independent stance defying external factions
            var independentEnding = EpilogueMatrix.Evaluate(new EpilogueMatrixInput
            {
                FactionOutcome = FactionTerminalOutcome.Independent
            });
            Assert.Equal(EpilogueMatrix.CoalitionIndependent, independentEnding);
            Assert.True(epilogueMap.ContainsKey(independentEnding));
            Assert.Contains("No regional colors hang over the gate", epilogueMap[independentEnding].prose);

            // 4. Resource control: The Scale (water authority) vs WaterPlantHeld epilogue
            var theScale = catalog.GetFaction("faction_the_scale");
            Assert.NotNull(theScale);
            Assert.Equal("industrial_belt", theScale.home_region);
            Assert.Contains("potable_ration_quota", theScale.offers);
            var waterEnding = EpilogueMatrix.Evaluate(new EpilogueMatrixInput
            {
                WaterPlantHeld = true
            });
            Assert.Equal(EpilogueMatrix.WaterPlantHeld, waterEnding);
            Assert.Contains("Desalination Unit 4", epilogueMap[waterEnding].prose);

            // 5. Compound: Mercy + Water plant held
            var mercyWaterEnding = EpilogueMatrix.Evaluate(new EpilogueMatrixInput
            {
                MercyPattern = true,
                WaterPlantHeld = true
            });
            Assert.Equal(EpilogueMatrix.MercyWaterHeld, mercyWaterEnding);
            Assert.Contains("public cistern", epilogueMap[mercyWaterEnding].prose);
        }

        [Fact]
        public void CrossSystem_EpilogueResolution_IsDeterministicAndStrictlyPrioritized()
        {
            // Catastrophic collapse overrides all faction standing or resource holds
            var collapseInput = new EpilogueMatrixInput
            {
                ShelterFallen = true,
                WaterPlantHeld = true,
                GrainSiloCaptured = true,
                FactionOutcome = FactionTerminalOutcome.GarrisonAbsorbed,
                MercyPattern = true
            };

            for (int i = 0; i < 100; i++)
            {
                string outcome = EpilogueMatrix.Evaluate(collapseInput);
                Assert.Equal(EpilogueMatrix.ShelterFalls, outcome);
            }

            // Compound overrides specific verdict or faction outcomes when shelter intact
            var compoundInput = new EpilogueMatrixInput
            {
                IronPattern = true,
                FuelDepotBurned = true,
                FactionOutcome = FactionTerminalOutcome.FoundryAnnexed
            };

            for (int i = 0; i < 100; i++)
            {
                string outcome = EpilogueMatrix.Evaluate(compoundInput);
                Assert.Equal(EpilogueMatrix.IronFuelAsh, outcome);
            }

            // Fallback unwritten outcome when no condition triggers
            string fallback = EpilogueMatrix.Evaluate(new EpilogueMatrixInput());
            Assert.Equal(EpilogueMatrix.Unwritten, fallback);
        }
    }
}
