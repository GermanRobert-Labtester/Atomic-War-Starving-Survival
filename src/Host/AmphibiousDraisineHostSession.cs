// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : AmphibiousDraisineHostSession
// Core Source  : Ashfall.Core.Expeditions.AmphibiousDraisineEngine
// Purpose      : Thin Godot adapter — kit commands + typed seams to vehicle
//                logistics / garage / route planner. No math, no truth.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Expeditions;

namespace AtomicWar.GodotApp
{
    public sealed class AmphibiousDraisineHostSession : HostSessionBase
    {
        public AmphibiousDraisineEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        /// <summary>Vehicle tag + condition from the canonical vehicle owners (garage/logistics).</summary>
        public Func<string, (string Tag, int ConditionBp)>? VehicleStateProvider { get; set; }
        /// <summary>Workshop readiness from the canonical shelter state.</summary>
        public Func<bool>? WorkshopAvailableProvider { get; set; }
        /// <summary>Kit install/repair item availability from the canonical inventory.</summary>
        public Func<string, bool>? PartsAvailableProvider { get; set; }
        /// <summary>Mechanic skill 0..100 from the canonical survivor owners.</summary>
        public Func<float>? MechanicSkillProvider { get; set; }
        /// <summary>Pump power availability from the canonical power owner.</summary>
        public Func<bool>? PumpPowerAvailableProvider { get; set; }

        public AmphibiousDraisineHostSession(AmphibiousDraisineEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public string InstallKit(string vehicleId, string kitProfileId)
        {
            var (tag, conditionBp) = VehicleStateProvider?.Invoke(vehicleId) ?? (string.Empty, 10000);
            var result = System.InstallKit(vehicleId, tag, kitProfileId,
                WorkshopAvailableProvider?.Invoke() ?? true,
                PartsAvailableProvider?.Invoke(kitProfileId) ?? true,
                conditionBp);
            RaiseStateChanged();
            LastEvent = result.IsSuccess
                ? $"Amphibious kit installed ({kitProfileId} on {vehicleId})."
                : $"Cannot install kit ({result.FailureCode}).";
            return LastEvent;
        }

        public string Deploy(string vehicleId)
        {
            var result = System.Deploy(vehicleId);
            RaiseStateChanged();
            LastEvent = result.IsSuccess ? "Outriggers deploying." : $"Cannot deploy ({result.FailureCode}).";
            return LastEvent;
        }

        /// <summary>Advances deployment/landing/recovery ticks once per expedition tick.</summary>
        public string TickTransitions(string vehicleId)
        {
            bool boundary = System.TickDeployment(vehicleId) || System.TickLanding(vehicleId)
                || System.TickEmergencyRecovery(vehicleId);
            RaiseStateChanged();
            LastEvent = boundary ? "Kit transition complete." : "Kit transition in progress.";
            return LastEvent;
        }

        /// <summary>Adjusts the cargo load fraction for the pending crossing decision.</summary>
        public string SetCargoLoad(string vehicleId, float cargoLoadFraction)
        {
            var result = System.SetCargoLoad(vehicleId, cargoLoadFraction);
            RaiseStateChanged();
            LastEvent = $"Cargo load set to {cargoLoadFraction * 100:0}%.";
            return LastEvent;
        }

        public string BeginCrossing(string vehicleId, string routeClassId, float vehicleMassFraction)
        {
            var (tag, conditionBp) = VehicleStateProvider?.Invoke(vehicleId) ?? (string.Empty, 10000);
            var result = System.BeginCrossing(vehicleId, routeClassId, vehicleMassFraction,
                conditionBp, PumpPowerAvailableProvider?.Invoke() ?? true);
            RaiseStateChanged();
            LastEvent = result.IsSuccess ? "Crossing attempt started." : $"Crossing refused ({result.FailureCode}).";
            return LastEvent;
        }

        public string AdvanceCrossing(string vehicleId, float currentRisk, bool badWeather, float navigatorSkill)
        {
            var (_, conditionBp) = VehicleStateProvider?.Invoke(vehicleId) ?? (string.Empty, 10000);
            var result = System.AdvanceCrossing(vehicleId, currentRisk, badWeather, navigatorSkill, conditionBp);
            RaiseStateChanged();
            LastEvent = result.IsSuccess ? "Crossing tick complete." : $"Crossing ended ({result.FailureCode}).";
            return LastEvent;
        }

        public string AbortCrossing(string vehicleId)
        {
            var result = System.AbortCrossing(vehicleId);
            RaiseStateChanged();
            LastEvent = result.IsSuccess ? "Crossing aborted; recovering." : $"Cannot abort ({result.FailureCode}).";
            return LastEvent;
        }

        public string RepairKit(string vehicleId)
        {
            var result = System.RepairKit(vehicleId,
                PartsAvailableProvider?.Invoke("amb_repair") ?? true,
                MechanicSkillProvider?.Invoke() ?? 0f);
            RaiseStateChanged();
            LastEvent = result.IsSuccess ? "Kit repaired." : $"Cannot repair kit ({result.FailureCode}).";
            return LastEvent;
        }

        /// <summary>Typed route-capability lookup consumed by the route planner.</summary>
        public bool IsRouteCapable(string vehicleId, string routeClassId, out string failureCode)
            => System.TryGetRouteCapability(vehicleId, routeClassId, out failureCode);
    
        public System.Collections.Generic.Dictionary<string, AmphibiousDraisineState> CaptureSave() => System.CaptureState();

        public void RestoreSave(System.Collections.Generic.Dictionary<string, AmphibiousDraisineState>? save)
        {
            if (save == null) return;
            System.RestoreState(save);
            LastEvent = "State restored from save.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            AmphibiousDraisineSaveStore.TrySave(CaptureSave());
        }
}
}
