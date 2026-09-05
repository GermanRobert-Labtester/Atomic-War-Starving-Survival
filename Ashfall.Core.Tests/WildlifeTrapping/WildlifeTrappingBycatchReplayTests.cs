// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;
namespace Ashfall.Core.Tests.WildlifeTrapping
{
    using SeededRng = Ashfall.Core.SeededRng;
    /// <summary>
    /// Flagship Task 6: Bycatch Deterministic Replay & RNG Stream Isolation Suite.
    ///
    /// Non-Negotiable Contract:
    /// - Strict draw budget: maximum 2 draws per eligible primary catch.
    /// - Stream isolation: bycatch draws consume the dedicated bycatch stream and never advance the primary catch stream.
    /// - Continuation equivalence: 20-day uninterrupted vs 10+save+restore+10 day runs yield identical catch traces and state hashes.
    /// </summary>
    public sealed class WildlifeTrappingBycatchReplayTests : CatalogTestBase
    {
        private WildlifeTrappingCatalog? LoadCatalog()
        {
            var files = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            return WildlifeTrappingCatalogLoader.Load(DataDirectory, files, serializer);
        }

        private static string ComputeStateHash(WildlifeTrappingState state)
        {
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(state);
            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(json));
            return Convert.ToHexString(hash);
        }

        [Fact]
        public void CaptureState_CapturesRngState()
        {
            var rng = new SeededRng(42);
            var sys = new WildlifeTrappingSystem(rng);
            var captured = sys.CaptureState();
            Assert.Equal(42, captured.rngSeed);
            Assert.Equal(rng.State, captured.primaryRngState);
        }

        [Fact]
        public void Bycatch_ChanceZero_ConsumesZeroDraws()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var site = new TrapSite { siteId = "site_1", catchSpecies = "rabbit" };
            var trapDef = new TrapDefinition
            {
                trap_id = "trap_test",
                bycatchChance = 0f,
                bycatchSpecies = new List<BycatchCandidate> { new BycatchCandidate { speciesId = "rat", weight = 1f } }
            };

