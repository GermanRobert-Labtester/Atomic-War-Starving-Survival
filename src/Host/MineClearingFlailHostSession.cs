// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.AdvancedMachinery;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Save;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public sealed class MineClearingFlailHostSession : HostSessionBase
    {
        public MineClearingFlailEngine System { get; }
        /// <summary>Optional route authority for panel corridor lists (owned by Main).</summary>
        public RouteInfrastructureSystem? Routes { get; set; }
        public string LastEvent { get; private set; } = string.Empty;

        public MineClearingFlailHostSession(MineClearingFlailEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
            System.OnBreachStarted += breach =>
            {
                LastEvent = $"Minefield breach initiated on route {breach.RouteId}:{breach.SegmentId} ({breach.TargetMeters:F0}m).";
                MarkDirty();
                RaiseStateChanged();
            };
            System.OnBreachCompleted += breach =>
            {
                LastEvent = $"Minefield breach completed on {breach.RouteId}:{breach.SegmentId}! Safe corridor established.";
                MarkDirty();
                RaiseStateChanged();
            };
            System.OnFlailIncident += incident =>
            {
                LastEvent = $"Flail Warning: {incident}!";
                MarkDirty();
                RaiseStateChanged();
            };
            System.OnStateChanged += _ =>
            {
                MarkDirty();
                RaiseStateChanged();
            };
        }

        public bool StartBreach(
            string routeId,
            string segmentId,
            string operatorId,
            int currentDay,
            float segmentLengthMeters,
            RouteInfrastructureSystem routeSystem,
            out string failureReason)
        {
            bool started = System.StartBreach(routeId, segmentId, operatorId, currentDay, segmentLengthMeters, routeSystem, out failureReason);
            if (!started)
            {
                LastEvent = $"Mine clearance blocked: {failureReason}";
                RaiseStateChanged();
            }
            return started;
        }

        public void TickBreach(
            float hours,
            int currentDay,
            RouteInfrastructureSystem routeSystem,
            AdvancedMachineOperatorContext? op,
            ISeededRng rng)
        {
            System.TickBreach(hours, currentDay, routeSystem, op, rng);
        }

        public void PerformMaintenance(string type)
        {
            System.PerformMaintenance(type);
            LastEvent = $"Flail maintenance completed: {type}.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            MineClearingFlailSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
