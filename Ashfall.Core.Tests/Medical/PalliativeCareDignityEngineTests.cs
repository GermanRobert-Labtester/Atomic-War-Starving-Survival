// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class PalliativeCareDignityEngineTests
    {
        [Fact]
        public void AdvanceDailyCare_ReducesPainAndCalculatesDignity()
        {
            var patient = new PalliativePatientRecord
            {
                SurvivorId = "survivor_elder_marta",
                DaysRemainingPrognosis = 5,
                PainLevelPermille = 800,
                LucidityPermille = 900,
                DignityIndexPermille = 400,
                ActiveProtocol = PalliativeCareProtocol.BalancedAnalgesia
            };

            var outcome = PalliativeCareDignityEngine.AdvanceDailyCare(
                patient,
                medicineAvailabilityPermille: 1000,
                caregiverSkillPermille: 800);

            Assert.True(outcome.PainDeltaPermille < 0);
            Assert.True(patient.PainLevelPermille < 800);
            Assert.True(patient.DignityIndexPermille > 400, "Dignity should rise with pain relief and skilled care");
            Assert.Equal(4, patient.DaysRemainingPrognosis);
            Assert.False(outcome.PrognosisExpired);
        }

        [Fact]
        public void EvaluateGriefStageProgression_AdvancesTowardAcceptance_WhenDignityHigh()
        {
            var patient = new PalliativePatientRecord
            {
                SurvivorId = "survivor_scout_jonas",
                CurrentGriefStage = GriefStage.Denial,
                DaysInCurrentGriefStage = 3,
                DignityIndexPermille = 900,
                FinalWishFulfilled = true,
                PainLevelPermille = 100
            };

            // Evaluate over simulation ticks until transition occurs
            for (long tick = 1; tick <= 10; tick++)
            {
                PalliativeCareDignityEngine.EvaluateGriefStageProgression(patient, simTick: tick, worldSeed: 55555);
                if (patient.CurrentGriefStage == GriefStage.Anger) break;
            }

            Assert.Equal(GriefStage.Anger, patient.CurrentGriefStage);
            Assert.Equal(0, patient.DaysInCurrentGriefStage);
        }

        [Fact]
        public void CalculateMemorialEcho_HighDignityAndWishFulfilled_GrantsMoraleBuff()
        {
            var patient = new PalliativePatientRecord
            {
                SurvivorId = "survivor_founder_elias",
                DignityIndexPermille = 850,
                CurrentGriefStage = GriefStage.Acceptance,
                FinalWishFulfilled = true
            };

            var echo = PalliativeCareDignityEngine.CalculateMemorialEcho(patient);

            Assert.True(echo.DiedInDignity);
            Assert.Equal(15, echo.MoraleDelta);
            Assert.Equal("memorial_died_in_peace_and_dignity", echo.MemorialJournalKey);
        }

        [Fact]
        public void CalculateMemorialEcho_AgonizingNeglect_ImposesMoralePenalty()
        {
            var patient = new PalliativePatientRecord
            {
                SurvivorId = "survivor_neglected",
                DignityIndexPermille = 200,
                PainLevelPermille = 850,
                CurrentGriefStage = GriefStage.Anger
            };

            var echo = PalliativeCareDignityEngine.CalculateMemorialEcho(patient);

            Assert.False(echo.DiedInDignity);
            Assert.Equal(-15, echo.MoraleDelta);
            Assert.Equal("memorial_agonizing_neglect", echo.MemorialJournalKey);
        }
    }
}
