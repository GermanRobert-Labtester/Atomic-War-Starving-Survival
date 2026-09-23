// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 40 — The Wheel
// Subsystem    : Mechanical Power Distribution, Driveline Friction & Machine Maintenance Engine
// Authority    : docs/expansions/wave6/expansion_40_the_wheel_plan.md
//                WAVE6_INDEX.md
// ============================================================================
using System;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Classification of prime movers supplying rotational mechanical power.
    /// </summary>
    public enum PrimeMoverType
    {
        ManualTreadle   = 0, // Human or beast pedal/treadle; low power (100–300W equivalent)
        WaterWheel      = 1, // Flume/stream driven; consistent base-load (1000–5000W equivalent)
        Windmill        = 2, // Wind turbine/mill; intermittent high-torque (500–6000W equivalent)
        FlywheelCoupled = 3  // Kinetic energy tapped from KineticStorageSystem (surge power)
    }

    /// <summary>
    /// Type of mechanical coupling between line shaft and machine tool.
    /// </summary>
    public enum DrivelineCouplingKind
    {
        DirectShaft     = 0, // Solid steel line shaft with rigid sleeve couplings
        FlatLeatherBelt = 1, // Traditional leather/canvas belt over crowned pulleys (slips under surge)
        SpurGearTrain   = 2, // Intermeshed cast iron/bronze gears (rigid, positive drive)
        RollerChain     = 3  // Heavy roller chain drive over sprockets (high torque, lubrication sensitive)
    }

    /// <summary>
    /// Mutable state of a mechanical power transmission line shaft branch.
    /// Extends KineticStorageSystem, ShelterWorkshopSystem, and PrecisionMetrologySystem without duplicating power state.
    /// </summary>
    public sealed class DrivelineBranchState
    {
        public string BranchId                     { get; set; } = string.Empty;
        public DrivelineCouplingKind CouplingKind  { get; set; } = DrivelineCouplingKind.FlatLeatherBelt;
        public int ShaftLengthMetres               { get; set; } = 10;
        public int LubricationQualityPermille      { get; set; } = 800;
        public int BearingWearPermille             { get; set; } = 150;
        public int AlignmentPrecisionPermille      { get; set; } = 850;
        public int MachineToolRunoutMicrons        { get; set; } = 25;
        public int OperatingHoursAccumulated       { get; set; } = 0;

        public DrivelineBranchState Clone() => new DrivelineBranchState
        {
            BranchId                  = BranchId,
            CouplingKind              = CouplingKind,
            ShaftLengthMetres         = ShaftLengthMetres,
            LubricationQualityPermille = LubricationQualityPermille,
            BearingWearPermille       = BearingWearPermille,
            AlignmentPrecisionPermille = AlignmentPrecisionPermille,
            MachineToolRunoutMicrons  = MachineToolRunoutMicrons,
            OperatingHoursAccumulated = OperatingHoursAccumulated
        };
    }

    /// <summary>
    /// Immutable result of a mechanical power transmission calculation.
    /// </summary>
    public readonly struct PowerTransmissionResult
    {
        public int InputTorqueWatts                { get; }
        public int DeliveredTorqueWatts            { get; }
        public int TransmissionEfficiencyPermille  { get; }
        public int FrictionLossWatts               { get; }
        public int LubricantDegradationPermille    { get; }
        public int BearingWearIncurredPermille     { get; }

        public PowerTransmissionResult(
            int inputTorqueWatts,
            int deliveredTorqueWatts,
            int transmissionEfficiencyPermille,
            int frictionLossWatts,
            int lubricantDegradationPermille,
            int bearingWearIncurredPermille)
        {
            InputTorqueWatts               = Math.Max(0, inputTorqueWatts);
            DeliveredTorqueWatts           = Math.Max(0, deliveredTorqueWatts);
            TransmissionEfficiencyPermille = Math.Clamp(transmissionEfficiencyPermille, 0, 1000);
            FrictionLossWatts              = Math.Max(0, frictionLossWatts);
            LubricantDegradationPermille   = Math.Clamp(lubricantDegradationPermille, 0, 1000);
            BearingWearIncurredPermille    = Math.Clamp(bearingWearIncurredPermille, 0, 1000);
        }
    }

    /// <summary>
    /// Immutable result of machining tolerance gate verification.
    /// </summary>
    public readonly struct MachiningToleranceResult
    {
        public bool MeetsTolerance                 { get; }
        public int RunoutDriftMicrons              { get; }
        public int EffectiveToolPrecisionPermille  { get; }
        public string BottleneckReason             { get; }

        public MachiningToleranceResult(
            bool meetsTolerance,
            int runoutDriftMicrons,
            int effectiveToolPrecisionPermille,
            string bottleneckReason)
        {
            MeetsTolerance                = meetsTolerance;
            RunoutDriftMicrons            = Math.Max(0, runoutDriftMicrons);
            EffectiveToolPrecisionPermille = Math.Clamp(effectiveToolPrecisionPermille, 0, 1000);
            BottleneckReason              = bottleneckReason ?? string.Empty;
        }
    }

    /// <summary>
    /// Pure domain engine governing rotational mechanical power generation, driveline transmission losses,
    /// bearing wear, lubricant degradation, and machine tool tolerance drift.
    /// Extends KineticStorageSystem and ShelterWorkshopSystem seams.
    /// Zero engine references, integer permille determinism.
    /// </summary>
    public static class MechanicalPowerDrivelineEngine
    {
        public const int MaximumSafeRunoutMicrons = 50;

        /// <summary>
        /// Calculates nominal mechanical power output in watts equivalent for a prime mover.
        /// </summary>
        /// <param name="moverType">Source of rotational power.</param>
        /// <param name="resourceAvailabilityPermille">Water flow, wind strength, or flywheel charge (0..1000).</param>
        /// <param name="operatorSkillPermille">Operator or millwright maintenance skill (0..1000).</param>
        public static int CalculatePrimeMoverOutput(
            PrimeMoverType moverType,
            int resourceAvailabilityPermille,
            int operatorSkillPermille)
        {
            resourceAvailabilityPermille = Math.Clamp(resourceAvailabilityPermille, 0, 1000);
            operatorSkillPermille        = Math.Clamp(operatorSkillPermille, 0, 1000);

            int baseRatingWatts = moverType switch
            {
                PrimeMoverType.ManualTreadle   => 250,
                PrimeMoverType.WaterWheel      => 3500,
                PrimeMoverType.Windmill        => 4500,
                PrimeMoverType.FlywheelCoupled => 6000,
                _                              => 500
            };

            int resourceFactor = resourceAvailabilityPermille;
            int skillFactor = 700 + (operatorSkillPermille * 300) / 1000;

            int output = (baseRatingWatts * resourceFactor) / 1000;
            output = (output * skillFactor) / 1000;

            return Math.Max(0, output);
        }

        /// <summary>
        /// Transmits mechanical power through a line shaft branch, evaluating friction losses and mechanical wear.
        /// Mutates DrivelineBranchState in place.
        /// </summary>
        public static PowerTransmissionResult TransmitMechanicalPower(
            DrivelineBranchState branch,
            int inputPowerWatts,
            int operatingHours)
        {
            if (branch == null) throw new ArgumentNullException(nameof(branch));

            inputPowerWatts = Math.Max(0, inputPowerWatts);
            operatingHours  = Math.Clamp(operatingHours, 1, 24);

            branch.OperatingHoursAccumulated += operatingHours;

            // Base transmission efficiency by coupling type
            int baseEfficiency = branch.CouplingKind switch
            {
                DrivelineCouplingKind.DirectShaft     => 940, // 94%
                DrivelineCouplingKind.RollerChain     => 910, // 91%
                DrivelineCouplingKind.SpurGearTrain   => 880, // 88%
                DrivelineCouplingKind.FlatLeatherBelt => 840, // 84% (belt slip)
                _                                     => 800
            };

            // Length friction loss: 5 permille per 5 metres
            int lengthLoss = (branch.ShaftLengthMetres * 10) / 5;

            // Alignment friction penalty
            int misalignmentPenalty = ((1000 - branch.AlignmentPrecisionPermille) * 120) / 1000;

            // Lubrication penalty: dry bearings cause immense drag
            int lubePenalty = ((1000 - branch.LubricationQualityPermille) * 200) / 1000;

            // Bearing wear drag
            int wearPenalty = (branch.BearingWearPermille * 100) / 1000;

            int netEfficiency = Math.Clamp(
                baseEfficiency - lengthLoss - misalignmentPenalty - lubePenalty - wearPenalty,
                100, 980);

            int deliveredWatts = (inputPowerWatts * netEfficiency) / 1000;
            int frictionWatts  = inputPowerWatts - deliveredWatts;

            // Lubricant breakdown per shift: ~2 permille per operating hour, doubled if misaligned
            int lubeDrainPerHour = (branch.AlignmentPrecisionPermille < 600) ? 4 : 2;
            int lubeDegraded = Math.Min(branch.LubricationQualityPermille, operatingHours * lubeDrainPerHour);
            branch.LubricationQualityPermille = Math.Max(0, branch.LubricationQualityPermille - lubeDegraded);

            // Bearing wear: increases dramatically when lubrication drops below 400
            int wearRate = (branch.LubricationQualityPermille < 400) ? 5 : 1;
            int bearingWearAdded = operatingHours * wearRate;
            branch.BearingWearPermille = Math.Clamp(branch.BearingWearPermille + bearingWearAdded, 0, 1000);

            // Runout drift accumulates with bearing wear
            branch.MachineToolRunoutMicrons = Math.Clamp(
                10 + (branch.BearingWearPermille * 50) / 1000, 5, 200);

            return new PowerTransmissionResult(
                inputPowerWatts,
                deliveredWatts,
                netEfficiency,
                frictionWatts,
                lubeDegraded,
                bearingWearAdded);
        }

        /// <summary>
        /// Evaluates whether the driveline branch precision satisfies a machining recipe's tolerance gate.
        /// </summary>
        public static MachiningToleranceResult EvaluateMachiningTolerance(
            DrivelineBranchState branch,
            int requiredToleranceMicrons)
        {
            if (branch == null) throw new ArgumentNullException(nameof(branch));
            requiredToleranceMicrons = Math.Max(1, requiredToleranceMicrons);

            int currentRunout = branch.MachineToolRunoutMicrons;
            bool meetsTolerance = currentRunout <= requiredToleranceMicrons;

            int toolPrecision = Math.Clamp(1000 - (currentRunout * 1000) / 100, 0, 1000);

            string bottleneck = string.Empty;
            if (!meetsTolerance)
            {
                if (branch.BearingWearPermille > 600)
                    bottleneck = "Severe line shaft bearing play causing spindle wobble.";
                else if (branch.AlignmentPrecisionPermille < 500)
                    bottleneck = "Shaft misalignment inducing excessive radial runout.";
                else
                    bottleneck = "Machine tool runout exceeds workpiece tolerance requirement.";
            }

            return new MachiningToleranceResult(
                meetsTolerance,
                currentRunout,
                toolPrecision,
                bottleneck);
        }

        /// <summary>
        /// Performs millwright maintenance, restoring lubrication and shaft alignment.
        /// </summary>
        public static void ApplyLubricationMaintenance(
            DrivelineBranchState branch,
            int freshLubricantVolumeMl,
            int alignmentAdjustmentPermille)
        {
            if (branch == null) throw new ArgumentNullException(nameof(branch));

            freshLubricantVolumeMl      = Math.Max(0, freshLubricantVolumeMl);
            alignmentAdjustmentPermille = Math.Clamp(alignmentAdjustmentPermille, 0, 1000);

            int lubeBoost = (freshLubricantVolumeMl * 1000) / 500; // 500ml = full reload
            branch.LubricationQualityPermille = Math.Clamp(branch.LubricationQualityPermille + lubeBoost, 0, 1000);

            branch.AlignmentPrecisionPermille = Math.Clamp(
                branch.AlignmentPrecisionPermille + alignmentAdjustmentPermille, 0, 1000);
        }
    }
}
