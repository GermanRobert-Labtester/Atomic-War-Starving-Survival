// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : LowBackgroundMetrologyHostSession
// Core Source  : Ashfall.Core.Radiation.LowBackgroundLeadEngine
// Purpose      : Thin Godot adapter — LastEvent + commands; no assay math.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Radiation;

namespace AtomicWar.GodotApp
{
    public sealed class LowBackgroundMetrologyHostSession : HostSessionBase
    {
        public LowBackgroundLeadEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        /// <summary>
        /// Environmental background source in bp (basis points of the ordinary
        /// baseline). Wired by Main from authored defaults or the live
        /// ExposureContext; null-safe fallback to the ordinary baseline.
        /// </summary>
        public Func<int>? EnvironmentBackgroundBpProvider { get; set; }

        public int EnvironmentBackgroundBp()
        {
            int bp = EnvironmentBackgroundBpProvider?.Invoke() ?? LowBackgroundLeadEngine.OrdinaryBaselineBp;
            return Math.Clamp(bp, 0, LowBackgroundLeadEngine.OrdinaryBaselineBp * 2);
        }

        /// <summary>
        /// Sample-truth source (id, contamination bp) owned by the contamination
        /// authority — Main wires it from the water-treatment intake. Never
        /// invented here; a null provider yields a reference sample at 0 bp.
        /// </summary>
        public Func<(string SampleId, int ContaminationBp)>? SampleTruthProvider { get; set; }

        public (string SampleId, int ContaminationBp) CurrentSample()
            => SampleTruthProvider?.Invoke() ?? ("bench_reference_sample", 0);

        public LowBackgroundMetrologyHostSession(LowBackgroundLeadEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public string InstallModule(string profileId, int modules, int day)
        {
            var result = System.InstallShieldModule(profileId, modules, day);
            RaiseStateChanged();
            return result.Status == ActionResult.StatusKind.Success
                ? $"Shield module installed ({profileId} x{modules})."
                : $"Cannot install module ({result.FailureCode}).";
        }

        public string Calibrate(int day)
        {
            var result = System.Calibrate(day);
            RaiseStateChanged();
            return result.Status == ActionResult.StatusKind.Success
                ? "Detector calibrated."
                : $"Cannot calibrate ({result.FailureCode}).";
        }

        public string StartBatch(string profileId, int units, int day)
        {
            var result = System.StartBatch(profileId, units, day);
            RaiseStateChanged();
            return result.Status == ActionResult.StatusKind.Success
                ? $"Smelting batch started ({profileId} x{units})."
                : $"Cannot start batch ({result.FailureCode}).";
        }

        public string AddFeedstock(string batchId, string profileId, int units)
        {
            var result = System.AddFeedstock(batchId, profileId, units);
            RaiseStateChanged();
            return result.Status == ActionResult.StatusKind.Success
                ? $"Feedstock added ({profileId} x{units})."
                : $"Cannot add feedstock ({result.FailureCode}).";
        }

        public string CommitBatch(string batchId)
        {
            var result = System.CommitBatch(batchId);
            RaiseStateChanged();
            return result.Status == ActionResult.StatusKind.Success
                ? $"Batch committed ({batchId})."
                : $"Cannot commit batch ({result.FailureCode}).";
        }

        public string RunAssay(AssaySample sample, int nativeBackgroundBp, int environmentalBp, int assayTicks, double prepQuality, double skill, int day)
        {
            var outcome = System.RunAssay(sample, nativeBackgroundBp, environmentalBp, assayTicks, prepQuality, skill, day);
            RaiseStateChanged();
            if (outcome.Confidence <= 0.0)
                return $"Assay unavailable ({outcome.EstimatedBand}).";
            return outcome.BelowDetectionLimit
                ? $"Assay below detection limit (confidence {outcome.Confidence:P0})."
                : $"Assay: {outcome.EstimatedBand} (confidence {outcome.Confidence:P0}).";
        }

        public LowBackgroundMetrologyState CaptureSave() => System.CaptureState();

        public void RestoreSave(LowBackgroundMetrologyState? save)
        {
            if (save == null) return;
            System.RestoreState(save);
            LastEvent = "Low-background metrology restored from save.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            LowBackgroundMetrologySaveStore.TrySave(CaptureSave());
        }
    }
}
