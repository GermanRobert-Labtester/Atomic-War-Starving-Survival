// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plan 138 Phase 2 — Low-background metrology host wire
// Subsystems   : catalog load, detector/shield/calibration state, smelting
//                batches, assay command bridging the live radiation ambient,
//                dedicated save section. Measurement only — never purifies.
// ============================================================================
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private LowBackgroundMetrologyHostSession? _lowBackgroundMetrology;
        private LowBackgroundLeadPanel? _lowBackgroundPanel;

        /// <summary>Authored native background of the shelter's scintillation bench, bp.</summary>
        public const int LowBackgroundNativeDetectorBp = 40;

        /// <summary>Authored ordinary environmental baseline, bp (Phase 1 default).</summary>
        public const int LowBackgroundDefaultEnvironmentalBp = LowBackgroundLeadEngine.OrdinaryBaselineBp;

        /// <summary>Panel-facing session (Plan 138 Phase 3).</summary>
        public LowBackgroundMetrologyHostSession EnsureLowBackgroundMetrologySession()
        {
            SetupLowBackgroundMetrology();
            return _lowBackgroundMetrology!;
        }

        private void SetupLowBackgroundMetrology()
        {
            if (_lowBackgroundMetrology != null) return;

            var catalog = LowBackgroundLeadCatalogLoader.Load(
                _dataDir,
                new FileSystemIO());

            var system = new LowBackgroundLeadEngine(catalog, new GodotLog());

            // Forked campaign stream — assay jitter is deterministic per seed,
            // and adding this stream cannot shift any other stream's seed.
            if (_campaignDay?.Rng != null)
                system.Rng = _campaignDay.Rng.Fork(Ashfall.Core.Random.CampaignStreamIds.LowBackgroundMetrology);

            _lowBackgroundMetrology = new LowBackgroundMetrologyHostSession(system)
            {
                // Environmental background source: live shelter-interior exposure
                // environment when available, authored baseline otherwise.
                EnvironmentBackgroundBpProvider = ResolveLowBackgroundEnvironmentalBp,
                // Sample truth comes from the water-treatment intake (flood-event
                // contamination level) — the panel never invents sample values.
                SampleTruthProvider = () =>
                {
                    if (_waterTreatment == null) return ("bench_reference_sample", 0);
                    var wt = _waterTreatment.System.State;
                    int bp = (int)Math.Clamp(wt.incomingContaminationLevel * 100f, 0f, 200f);
                    return ("raw_water_intake", bp);
                }
            };

            var saved = LowBackgroundMetrologySaveStore.TryLoad();
            if (saved != null)
            {
                _lowBackgroundMetrology.RestoreSave(saved);
                GD.Print("[Ashfall Godot] Low-background metrology restored.");
            }

            // Phase 3 panel — created hidden; opened via the expanded-panel route.
            if (_lowBackgroundPanel != null && _lowBackgroundPanel.IsInsideTree())
                RemoveChild(_lowBackgroundPanel);
            _lowBackgroundPanel = new LowBackgroundLeadPanel
            {
                AppDayProvider = () => _simDay
            };
            _lowBackgroundPanel.Bind(_lowBackgroundMetrology);
            _lowBackgroundPanel.Visible = false;
            AddChild(_lowBackgroundPanel);
        }

        /// <summary>
        /// Live environmental background: shelter-interior exposure environment
        /// of the first registered survivor, mapped 1:1 to the bp scale and
        /// clamped to engine bounds. Falls back to the authored baseline when
        /// no exposure has been resolved yet (fresh campaign, headless tests).
        /// </summary>
        private int ResolveLowBackgroundEnvironmentalBp()
        {
            var survivors = _survivors;
            var roster = survivors?.RosterState;
            if (survivors != null && roster != null && roster.Count > 0 && !string.IsNullOrEmpty(roster[0].Id))
            {
                var env = survivors.GetLastExposureEnvironment(roster[0].Id);
                if (env != null)
                    return Math.Clamp((int)env.EffectiveZoneRadLevel, 0, LowBackgroundLeadEngine.OrdinaryBaselineBp * 2);
            }
            return LowBackgroundDefaultEnvironmentalBp;
        }

        public string InstallLowBackgroundShieldModule(string profileId, int modules)
        {
            SetupLowBackgroundMetrology();
            if (_lowBackgroundMetrology == null) return "Low-background metrology is unavailable.";
            return _lowBackgroundMetrology.InstallModule(profileId, modules, _simDay);
        }

        public string CalibrateLowBackgroundDetector()
        {
            SetupLowBackgroundMetrology();
            if (_lowBackgroundMetrology == null) return "Low-background metrology is unavailable.";
            return _lowBackgroundMetrology.Calibrate(_simDay);
        }

        public string StartLowBackgroundBatch(string profileId, int units)
        {
            SetupLowBackgroundMetrology();
            if (_lowBackgroundMetrology == null) return "Low-background metrology is unavailable.";
            return _lowBackgroundMetrology.StartBatch(profileId, units, _simDay);
        }

        public string AddLowBackgroundFeedstock(string batchId, string profileId, int units)
        {
            SetupLowBackgroundMetrology();
            if (_lowBackgroundMetrology == null) return "Low-background metrology is unavailable.";
            return _lowBackgroundMetrology.AddFeedstock(batchId, profileId, units);
        }

        public string CommitLowBackgroundBatch(string batchId)
        {
            SetupLowBackgroundMetrology();
            if (_lowBackgroundMetrology == null) return "Low-background metrology is unavailable.";
            return _lowBackgroundMetrology.CommitBatch(batchId);
        }

        /// <summary>
        /// Run a contamination assay against the live shelter environment.
        /// The result is screening intelligence only — it does not modify
        /// water/kitchen state (those authorities consume the verdict).
        /// </summary>
        public string RunLowBackgroundAssay(string sampleId, int contaminationBp)
        {
            SetupLowBackgroundMetrology();
            if (_lowBackgroundMetrology == null) return "Low-background metrology is unavailable.";
            return _lowBackgroundMetrology.RunAssay(
                new AssaySample(sampleId, contaminationBp),
                LowBackgroundNativeDetectorBp,
                _lowBackgroundMetrology.EnvironmentBackgroundBp(),
                assayTicks: 6,
                prepQuality: 0.8,
                skill: 0.5,
                day: _simDay);
        }

        private void SaveLowBackgroundMetrology()
        {
            if (_lowBackgroundMetrology == null) return;
            CaptureSection(
                LowBackgroundMetrologySaveStore.SectionName,
                LowBackgroundMetrologySaveStore.TryCapturePersisted(_lowBackgroundMetrology.CaptureSave()));
        }
    }
}
