// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 146 residual — Year-of-Ash ice road integration contract.
// Threshold gate, storm windows, multiplier flips, exposure accrual,
// envelope ride-along, and legacy-save tolerance.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests.Plan146
{
    public sealed class Plan146IceRoadIntegrationTests
    {
        [Fact]
        public void FreshState_IsClosed_WithNoWindowHistory()
        {
            var ice = new YearOfAshIceRoadSystem();
            Assert.False(ice.IsIceRoadOpen);
            Assert.Equal(-1, ice.State.lastOpenDay);
            Assert.Equal(-1, ice.State.lastClosedDay);
            Assert.Equal(0, ice.State.totalTradeWindowDays);
        }

        [Fact]
        public void ThresholdExactlyTwentyBelowZero_GatesTheRoad()
        {
            var ice = new YearOfAshIceRoadSystem();
            ice.TickDay(10, -20.5f);
            Assert.True(ice.IsIceRoadOpen);
            ice.TickDay(11, -19.9f);
            Assert.False(ice.IsIceRoadOpen);
            Assert.Equal(10, ice.State.lastOpenDay);
            Assert.Equal(11, ice.State.lastClosedDay);
        }

        [Fact]
        public void BlockingStorms_KeepTheRoadClosed_InDeepCold()
        {
            var storms = new List<StormWindowEntry>
            {
                new StormWindowEntry { type = "thaw_flood", day_start = 5, day_end = 15 },
                new StormWindowEntry { type = "thermal_inversion", day_start = 20, day_end = 25 },
            };
            var ice = new YearOfAshIceRoadSystem();
            ice.TickDay(10, -30f, storms);
            Assert.False(ice.IsIceRoadOpen);
            ice.TickDay(22, -30f, storms);
            Assert.False(ice.IsIceRoadOpen);
            // A non-blocking storm (ice_fog) leaves the cold threshold in charge.
            var fog = new List<StormWindowEntry> { new StormWindowEntry { type = "ice_fog", day_start = 30, day_end = 31 } };
            ice.TickDay(30, -30f, fog);
            Assert.True(ice.IsIceRoadOpen);
        }

        [Fact]
        public void Multipliers_AndExposureFlip_WithTheRoad()
        {
            var ice = new YearOfAshIceRoadSystem();
            ice.TickDay(1, -25f);
            Assert.Equal(1.4f, ice.GetTradeMultiplier(), 3);
            Assert.Equal(0.30f, ice.GetExpeditionExposureRisk(), 3);
            ice.TickDay(2, 5f);
            Assert.Equal(0.6f, ice.GetTradeMultiplier(), 3);
            Assert.Equal(0.10f, ice.GetExpeditionExposureRisk(), 3);
        }

        [Fact]
        public void ExposureScore_AccruesOnlyOnOpenDays()
        {
            var ice = new YearOfAshIceRoadSystem();
            ice.TickDay(1, -25f); // open day 1
            float afterOneOpen = ice.State.cumulativeExposureScore;
            ice.TickDay(2, 5f);   // closed
            Assert.Equal(afterOneOpen, ice.State.cumulativeExposureScore, 5);
            ice.TickDay(3, -25f); // open again
            Assert.True(ice.State.cumulativeExposureScore > afterOneOpen);
        }

        [Fact]
        public void StatusEvents_FireExactlyOnFlips()
        {
            var ice = new YearOfAshIceRoadSystem();
            int flips = 0;
            ice.OnIceRoadStatusChanged += (_, _) => flips++;
            ice.TickDay(1, -25f);  // closed → open
            ice.TickDay(2, -26f);  // stays open
            ice.TickDay(3, 2f);    // open → closed
            Assert.Equal(2, flips);
        }

        [Fact]
        public void CaptureRestore_RoundTripsTheLedger()
        {
            var ice = new YearOfAshIceRoadSystem();
            ice.TickDay(55, -25f);
            ice.TickDay(56, 4f);
            var state = ice.CaptureState();

            var fresh = new YearOfAshIceRoadSystem();
            fresh.RestoreState(state);
            Assert.Equal(ice.State.iceRoadOpen, fresh.IsIceRoadOpen);
            Assert.Equal(ice.State.lastOpenDay, fresh.State.lastOpenDay);
            Assert.Equal(ice.State.lastClosedDay, fresh.State.lastClosedDay);
            Assert.Equal(ice.State.totalTradeWindowDays, fresh.State.totalTradeWindowDays);
            Assert.Equal(ice.State.cumulativeExposureScore, fresh.State.cumulativeExposureScore, 5);
        }

        [Fact]
        public void Codec_AcceptsIceRoad_AndRestoresIt()
        {
            // The year_of_ash envelope must carry the ice-road section through
            // the codec exactly as the host session passes it.
            var timeline = new YearOfAshTimelineSystem();
            var iceRoad = new YearOfAshIceRoadSystem();
            iceRoad.TickDay(55, -25f);
            var save = YearOfAshSaveCodec.Capture(
                timeline, new DoorEncounterSystem(), new FactionWarSystem(),
                null!, null, null, null, null, null, iceRoad);
            Assert.NotNull(save.iceRoad);
            Assert.True(save.iceRoad!.iceRoadOpen);

            var freshRoad = new YearOfAshIceRoadSystem();
            YearOfAshSaveCodec.Restore(
                save, timeline, new DoorEncounterSystem(), new FactionWarSystem(),
                null, null, null, null, null, freshRoad);
            Assert.True(freshRoad.IsIceRoadOpen);
            Assert.Equal(55, freshRoad.State.lastOpenDay);
        }

        [Fact]
        public void Codec_WithoutIceRoadSection_LeavesRoadVirgin()
        {
            var timeline = new YearOfAshTimelineSystem();
            var iceRoad = new YearOfAshIceRoadSystem();
            var save = YearOfAshSaveCodec.Capture(
                timeline, new DoorEncounterSystem(), new FactionWarSystem(),
                null!, null, null, null, null, null);  // no iceRoad (v2/v3 import)
            // No system passed → the section stays at its field-initializer
            // defaults (fresh state), which restores closed (legacy import path).
            Assert.False(save.iceRoad.iceRoadOpen);
            Assert.Equal(-1, save.iceRoad.lastOpenDay);

            YearOfAshSaveCodec.Restore(
                save, timeline, new DoorEncounterSystem(), new FactionWarSystem(),
                null, null, null, null, null, iceRoad);
            Assert.False(iceRoad.IsIceRoadOpen);
            Assert.Equal(-1, iceRoad.State.lastOpenDay);
        }
    }
}
