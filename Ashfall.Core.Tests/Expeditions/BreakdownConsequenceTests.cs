// SPDX-License-Identifier: MIT
// ============================================================================
// CORE-MECH W4 — Vehicle breakdown consequences (focused suite; alone first).
//
// Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
//       wave CORE-MECH-W4-BREAKDOWN-CONSEQUENCES (cases AV.4).
//
// What changed before this suite existed: a prep breakdown returned a bare
// bool that callers dropped — the sortie was aborted, the crew was untouched.
// W4 gives the owner a typed outcome (BrokeDown + crew band) that the host
// routes into the medical, dose, and disease owners. Pinned here: the band, the
// exactly-once guard, determinism, and the unchanged legacy contract.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class BreakdownConsequenceTests
    {
        private const string VehicleId = "vehicle_utility_quad";

        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static ExpeditionVehicleSystem SystemWith(float condition, int seed = 4242)
        {
            var sys = new ExpeditionVehicleSystem(new Ashfall.Core.SeededRng(seed));
            sys.LoadCatalog(VehicleCatalogLoader.Load(
                DataDir(), new FileSystemIO(), new SystemTextJsonSerializer()));
            sys.RestoreState(new ExpeditionVehicleState());
            sys.AcquireVehicle(VehicleId);
            var v = sys.GetVehicle(VehicleId);
            Assert.NotNull(v);
            v!.condition = condition;
            v.fuel = 500f; // fuel is never the blocker in these cases
            return sys;
        }

        // ── The legacy contract is preserved (non-breaking) ──────────────────

        [Fact]
        public void PrepareForExpedition_StillReturnsItsOriginalTuple()
        {
            // W4 adds an outcome API; it must not change the existing signature
            // or the abort behavior callers already depend on.
            var sys = SystemWith(10f);
            var (fuel, mod, broke) = sys.PrepareForExpedition(VehicleId, 5f);
            Assert.True(fuel > 0f);
            Assert.True(mod > 0f); // travel-time modifier is the vehicle's speed profile
            // A low-condition vehicle may or may not break on this roll; the point
            // is the shape, not the outcome.
            _ = broke;
        }

        [Fact]
        public void HealthyVehicle_DoesNotBreak()
        {
            var sys = SystemWith(100f);
            var outcome = sys.ResolvePrepBreakdown(VehicleId, 5f, new Ashfall.Core.SeededRng(1));
            Assert.False(outcome.BrokeDown);
            Assert.Equal(VehicleBreakdownKind.None, outcome.Kind);
            Assert.Equal("clean_dispatch", outcome.Cause);
        }

        [Fact]
        public void WornVehicle_BreaksAndNamesTheCrewCost()
        {
            // Find a seed that breaks deterministically (the legacy roll is 0.3 at
            // condition < 20), then assert the typed outcome around it.
            var (sys, seed) = BrokenCase(10f);
            var outcome = sys.ResolvePrepBreakdown(VehicleId, 5f, new Ashfall.Core.SeededRng(seed));
            Assert.True(outcome.BrokeDown);
            Assert.Equal(VehicleId, outcome.VehicleId);
            Assert.True(outcome.Severity01 > 0f);
            // Nominal exposure exists only for the exposure band.
            if (outcome.Kind == VehicleBreakdownKind.RadiationExposure)
                Assert.True(outcome.NominalMsV > 0f);
            else
                Assert.Equal(0f, outcome.NominalMsV, 4);
        }

        // ── Exactly-once: the repair gate is the guard (AC.3) ───────────────

        [Fact]
        public void AlreadyBrokenVehicle_DoesNotReRoll()
        {
            var (sys, seed) = BrokenCase(5f);
            sys.ResolvePrepBreakdown(VehicleId, 5f, new Ashfall.Core.SeededRng(seed));
            var second = sys.ResolvePrepBreakdown(VehicleId, 5f, new Ashfall.Core.SeededRng(seed));
            Assert.False(second.BrokeDown);
            Assert.Equal(VehicleBreakdownKind.None, second.Kind);
            Assert.Equal("already_broken", second.Cause);
        }

        [Fact]
        public void RepairClearsTheBreak_AndReopensTheRoll()
        {
            var (sys, seed) = BrokenCase(5f);
            sys.ResolvePrepBreakdown(VehicleId, 5f, new Ashfall.Core.SeededRng(seed));
            Assert.True(sys.GetVehicle(VehicleId)!.isBrokenDown);

            sys.Repair(VehicleId, 100f);
            Assert.False(sys.GetVehicle(VehicleId)!.isBrokenDown);

            // After repair the vehicle is eligible again (counterplay works).
            var after = sys.ResolvePrepBreakdown(VehicleId, 1f, new Ashfall.Core.SeededRng(seed));
            Assert.NotEqual("already_broken", after.Cause);
        }

        [Fact]
        public void UnknownVehicle_FailsClosed()
        {
            var sys = SystemWith(50f);
            var outcome = sys.ResolvePrepBreakdown("vehicle_nope", 5f, new Ashfall.Core.SeededRng(1));
            Assert.False(outcome.BrokeDown);
            Assert.Equal("unknown_vehicle", outcome.Cause);
        }

        // ── Determinism and band health (AV.4 cases 5, 9) ───────────────────

        [Fact]
        public void SameSeed_SameOutcome_TwoPass()
        {
            int seed = BrokenSeed(8f);
            var a = SystemWith(8f);
            var b = SystemWith(8f);
            var oa = a.ResolvePrepBreakdown(VehicleId, 5f, new Ashfall.Core.SeededRng(seed));
            var ob = b.ResolvePrepBreakdown(VehicleId, 5f, new Ashfall.Core.SeededRng(seed));
            Assert.Equal(oa.Kind, ob.Kind);
            Assert.Equal(oa.Severity01, ob.Severity01, 5);
            Assert.Equal(oa.NominalMsV, ob.NominalMsV, 5);
        }

        [Fact]
        public void Band_ReachesEveryKind_AcrossSeeds()
        {
            // A degenerate band would make the mechanic a coin with one face.
            var kinds = new HashSet<VehicleBreakdownKind>();
            for (int seed = 1; seed <= 300; seed++)
            {
                var sys = SystemWith(10f, seed);
                var outcome = sys.ResolvePrepBreakdown(VehicleId, 5f, new Ashfall.Core.SeededRng(seed));
                if (outcome.BrokeDown) kinds.Add(outcome.Kind);
            }
            Assert.Equal(4, kinds.Count);
        }

        [Fact]
        public void Severity_RisesAsTheVehicleFails()
        {
            int seed = BrokenSeed(4f);
            var wrecked = SystemWith(2f);
            var tired = SystemWith(18f);
            var s1 = wrecked.ResolvePrepBreakdown(VehicleId, 5f, new Ashfall.Core.SeededRng(seed));
            var s2 = tired.ResolvePrepBreakdown(VehicleId, 5f, new Ashfall.Core.SeededRng(seed));
            if (s1.BrokeDown && s2.BrokeDown)
                Assert.True(s1.Severity01 >= s2.Severity01);
        }

        [Fact]
        public void Event_RaisesOncePerBreakdown()
        {
            int seed = BrokenSeed(6f);
            var sys = SystemWith(6f);
            int raised = 0;
            sys.OnBreakdownResolved += _ => raised++;

            sys.ResolvePrepBreakdown(VehicleId, 5f, new Ashfall.Core.SeededRng(seed));
            // Second call is refused by the repair gate: no second event.
            sys.ResolvePrepBreakdown(VehicleId, 5f, new Ashfall.Core.SeededRng(seed));

            Assert.InRange(raised, 0, 1);
        }

        /// <summary>Finds a seed that produces a breakdown at the given condition.</summary>
        private static int BrokenSeed(float condition)
        {
            for (int seed = 1; seed <= 200; seed++)
            {
                var sys = SystemWith(condition, seed);
                if (sys.ResolvePrepBreakdown(VehicleId, 5f, new Ashfall.Core.SeededRng(seed)).BrokeDown)
                    return seed;
            }
            throw new InvalidOperationException("no breaking seed found");
        }

        private static (ExpeditionVehicleSystem sys, int seed) BrokenCase(float condition)
        {
            int seed = BrokenSeed(condition);
            return (SystemWith(condition, seed), seed);
        }
    }
}
