// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 40 — The Wheel
// Subsystem    : Mechanical Power Distribution, Driveline Friction & Machine Maintenance Ledger
// Authority    : docs/expansions/wave6/expansion_40_the_wheel_plan.md
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Persistent serializable state of the mechanical driveline network.
    /// Extends KineticStorageSystem, ShelterWorkshopSystem, and PrecisionMetrologySystem.
    /// </summary>
    public sealed class MechanicalDrivelineState
    {
        public int SchemaVersion { get; set; } = 1;
        public List<DrivelineBranchState> Branches { get; set; } = new List<DrivelineBranchState>();
        public int LubricantReserveMl { get; set; } = 2500;
        public int TotalOperatingHoursTicked { get; set; } = 0;
        public int MaintenanceAlertCount { get; set; } = 0;
        public int AlignmentAlertCount { get; set; } = 0;

        public MechanicalDrivelineState Clone()
        {
            var clone = new MechanicalDrivelineState
            {
                SchemaVersion = SchemaVersion,
                LubricantReserveMl = LubricantReserveMl,
                TotalOperatingHoursTicked = TotalOperatingHoursTicked,
                MaintenanceAlertCount = MaintenanceAlertCount,
                AlignmentAlertCount = AlignmentAlertCount
            };
            foreach (var branch in Branches)
            {
                if (branch != null)
                {
                    clone.Branches.Add(branch.Clone());
                }
            }
            return clone;
        }
    }

    /// <summary>
    /// Read-only snapshot census of mechanical driveline operations.
    /// </summary>
    public struct MechanicalDrivelineCensus
    {
        public int ActiveBranchesCount { get; }
        public int TotalShaftMetres { get; }
        public int AverageLubricationQualityPermille { get; }
        public int AverageBearingWearPermille { get; }
        public int LubricantReserveMl { get; }
        public int HighRunoutBranchCount { get; }
        public int MaintenanceAlertCount { get; }

        public MechanicalDrivelineCensus(
            int activeBranchesCount,
            int totalShaftMetres,
            int averageLubricationQualityPermille,
            int averageBearingWearPermille,
            int lubricantReserveMl,
            int highRunoutBranchCount,
            int maintenanceAlertCount)
        {
            ActiveBranchesCount = Math.Max(0, activeBranchesCount);
            TotalShaftMetres = Math.Max(0, totalShaftMetres);
            AverageLubricationQualityPermille = Math.Clamp(averageLubricationQualityPermille, 0, 1000);
            AverageBearingWearPermille = Math.Clamp(averageBearingWearPermille, 0, 1000);
            LubricantReserveMl = Math.Max(0, lubricantReserveMl);
            HighRunoutBranchCount = Math.Max(0, highRunoutBranchCount);
            MaintenanceAlertCount = Math.Max(0, maintenanceAlertCount);
        }
    }

    /// <summary>
    /// Stateful domain ledger managing mechanical power line shafts, power transmission runs,
    /// millwright maintenance, lubricant inventory, and machining tolerance evaluations.
    /// </summary>
    public sealed class MechanicalDrivelineLedger
    {
        public const string DefaultBranchId = "primary_machine_shop_line";

        private MechanicalDrivelineState _state = new MechanicalDrivelineState();

        public MechanicalDrivelineLedger(MechanicalDrivelineState? state = null)
        {
            RestoreState(state);
        }

        private void EnsureDefaultBranch()
        {
            if (_state.Branches.Count == 0)
            {
                _state.Branches.Add(new DrivelineBranchState
                {
                    BranchId = DefaultBranchId,
                    CouplingKind = DrivelineCouplingKind.FlatLeatherBelt,
                    ShaftLengthMetres = 12,
                    LubricationQualityPermille = 850,
                    BearingWearPermille = 100,
                    AlignmentPrecisionPermille = 900,
                    MachineToolRunoutMicrons = 15,
                    OperatingHoursAccumulated = 0
                });
            }
        }

        public void RegisterOrUpdateBranch(DrivelineBranchState branch)
        {
            if (branch == null || string.IsNullOrWhiteSpace(branch.BranchId)) return;

            int idx = _state.Branches.FindIndex(b => string.Equals(b.BranchId, branch.BranchId, StringComparison.OrdinalIgnoreCase));
            if (idx >= 0)
            {
                _state.Branches[idx] = branch.Clone();
            }
            else
            {
                _state.Branches.Add(branch.Clone());
            }
        }

        public DrivelineBranchState? GetBranch(string branchId)
        {
            if (string.IsNullOrWhiteSpace(branchId)) return null;
            return _state.Branches.FirstOrDefault(b => string.Equals(b.BranchId, branchId, StringComparison.OrdinalIgnoreCase));
        }

        public IReadOnlyList<DrivelineBranchState> Branches => _state.Branches;

        public PowerTransmissionResult ExecutePowerTransmission(
            string branchId,
            PrimeMoverType moverType,
            int resourceAvailabilityPermille,
            int operatorSkillPermille,
            int operatingHours)
        {
            var branch = GetBranch(branchId);
            if (branch == null)
            {
                EnsureDefaultBranch();
                branch = _state.Branches[0];
            }

            int inputWatts = MechanicalPowerDrivelineEngine.CalculatePrimeMoverOutput(
                moverType,
                resourceAvailabilityPermille,
                operatorSkillPermille);

            var result = MechanicalPowerDrivelineEngine.TransmitMechanicalPower(
                branch,
                inputWatts,
                operatingHours);

            _state.TotalOperatingHoursTicked += operatingHours;

            if (branch.MachineToolRunoutMicrons > MechanicalPowerDrivelineEngine.MaximumSafeRunoutMicrons)
            {
                _state.AlignmentAlertCount++;
            }

            if (branch.LubricationQualityPermille < 300)
            {
                _state.MaintenanceAlertCount++;
            }

            return result;
        }

        public MachiningToleranceResult EvaluateBranchMachiningTolerance(
            string branchId,
            int requiredToleranceMicrons)
        {
            var branch = GetBranch(branchId);
            if (branch == null)
            {
                EnsureDefaultBranch();
                branch = _state.Branches[0];
            }

            return MechanicalPowerDrivelineEngine.EvaluateMachiningTolerance(branch, requiredToleranceMicrons);
        }

        public bool PerformMillwrightMaintenance(
            string branchId,
            int lubricantVolumeMl,
            int alignmentAdjustmentPermille)
        {
            if (lubricantVolumeMl <= 0 || _state.LubricantReserveMl <= 0)
            {
                return false;
            }

            var branch = GetBranch(branchId);
            if (branch == null) return false;

            int appliedLube = Math.Min(lubricantVolumeMl, _state.LubricantReserveMl);
            _state.LubricantReserveMl -= appliedLube;

            MechanicalPowerDrivelineEngine.ApplyLubricationMaintenance(
                branch,
                appliedLube,
                alignmentAdjustmentPermille);

            return true;
        }

        public void RestockLubricantReserve(int volumeMl)
        {
            if (volumeMl > 0)
            {
                _state.LubricantReserveMl += volumeMl;
            }
        }

        public void AdvanceDay(int hoursOperatingPerDay = 8)
        {
            EnsureDefaultBranch();
            hoursOperatingPerDay = Math.Clamp(hoursOperatingPerDay, 0, 24);

            foreach (var branch in _state.Branches)
            {
                if (hoursOperatingPerDay > 0)
                {
                    // Regular daytime rotational friction wear
                    ExecutePowerTransmission(
                        branch.BranchId,
                        PrimeMoverType.ManualTreadle,
                        resourceAvailabilityPermille: 500,
                        operatorSkillPermille: 700,
                        operatingHours: hoursOperatingPerDay);
                }
                else
                {
                    // Idle oil settling / degradation
                    branch.LubricationQualityPermille = Math.Max(0, branch.LubricationQualityPermille - 5);
                }

                if (branch.LubricationQualityPermille < 300)
                {
                    _state.MaintenanceAlertCount++;
                }
                if (branch.MachineToolRunoutMicrons > MechanicalPowerDrivelineEngine.MaximumSafeRunoutMicrons)
                {
                    _state.AlignmentAlertCount++;
                }
            }
        }

        public MechanicalDrivelineCensus GetCensus()
        {
            EnsureDefaultBranch();

            int totalBranches = _state.Branches.Count;
            int totalShaft = 0;
            int totalLube = 0;
            int totalWear = 0;
            int highRunoutCount = 0;

            foreach (var b in _state.Branches)
            {
                totalShaft += b.ShaftLengthMetres;
                totalLube += b.LubricationQualityPermille;
                totalWear += b.BearingWearPermille;
                if (b.MachineToolRunoutMicrons > MechanicalPowerDrivelineEngine.MaximumSafeRunoutMicrons)
                {
                    highRunoutCount++;
                }
            }

            int avgLube = totalBranches > 0 ? totalLube / totalBranches : 0;
            int avgWear = totalBranches > 0 ? totalWear / totalBranches : 0;

            return new MechanicalDrivelineCensus(
                activeBranchesCount: totalBranches,
                totalShaftMetres: totalShaft,
                averageLubricationQualityPermille: avgLube,
                averageBearingWearPermille: avgWear,
                lubricantReserveMl: _state.LubricantReserveMl,
                highRunoutBranchCount: highRunoutCount,
                maintenanceAlertCount: _state.MaintenanceAlertCount);
        }

        public MechanicalDrivelineState CaptureState()
        {
            return _state.Clone();
        }

        public void RestoreState(MechanicalDrivelineState? state)
        {
            if (state == null)
            {
                _state = new MechanicalDrivelineState();
            }
            else
            {
                _state = state.Clone();
            }
            EnsureDefaultBranch();
        }
    }
}
