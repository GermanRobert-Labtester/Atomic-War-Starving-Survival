// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : MechanicalDrivelineSaveStore
// Core State : Ashfall.Core.Shelter.MechanicalDrivelineState
// Host Caller: Main.MechanicalDriveline
// Purpose    : Expansion 40 — Mechanical Power Driveline, Line Shafts & Machine Tools
//              host session and persistence.
// ============================================================================

using System;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class MechanicalDrivelineSaveStore
    {
        public const string FileName = "mechanical_driveline_save.json";
        public const string SectionName = "mechanical_driveline";

        private static readonly SaveStore<MechanicalDrivelineState> s_store =
            SaveStoreHub.Checksummed<MechanicalDrivelineState>(FileName, nameof(MechanicalDrivelineSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(MechanicalDrivelineState state) => s_store.CaptureBare(state);
        public static MechanicalDrivelineState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(MechanicalDrivelineState state) => s_store.TrySave(state);
        public static MechanicalDrivelineState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Expansion 40 host session. Wraps the stateful <see cref="MechanicalDrivelineLedger"/>
    /// over the signed pure calculation engine <see cref="MechanicalPowerDrivelineEngine"/>.
    /// Extends KineticStorageSystem, ShelterWorkshopSystem, and PrecisionMetrologySystem.
    /// Governs rotational line shafts, couplings, power transmission, bearing wear, and machining tolerances.
    /// </summary>
    public sealed class MechanicalDrivelineHostSession : HostSessionBase
    {
        private readonly MechanicalDrivelineLedger _ledger;

        public MechanicalDrivelineLedger Ledger => _ledger;
        public MechanicalDrivelineCensus Census => _ledger.GetCensus();
        public string LastEvent { get; private set; } = string.Empty;

        public MechanicalDrivelineHostSession(MechanicalDrivelineState? state = null)
        {
            _ledger = new MechanicalDrivelineLedger(state);
        }

        public static MechanicalDrivelineHostSession Create(MechanicalDrivelineState? state = null) =>
            new MechanicalDrivelineHostSession(state);

        public void RegisterOrUpdateBranch(DrivelineBranchState branch)
        {
            _ledger.RegisterOrUpdateBranch(branch);
            LastEvent = $"Registered/updated line shaft branch '{branch.BranchId}'.";
            RaiseStateChanged();
        }

        public PowerTransmissionResult ExecutePowerTransmission(
            string branchId,
            PrimeMoverType moverType,
            int resourceAvailabilityPermille,
            int operatorSkillPermille,
            int operatingHours)
        {
            var result = _ledger.ExecutePowerTransmission(
                branchId, moverType, resourceAvailabilityPermille, operatorSkillPermille, operatingHours);

            LastEvent = $"Driveline branch '{branchId}' transmitted {result.DeliveredTorqueWatts}W (Eff: {result.TransmissionEfficiencyPermille}\u2030, Wear: +{result.BearingWearIncurredPermille}\u2030).";
            RaiseStateChanged();
            return result;
        }

        public MachiningToleranceResult EvaluateMachiningTolerance(
            string branchId,
            int requiredToleranceMicrons)
        {
            return _ledger.EvaluateBranchMachiningTolerance(branchId, requiredToleranceMicrons);
        }

        public bool PerformMillwrightMaintenance(
            string branchId,
            int lubricantVolumeMl,
            int alignmentAdjustmentPermille)
        {
            bool success = _ledger.PerformMillwrightMaintenance(
                branchId, lubricantVolumeMl, alignmentAdjustmentPermille);

            LastEvent = success
                ? $"Millwright maintenance completed on branch '{branchId}' ({lubricantVolumeMl}ml lube applied)."
                : $"Millwright maintenance failed on branch '{branchId}': Insufficient lubricant reserve!";

            RaiseStateChanged();
            return success;
        }

        public void RestockLubricantReserve(int volumeMl)
        {
            _ledger.RestockLubricantReserve(volumeMl);
            LastEvent = $"Restocked mechanical lubricant reserve (+{volumeMl}ml). Total: {_ledger.GetCensus().LubricantReserveMl}ml.";
            RaiseStateChanged();
        }

        public void AdvanceDay(int hoursOperatingPerDay = 8)
        {
            _ledger.AdvanceDay(hoursOperatingPerDay);
            LastEvent = $"Advanced driveline operations for day (+{hoursOperatingPerDay} operating hours).";
            RaiseStateChanged();
        }

        public MechanicalDrivelineState CaptureState() => _ledger.CaptureState();
        public void RestoreState(MechanicalDrivelineState? state) => _ledger.RestoreState(state);
    }
}
