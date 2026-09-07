// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// B5–B8 Phase 1 shared-contract tests (flagship brief §6, Phase 1):
    ///   1. <see cref="ResearchSystem.HasCapability"/> — capability query parity.
    ///   2. Water request seam — preview purity, commit atomicity, exact-once consumption.
    ///   3. Power tick-summary subscription — event fires once per TickDay with
    ///      a payload identical to the returned summary (subscription contract
    ///      for vinyl/audio/schedule and future greenhouse/sentry consumers).
    ///
    /// No gameplay balance is changed by these contracts; nothing in production
    /// calls them yet. They exist so Phases 3–7 consume one stable shape.
    /// </summary>
    public sealed class Phase1SharedContractsTests
    {
        // ---- 1. capability query --------------------------------------------

        [Fact]
        public void HasCapability_MirrorsManualUnlock_Exactly()
        {
            var research = new ResearchSystem();
            const string id = "knowledge_greenhouse_microclimate";

            Assert.False(research.HasCapability(id));
            Assert.Equal(research.IsManualUnlocked(id), research.HasCapability(id));

            research.UnlockManual(id);
            Assert.True(research.HasCapability(id));
            Assert.Equal(research.IsManualUnlocked(id), research.HasCapability(id));
        }

        [Fact]
        public void HasCapability_NullOrEmpty_IsFalse()
        {
            var research = new ResearchSystem();
            Assert.False(research.HasCapability(null!));
            Assert.False(research.HasCapability(string.Empty));
        }

        [Fact]
        public void HasCapability_UnknownId_IsFalse()
        {
            var research = new ResearchSystem();
            Assert.False(research.HasCapability("knowledge_definitely_not_authored"));
        }

        // ---- 2. water request seam -------------------------------------------

        private static WaterTreatmentSystem TreatmentWithClean(float units)
        {
            var system = new WaterTreatmentSystem();
            system.AddWater(WaterType.Clean, units);
            return system;
        }

        [Fact]
        public void WaterPreview_SufficientWater_CanCommit_AndMutatesNothing()
        {
            var system = TreatmentWithClean(50f);
            float before = system.CleanWater;

            var preview = system.PreviewWaterRequest(new WaterRequest("greenhouse", WaterType.Clean, 30f, "irrigation"));

            Assert.True(preview.CanCommit);
            Assert.Equal(string.Empty, preview.ReasonCode);
            Assert.Equal(50f, preview.AvailableAmount);
            Assert.Equal(30f, preview.RequestedAmount);
            Assert.Equal(before, system.CleanWater); // preview purity
        }

        [Fact]
        public void WaterPreview_InsufficientWater_ReportsReason_AndMutatesNothing()
        {
            var system = TreatmentWithClean(10f);
            float before = system.CleanWater;

            var preview = system.PreviewWaterRequest(new WaterRequest("kitchen", WaterType.Clean, 30f, "cooking"));

            Assert.False(preview.CanCommit);
            Assert.Equal("insufficient_water", preview.ReasonCode);
            Assert.Equal(10f, preview.AvailableAmount);
            Assert.Equal(before, system.CleanWater);
        }

        [Fact]
        public void WaterPreview_InvalidAmount_IsBlocked()
        {
            var system = TreatmentWithClean(50f);
            var preview = system.PreviewWaterRequest(new WaterRequest("decon", WaterType.Clean, 0f, "decon"));
            Assert.False(preview.CanCommit);
            Assert.Equal("invalid_amount", preview.ReasonCode);
        }

        [Fact]
        public void WaterCommit_ConsumesExactAmount_ExactlyOnce()
        {
            var system = TreatmentWithClean(50f);

            var result = system.CommitWaterRequest(new WaterRequest("greenhouse", WaterType.Clean, 30f, "irrigation"));

            Assert.True(result.IsSuccess);
            Assert.Equal(20f, system.CleanWater);
        }

        [Fact]
        public void WaterCommit_InsufficientWater_MutatesNothing()
        {
            var system = TreatmentWithClean(10f);

            var result = system.CommitWaterRequest(new WaterRequest("kitchen", WaterType.Clean, 30f, "cooking"));

            Assert.False(result.IsSuccess);
            Assert.Equal("insufficient_water", result.FailureCode);
            Assert.Equal(10f, system.CleanWater); // atomic: all-or-nothing
        }

        [Fact]
        public void WaterCommit_PreviewThenCommit_IsConsistent()
        {
            var system = TreatmentWithClean(50f);
            var request = new WaterRequest("decon", WaterType.Clean, 50f, "decon");

            var preview = system.PreviewWaterRequest(request);
            Assert.True(preview.CanCommit);

            var result = system.CommitWaterRequest(request);
            Assert.True(result.IsSuccess);
            Assert.Equal(0f, system.CleanWater); // exact requested amount, nothing more
        }

        [Fact]
        public void WaterCommit_QualityIsEnforced_PerPool()
        {
            var system = TreatmentWithClean(0f);
            system.AddWater(WaterType.Raw, 40f);

            // Asking for Clean must not fall through to Raw — no silent substitution.
            var result = system.CommitWaterRequest(new WaterRequest("greenhouse", WaterType.Clean, 10f, "irrigation"));
            Assert.False(result.IsSuccess);
            Assert.Equal(40f, system.GetWater(WaterType.Raw)); // untouched
            Assert.Equal(0f, system.GetWater(WaterType.Clean));
        }

        // ---- 3. power tick-summary subscription -------------------------------

        private static PowerGridSystem Grid(float generation, float batteryWh)
        {
            var state = new PowerGridState
            {
                GenerationWatts = generation,
                FuelUnits = 100f,
                BatteryReserveWh = batteryWh,
                BatteryCapacityWh = 4000f
            };
            var rooms = new[] { new PowerGridRoom("room_lighting_main", "Main Lighting", 80f) };
            return new PowerGridSystem(state, rooms, new SeededRng(12345));
        }

        [Fact]
        public void OnTickSummary_FiresExactlyOnce_PerTickDay_WithReturnedPayload()
        {
            var grid = Grid(generation: 800f, batteryWh: 1000f);
            var summaries = new List<PowerGridTickSummary>();
            grid.OnTickSummary += s => summaries.Add(s);

            var returned = grid.TickDay(5, new SeededRng(777));

            Assert.Single(summaries);
            Assert.Equal(returned.Day, summaries[0].Day);
            Assert.Equal(returned.FuelConsumed, summaries[0].FuelConsumed);
            Assert.Equal(returned.BatteryEndWh, summaries[0].BatteryEndWh);
            Assert.Equal(returned.BrownoutHours, summaries[0].BrownoutHours);
            Assert.Equal(returned.IsBrownout, summaries[0].IsBrownout);
        }

        [Fact]
        public void OnTickSummary_NoSubscribers_DoesNotThrow()
        {
            var grid = Grid(generation: 800f, batteryWh: 1000f);
            var summary = grid.TickDay(3, new SeededRng(42));
            Assert.Equal(3, summary.Day);
        }

        [Fact]
        public void OnTickSummary_Deterministic_ForSameSeedAndState()
        {
            var summaryA = Grid(800f, 1000f).TickDay(9, new SeededRng(31337));
            var summaryB = Grid(800f, 1000f).TickDay(9, new SeededRng(31337));

            Assert.Equal(summaryA.FuelConsumed, summaryB.FuelConsumed);
            Assert.Equal(summaryA.BatteryEndWh, summaryB.BatteryEndWh);
            Assert.Equal(summaryA.BrownoutHours, summaryB.BrownoutHours);
            Assert.Equal(summaryA.IsBrownout, summaryB.IsBrownout);
        }
    }
}