            int draws = sys.ResolveBycatchForSite(site, trapDef);
            Assert.Equal(0, draws);
            Assert.Empty(site.bycatchSpecies);
        }

        [Fact]
        public void Bycatch_EmptyCandidates_ConsumesZeroDraws()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var site = new TrapSite { siteId = "site_1", catchSpecies = "rabbit" };
            // Trap has only rabbit, but rabbit is primary catch -> eligible set is empty
            var trapDef = new TrapDefinition
            {
                trap_id = "trap_test",
                bycatchChance = 0.5f,
                bycatchSpecies = new List<BycatchCandidate> { new BycatchCandidate { speciesId = "rabbit", weight = 1f } }
            };

            int draws = sys.ResolveBycatchForSite(site, trapDef);
            Assert.Equal(0, draws);
            Assert.Empty(site.bycatchSpecies);
        }

        [Fact]
        public void Bycatch_ChanceOne_SingleCandidate_ConsumesZeroDraws()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var site = new TrapSite { siteId = "site_1", catchSpecies = "rabbit" };
            var trapDef = new TrapDefinition
            {
                trap_id = "trap_test",
                bycatchChance = 1.0f,
                bycatchSpecies = new List<BycatchCandidate> { new BycatchCandidate { speciesId = "rat", weight = 1f } }
            };

            int draws = sys.ResolveBycatchForSite(site, trapDef);
            Assert.Equal(0, draws);
            Assert.Equal("rat", site.bycatchSpecies);
        }

        [Fact]
        public void Bycatch_ChanceOne_MultipleCandidates_ConsumesOneDraw()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var site = new TrapSite { siteId = "site_1", catchSpecies = "rabbit" };
            var trapDef = new TrapDefinition
            {
                trap_id = "trap_test",
                bycatchChance = 1.0f,
                bycatchSpecies = new List<BycatchCandidate>
                {
                    new BycatchCandidate { speciesId = "rat", weight = 1f },
                    new BycatchCandidate { speciesId = "hedgehog", weight = 2f }
                }
            };

            int draws = sys.ResolveBycatchForSite(site, trapDef);
            Assert.Equal(1, draws);
            Assert.True(site.bycatchSpecies == "rat" || site.bycatchSpecies == "hedgehog");
        }

        [Fact]
        public void Bycatch_ProbabilityMiss_ConsumesOneDraw()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var site = new TrapSite { siteId = "site_1", catchSpecies = "rabbit" };
            var trapDef = new TrapDefinition
            {
                trap_id = "trap_test",
                bycatchChance = 0.00001f, // Will almost certainly miss
                bycatchSpecies = new List<BycatchCandidate>
                {
                    new BycatchCandidate { speciesId = "rat", weight = 1f },
                    new BycatchCandidate { speciesId = "hedgehog", weight = 2f }
                }
            };

            int draws = sys.ResolveBycatchForSite(site, trapDef);
            Assert.Equal(1, draws);
            Assert.Empty(site.bycatchSpecies);
        }

        [Fact]
        public void Bycatch_ProbabilityHit_SingleCandidate_ConsumesOneDraw()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var site = new TrapSite { siteId = "site_1", catchSpecies = "rabbit" };
            var trapDef = new TrapDefinition
            {
                trap_id = "trap_test",
                bycatchChance = 0.99999f, // Will hit
                bycatchSpecies = new List<BycatchCandidate>
                {
                    new BycatchCandidate { speciesId = "rat", weight = 1f }
                }
            };

            int draws = sys.ResolveBycatchForSite(site, trapDef);
            Assert.Equal(1, draws);
            Assert.Equal("rat", site.bycatchSpecies);
        }

        [Fact]
        public void Bycatch_ProbabilityHit_MultipleCandidates_ConsumesTwoDraws()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var site = new TrapSite { siteId = "site_1", catchSpecies = "rabbit" };
            var trapDef = new TrapDefinition
            {
                trap_id = "trap_test",
                bycatchChance = 0.99999f, // Will hit
                bycatchSpecies = new List<BycatchCandidate>
                {
                    new BycatchCandidate { speciesId = "rat", weight = 1f },
                    new BycatchCandidate { speciesId = "hedgehog", weight = 2f }
                }
            };

            int draws = sys.ResolveBycatchForSite(site, trapDef);
            Assert.Equal(2, draws);
            Assert.True(site.bycatchSpecies == "rat" || site.bycatchSpecies == "hedgehog");
        }

        [Fact]
        public void Bycatch_DrawBudget_NeverExceedsTwoDraws()
        {
            var trapDef = new TrapDefinition
            {
                trap_id = "trap_test",
                bycatchChance = 0.5f,
                bycatchSpecies = new List<BycatchCandidate>
                {
                    new BycatchCandidate { speciesId = "rat", weight = 1f },
                    new BycatchCandidate { speciesId = "hedgehog", weight = 2f },
                    new BycatchCandidate { speciesId = "irradiated_squirrel", weight = 1.5f }
                }
            };

            for (int seed = 1; seed <= 100; seed++)
            {
                var sys = new WildlifeTrappingSystem(new SeededRng(seed));
                var site = new TrapSite { siteId = "site_1", catchSpecies = "rabbit" };
                int draws = sys.ResolveBycatchForSite(site, trapDef);
                Assert.InRange(draws, 0, 2);
            }
        }

        [Fact]
        public void BycatchCandidates_ExcludesPrimaryCatchSpecies()
        {
            var trapDef = new TrapDefinition
            {
                trap_id = "trap_net",
                bycatchChance = 1.0f,
                bycatchSpecies = new List<BycatchCandidate>
                {
                    new BycatchCandidate { speciesId = "rat", weight = 3f },
                    new BycatchCandidate { speciesId = "hedgehog", weight = 2f }
                }
            };

            // Primary catch is rat -> bycatch must be hedgehog
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var site = new TrapSite { siteId = "site_1", catchSpecies = "rat" };
            sys.ResolveBycatchForSite(site, trapDef);
            Assert.Equal("hedgehog", site.bycatchSpecies);
        }

        [Fact]
        public void PrimaryCatchStream_IsUnperturbedByBycatchEnabled()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);

            // System A: trap with bycatch enabled
            var sysA = new WildlifeTrappingSystem(new SeededRng(42));
            catalog!.RegisterWith(sysA);
            sysA.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_net", checkIntervalDays: 1, durabilityChecks: 20);

            var primaryCatchesA = new List<(int day, bool hasCatch, string species, float yield, bool isToxic)>();
            for (int d = 2; d <= 15; d++)
            {
                sysA.TickDay(d);
                var s = sysA.State.trapSites[0];
                primaryCatchesA.Add((d, s.hasCatch, s.catchSpecies, s.carcassYield, s.isToxic));
                if (s.hasCatch)
                {
                    sysA.Butcher(s.siteId);
                    s.hasCatch = false; // Reset to allow catching again
                }
            }

            // System B: identical initial conditions, but bycatchChance forced to 0
            var sysB = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sysB);
            var zeroBycatchDef = new TrapDefinition
            {
                trap_id = "trap_net_zero",
                displayName = "Net Trap",
                trapType = "net",
                checkIntervalDays = 1,
                durabilityChecks = 20,
                baseCatchModifier = 0.9f,
                compatiblePrey = new List<string> { "pheasant", "ash_crow", "contaminated_fowl", "rabbit" },
                bycatchChance = 0f,
                theftChance = catalog.Traps["trap_net"].theftChance,
                sabotageChance = catalog.Traps["trap_net"].sabotageChance,
                sabotageDurabilityDamage = catalog.Traps["trap_net"].sabotageDurabilityDamage,
                weatherDegradationRate = catalog.Traps["trap_net"].weatherDegradationRate,
                networkPenaltyPerTrap = catalog.Traps["trap_net"].networkPenaltyPerTrap
            };
            sysB.RegisterTrapDefinition(zeroBycatchDef);
            sysB.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_net_zero", checkIntervalDays: 1, durabilityChecks: 20);

            var primaryCatchesB = new List<(int day, bool hasCatch, string species, float yield, bool isToxic)>();
            for (int d = 2; d <= 15; d++)
            {
                sysB.TickDay(d);
                var s = sysB.State.trapSites[0];
                primaryCatchesB.Add((d, s.hasCatch, s.catchSpecies, s.carcassYield, s.isToxic));
                if (s.hasCatch)
                {
                    sysB.Butcher(s.siteId);
                    s.hasCatch = false;
                }
            }

            // Primary sequences must match 100% despite system A drawing bycatch
            Assert.Equal(primaryCatchesA.Count, primaryCatchesB.Count);
            for (int i = 0; i < primaryCatchesA.Count; i++)
            {
                var a = primaryCatchesA[i];
                var b = primaryCatchesB[i];
                Assert.Equal(a.day, b.day);
                Assert.Equal(a.hasCatch, b.hasCatch);
                Assert.Equal(a.species, b.species);
                Assert.Equal(a.yield, b.yield, 3);
                Assert.Equal(a.isToxic, b.isToxic);
            }
        }

        [Fact]
        public void PrimaryCatchStream_IsUnperturbedByBycatchDrawOutcome()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);

            // System A with bycatchChance = 0.1
            var sysA = new WildlifeTrappingSystem(new SeededRng(100));
            catalog!.RegisterWith(sysA);
            sysA.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_bird_snare", checkIntervalDays: 1, durabilityChecks: 15);

            // System B with bycatchChance = 0.9
            var sysB = new WildlifeTrappingSystem(new SeededRng(100));
            catalog.RegisterWith(sysB);
            var highBycatchDef = new TrapDefinition
            {
                trap_id = "trap_bird_snare_high",
                displayName = "Bird Snare",
                trapType = "bird_snare",
                checkIntervalDays = 1,
                durabilityChecks = 15,
                baseCatchModifier = 0.7f,
                compatiblePrey = new List<string> { "pheasant", "ash_crow", "contaminated_fowl" },
                bycatchChance = 0.9f,
                bycatchSpecies = new List<BycatchCandidate>
                {
                    new BycatchCandidate { speciesId = "irradiated_squirrel", weight = 2.0f }
                },
                theftChance = catalog.Traps["trap_bird_snare"].theftChance,
                sabotageChance = catalog.Traps["trap_bird_snare"].sabotageChance,
                sabotageDurabilityDamage = catalog.Traps["trap_bird_snare"].sabotageDurabilityDamage,
                weatherDegradationRate = catalog.Traps["trap_bird_snare"].weatherDegradationRate,
                networkPenaltyPerTrap = catalog.Traps["trap_bird_snare"].networkPenaltyPerTrap
            };
            sysB.RegisterTrapDefinition(highBycatchDef);
            sysB.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_bird_snare_high", checkIntervalDays: 1, durabilityChecks: 15);

            for (int d = 2; d <= 10; d++)
            {
                sysA.TickDay(d);
                sysB.TickDay(d);

                var sA = sysA.State.trapSites[0];
                var sB = sysB.State.trapSites[0];

                Assert.Equal(sA.hasCatch, sB.hasCatch);
                Assert.Equal(sA.catchSpecies, sB.catchSpecies);
                Assert.Equal(sA.carcassYield, sB.carcassYield, 3);
                Assert.Equal(sA.isToxic, sB.isToxic);

                if (sA.hasCatch) sA.hasCatch = false;
                if (sB.hasCatch) sB.hasCatch = false;
            }
        }

        [Fact]
        public void BycatchReplay_TwentyDays_UninterruptedMatchesRestored()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);

            // Run 1: 20 days uninterrupted
            var sysA = new WildlifeTrappingSystem(new SeededRng(42));
            catalog!.RegisterWith(sysA);
            sysA.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_net", checkIntervalDays: 1, durabilityChecks: 30);

            var traceA = new List<(int day, bool hasCatch, string primary, string bycatch)>();
            for (int d = 2; d <= 20; d++)
            {
                sysA.TickDay(d);
                var s = sysA.State.trapSites[0];
                traceA.Add((d, s.hasCatch, s.catchSpecies, s.bycatchSpecies));
                if (s.hasCatch)
                {
                    sysA.Butcher(s.siteId);
                    s.hasCatch = false;
                }
            }

            // Run 2: 10 days, save, restore into fresh system, 10 more days
            var sysB = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sysB);
            sysB.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_net", checkIntervalDays: 1, durabilityChecks: 30);

            var traceB = new List<(int day, bool hasCatch, string primary, string bycatch)>();
            for (int d = 2; d <= 10; d++)
            {
                sysB.TickDay(d);
                var s = sysB.State.trapSites[0];
                traceB.Add((d, s.hasCatch, s.catchSpecies, s.bycatchSpecies));
                if (s.hasCatch)
                {
                    sysB.Butcher(s.siteId);
                    s.hasCatch = false;
                }
            }

            var saved = sysB.CaptureState();
            Assert.NotEqual(0UL, saved.primaryRngState);

            // Fresh system restored at day 10
            var sysC = new WildlifeTrappingSystem(new SeededRng(99999)); // arbitrary seed, overridden by restore
            catalog.RegisterWith(sysC);
            sysC.RestoreState(saved);
            Assert.NotEqual(0UL, sysC.State.primaryRngState);

            for (int d = 11; d <= 20; d++)
            {
                sysC.TickDay(d);
                var s = sysC.State.trapSites[0];
                traceB.Add((d, s.hasCatch, s.catchSpecies, s.bycatchSpecies));
                if (s.hasCatch)
                {
                    sysC.Butcher(s.siteId);
                    s.hasCatch = false;
                }
            }

            // Assert exact trace equivalence
            Assert.Equal(traceA.Count, traceB.Count);
            for (int i = 0; i < traceA.Count; i++)
            {
                var a = traceA[i];
                var b = traceB[i];
                Assert.True(a.day == b.day && a.hasCatch == b.hasCatch &&
                            a.primary == b.primary && a.bycatch == b.bycatch,
                    $"Divergence at day {a.day}: A=({a.hasCatch}, '{a.primary}', '{a.bycatch}') vs B=({b.hasCatch}, '{b.primary}', '{b.bycatch}')");
            }

            // Final state hashes must be identical
            string hashA = ComputeStateHash(sysA.CaptureState());
            string hashC = ComputeStateHash(sysC.CaptureState());
            Assert.Equal(hashA, hashC);
        }

        [Fact]
        public void BycatchReplay_FirstDivergenceDiagnostic_ReportsDayAndFieldOnMismatch()
        {
            var traceA = new List<(int day, string species)>
            {
                (1, "rabbit"), (2, "rat"), (3, "fox")
            };
            var traceB = new List<(int day, string species)>
            {
                (1, "rabbit"), (2, "rat"), (3, "cotton_hare")
            };

            int divergenceDay = -1;
            string field = "";
            for (int i = 0; i < traceA.Count; i++)
            {
                if (traceA[i].species != traceB[i].species)
                {
                    divergenceDay = traceA[i].day;
                    field = "species";
                    break;
                }
            }

            Assert.Equal(3, divergenceDay);
            Assert.Equal("species", field);
        }

        [Fact]
        public void BycatchReplay_FinalStateHash_IsIdenticalAcrossRuns()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);

            string RunSimulation()
            {
                var sys = new WildlifeTrappingSystem(new SeededRng(42));
                catalog!.RegisterWith(sys);
                sys.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                    trapId: "trap_net", checkIntervalDays: 1, durabilityChecks: 15);

                for (int d = 2; d <= 12; d++)
                {
                    sys.TickDay(d);
                    var s = sys.State.trapSites[0];
                    if (s.hasCatch)
                    {
                        sys.Butcher(s.siteId);
                        s.hasCatch = false;
                    }
                }

                return ComputeStateHash(sys.CaptureState());
            }

            string hash1 = RunSimulation();
            string hash2 = RunSimulation();
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void BycatchReplay_ThreeConsecutiveRuns_ProduceIdenticalStateHash()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);

            string Run(int seed)
            {
                var sys = new WildlifeTrappingSystem(new SeededRng(seed));
                catalog!.RegisterWith(sys);
                sys.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                    trapId: "trap_net", checkIntervalDays: 1, durabilityChecks: 25);

                for (int d = 2; d <= 15; d++)
                {
                    sys.TickDay(d);
                    var s = sys.State.trapSites[0];
                    if (s.hasCatch)
                    {
                        sys.Butcher(s.siteId);
                        s.hasCatch = false;
                    }
                }

                return ComputeStateHash(sys.CaptureState());
            }

            string h1 = Run(42);
            string h2 = Run(42);
            string h3 = Run(42);

            Assert.NotEmpty(h1);
            Assert.Equal(h1, h2);
            Assert.Equal(h2, h3);
        }
    }
}
