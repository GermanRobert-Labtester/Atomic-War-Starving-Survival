// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public sealed class MicrofluidicDiagnosticHostSession : HostSessionBase
    {
        public MicrofluidicDiagnosticEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public MicrofluidicDiagnosticHostSession(MicrofluidicDiagnosticEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnRunStarted += run =>
            {
                LastEvent = $"Assay run initiated: {run.AssayId} for Patient {run.PatientId}.";
                MarkDirty();
                RaiseStateChanged();
            };
            System.OnRunCompleted += res =>
            {
                LastEvent = $"Diagnostic Result: {res.AssayId} -> {res.ResultKind} ({res.Confidence01:P0} confidence).";
                MarkDirty();
                RaiseStateChanged();
            };
            System.OnDiagnosticHazard += hazard =>
            {
                LastEvent = $"Diagnostic Analyzer Warning: {hazard}!";
                MarkDirty();
                RaiseStateChanged();
            };
            System.OnStateChanged += _ =>
            {
                MarkDirty();
                RaiseStateChanged();
            };
        }

        public bool StartManufacturing(
            string jobId,
            string assayId,
            string operatorId,
            Func<System.Collections.Generic.IReadOnlyList<InventoryDemand>, bool> consumeInventory,
            out string failureReason)
        {
            bool started = System.StartCartridgeManufacturing(jobId, assayId, operatorId, consumeInventory, out failureReason);
            if (!started)
            {
                LastEvent = $"Cartridge manufacturing blocked: {failureReason}";
                RaiseStateChanged();
            }
            return started;
        }

        public bool StartRun(
            string runId,
            string patientId,
            string assayId,
            string operatorId,
            int currentDay,
            float currentMinute,
            Func<string, int, bool> consumeInventory,
            out string failureReason)
        {
            bool started = System.StartDiagnosticRun(runId, patientId, assayId, operatorId, currentDay, currentMinute, consumeInventory, out failureReason);
            if (!started)
            {
                LastEvent = $"Assay run blocked: {failureReason}";
                RaiseStateChanged();
            }
            return started;
        }

        public void Tick(float deltaMinutes, PowerSupplyContext power, AdvancedMachineOperatorContext? op, ISeededRng rng, Func<string, string, bool> patientHasDisease)
        {
            System.Tick(deltaMinutes, power, op, rng, patientHasDisease);
        }

        public void PerformMaintenance(string type)
        {
            System.PerformMaintenance(type);
            LastEvent = $"Diagnostic analyzer serviced: {type}.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            MicrofluidicDiagnosticSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
