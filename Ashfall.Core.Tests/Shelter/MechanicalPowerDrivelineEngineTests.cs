// SPDX-License-Identifier: MIT
// Expansion 40 — The Wheel : MechanicalPowerDrivelineEngine focused tests
using Xunit;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class MechanicalPowerDrivelineEngineTests
    {
        // ── 1. Prime mover power scales with resource availability and operator skill ──
        [Fact]
        public void CalculatePrimeMoverOutput_ScalesWithResourceAndSkill()
        {
            int lowWaterOutput = MechanicalPowerDrivelineEngine.CalculatePrimeMoverOutput(
                PrimeMoverType.WaterWheel,
                resourceAvailabilityPermille: 300,
                operatorSkillPermille: 400);

            int highWaterOutput = MechanicalPowerDrivelineEngine.CalculatePrimeMoverOutput(
                PrimeMoverType.WaterWheel,
                resourceAvailabilityPermille: 950,
                operatorSkillPermille: 900);

            Assert.True(highWaterOutput > lowWaterOutput * 2,
                $"High resource waterwheel ({highWaterOutput}W) should yield far more power than low ({lowWaterOutput}W)");
        }

        // ── 2. Transmission efficiency is reduced by bearing wear, dry lubrication, and length ──
        [Fact]
        public void TransmitMechanicalPower_DegradesUnderDryBearingsAndMisalignment()
        {
            var branch = new DrivelineBranchState
            {
                BranchId = "shaft-01",
                CouplingKind = DrivelineCouplingKind.FlatLeatherBelt,
                ShaftLengthMetres = 20,
                LubricationQualityPermille = 100, // Dry
                BearingWearPermille = 500,
                AlignmentPrecisionPermille = 400
            };

            var result = MechanicalPowerDrivelineEngine.TransmitMechanicalPower(
                branch: branch,
                inputPowerWatts: 2000,
                operatingHours: 8);

            Assert.True(result.TransmissionEfficiencyPermille < 600,
                $"Efficiency should be severely penalized under poor maintenance, got {result.TransmissionEfficiencyPermille}");
            Assert.True(result.FrictionLossWatts > 500);
            Assert.True(branch.OperatingHoursAccumulated == 8);
        }

        // ── 3. High bearing wear causes machine tool runout and fails tight tolerance gates ──
        [Fact]
        public void EvaluateMachiningTolerance_Fails_WhenRunoutExceedsRequirement()
        {
            var branch = new DrivelineBranchState
            {
                BranchId = "lathe-bench",
                BearingWearPermille = 800, // Excessive wear causing spindle wobble
                MachineToolRunoutMicrons = 65
            };

            // Requires 25 microns precision
            var result = MechanicalPowerDrivelineEngine.EvaluateMachiningTolerance(
                branch: branch,
                requiredToleranceMicrons: 25);

            Assert.False(result.MeetsTolerance);
            Assert.Contains("bearing play", result.BottleneckReason);
        }

        // ── 4. Well-maintained direct shaft meets high precision tolerance gates ──
        [Fact]
        public void EvaluateMachiningTolerance_Passes_WhenWellMaintained()
        {
            var branch = new DrivelineBranchState
            {
                BranchId = "grinder-bench",
                BearingWearPermille = 50,
                AlignmentPrecisionPermille = 950,
                MachineToolRunoutMicrons = 12
            };

            var result = MechanicalPowerDrivelineEngine.EvaluateMachiningTolerance(
                branch: branch,
                requiredToleranceMicrons: 20);

            Assert.True(result.MeetsTolerance);
            Assert.Empty(result.BottleneckReason);
            Assert.True(result.EffectiveToolPrecisionPermille >= 800);
        }

        // ── 5. Millwright lubrication maintenance restores lube and alignment ──
        [Fact]
        public void ApplyLubricationMaintenance_RestoresLubricationAndAlignment()
        {
            var branch = new DrivelineBranchState
            {
                BranchId = "mill-01",
                LubricationQualityPermille = 200,
                AlignmentPrecisionPermille = 600
            };

            MechanicalPowerDrivelineEngine.ApplyLubricationMaintenance(
                branch: branch,
                freshLubricantVolumeMl: 400,
                alignmentAdjustmentPermille: 300);

            Assert.True(branch.LubricationQualityPermille >= 800,
                "Lubrication should be substantially restored");
            Assert.True(branch.AlignmentPrecisionPermille >= 900,
                "Alignment should improve with millwright adjustment");
        }
    }
}
