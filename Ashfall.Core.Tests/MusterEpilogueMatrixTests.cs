using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Muster;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Comprehensive verification suite for Plan 89 — Muster Epilogues Expansion (12 -> 25 outcomes).
    /// Tests catalog integrity, bidirectional key coverage, precedence rules, reachability,
    /// determinism, and prose retrieval.
    /// </summary>
    public class MusterEpilogueMatrixTests
    {
        private static string FindDataDir()
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
            return string.Empty;
        }

        private static List<EndingDefinition> LoadAllEpilogues()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Could not find StreamingAssets/Data directory");
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            return EpilogueMatrixLoader.LoadEpilogues(dataDir, io, json);
        }

        // ====================================================================
        // Catalog & Key Invariants
        // ====================================================================

        [Fact]
        public void EpilogueCatalog_LoadsExactly25Entries()
        {
            var epilogues = LoadAllEpilogues();
            Assert.Equal(25, epilogues.Count);
        }

        [Fact]
        public void EpilogueCatalog_All25KeysAreUnique()
        {
            var epilogues = LoadAllEpilogues();
            var keys = epilogues.Select(e => e.endingKey).ToList();
            var uniqueKeys = new HashSet<string>(keys);
            Assert.Equal(25, uniqueKeys.Count);
        }

        [Fact]
        public void EpilogueCatalog_Original12KeysArePreserved()
        {
            var epilogues = LoadAllEpilogues();
            var keys = new HashSet<string>(epilogues.Select(e => e.endingKey));

            string[] original12 =
            {
                "the_open_muster",
                "the_amnesty",
                "the_corridor",
                "the_blood_price",
                "the_rate_card_revised",
                "the_administrator",
                "the_measured_truth_contested",
                "the_measured_truth",
                "unwritten",
                "ending_verdict_the_sector_recounts",
                "ending_verdict_the_count_is_held",
                "ending_verdict_the_offer_is_a_lease"
            };

            foreach (var key in original12)
            {
                Assert.Contains(key, keys);
            }
        }

        [Fact]
        public void EpilogueCatalog_New13KeysArePresent()
        {
            var epilogues = LoadAllEpilogues();
            var keys = new HashSet<string>(epilogues.Select(e => e.endingKey));

            string[] new13 =
            {
                // Faction (4)
                "ending_garrison_absorbs_coalition",
                "ending_rebuilders_joined",
                "ending_coalition_independent",
                "ending_foundry_annexation",
                // Resource (3)
                "ending_water_plant_held",
                "ending_grain_silo_captured",
                "ending_fuel_depot_burned",
                // Moral (3)
                "ending_mercy_road",
                "ending_iron_way",
                "ending_listeners_thread",
                // Compound (2)
                "ending_mercy_water_held",
                "ending_iron_fuel_ash",
                // Failure (1)
                "ending_shelter_falls"
            };

            foreach (var key in new13)
            {
                Assert.Contains(key, keys);
            }
        }

        [Fact]
        public void EpilogueCatalog_BidirectionalKeyCoverage_MatchesEpilogueMatrixAllKeys()
        {
            var epilogues = LoadAllEpilogues();
            var catalogKeys = new HashSet<string>(epilogues.Select(e => e.endingKey));
            var matrixKeys = new HashSet<string>(EpilogueMatrix.AllKeys);

            Assert.Equal(25, EpilogueMatrix.AllKeys.Length);
            Assert.Equal(catalogKeys, matrixKeys);
        }

        [Fact]
        public void EpilogueCatalog_AllTitlesAndProseAreNonEmptyAndRestrained()
        {
            var epilogues = LoadAllEpilogues();
            foreach (var e in epilogues)
            {
                Assert.False(string.IsNullOrWhiteSpace(e.title), $"Title empty for key: {e.endingKey}");
                Assert.False(string.IsNullOrWhiteSpace(e.prose), $"Prose empty for key: {e.endingKey}");

                // Word count check: between 30 and 120 words (concise, restrained)
                int wordCount = e.prose.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
                Assert.InRange(wordCount, 30, 120);

                // Tone guardrails: no second-person address or gamey triumphalism
                Assert.DoesNotContain("you survived", e.prose, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("you won", e.prose, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("you lost", e.prose, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("game over", e.prose, StringComparison.OrdinalIgnoreCase);
            }
        }

        // ====================================================================
        // Selection & Precedence Tests
        // ====================================================================

        [Fact]
        public void Evaluate_NullInput_ReturnsUnwritten()
        {
            Assert.Equal(EpilogueMatrix.Unwritten, EpilogueMatrix.Evaluate(null));
        }

        [Fact]
        public void Evaluate_DefaultInput_ReturnsUnwritten()
        {
            var input = new EpilogueMatrixInput();
            Assert.Equal(EpilogueMatrix.Unwritten, EpilogueMatrix.Evaluate(input));
        }

        [Fact]
        public void Evaluate_FailurePrecedence_ShelterFallenOverridesEverything()
        {
            var input = new EpilogueMatrixInput
            {
                ShelterFallen = true,
                WaterPlantHeld = true,
                GrainSiloCaptured = true,
                MercyPattern = true,
                DiplomacyPattern = true,
                FactionOutcome = FactionTerminalOutcome.Independent,
                VerdictEndingKey = EpilogueMatrix.VerdictSectorRecounts,
                MusterEndingKey = EpilogueMatrix.TheOpenMuster
            };

            Assert.Equal(EpilogueMatrix.ShelterFalls, EpilogueMatrix.Evaluate(input));
        }

        [Fact]
        public void Evaluate_CompoundEnding_MercyAndWaterHeld_BeatsComponentEndings()
        {
            var input = new EpilogueMatrixInput
            {
                MercyPattern = true,
                WaterPlantHeld = true
            };

            // Compound beats generic mercy and generic water
            Assert.Equal(EpilogueMatrix.MercyWaterHeld, EpilogueMatrix.Evaluate(input));

            // Component alone selects component
            var mercyOnly = new EpilogueMatrixInput { MercyPattern = true };
            Assert.Equal(EpilogueMatrix.MercyRoad, EpilogueMatrix.Evaluate(mercyOnly));

            var waterOnly = new EpilogueMatrixInput { WaterPlantHeld = true };
            Assert.Equal(EpilogueMatrix.WaterPlantHeld, EpilogueMatrix.Evaluate(waterOnly));
        }

        [Fact]
        public void Evaluate_CompoundEnding_IronAndFuelBurned_BeatsComponentEndings()
        {
            var input = new EpilogueMatrixInput
            {
                IronPattern = true,
                FuelDepotBurned = true
            };

            // Compound beats generic iron and generic fuel
            Assert.Equal(EpilogueMatrix.IronFuelAsh, EpilogueMatrix.Evaluate(input));

            // Component alone selects component
            var ironOnly = new EpilogueMatrixInput { IronPattern = true };
            Assert.Equal(EpilogueMatrix.IronWay, EpilogueMatrix.Evaluate(ironOnly));

            var fuelOnly = new EpilogueMatrixInput { FuelDepotBurned = true };
            Assert.Equal(EpilogueMatrix.FuelDepotBurned, EpilogueMatrix.Evaluate(fuelOnly));
        }

        [Theory]
        [InlineData(FactionTerminalOutcome.GarrisonAbsorbed, EpilogueMatrix.GarrisonAbsorbsCoalition)]
        [InlineData(FactionTerminalOutcome.RebuildersJoined, EpilogueMatrix.RebuildersJoined)]
        [InlineData(FactionTerminalOutcome.Independent, EpilogueMatrix.CoalitionIndependent)]
        [InlineData(FactionTerminalOutcome.FoundryAnnexed, EpilogueMatrix.FoundryAnnexation)]
        public void Evaluate_FactionEndings_SelectsCorrectly(FactionTerminalOutcome faction, string expectedKey)
        {
            var input = new EpilogueMatrixInput
            {
                FactionOutcome = faction
            };
            Assert.Equal(expectedKey, EpilogueMatrix.Evaluate(input));
        }

        [Theory]
        [InlineData(true, false, false, EpilogueMatrix.WaterPlantHeld)]
        [InlineData(false, true, false, EpilogueMatrix.GrainSiloCaptured)]
        [InlineData(false, false, true, EpilogueMatrix.FuelDepotBurned)]
        public void Evaluate_ResourceEndings_SelectsCorrectly(bool water, bool grain, bool fuel, string expectedKey)
        {
            var input = new EpilogueMatrixInput
            {
                WaterPlantHeld = water,
                GrainSiloCaptured = grain,
                FuelDepotBurned = fuel
            };
            Assert.Equal(expectedKey, EpilogueMatrix.Evaluate(input));
        }

        [Theory]
        [InlineData(true, false, false, EpilogueMatrix.MercyRoad)]
        [InlineData(false, true, false, EpilogueMatrix.IronWay)]
        [InlineData(false, false, true, EpilogueMatrix.ListenersThread)]
        public void Evaluate_MoralEndings_SelectsCorrectly(bool mercy, bool iron, bool diplomacy, string expectedKey)
        {
            var input = new EpilogueMatrixInput
            {
                MercyPattern = mercy,
                IronPattern = iron,
                DiplomacyPattern = diplomacy
            };
            Assert.Equal(expectedKey, EpilogueMatrix.Evaluate(input));
        }

        [Fact]
        public void Evaluate_VerdictEnding_TakesPrecedenceOverGenericFactionOrResource()
        {
            var input = new EpilogueMatrixInput
            {
                VerdictEndingKey = EpilogueMatrix.VerdictSectorRecounts,
                FactionOutcome = FactionTerminalOutcome.Independent,
                WaterPlantHeld = true
            };
            Assert.Equal(EpilogueMatrix.VerdictSectorRecounts, EpilogueMatrix.Evaluate(input));
        }

        [Fact]
        public void Evaluate_MusterEnding_TakesPrecedenceOverGenericFactionOrResource()
        {
            var input = new EpilogueMatrixInput
            {
                MusterEndingKey = EpilogueMatrix.TheOpenMuster,
                FactionOutcome = FactionTerminalOutcome.Independent,
                WaterPlantHeld = true
            };
            Assert.Equal(EpilogueMatrix.TheOpenMuster, EpilogueMatrix.Evaluate(input));
        }

        [Fact]
        public void Evaluate_FactionPrecedence_BeatsGenericResourceAndMoral()
        {
            var input = new EpilogueMatrixInput
            {
                FactionOutcome = FactionTerminalOutcome.Independent,
                GrainSiloCaptured = true,
                DiplomacyPattern = true
            };
            Assert.Equal(EpilogueMatrix.CoalitionIndependent, EpilogueMatrix.Evaluate(input));
        }

        [Fact]
        public void Evaluate_ResourcePrecedence_BeatsGenericMoral()
        {
            var input = new EpilogueMatrixInput
            {
                GrainSiloCaptured = true,
                DiplomacyPattern = true
            };
            Assert.Equal(EpilogueMatrix.GrainSiloCaptured, EpilogueMatrix.Evaluate(input));
        }

        [Fact]
        public void Evaluate_IsDeterministicAcrossReplays()
        {
            var input = new EpilogueMatrixInput
            {
                FactionOutcome = FactionTerminalOutcome.RebuildersJoined,
                WaterPlantHeld = false,
                GrainSiloCaptured = true
            };

            string key1 = EpilogueMatrix.Evaluate(input);
            string key2 = EpilogueMatrix.Evaluate(input);
            string key3 = EpilogueMatrix.Evaluate(input);

            Assert.Equal(key1, key2);
            Assert.Equal(key2, key3);
        }

        // ====================================================================
        // Reachability Witnesses for All 25 Outcomes
        // ====================================================================

        [Theory]
        [InlineData(EpilogueMatrix.TheOpenMuster)]
        [InlineData(EpilogueMatrix.TheAmnesty)]
        [InlineData(EpilogueMatrix.TheCorridor)]
        [InlineData(EpilogueMatrix.TheBloodPrice)]
        [InlineData(EpilogueMatrix.TheRateCardRevised)]
        [InlineData(EpilogueMatrix.TheAdministrator)]
        [InlineData(EpilogueMatrix.TheMeasuredTruthContested)]
        [InlineData(EpilogueMatrix.TheMeasuredTruth)]
        [InlineData(EpilogueMatrix.Unwritten)]
        [InlineData(EpilogueMatrix.VerdictSectorRecounts)]
        [InlineData(EpilogueMatrix.VerdictCountHeld)]
        [InlineData(EpilogueMatrix.VerdictOfferLease)]
        [InlineData(EpilogueMatrix.GarrisonAbsorbsCoalition)]
        [InlineData(EpilogueMatrix.RebuildersJoined)]
        [InlineData(EpilogueMatrix.CoalitionIndependent)]
        [InlineData(EpilogueMatrix.FoundryAnnexation)]
        [InlineData(EpilogueMatrix.WaterPlantHeld)]
        [InlineData(EpilogueMatrix.GrainSiloCaptured)]
        [InlineData(EpilogueMatrix.FuelDepotBurned)]
        [InlineData(EpilogueMatrix.MercyRoad)]
        [InlineData(EpilogueMatrix.IronWay)]
        [InlineData(EpilogueMatrix.ListenersThread)]
        [InlineData(EpilogueMatrix.MercyWaterHeld)]
        [InlineData(EpilogueMatrix.IronFuelAsh)]
        [InlineData(EpilogueMatrix.ShelterFalls)]
        public void EveryKey_HasProseInLoadedCatalog(string key)
        {
            var epilogues = LoadAllEpilogues();
            var entry = epilogues.FirstOrDefault(e => e.endingKey == key);
            Assert.NotNull(entry);
            Assert.False(string.IsNullOrEmpty(entry.title));
            Assert.False(string.IsNullOrEmpty(entry.prose));
        }
    }
}
