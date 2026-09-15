// SPDX-License-Identifier: MIT
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

        [Fact]
        public void Evaluate_FactionEndings_SelectsCorrectly()
        {
            var cases = new[]
            {
                (FactionTerminalOutcome.GarrisonAbsorbed, EpilogueMatrix.GarrisonAbsorbsCoalition),
                (FactionTerminalOutcome.RebuildersJoined, EpilogueMatrix.RebuildersJoined),
                (FactionTerminalOutcome.Independent, EpilogueMatrix.CoalitionIndependent),
                (FactionTerminalOutcome.FoundryAnnexed, EpilogueMatrix.FoundryAnnexation)
            };
            var failures = new List<string>();

            foreach (var (faction, expectedKey) in cases)
            {
                string actual = EpilogueMatrix.Evaluate(new EpilogueMatrixInput
                {
                    FactionOutcome = faction
                });
                if (!string.Equals(expectedKey, actual, StringComparison.Ordinal))
                    failures.Add($"{faction}: expected {expectedKey}, got {actual}");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void Evaluate_ResourceEndings_SelectsCorrectly()
        {
            var cases = new[]
            {
                (Water: true, Grain: false, Fuel: false, Expected: EpilogueMatrix.WaterPlantHeld),
                (Water: false, Grain: true, Fuel: false, Expected: EpilogueMatrix.GrainSiloCaptured),
                (Water: false, Grain: false, Fuel: true, Expected: EpilogueMatrix.FuelDepotBurned)
            };
            var failures = new List<string>();

            foreach (var test in cases)
            {
                string actual = EpilogueMatrix.Evaluate(new EpilogueMatrixInput
                {
                    WaterPlantHeld = test.Water,
                    GrainSiloCaptured = test.Grain,
                    FuelDepotBurned = test.Fuel
                });
                if (!string.Equals(test.Expected, actual, StringComparison.Ordinal))
                {
                    failures.Add(
                        $"water={test.Water}, grain={test.Grain}, fuel={test.Fuel}: " +
                        $"expected {test.Expected}, got {actual}");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void Evaluate_MoralEndings_SelectsCorrectly()
        {
            var cases = new[]
            {
                (Mercy: true, Iron: false, Diplomacy: false, Expected: EpilogueMatrix.MercyRoad),
                (Mercy: false, Iron: true, Diplomacy: false, Expected: EpilogueMatrix.IronWay),
                (Mercy: false, Iron: false, Diplomacy: true, Expected: EpilogueMatrix.ListenersThread)
            };
            var failures = new List<string>();

            foreach (var test in cases)
            {
                string actual = EpilogueMatrix.Evaluate(new EpilogueMatrixInput
                {
                    MercyPattern = test.Mercy,
                    IronPattern = test.Iron,
                    DiplomacyPattern = test.Diplomacy
                });
                if (!string.Equals(test.Expected, actual, StringComparison.Ordinal))
                {
                    failures.Add(
                        $"mercy={test.Mercy}, iron={test.Iron}, diplomacy={test.Diplomacy}: " +
                        $"expected {test.Expected}, got {actual}");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
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

        [Fact]
        public void EveryKey_HasProseInLoadedCatalog()
        {
            var epilogues = LoadAllEpilogues();
            var failures = new List<string>();

            foreach (var key in EpilogueMatrix.AllKeys)
            {
                var entry = epilogues.FirstOrDefault(e => e.endingKey == key);
                if (entry == null)
                {
                    failures.Add($"{key}: missing from loaded catalog");
                    continue;
                }

                if (string.IsNullOrEmpty(entry.title))
                    failures.Add($"{key}: title is empty");
                if (string.IsNullOrEmpty(entry.prose))
                    failures.Add($"{key}: prose is empty");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        // ====================================================================
        // Additional Overlap, Failure Precedence, and Plan 96 Integration Tests
        // ====================================================================

        [Fact]
        public void Evaluate_Failure_BeatsPositiveResource_Individually()
        {
            var input = new EpilogueMatrixInput
            {
                ShelterFallen = true,
                WaterPlantHeld = true
            };
            Assert.Equal(EpilogueMatrix.ShelterFalls, EpilogueMatrix.Evaluate(input));
        }

        [Fact]
        public void Evaluate_Failure_BeatsPositiveFaction_Individually()
        {
            var input = new EpilogueMatrixInput
            {
                ShelterFallen = true,
                FactionOutcome = FactionTerminalOutcome.GarrisonAbsorbed
            };
            Assert.Equal(EpilogueMatrix.ShelterFalls, EpilogueMatrix.Evaluate(input));
        }

        [Fact]
        public void Evaluate_Failure_BeatsPositiveMoral_Individually()
        {
            var input = new EpilogueMatrixInput
            {
                ShelterFallen = true,
                MercyPattern = true
            };
            Assert.Equal(EpilogueMatrix.ShelterFalls, EpilogueMatrix.Evaluate(input));
        }

        [Fact]
        public void Evaluate_Overlap_IndependentAndWater_FactionWins()
        {
            var input = new EpilogueMatrixInput
            {
                FactionOutcome = FactionTerminalOutcome.Independent,
                WaterPlantHeld = true
            };
            Assert.Equal(EpilogueMatrix.CoalitionIndependent, EpilogueMatrix.Evaluate(input));
        }

        [Fact]
        public void Evaluate_Overlap_GarrisonAndWater_FactionWins()
        {
            var input = new EpilogueMatrixInput
            {
                FactionOutcome = FactionTerminalOutcome.GarrisonAbsorbed,
                WaterPlantHeld = true
            };
            Assert.Equal(EpilogueMatrix.GarrisonAbsorbsCoalition, EpilogueMatrix.Evaluate(input));
        }

        [Fact]
        public void Evaluate_Overlap_GrainAndListener_ResourceWins()
        {
            var input = new EpilogueMatrixInput
            {
                GrainSiloCaptured = true,
                DiplomacyPattern = true
            };
            Assert.Equal(EpilogueMatrix.GrainSiloCaptured, EpilogueMatrix.Evaluate(input));
        }

        [Fact]
        public void Evaluate_PureFunction_DoesNotMutateInput()
        {
            var input = new EpilogueMatrixInput
            {
                FactionOutcome = FactionTerminalOutcome.FoundryAnnexed,
                WaterPlantHeld = true,
                GrainSiloCaptured = false,
                FuelDepotBurned = false,
                MercyPattern = false,
                IronPattern = true,
                DiplomacyPattern = false,
                ShelterFallen = false,
                VerdictEndingKey = string.Empty,
                MusterEndingKey = string.Empty
            };

            string res = EpilogueMatrix.Evaluate(input);
            Assert.Equal(EpilogueMatrix.FoundryAnnexation, res);

            // Assert input state was unaffected
            Assert.Equal(FactionTerminalOutcome.FoundryAnnexed, input.FactionOutcome);
            Assert.True(input.WaterPlantHeld);
            Assert.False(input.GrainSiloCaptured);
            Assert.False(input.FuelDepotBurned);
            Assert.False(input.MercyPattern);
            Assert.True(input.IronPattern);
            Assert.False(input.DiplomacyPattern);
            Assert.False(input.ShelterFallen);
            Assert.Equal(string.Empty, input.VerdictEndingKey);
            Assert.Equal(string.Empty, input.MusterEndingKey);
        }

        [Fact]
        public void Plan96_All25Keys_CanBeBuiltIntoEpilogueChronicle()
        {
            var failures = new List<string>();

            foreach (var key in EpilogueMatrix.AllKeys)
            {
                try
                {
                    var builder = new Ashfall.Core.Endgame.EpilogueChronicleBuilder();
                    var chronicle = builder.Build(new Ashfall.Core.Endgame.EpilogueChronicleInput
                    {
                        EndingKey = key,
                        Day = 360,
                        BuildSeed = 12345
                    });

                    if (chronicle == null)
                    {
                        failures.Add($"{key}: builder returned null");
                        continue;
                    }

                    if (!string.Equals(key, chronicle.EndingKey, StringComparison.Ordinal))
                        failures.Add($"{key}: chronicle key was {chronicle.EndingKey}");
                    if (chronicle.GeneratedDay != 360)
                        failures.Add($"{key}: generated day was {chronicle.GeneratedDay}");
                    if (chronicle.BuildSeed != 12345)
                        failures.Add($"{key}: build seed was {chronicle.BuildSeed}");
                    if (string.IsNullOrEmpty(chronicle.Title))
                        failures.Add($"{key}: title is empty");
                }
                catch (Exception exception)
                {
                    failures.Add($"{key}: {exception.GetType().Name}: {exception.Message}");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }
    }
}
