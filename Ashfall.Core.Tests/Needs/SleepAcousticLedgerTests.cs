// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Xunit;
using Ashfall.Core.Needs;

namespace Ashfall.Core.Tests.Needs
{
    public class SleepAcousticLedgerTests
    {
        [Fact]
        public void DefaultInitialization_ProvidesPrimaryDormitory()
        {
            var ledger = new SleepAcousticLedger();
            var quarter = ledger.GetQuarter("primary_shelter_dormitory");

            Assert.NotNull(quarter);
            Assert.Equal(28, quarter!.RoomAreaSquareMetres);
            Assert.Equal(4, quarter.AssignedOccupants);
            Assert.Equal(650, quarter.WallSoundproofingPermille);
            Assert.Equal(550, quarter.DoorSoundproofingPermille);

            var census = ledger.GetCensus();
            Assert.Equal(1, census.QuartersCount);
            Assert.Equal(4, census.TotalOccupants);
            Assert.Equal(10, census.SensoryKitsReserve);
            Assert.True(census.AverageSleepQualityPermille > 500);
        }

        [Fact]
        public void RegisterOrUpdateQuarter_AddsNewQuarterOrModifiesExisting()
        {
            var ledger = new SleepAcousticLedger();
            var newQuarter = new SleepingQuarterState
            {
                RoomId = "private_officer_bunk",
                RoomAreaSquareMetres = 12,
                AssignedOccupants = 1,
                WallSoundproofingPermille = 800,
                DoorSoundproofingPermille = 750,
                AmbientNoiseLevelDecibels = 35,
                DarknessQualityPermille = 900
            };

            ledger.RegisterOrUpdateQuarter(newQuarter);
            var retrieved = ledger.GetQuarter("private_officer_bunk");

            Assert.NotNull(retrieved);
            Assert.Equal(1, retrieved!.AssignedOccupants);
            Assert.Equal(2, ledger.Quarters.Count);
        }

        [Fact]
        public void EvaluateQuarterSleep_EvaluatesQualityAndTracksViolations()
        {
            var ledger = new SleepAcousticLedger();

            var compliantResult = ledger.EvaluateQuarterSleep(
                "primary_shelter_dormitory",
                isQuietHours: true,
                compliance: QuietHoursCompliance.Compliant,
                hoursSlept: 8);

            Assert.True(compliantResult.SleepQualityIndexPermille >= 650);
            Assert.True(compliantResult.FatigueRestorationMultiplierPermille >= 1000);
            Assert.Equal(0, ledger.GetCensus().QuietHoursViolationsCount);

            var violatedResult = ledger.EvaluateQuarterSleep(
                "primary_shelter_dormitory",
                isQuietHours: true,
                compliance: QuietHoursCompliance.ViolatedSevere,
                hoursSlept: 8);

            Assert.True(violatedResult.SleepQualityIndexPermille < compliantResult.SleepQualityIndexPermille);
            Assert.Equal(1, ledger.GetCensus().QuietHoursViolationsCount);
        }

        [Fact]
        public void UpdateRoomSoundproofing_ImprovesSleepScore()
        {
            var ledger = new SleepAcousticLedger();
            var quarter = ledger.GetQuarter("primary_shelter_dormitory")!;
            quarter.AmbientNoiseLevelDecibels = 70;
            quarter.WallSoundproofingPermille = 300;
            quarter.DoorSoundproofingPermille = 200;
            ledger.RegisterOrUpdateQuarter(quarter);

            var before = ledger.EvaluateQuarterSleep(
                "primary_shelter_dormitory",
                isQuietHours: true,
                compliance: QuietHoursCompliance.Compliant,
                hoursSlept: 8);

            ledger.UpdateRoomSoundproofing("primary_shelter_dormitory", 950, 900);

            var after = ledger.EvaluateQuarterSleep(
                "primary_shelter_dormitory",
                isQuietHours: true,
                compliance: QuietHoursCompliance.Compliant,
                hoursSlept: 8);

            Assert.True(after.NetDecibelsAtBunk < before.NetDecibelsAtBunk);
            Assert.True(after.SleepQualityIndexPermille > before.SleepQualityIndexPermille);
        }

        [Fact]
        public void InstallSensoryReliefKit_ConsumesReserveAndGrantsBonus()
        {
            var ledger = new SleepAcousticLedger();
            int startReserve = ledger.GetCensus().SensoryKitsReserve;

            bool success = ledger.InstallSensoryReliefKit("primary_shelter_dormitory");
            Assert.True(success);
            Assert.Equal(startReserve - 1, ledger.GetCensus().SensoryKitsReserve);

            var quarter = ledger.GetQuarter("primary_shelter_dormitory")!;
            Assert.True(quarter.HasSensoryReliefKit);

            // Repeated install is idempotent and does not consume extra reserve
            bool reSuccess = ledger.InstallSensoryReliefKit("primary_shelter_dormitory");
            Assert.True(reSuccess);
            Assert.Equal(startReserve - 1, ledger.GetCensus().SensoryKitsReserve);
        }

        [Fact]
        public void InstallSensoryReliefKit_FailsWhenReserveDepleted()
        {
            var ledger = new SleepAcousticLedger();
            var state = ledger.CaptureState();
            state.SensoryReliefKitsReserve = 0;
            ledger.RestoreState(state);

            bool success = ledger.InstallSensoryReliefKit("primary_shelter_dormitory");
            Assert.False(success);
        }

        [Fact]
        public void AdvanceDay_TicksNightsSleptAndEvaluatesQuarters()
        {
            var ledger = new SleepAcousticLedger();
            int nightsBefore = ledger.CaptureState().TotalNightsSlept;

            ledger.AdvanceDay(hoursSlept: 8);

            Assert.Equal(nightsBefore + 1, ledger.CaptureState().TotalNightsSlept);
        }

        [Fact]
        public void CaptureAndRestoreState_PreservesStateFidelity()
        {
            var ledger = new SleepAcousticLedger();
            ledger.RestockSensoryReliefKits(5);
            ledger.InstallSensoryReliefKit("primary_shelter_dormitory");
            ledger.SetQuietHoursSchedule(23, 5, true);
            ledger.AdvanceDay(8);

            var saved = ledger.CaptureState();
            var restored = new SleepAcousticLedger(saved);

            Assert.Equal(saved.SensoryReliefKitsReserve, restored.GetCensus().SensoryKitsReserve);
            Assert.Equal(saved.TotalNightsSlept, restored.CaptureState().TotalNightsSlept);
            Assert.Equal(saved.QuietHoursStartHour, restored.CaptureState().QuietHoursStartHour);
            Assert.Equal(saved.QuietHoursEndHour, restored.CaptureState().QuietHoursEndHour);
            Assert.True(restored.GetQuarter("primary_shelter_dormitory")!.HasSensoryReliefKit);
        }
    }
}
