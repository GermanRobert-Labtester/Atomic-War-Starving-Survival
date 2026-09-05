using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Narrative;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// F10 — micro-location selection determinism. Named seed scenarios must
    /// repeat exactly; save/load continuation must equal uninterrupted
    /// execution (INV-09); eligibility metadata must consume zero RNG;
    /// selection consumes exactly the documented draws; the stream is
    /// authoritative (INV-06); depletion filtering and the 100-seed sweep are
    /// deterministic. Traces come from MicroLocationDeterminismHarness, which
    /// mirrors production wiring and never touches the simulation RNG.
    /// </summary>
    public class MicroLocationDeterminismTests
    {
        private const string Allotments = "loc_the_allotments";
        private const string GasStation = "rural_gas_station";
        private const string DenialCut = "loc_denial_cut_substation";

        // ── F10.5–F10.7 named seed scenarios (F10.14 repetition included) ──

        [Fact]
        public void Seed42_Allotments_EightTicks_RepeatsExactly()
        {
            var a = MicroLocationDeterminismHarness.Run(42, Allotments, 8);
            var b = MicroLocationDeterminismHarness.Run(42, Allotments, 8);
            var c = MicroLocationDeterminismHarness.Run(42, Allotments, 8);
            MicroLocationDeterminismHarness.AssertTracesEqual(a, b, "seed42 runB");
            MicroLocationDeterminismHarness.AssertTracesEqual(a, c, "seed42 runC");
        }

        [Fact]
        public void Seed99_GasStation_EightTicks_RepeatsExactly()
        {
            var a = MicroLocationDeterminismHarness.Run(99, GasStation, 8);
            var b = MicroLocationDeterminismHarness.Run(99, GasStation, 8);
            MicroLocationDeterminismHarness.AssertTracesEqual(a, b, "seed99 runB");
        }

        [Fact]
        public void Seed7_DenialCut_EightTicks_RepeatsExactly()
        {
            var a = MicroLocationDeterminismHarness.Run(7, DenialCut, 8);
            var b = MicroLocationDeterminismHarness.Run(7, DenialCut, 8);
            MicroLocationDeterminismHarness.AssertTracesEqual(a, b, "seed7 runB");
        }

        // ── F10.10 — save at tick 4, continue 5..8 (INV-09) ──

        [Fact]
        public void SaveAtTick4_Continuation_EqualsUninterruptedEightTicks()
        {
            const int seed = 42;
            var uninterrupted = MicroLocationDeterminismHarness.Run(seed, Allotments, 8);

            // Interrupted: 1..4, capture everything serialized, rebuild the
            // world (fresh fixture = fresh process-like state), restore, 5..8.
            var first = MicroLocationDeterminismHarness.CreateFixture(seed);
            first.StartExpedition(Allotments);
            first.Tick(4);

            var engineSave = first.Engine.CaptureState();
            var narrativeSave = first.Narrative.CaptureState();
            int drawsAtCheckpoint = first.Rng.Draws;

            var second = MicroLocationDeterminismHarness.CreateFixture(seed);
            second.Engine.RestoreState(engineSave);
            second.Narrative.RestoreState(narrativeSave);
            second.Rng.ReplayDraws(drawsAtCheckpoint);
            second.TicksRun = first.TicksRun;
            second.Trace.AddRange(first.Trace);
            second.Tick(4);

            var resumed = second.Snapshot(Allotments);
            MicroLocationDeterminismHarness.AssertTracesEqual(uninterrupted, resumed, "save@4 continuation");
        }

        // ── F10.8 — RNG perturbation: metadata is free, selection is priced ──

        [Fact]
        public void EligibilityMetadata_ConsumesZeroRngDraws()
        {
            var sys = new NarrativeEncounterSystem();
            sys.RegisterRange(NarrativeEncounterCatalogLoader.Load(
                MicroLocationDeterminismHarness.DataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            var counting = new CountingRng(new SeededRng(5));
            var cats = new List<string> { "fuel", "scrap_metal", "mechanical_parts", "canned_food" };

            var truck = sys.Find("micro_crashed_truck");
            Assert.NotNull(truck);
            Assert.False(sys.IsDepleted(truck!.id));
            truck.GetEffectiveWeight("Stealth", 2f, GasStation, cats);
            truck.GetEffectiveWeight("Speed", 0f, GasStation, null);
            sys.Find("micro_supply_drop");
            sys.SelectEncounter("Stealth", 0f, GasStation, counting, cats); // eligible → documented roll

            Assert.Equal(1, counting.Draws);
        }

        [Fact]
        public void SelectEncounter_ZeroEligibleContext_ConsumesZeroDraws()
        {
            var sys = new NarrativeEncounterSystem();
            sys.RegisterRange(NarrativeEncounterCatalogLoader.Load(
                MicroLocationDeterminismHarness.DataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));

            // Every encounter weather-blocked: pass 1 finds no weight, returns
            // null before the weighted roll — zero draws.
            sys.WeatherGateFilter = _ => true;
            var counting = new CountingRng(new SeededRng(5));
            Assert.Null(sys.SelectEncounter("Normal", 3f, GasStation, counting));
            Assert.Equal(0, counting.Draws);
        }

        // ── INV-06 — authoritative stream only ──

        [Fact]
        public void SelectionPath_IntroducesNoIndependentRng()
        {
            // Static gate over the three selection-path sources: no inline RNG
            // construction, no wall-clock or GUID entropy.
            string core = Path.Combine(RepoRoot(), "Assets", "Ashfall.Core");
            foreach (var file in new[]
            {
                Path.Combine(core, "Narrative", "NarrativeEncounterSystem.cs"),
                Path.Combine(core, "Expeditions", "ExpeditionEncounterBridge.cs"),
                Path.Combine(core, "Expeditions", "ExpeditionSystem.cs")
            })
            {
                string src = File.ReadAllText(file);
                Assert.DoesNotContain("new SeededRng(", src);
                Assert.DoesNotContain("new Random(", src);
                Assert.DoesNotContain("Guid.NewGuid", src);
                Assert.DoesNotContain("DateTime.Now", src);
            }
        }

        [Fact]
        public void SameStreamState_ReplaysIdenticalSelection()
        {
            var a = MicroLocationDeterminismHarness.CreateFixture(42);
            var b = MicroLocationDeterminismHarness.CreateFixture(42);
            a.StartExpedition(Allotments);
            b.StartExpedition(Allotments);
            // Two worlds on the same seed/stream replay identical surfaced
            // sequences tick for tick — the bridge holds no private entropy.
            for (int i = 0; i < 8; i++)
            {
                a.Tick(1);
                b.Tick(1);
                Assert.Equal(a.LastTickSurfaced, b.LastTickSurfaced);
            }
        }

        // ── F10.11 — depletion filtering is deterministic ──

        [Fact]
        public void DepletedCandidate_Filtering_IsDeterministic()
        {
            var counts = new List<int>();
            string? canonical = null;

            for (int run = 0; run < 2; run++)
            {
                var f = MicroLocationDeterminismHarness.CreateFixture(42);
                f.StartExpedition(Allotments);
                // Deplete a real production entry before any travel.
                f.Narrative.TryResolve("micro_roadside_memorial", "take_offering", Allotments, 1);
                Assert.True(f.Narrative.IsDepleted("micro_roadside_memorial"));
                f.Tick(8);
                var snap = f.Snapshot(Allotments);
                counts.Add(CountDrawsForContext(f));
                string c = snap.Canonical();
                if (canonical == null) canonical = c;
                else Assert.Equal(canonical, c);
            }

            // Identical depleted state ⇒ identical draws for the same context.
            Assert.Equal(counts[0], counts[1]);
        }

        private static int CountDrawsForContext(MicroLocationDeterminismHarness.Fixture f)
        {
            var counting = new CountingRng(new SeededRng(31));
            var cats = ExpeditionDefinitionRegistry.Get(Allotments)?.lootCategories;
            for (int i = 0; i < 20; i++)
                f.Narrative.SelectEncounter("Stealth", 2f, Allotments, counting, cats);
            return counting.Draws;
        }

        // ── F10.13 — 100-seed sweep ──

        [Fact]
        public void HundredSeedHarness_HasZeroDivergences()
        {
            int divergences = 0;
            var divergentSeeds = new List<int>();
            for (int seed = 0; seed < 100; seed++)
            {
                var a = MicroLocationDeterminismHarness.Run(seed, Allotments, 6);
                var b = MicroLocationDeterminismHarness.Run(seed, Allotments, 6);
                if (a.Canonical() != b.Canonical())
                {
                    divergences++;
                    divergentSeeds.Add(seed);
                }
            }
            Assert.True(divergences == 0,
                $"MICRO-LOCATION DETERMINISM SWEEP: {divergences}/100 divergent seeds: {string.Join(",", divergentSeeds)}");
        }

        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return dir!.FullName;
        }
    }
}
