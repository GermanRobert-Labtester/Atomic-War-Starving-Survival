// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Endgame;
using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ReckoningClockTests
    {
        [Fact]
        public void ToVerdictDay_WithZeroOffset_ReturnsSameDay()
        {
            Assert.Equal(0, ReckoningClock.ToVerdictDay(0, 0));
            Assert.Equal(160, ReckoningClock.ToVerdictDay(160, 0));
            Assert.Equal(240, ReckoningClock.ToVerdictDay(240, 0));
            Assert.Equal(360, ReckoningClock.ToVerdictDay(360, 0));

            Assert.Equal(160, ReckoningClock.ToCampaignDay(160, 0));
        }

        [Fact]
        public void ToVerdictDay_WithOffset_TranslatesBidirectionally()
        {
            int offset = 120;
            // Campaign Day 120 translates to Verdict Day 0
            Assert.Equal(0, ReckoningClock.ToVerdictDay(120, offset));

            // Campaign Day 280 translates to Verdict Day 160 (Knowing threshold)
            Assert.Equal(160, ReckoningClock.ToVerdictDay(280, offset));

            // Verdict Day 160 translates back to Campaign Day 280
            Assert.Equal(280, ReckoningClock.ToCampaignDay(160, offset));

            // Linear reversible offset: 50 - 120 = -70
            Assert.Equal(-70, ReckoningClock.ToVerdictDay(50, offset));
            Assert.Equal(50, ReckoningClock.ToCampaignDay(-70, offset));
        }

        [Fact]
        public void ReckoningClock_WithProfile_AppliesProfileOffset()
        {
            var profile = new ChapterProfileDef
            {
                profile_id = "profile_muster",
                reckoning_offset = 60
            };

            Assert.Equal(160, ReckoningClock.ToVerdictDay(220, profile));
            Assert.Equal(220, ReckoningClock.ToCampaignDay(160, profile));

            // Null profile defaults to 0 offset
            Assert.Equal(220, ReckoningClock.ToVerdictDay(220, (ChapterProfileDef?)null));
        }

        [Fact]
        public void ReckoningSystem_ConfigureTiming_AdjustsPhaseThresholds()
        {
            var reckoning = new ReckoningSystem();
            reckoning.ConfigureTiming(knowingDay: 180, culpableDay: 230, countedDay: 260);

            // Day 170: Dormant
            reckoning.Poll(170, livingCount: 10, logReadCount: 0, evidenceCount: 1);
            Assert.Equal(ReckoningPhase.Dormant, reckoning.Phase);

            // Day 180: Knowing
            reckoning.Poll(180, livingCount: 10, logReadCount: 0, evidenceCount: 1);
            Assert.Equal(ReckoningPhase.Knowing, reckoning.Phase);

            // Day 230: Culpable (with evidence)
            reckoning.Poll(230, livingCount: 10, logReadCount: 0, evidenceCount: 1);
            Assert.Equal(ReckoningPhase.Culpable, reckoning.Phase);

            // Day 260: Counted
            reckoning.Poll(260, livingCount: 10, logReadCount: 0, evidenceCount: 1);
            Assert.Equal(ReckoningPhase.Counted, reckoning.Phase);
        }

        [Fact]
        public void ReckoningSystem_ConfigureFromProfile_AppliesAllTimingParameters()
        {
            var reckoning = new ReckoningSystem();
            var profile = new ChapterProfileDef
            {
                profile_id = "profile_standing_d",
                knowing_day = 175,
                culpable_day = 225,
                counted_day = 255,
                waive_evidence_gate_after_day = 320
            };

            reckoning.ConfigureFromProfile(profile);

            Assert.Equal(175, reckoning.KnowingThreshold);
            Assert.Equal(225, reckoning.CulpableThreshold);
            Assert.Equal(255, reckoning.CountedThreshold);
            Assert.Equal(320, reckoning.WaiveEvidenceGateAfterDay);
        }

        [Fact]
        public void ReckoningSystem_WaiveEvidenceGate_OpensCensusWindowWithoutEvidence()
        {
            var reckoning = new ReckoningSystem();
            reckoning.ConfigureTiming(
                knowingDay: 160,
                culpableDay: 210,
                countedDay: 350,
                waiveEvidenceGateAfterDay: 320);

            // Advance to knowing phase at day 160
            reckoning.Poll(160, livingCount: 10, logReadCount: 0, evidenceCount: 0);
            Assert.Equal(ReckoningPhase.Knowing, reckoning.Phase);

            // Day 215, evidenceCount = 0 -> evidence gate blocks transition to Culpable
            reckoning.Poll(215, livingCount: 10, logReadCount: 0, evidenceCount: 0);
            Assert.Equal(ReckoningPhase.Knowing, reckoning.Phase);
            Assert.False(reckoning.IsCensusWindowOpen(215));

            // Day 319, evidenceCount = 0 -> still blocked (< 320)
            reckoning.Poll(319, livingCount: 10, logReadCount: 0, evidenceCount: 0);
            Assert.Equal(ReckoningPhase.Knowing, reckoning.Phase);
            Assert.False(reckoning.IsCensusWindowOpen(319));

            // Day 320, evidenceCount = 0 -> waived! Enters Culpable (and not yet Counted because 320 < 350)
            reckoning.Poll(320, livingCount: 10, logReadCount: 0, evidenceCount: 0);
            Assert.Equal(ReckoningPhase.Culpable, reckoning.Phase);
            Assert.True(reckoning.IsCensusWindowOpen(320));

            // Reset waive threshold to -1; without evidence, cannot enter culpable even at day 320
            var reckoningStrict = new ReckoningSystem();
            reckoningStrict.ConfigureTiming(
                knowingDay: 160,
                culpableDay: 210,
                countedDay: 350,
                waiveEvidenceGateAfterDay: -1);
            reckoningStrict.Poll(320, livingCount: 10, logReadCount: 0, evidenceCount: 0);
            Assert.Equal(ReckoningPhase.Knowing, reckoningStrict.Phase);
            Assert.False(reckoningStrict.IsCensusWindowOpen(320));
        }

        [Fact]
        public void ReckoningClock_OffsetWithReckoningSystem_TranslatesCampaignDaysCorrectly()
        {
            var reckoning = new ReckoningSystem();
            int offset = 100;

            // Campaign Day 250 -> Verdict Day 150 (< 160) -> Dormant
            int verdictDay1 = ReckoningClock.ToVerdictDay(250, offset);
            reckoning.Poll(verdictDay1, livingCount: 8, logReadCount: 0, evidenceCount: 0);
            Assert.Equal(ReckoningPhase.Dormant, reckoning.Phase);

            // Campaign Day 260 -> Verdict Day 160 (>= 160) -> Knowing
            int verdictDay2 = ReckoningClock.ToVerdictDay(260, offset);
            reckoning.Poll(verdictDay2, livingCount: 8, logReadCount: 0, evidenceCount: 0);
            Assert.Equal(ReckoningPhase.Knowing, reckoning.Phase);
        }
    }
}
