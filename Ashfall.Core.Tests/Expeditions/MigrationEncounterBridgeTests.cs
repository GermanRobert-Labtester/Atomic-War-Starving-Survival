// SPDX-License-Identifier: MIT
// ============================================================================
// CORE-MECH W6 — Migration → travel-encounter bridge (focused suite; alone first).
//
// Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
//       wave CORE-MECH-W6-MIGRATION-ENCOUNTER-BRIDGE (cases AV.6).
//
// The bridge joins the weighting seam that already carries stance, faction-war,
// and patrol-recognition multipliers, so the invariants that matter are: zero
// pressure is exact identity, pressure can lift but never erase an encounter,
// the floor distribution is preserved, and everything stays deterministic.
// ============================================================================

using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class MigrationEncounterBridgeTests
    {
        // A real authored region tag (travel encounters are region-tagged).
        private const string Region = "high_scarp";

        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static TravelEncounterSystem FreshTravel()
        {
            var catalog = TravelEncounterCatalog.LoadFromDirectory(DataDir(), new FileSystemIO());
            return new TravelEncounterSystem(catalog);
        }

        private static TravelEncounterDefinition AnyEncounter()
        {
            var catalog = TravelEncounterCatalog.LoadFromDirectory(DataDir(), new FileSystemIO());
            return catalog.Encounters.First();
        }

        // ── Identity and bounds (AV.6 cases 1–3) ────────────────────────────

        [Fact]
        public void UnboundProvider_IsExactIdentity()
        {
            var travel = FreshTravel();
            var encounter = AnyEncounter();
            Assert.Equal(
                travel.GetEffectiveWeight(encounter, "neutral"),
                travel.GetEffectiveWeight(encounter, "neutral", Region), 5);
        }

        [Fact]
        public void ZeroPressure_IsExactIdentity()
        {
            var travel = FreshTravel();
            travel.RegionEncounterPressureProvider = _ => 0f;
            var encounter = AnyEncounter();
            Assert.Equal(
                travel.GetEffectiveWeight(encounter, "neutral"),
                travel.GetEffectiveWeight(encounter, "neutral", Region), 5);
        }

        [Fact]
        public void Pressure_LiftsWeightWithinTheAuthoredBound()
        {
            var travel = FreshTravel();
            var encounter = AnyEncounter();
            float baseWeight = travel.GetEffectiveWeight(encounter, "neutral");

            travel.RegionEncounterPressureProvider = _ => 1f;
            float pressured = travel.GetEffectiveWeight(encounter, "neutral", Region);

            // Susceptibility-weighted: the lift equals bias × susceptibility.
            float expected = baseWeight
                * (1f + TravelEncounterSystem.MigrationEncounterBiasK
                     * TravelEncounterSystem.MigrationSusceptibility(encounter));
            Assert.Equal(expected, pressured, 3);
            Assert.True(pressured >= baseWeight, "pressure may lift, never lower");
        }

        [Fact]
        public void InvalidPressure_NeutralizesToIdentity()
        {
            var travel = FreshTravel();
            var encounter = AnyEncounter();
            float baseWeight = travel.GetEffectiveWeight(encounter, "neutral");

            travel.RegionEncounterPressureProvider = _ => float.NaN;
            Assert.Equal(baseWeight, travel.GetEffectiveWeight(encounter, "neutral", Region), 5);

            travel.RegionEncounterPressureProvider = _ => -3f;
            Assert.Equal(baseWeight, travel.GetEffectiveWeight(encounter, "neutral", Region), 5);
        }

        [Fact]
        public void OutOfRangePressure_IsClamped()
        {
            var travel = FreshTravel();
            var encounter = AnyEncounter();
            travel.RegionEncounterPressureProvider = _ => 99f;
            float clamped = travel.GetEffectiveWeight(encounter, "neutral", Region);
            travel.RegionEncounterPressureProvider = _ => 1f;
            Assert.Equal(clamped, travel.GetEffectiveWeight(encounter, "neutral", Region), 4);
        }

        [Fact]
        public void NoEncounterIsEverErased()
        {
            // The authored floor must survive any pressure: a migration corridor
            // makes an encounter likelier, never impossible.
            var travel = FreshTravel();
            travel.RegionEncounterPressureProvider = _ => 1f;
            var all = TravelEncounterCatalog.LoadFromDirectory(DataDir(), new FileSystemIO()).Encounters;
            foreach (var enc in all.Take(25))
            {
                Assert.True(travel.GetEffectiveWeight(enc, "neutral", Region) > 0f);
            }
        }

        [Fact]
        public void EmptyRegion_SkipsTheProvider()
        {
            var travel = FreshTravel();
            var encounter = AnyEncounter();
            bool called = false;
            travel.RegionEncounterPressureProvider = _ => { called = true; return 1f; };
            travel.GetEffectiveWeight(encounter, "neutral", "");
            Assert.False(called);
        }

        // ── Distribution and determinism (AV.6 cases 4–6) ───────────────────

        [Fact]
        public void SelectionDistribution_ShiftsUnderPressure()
        {
            // Sampling the same seed many times, a pressured region must produce a
            // different mix than an empty one — that is the mechanic.
            var quiet = FreshTravel();
            var busy = FreshTravel();
            busy.RegionEncounterPressureProvider = _ => 1f;

            var quietCounts = SampleCounts(quiet);
            var busyCounts = SampleCounts(busy);

            Assert.NotEqual(quietCounts, busyCounts);
        }

        [Fact]
        public void Selection_IsDeterministic_TwoPass()
        {
            var a = FreshTravel();
            var b = FreshTravel();
            a.RegionEncounterPressureProvider = _ => 0.6f;
            b.RegionEncounterPressureProvider = _ => 0.6f;

            for (int day = 1; day <= 10; day++)
            {
                var ra = a.SelectEncounter(Region, dangerLevel: 2, stance: "neutral",
                    currentSeason: "ash_winds", currentDay: day, new Ashfall.Core.SeededRng(1234));
                var rb = b.SelectEncounter(Region, dangerLevel: 2, stance: "neutral",
                    currentSeason: "ash_winds", currentDay: day, new Ashfall.Core.SeededRng(1234));
                Assert.Equal(ra?.Id, rb?.Id);
            }
        }

        [Fact]
        public void PressureDoesNotChangeEligibility()
        {
            // Bias is a weight, never a gate: the eligible set is identical with
            // and without migration pressure.
            var travel = FreshTravel();
            int before = travel.GetEligiblePatrolCandidates(Region, 2, "neutral", "ash_winds", 5).Count;
            travel.RegionEncounterPressureProvider = _ => 1f;
            int after = travel.GetEligiblePatrolCandidates(Region, 2, "neutral", "ash_winds", 5).Count;
            Assert.Equal(before, after);
        }

        private static string SampleCounts(TravelEncounterSystem travel)
        {
            var counts = new System.Collections.Generic.SortedDictionary<string, int>(StringComparer.Ordinal);
            for (int day = 1; day <= 60; day++)
            {
                var picked = travel.SelectEncounter(Region, 2, "neutral", "ash_winds", day,
                    new Ashfall.Core.SeededRng(9000 + day));
                if (picked == null) continue;
                counts.TryGetValue(picked.Id, out int c);
                counts[picked.Id] = c + 1;
            }
            return string.Join(",", counts.Select(kv => kv.Key + "=" + kv.Value));
        }
    }
}
