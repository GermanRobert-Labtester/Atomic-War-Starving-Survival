// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : RunFlatTireHostSession
// Core Source  : Ashfall.Core.Expeditions.RunFlatTireEngine
// Purpose      : Thin Godot adapter — install/hazard/heat commands; no math.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Expeditions;

namespace AtomicWar.GodotApp
{
    public sealed class RunFlatTireHostSession : HostSessionBase
    {
        public RunFlatTireEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public bool LastInstallCommitted { get; private set; }

        /// <summary>Workshop readiness from the canonical shelter/power state.</summary>
        public Func<bool>? WorkshopAvailableProvider { get; set; }
        /// <summary>Per-profile kit availability from the canonical inventory.</summary>
        public Func<string, bool>? PartsAvailableProvider { get; set; }
        /// <summary>Ambient temperature (°C) from the single weather authority.</summary>
        public Func<int>? AmbientTempProvider { get; set; }

        public RunFlatTireHostSession(RunFlatTireEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public int AmbientTempC() => AmbientTempProvider?.Invoke() ?? RunFlatTireEngine.AmbientC;

        public string Install(string vehicleId, string vehicleTag, string profileId, double skill)
        {
            LastInstallCommitted = false;
            bool workshop = WorkshopAvailableProvider?.Invoke() ?? true;
            bool parts = PartsAvailableProvider?.Invoke(profileId) ?? true;
            var result = System.Install(vehicleId, vehicleTag, profileId, workshop, parts, skill);
            RaiseStateChanged();
            LastInstallCommitted = result.IsSuccess;
            LastEvent = result.IsSuccess
                ? $"Run-flat fitted ({profileId} on {vehicleId})."
                : $"Cannot fit run-flat ({result.FailureCode}).";
            return LastEvent;
        }

        public string ApplyHazard(string vehicleId, string hazardClass, int speedKph)
        {
            var result = System.ApplyHazard(vehicleId, hazardClass, speedKph, AmbientTempC());
            RaiseStateChanged();
            LastEvent = result.IsSuccess
                ? $"Hazard absorbed ({hazardClass}) on {vehicleId}."
                : $"Hazard outcome ({result.FailureCode}) on {vehicleId}.";
            return LastEvent;
        }

        public string TickHeat(string vehicleId, int speedKph, int loadBp)
        {
            System.TickHeat(vehicleId, speedKph, loadBp, AmbientTempC());
            RaiseStateChanged();
            var wheel = System.FindWheelSet(vehicleId);
            LastEvent = wheel == null
                ? "No run-flat wheel set on that vehicle."
                : $"Wheel heat {wheel.HeatC}C on {vehicleId}.";
            return LastEvent;
        }

        public string Repair(string vehicleId, double skill)
        {
            bool workshop = WorkshopAvailableProvider?.Invoke() ?? true;
            var result = System.Repair(vehicleId, workshop, skill);
            RaiseStateChanged();
            LastEvent = result.IsSuccess
                ? $"Run-flat serviced on {vehicleId}."
                : $"Cannot service run-flat ({result.FailureCode}).";
            return LastEvent;
        }

        public RunFlatTireState CaptureSave() => System.CaptureState();

        public void RestoreSave(RunFlatTireState? save)
        {
            if (save == null) return;
            System.RestoreState(save);
            LastEvent = "Run-flat wheel state restored from save.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            RunFlatTireSaveStore.TrySave(CaptureSave());
        }
    }
}
