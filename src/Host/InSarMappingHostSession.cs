// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : InSarMappingHostSession
// Core Source  : Ashfall.Core.World.InSarDeformationEngine
// Purpose      : Thin Godot adapter — LastEvent + commands; no interferometry.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public sealed class InSarMappingHostSession : HostSessionBase
    {
        public InSarDeformationEngine System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        /// <summary>
        /// Weather-quality source in bp (0..100). Wired by Main from the live
        /// WeatherSystem; null-safe fallback to clear-sky (100).
        /// </summary>
        public Func<int>? WeatherQualityBpProvider { get; set; }

        /// <summary>
        /// Per-sector terrain decorrelation tags. Wired by Main from map/world
        /// truth; an empty set means no authored terrain penalty.
        /// </summary>
        public Func<string, IReadOnlyCollection<string>>? TerrainTagsProvider { get; set; }

        /// <summary>
        /// Observation-quality source (sensor, sector, day) -> 0..100. Wired by
        /// Main to the deterministic authored read; the panel never invents a value.
        /// </summary>
        public Func<string, string, int, int>? ObservationQualityProvider { get; set; }

        public InSarMappingHostSession(InSarDeformationEngine system)
        {
            System = system ?? throw new ArgumentNullException(nameof(system));
        }

        public int WeatherQualityBp()
        {
            int bp = WeatherQualityBpProvider?.Invoke() ?? 100;
            return Math.Clamp(bp, 0, 100);
        }

        public IReadOnlyCollection<string> TerrainTags(string sectorId)
            => TerrainTagsProvider?.Invoke(sectorId) ?? Array.Empty<string>();

        /// <summary>
        /// Records a repeat-pass observation. The reference geometry is the
        /// sensor profile, so passes from different platforms over the same
        /// sector are correctly treated as incompatible.
        /// </summary>
        public string RecordSurveyPass(string sensorProfileId, string sectorId, int day)
        {
            int quality = ObservationQualityProvider?.Invoke(sensorProfileId, sectorId, day) ?? 50;
            return RecordSurveyPass(sensorProfileId, sectorId, day, quality);
        }

        public string RecordSurveyPass(string sensorProfileId, string sectorId, int day, int qualityBp)
        {
            var result = System.RecordSurveyPass(
                sensorProfileId, sectorId, day, qualityBp, WeatherQualityBp(), sensorProfileId);
            RaiseStateChanged();
            if (result.IsSuccess)
            {
                LastEvent = $"Survey pass recorded for {sectorId}.";
                return LastEvent;
            }
            LastEvent = $"Cannot record survey pass ({result.FailureCode}).";
            return LastEvent;
        }

        /// <summary>Processes the compatible repeat-pass stack into a deformation summary.</summary>
        public string ProcessSector(string sectorId, double processingSkill)
        {
            var result = System.ProcessSector(sectorId, processingSkill, TerrainTags(sectorId));
            RaiseStateChanged();
            if (result.IsSuccess)
            {
                var s = System.GetSummary(sectorId);
                LastEvent = $"Deformation map updated for {sectorId} ({s?.Classification ?? "unknown"}).";
                return LastEvent;
            }

            LastEvent = result.FailureCode switch
            {
                "low_coherence" => "Deformation read rejected: coherence below threshold — inconclusive, not safe.",
                "pass_geometry_incompatible" => "Pass geometry incompatible — repeat passes must share a platform line.",
                "insufficient_survey_passes" => "At least two compatible repeat passes are required.",
                "sector_not_surveyed" => "No survey passes recorded for this sector.",
                _ => $"Cannot process sector ({result.FailureCode})."
            };
            return LastEvent;
        }

        public InSarDeformationState CaptureSave() => System.CaptureState();

        public void RestoreSave(InSarDeformationState? save)
        {
            if (save == null) return;
            System.RestoreState(save);
            LastEvent = "InSAR deformation intelligence restored from save.";
            RaiseStateChanged();
        }

        public override void Save()
        {
            InSarMappingSaveStore.TrySave(CaptureSave());
        }
    }
}
