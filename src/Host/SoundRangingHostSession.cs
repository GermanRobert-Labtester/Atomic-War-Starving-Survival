// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : SoundRangingHostSession
// Core Source  : Ashfall.Core.Combat.SoundRangingThreatEngine
// Purpose      : Thin Godot adapter — DEFENSIVE threat-intel projection to
//                map/warning consumers via typed events. No targeting math.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Combat;

namespace AtomicWar.GodotApp
{
    public sealed class SoundRangingHostSession : HostSessionBase
    {
        public SoundRangingThreatEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        /// <summary>
        /// Typed threat-intel event for map/warning consumers. Carries the
        /// defensive estimate only — bearing sector, region radius,
        /// confidence. Never a targeting coordinate.
        /// </summary>
        public event Action<SoundRangingThreatEngine.AcousticThreatEstimate>? OnThreatEstimate;

        /// <summary>Atmospheric profile selection from the single weather authority.</summary>
        public Func<string?>? AtmosphericProfileProvider { get; set; }
        /// <summary>Surface node condition from the equipment-condition owner.</summary>
        public Func<string, bool>? SensorNodeOperationalProvider { get; set; }

        public SoundRangingHostSession(SoundRangingThreatEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public string Deploy(string arrayProfileId, bool partsAvailable)
        {
            var result = System.Install(arrayProfileId, partsAvailable);
            RaiseStateChanged();
            LastEvent = result.IsSuccess ? $"Sound array deployed ({arrayProfileId})." : $"Cannot deploy array ({result.FailureCode}).";
            return LastEvent;
        }

        /// <summary>
        /// Records one hostile long-range fire event (defensive intake) and
        /// projects the resulting estimate to consumers.
        /// </summary>
        public SoundRangingThreatEngine.AcousticThreatEstimate? RecordHostileFire(
            SoundRangingThreatEngine.HostileFireObservation observation)
        {
            var result = System.RecordObservation(observation, AtmosphericProfileProvider?.Invoke());
            RaiseStateChanged();
            if (result is { IsSuccess: true })
            {
                LastEvent = $"Hostile fire: {result.Value!.BearingDeg}° ±{result.Value.BearingErrorDeg:0}°, "
                    + $"confidence {result.Value.ConfidenceBp / 100.0:0.0%}, region ±{result.Value.RegionRadiusCells} cells.";
                OnThreatEstimate?.Invoke(result.Value);
                return result.Value;
            }
            LastEvent = $"Observation not usable ({result?.FailureCode}).";
            return null;
        }

        public string TickDay(int currentDay)
        {
            var result = System.DecayDay(currentDay);
            RaiseStateChanged();
            LastEvent = result.IsSuccess ? "Array day tick: drift + staleness applied." : $"Array tick ({result.FailureCode}).";
            return LastEvent;
        }

        public string PerformMaintenance(bool partsAvailable)
        {
            var result = System.PerformMaintenance(partsAvailable);
            RaiseStateChanged();
            LastEvent = result.IsSuccess ? "Array recalibrated." : $"Cannot recalibrate ({result.FailureCode}).";
            return LastEvent;
        }

        /// <summary>Sabotage/repair surface — state flips via the engine's node seam.</summary>
        public string SetNodeOperational(string nodeId, bool operational)
        {
            var result = System.SetSensorNodeOperational(nodeId, operational);
            RaiseStateChanged();
            LastEvent = result.IsSuccess
                ? (operational ? $"Sensor node {nodeId} back online." : $"Sensor node {nodeId} lost.")
                : $"Node update failed ({result.FailureCode}).";
            return LastEvent;
        }
    
        public SoundRangingThreatEngine.SoundRangingStationState CaptureSave() => System.CaptureState();

        public void RestoreSave(SoundRangingThreatEngine.SoundRangingStationState? save)
        {
            if (save == null) return;
            System.RestoreState(save);
            LastEvent = "State restored from save.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            SoundRangingSaveStore.TrySave(CaptureSave());
        }
}
}
