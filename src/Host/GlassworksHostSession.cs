// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : GlassworksSaveStore
// Core State : Ashfall.Core.Optics.GlassworksState
// Host Caller: Main.Glassworks
// Purpose    : Expansion 29 — vitrification batches, vision prescriptions, and
//              abrasive grit stock. The precision-optics catalog authority stays
//              with PrecisionOpticsEngine; this section is the glass/vision ledger.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Optics;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class GlassworksSaveStore
    {
        public const string FileName = "glassworks_save.json";
        public const string SectionName = "glassworks";

        private static readonly SaveStore<GlassworksState> s_store =
            SaveStoreHub.Checksummed<GlassworksState>(FileName, nameof(GlassworksSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(GlassworksState state) => s_store.CaptureBare(state);
        public static GlassworksState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(GlassworksState state) => s_store.TrySave(state);
        public static GlassworksState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Expansion 29 host session. Wraps the stateful <see cref="GlassworksLedger"/>
    /// over the signed pure <see cref="PrecisionGlassworksOpticsEngine"/>. It owns
    /// batches, vision prescriptions, and grit stock — never the optical catalog,
    /// radiation browning, greenhouse glazing, or survey models.
    /// </summary>
    public sealed class GlassworksHostSession : HostSessionBase
    {
        private readonly GlassworksLedger _ledger;
        private int _lastAnnealDay = -1;

        public GlassworksLedger Ledger => _ledger;
        public GlassworksCensus Census => _ledger.GetCensus();
        public int BatchCount => _ledger.BatchCount;
        public int GritStockPermille => _ledger.GritStockPermille;
        public string LastEvent { get; private set; } = string.Empty;

        public GlassworksHostSession(GlassworksState? state = null)
        {
            _ledger = new GlassworksLedger(state);
        }

        public static GlassworksHostSession Create(GlassworksState? state = null) => new GlassworksHostSession(state);

        public bool AddBatch(string batchId, int silicaPurityPermille)
        {
            bool ok = _ledger.AddBatch(batchId, silicaPurityPermille);
            if (ok)
            {
                LastEvent = $"Glass batch '{batchId}' charged at {silicaPurityPermille}\u2030 silica purity.";
                RaiseStateChanged();
            }
            return ok;
        }

        public GlassBatchState? AdvanceAnnealing(string batchId, int kilnTemperaturePermille)
        {
            var batch = _ledger.AdvanceAnnealing(batchId, kilnTemperaturePermille);
            if (batch != null)
            {
                LastEvent = batch.IsCracked
                    ? $"Batch '{batchId}' cracked during annealing."
                    : $"Batch '{batchId}' advanced to annealing stage {batch.AnnealingStage}/{PrecisionGlassworksOpticsEngine.MaxAnnealingStages}.";
                RaiseStateChanged();
            }
            return batch;
        }

        public LensGrindingResult GrindCorrectionLens(string batchId, VisionCorrectionBand band, int grinderSkillPermille, int grindSeed)
        {
            var result = _ledger.GrindCorrectionLens(batchId, band, grinderSkillPermille, grindSeed);
            LastEvent = result.MeetsPrescriptionTolerance
                ? $"Lens ground for {band} within tolerance ({result.OpticalPrecisionPermille}\u2030)."
                : $"Lens ground for {band} did not meet tolerance ({result.OpticalPrecisionPermille}\u2030).";
            RaiseStateChanged();
            return result;
        }

        public TheodoliteCalibrationResult CalibrateTheodolite(string batchId, int calibratorSkillPermille)
        {
            var result = _ledger.CalibrateTheodolite(batchId, calibratorSkillPermille);
            LastEvent = result.IsFieldReady
                ? $"Theodolite field-ready at {result.CalibrationAccuracyPermille}\u2030 ({result.SurveyRangeMetres} m range)."
                : $"Theodolite not field-ready ({result.CalibrationAccuracyPermille}\u2030).";
            RaiseStateChanged();
            return result;
        }

        public void SetVisionPrescription(string survivorId, VisionCorrectionBand band)
        {
            _ledger.SetVisionPrescription(survivorId, band);
            RaiseStateChanged();
        }

        public VisionCorrectionBand GetVisionPrescription(string survivorId) => _ledger.GetVisionPrescription(survivorId);

        public GlassworksState CaptureState() => _ledger.CaptureState();
        public void RestoreState(GlassworksState? state) => _ledger.RestoreState(state);
        public void Clear() => _ledger.Clear();
    }
}
