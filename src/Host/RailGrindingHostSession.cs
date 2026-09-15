// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public sealed class RailGrindingHostSession : HostSessionBase
    {
        public RailGrindingEngine System { get; }
        /// <summary>Optional route authority for panel corridor lists (owned by Main).</summary>
        public RouteInfrastructureSystem? Routes { get; set; }
        public string LastEvent { get; private set; } = string.Empty;

        public RailGrindingHostSession(RailGrindingEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnJobStarted += job =>
            {
                LastEvent = $"Rail grinding started on route {job.RouteId}:{job.SegmentId} ({job.TargetKm:F1} km).";
                MarkDirty();
                RaiseStateChanged();
            };
            System.OnJobCompleted += job =>
            {
                LastEvent = $"Rail grinding complete on {job.RouteId}:{job.SegmentId}! Roughness reduced to {job.CurrentRoughness:F2}.";
                MarkDirty();
                RaiseStateChanged();
            };
            System.OnGrindingHazard += hazard =>
            {
                LastEvent = $"Rail Grinder Warning: {hazard}!";
                MarkDirty();
                RaiseStateChanged();
            };
            System.OnStateChanged += _ =>
            {
                MarkDirty();
                RaiseStateChanged();
            };
        }

        public bool StartGrindingJob(
            string routeId,
            string segmentId,
            string operatorId,
            float segmentLengthKm,
            RouteInfrastructureSystem routeSystem,
            out string failureReason)
        {
            bool started = System.StartGrindingJob(routeId, segmentId, operatorId, segmentLengthKm, routeSystem, out failureReason);
            if (!started)
            {
                LastEvent = $"Rail grinding blocked: {failureReason}";
                RaiseStateChanged();
            }
            return started;
        }

        public void TickGrinding(
            float hours,
            int currentDay,
            RouteInfrastructureSystem routeSystem,
            AdvancedMachineOperatorContext? op,
            ISeededRng rng)
        {
            System.TickGrinding(hours, currentDay, routeSystem, op, rng);
        }

        public void PerformMaintenance(string type)
        {
            System.PerformMaintenance(type);
            LastEvent = $"Rail grinder serviced: {type}.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            RailGrindingSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
