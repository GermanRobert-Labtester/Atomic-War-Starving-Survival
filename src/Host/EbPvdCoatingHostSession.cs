// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public sealed class EbPvdCoatingHostSession : HostSessionBase
    {
        public EbPvdCoatingEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public EbPvdCoatingHostSession(EbPvdCoatingEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnJobStarted += job =>
            {
                LastEvent = $"EB-PVD process started: {job.CoatingId} on {job.SubstrateTag}.";
                MarkDirty();
                RaiseStateChanged();
            };
            System.OnJobCompleted += record =>
            {
                LastEvent = $"EB-PVD coating completed: {record.InstanceId} ({record.ThicknessUm:F0}µm, Uniformity: {record.Uniformity01:P0}).";
                MarkDirty();
                RaiseStateChanged();
            };
            System.OnMachineHazard += hazard =>
            {
                LastEvent = $"EB-PVD Hazard Detected: {hazard}!";
                MarkDirty();
                RaiseStateChanged();
            };
            System.OnStateChanged += _ =>
            {
                MarkDirty();
                RaiseStateChanged();
            };
        }

        public bool StartCoatingJob(
            string jobId,
            string coatingId,
            string substrateTag,
            string operatorId,
            int currentDay,
            bool applyBondCoat,
            Func<System.Collections.Generic.IReadOnlyList<InventoryDemand>, bool> consumeInventoryCallback,
            out string failureReason)
        {
            bool started = System.StartJob(jobId, coatingId, substrateTag, operatorId, currentDay, applyBondCoat, consumeInventoryCallback, out failureReason);
            if (!started)
            {
                LastEvent = $"EB-PVD job failed to start: {failureReason}";
                RaiseStateChanged();
            }
            return started;
        }

        public void TickProcess(float hours, PowerSupplyContext power, AdvancedMachineOperatorContext? op, ISeededRng rng)
        {
            System.Tick(hours, power, op, rng);
        }

        public void PerformMaintenance(string maintenanceType)
        {
            System.PerformMaintenance(maintenanceType);
            LastEvent = $"EB-PVD maintenance completed: {maintenanceType}.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            EbPvdCoatingSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
