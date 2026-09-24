// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Expansion 29 — The Glass: glassworks host wiring.
// The signed pure PrecisionGlassworksOpticsEngine (DEC-83) is the vitrification,
// lens-grinding, and theodolite-calibration authority. PrecisionOpticsEngine keeps
// the optical catalog; RadiationSystem keeps browning; GreenhouseSystem keeps
// glazing; GeodeticSurveyEngine keeps survey. This host owns only batches,
// vision prescriptions, and abrasive grit stock.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.Optics;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private GlassworksHostSession? _glassworks;
        private bool _glassworksDirty;

        /// <summary>Authored-optimal anneal temperature (permille), matching the engine's 900 optimum.</summary>
        public const int GlassworksOptimalKilnPermille = 900;

        /// <summary>Days between anneal stages in the shelter kiln.</summary>
        public const int GlassworksDaysPerAnnealStage = 2;

        public GlassworksHostSession? Glassworks => _glassworks;

        public void SetupGlassworks()
        {
            if (_glassworks != null) return;

            var saved = GlassworksSaveStore.TryLoad();
            _glassworks = GlassworksHostSession.Create(saved);
            _glassworks.StateChanged += () => _glassworksDirty = true;
        }

        /// <summary>
        /// Day owner: advances one annealing stage for each uncracked, unfinished
        /// batch every <see cref="GlassworksDaysPerAnnealStage"/> days at the
        /// authored-optimal kiln temperature, so annealing is deterministic.
        /// </summary>
        public void TickGlassworks(int day)
        {
            SetupGlassworks();
            if (_glassworks == null) return;
            if (day <= 0 || day % GlassworksDaysPerAnnealStage != 0) return;

            foreach (var batch in _glassworks.Ledger.Batches)
            {
                if (batch.IsCracked) continue;
                if (batch.AnnealingStage >= PrecisionGlassworksOpticsEngine.MaxAnnealingStages) continue;
                _glassworks.AdvanceAnnealing(batch.BatchId, GlassworksOptimalKilnPermille);
            }
        }

        public bool ChargeGlassBatch(string batchId, int silicaPurityPermille)
        {
            SetupGlassworks();
            return _glassworks?.AddBatch(batchId, silicaPurityPermille) ?? false;
        }

        public LensGrindingResult GrindVisionLens(string batchId, VisionCorrectionBand band, int grinderSkillPermille, int grindSeed)
        {
            SetupGlassworks();
            return _glassworks?.GrindCorrectionLens(batchId, band, grinderSkillPermille, grindSeed)
                ?? new LensGrindingResult(0, false, 0, false);
        }

        public TheodoliteCalibrationResult CalibrateSurveyTheodolite(string batchId, int calibratorSkillPermille)
        {
            SetupGlassworks();
            return _glassworks?.CalibrateTheodolite(batchId, calibratorSkillPermille)
                ?? new TheodoliteCalibrationResult(0, 0, false);
        }

        public void SetSurvivorVisionPrescription(string survivorId, VisionCorrectionBand band) =>
            _glassworks?.SetVisionPrescription(survivorId, band);

        public VisionCorrectionBand GetSurvivorVisionPrescription(string survivorId) =>
            _glassworks?.GetVisionPrescription(survivorId) ?? VisionCorrectionBand.NoCorrectionNeeded;

        public GlassworksCensus GetGlassworksCensus() => _glassworks?.Census ?? default;

        public void SaveGlassworks()
        {
            if (_glassworks == null) return;
            var state = _glassworks.CaptureState();
            GlassworksSaveStore.TrySave(state);
            if (CaptureSection(GlassworksSaveStore.SectionName, GlassworksSaveStore.TryCapturePersisted(state)))
                _glassworksDirty = false;
        }

        public void FlushGlassworksIfDirty()
        {
            if (_glassworksDirty)
                SaveGlassworks();
        }

        public void ResetGlassworks()
        {
            _glassworks = null;
            _glassworksDirty = false;
        }
    }
}
