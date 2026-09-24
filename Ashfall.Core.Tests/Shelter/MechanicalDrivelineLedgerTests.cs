// SPDX-License-Identifier: MIT
using System;
using System.Linq;
using Xunit;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests.Shelter
{
    public class MechanicalDrivelineLedgerTests
    {
        [Fact]
        public void DefaultInitialization_ProvidesPrimaryMachineShopBranch()
        {
            var ledger = new MechanicalDrivelineLedger();
            var branch = ledger.GetBranch("primary_machine_shop_line");

            Assert.NotNull(branch);
            Assert.Equal(12, branch!.ShaftLengthMetres);
            Assert.Equal(DrivelineCouplingKind.FlatLeatherBelt, branch.CouplingKind);
            Assert.Equal(850, branch.LubricationQualityPermille);

            var census = ledger.GetCensus();
            Assert.Equal(1, census.ActiveBranchesCount);
            Assert.Equal(2500, census.LubricantReserveMl);
            Assert.Equal(0, census.HighRunoutBranchCount);
        }

        [Fact]
        public void RegisterOrUpdateBranch_AddsNewBranchOrModifiesExisting()
        {
            var ledger = new MechanicalDrivelineLedger();
            var newBranch = new DrivelineBranchState
            {
                BranchId = "heavy_lathe_line",
                CouplingKind = DrivelineCouplingKind.SpurGearTrain,
                ShaftLengthMetres = 20,
                LubricationQualityPermille = 900,
                BearingWearPermille = 50,
                AlignmentPrecisionPermille = 950,
                MachineToolRunoutMicrons = 10
            };

            ledger.RegisterOrUpdateBranch(newBranch);
            var retrieved = ledger.GetBranch("heavy_lathe_line");

            Assert.NotNull(retrieved);
            Assert.Equal(20, retrieved!.ShaftLengthMetres);
            Assert.Equal(DrivelineCouplingKind.SpurGearTrain, retrieved.CouplingKind);
            Assert.Equal(2, ledger.Branches.Count);
        }

        [Fact]
        public void ExecutePowerTransmission_DeliversPowerAndDegradesLubricant()
        {
            var ledger = new MechanicalDrivelineLedger();
            var branchBefore = ledger.GetBranch("primary_machine_shop_line");
            int lubeBefore = branchBefore!.LubricationQualityPermille;

            var result = ledger.ExecutePowerTransmission(
                "primary_machine_shop_line",
                PrimeMoverType.WaterWheel,
                resourceAvailabilityPermille: 800,
                operatorSkillPermille: 850,
                operatingHours: 8);

            Assert.True(result.InputTorqueWatts > 0);
            Assert.True(result.DeliveredTorqueWatts > 0);
            Assert.True(result.DeliveredTorqueWatts < result.InputTorqueWatts);
            Assert.True(result.FrictionLossWatts > 0);

            var branchAfter = ledger.GetBranch("primary_machine_shop_line");
            Assert.True(branchAfter!.LubricationQualityPermille < lubeBefore);
            Assert.Equal(8, branchAfter.OperatingHoursAccumulated);
        }

        [Fact]
        public void EvaluateMachiningTolerance_AccuratelyDistinguishesTightFromLooseTolerances()
        {
            var ledger = new MechanicalDrivelineLedger();

            // Default branch runout is 15 microns
            var passResult = ledger.EvaluateBranchMachiningTolerance("primary_machine_shop_line", 20);
            Assert.True(passResult.MeetsTolerance);
            Assert.Equal(15, passResult.RunoutDriftMicrons);
            Assert.Empty(passResult.BottleneckReason);

            var failResult = ledger.EvaluateBranchMachiningTolerance("primary_machine_shop_line", 10);
            Assert.False(failResult.MeetsTolerance);
            Assert.NotEmpty(failResult.BottleneckReason);
        }

        [Fact]
        public void PerformMillwrightMaintenance_ReplenishesLubricantAndDeductsFromReserve()
        {
            var ledger = new MechanicalDrivelineLedger();
            var branch = ledger.GetBranch("primary_machine_shop_line")!;
            branch.LubricationQualityPermille = 200;
            branch.AlignmentPrecisionPermille = 500;
            ledger.RegisterOrUpdateBranch(branch);

            int startReserve = ledger.GetCensus().LubricantReserveMl;
            bool success = ledger.PerformMillwrightMaintenance("primary_machine_shop_line", 300, 200);

            Assert.True(success);
            var updated = ledger.GetBranch("primary_machine_shop_line")!;
            Assert.True(updated.LubricationQualityPermille > 200);
            Assert.True(updated.AlignmentPrecisionPermille > 500);
            Assert.Equal(startReserve - 300, ledger.GetCensus().LubricantReserveMl);
        }

        [Fact]
        public void PerformMillwrightMaintenance_RejectsWhenReserveIsDepleted()
        {
            var ledger = new MechanicalDrivelineLedger();
            ledger.PerformMillwrightMaintenance("primary_machine_shop_line", 2500, 100);
            Assert.Equal(0, ledger.GetCensus().LubricantReserveMl);

            bool success = ledger.PerformMillwrightMaintenance("primary_machine_shop_line", 200, 100);
            Assert.False(success);
        }

        [Fact]
        public void AdvanceDay_TicksOperatingHoursAndFlagsAlerts()
        {
            var ledger = new MechanicalDrivelineLedger();
            var branch = ledger.GetBranch("primary_machine_shop_line")!;
            branch.BearingWearPermille = 900;
            branch.MachineToolRunoutMicrons = 65; // > MaximumSafeRunoutMicrons
            ledger.RegisterOrUpdateBranch(branch);

            ledger.AdvanceDay(hoursOperatingPerDay: 8);
            var census = ledger.GetCensus();

            Assert.True(census.HighRunoutBranchCount > 0);
            Assert.True(ledger.CaptureState().TotalOperatingHoursTicked >= 8);
        }

        [Fact]
        public void CaptureAndRestoreState_PreservesStateFidelity()
        {
            var ledger = new MechanicalDrivelineLedger();
            ledger.RestockLubricantReserve(500);
            ledger.ExecutePowerTransmission("primary_machine_shop_line", PrimeMoverType.Windmill, 600, 800, 6);

            var saved = ledger.CaptureState();
            var restoredLedger = new MechanicalDrivelineLedger(saved);

            Assert.Equal(saved.LubricantReserveMl, restoredLedger.GetCensus().LubricantReserveMl);
            Assert.Equal(saved.Branches[0].OperatingHoursAccumulated, restoredLedger.Branches[0].OperatingHoursAccumulated);
            Assert.Equal(saved.TotalOperatingHoursTicked, restoredLedger.CaptureState().TotalOperatingHoursTicked);
        }
    }
}
