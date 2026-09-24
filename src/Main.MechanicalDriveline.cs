// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Expansion 40 — The Wheel: Mechanical Power Driveline, Line Shafts &
// Machine Tools host wiring.
// The signed pure MechanicalPowerDrivelineEngine is the calculation authority.
// Existing kinetic storage, shelter workshop, and metrology systems remain their
// own authorities; this host owns rotational line shaft branches, line shaft
// friction drag, bearing wear, lubricant depletion, and machine tool runout.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private MechanicalDrivelineHostSession? _mechanicalDriveline;
        private bool _mechanicalDrivelineDirty;

        public MechanicalDrivelineHostSession? MechanicalDriveline => _mechanicalDriveline;

        public void SetupMechanicalDriveline()
        {
            if (_mechanicalDriveline != null) return;

            var saved = MechanicalDrivelineSaveStore.TryLoad();
            _mechanicalDriveline = MechanicalDrivelineHostSession.Create(saved);
            _mechanicalDriveline.StateChanged += () => _mechanicalDrivelineDirty = true;
        }

        public PowerTransmissionResult ExecuteDrivelinePowerTransmission(
            string branchId,
            PrimeMoverType moverType,
            int resourceAvailabilityPermille,
            int operatorSkillPermille,
            int operatingHours)
        {
            SetupMechanicalDriveline();
            return _mechanicalDriveline!.ExecutePowerTransmission(
                branchId, moverType, resourceAvailabilityPermille, operatorSkillPermille, operatingHours);
        }

        public MachiningToleranceResult EvaluateDrivelineMachiningTolerance(
            string branchId,
            int requiredToleranceMicrons)
        {
            SetupMechanicalDriveline();
            return _mechanicalDriveline!.EvaluateMachiningTolerance(branchId, requiredToleranceMicrons);
        }

        public bool PerformMillwrightMaintenance(
            string branchId,
            int lubricantVolumeMl,
            int alignmentAdjustmentPermille)
        {
            SetupMechanicalDriveline();
            return _mechanicalDriveline!.PerformMillwrightMaintenance(
                branchId, lubricantVolumeMl, alignmentAdjustmentPermille);
        }

        public void RestockMechanicalLubricant(int volumeMl)
        {
            SetupMechanicalDriveline();
            _mechanicalDriveline!.RestockLubricantReserve(volumeMl);
        }

        public void AdvanceMechanicalDrivelineDay(int hoursOperatingPerDay = 8)
        {
            SetupMechanicalDriveline();
            _mechanicalDriveline!.AdvanceDay(hoursOperatingPerDay);
        }

        public MechanicalDrivelineCensus GetMechanicalDrivelineCensus() =>
            _mechanicalDriveline?.Census ?? default;

        public void SaveMechanicalDriveline()
        {
            if (_mechanicalDriveline == null) return;
            var state = _mechanicalDriveline.Ledger.CaptureState();
            MechanicalDrivelineSaveStore.TrySave(state);
            if (CaptureSection(
                    MechanicalDrivelineSaveStore.SectionName,
                    MechanicalDrivelineSaveStore.TryCapturePersisted(state)))
            {
                _mechanicalDrivelineDirty = false;
            }
        }

        public void FlushMechanicalDrivelineIfDirty()
        {
            if (_mechanicalDrivelineDirty)
            {
                SaveMechanicalDriveline();
            }
        }

        public void ResetMechanicalDriveline()
        {
            _mechanicalDriveline = null;
            _mechanicalDrivelineDirty = false;
        }
    }
}
