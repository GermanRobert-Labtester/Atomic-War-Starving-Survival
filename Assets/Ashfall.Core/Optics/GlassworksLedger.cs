// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 29 — The Glass
// Subsystem    : Glassworks Ledger (stateful owner over the signed pure
//                PrecisionGlassworksOpticsEngine, DEC-83)
// ============================================================================
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Optics
{
    /// <summary>
    /// Persisted glassworks state: vitrification batches, survivor vision
    /// prescriptions, and the abrasive grit stock. This is the batch/vision layer
    /// only; the optical-element catalog authority stays with PrecisionOpticsEngine.
    /// </summary>
    [Serializable]
    public sealed class GlassworksState
    {
        public int SchemaVersion { get; set; } = 1;
        public List<GlassBatchState> Batches { get; set; } = new List<GlassBatchState>();
        /// <summary>survivorId → (int)VisionCorrectionBand.</summary>
        public Dictionary<string, int> VisionPrescriptions { get; set; } =
            new Dictionary<string, int>(StringComparer.Ordinal);
        public int AbrasiveGritStockPermille { get; set; } = 1000;

        public GlassworksState Clone() => new GlassworksState
        {
            SchemaVersion = SchemaVersion,
            Batches = new List<GlassBatchState>(Batches),
            VisionPrescriptions = new Dictionary<string, int>(VisionPrescriptions, StringComparer.Ordinal),
            AbrasiveGritStockPermille = AbrasiveGritStockPermille
        };
    }

    /// <summary>Bounded read model of the glassworks ledger.</summary>
    public struct GlassworksCensus
    {
        public int BatchCount { get; }
        public int AnnealedBatches { get; }
        public int CrackedBatches { get; }
        public int PrecisionOpticBatches { get; }
        public int PrescriptionCount { get; }
        public int GritStockPermille { get; }

        public GlassworksCensus(
            int batchCount, int annealedBatches, int crackedBatches,
            int precisionOpticBatches, int prescriptionCount, int gritStockPermille)
        {
            BatchCount = batchCount;
            AnnealedBatches = annealedBatches;
            CrackedBatches = crackedBatches;
            PrecisionOpticBatches = precisionOpticBatches;
            PrescriptionCount = prescriptionCount;
            GritStockPermille = gritStockPermille;
        }
    }

    /// <summary>
    /// Owns the mutable batch/vision state and delegates all vitrification and
    /// grinding arithmetic to the signed pure <see cref="PrecisionGlassworksOpticsEngine"/>.
    /// It never owns the precision-optics catalog or radiation/greenhouse models.
    /// </summary>
    public sealed class GlassworksLedger
    {
        private readonly GlassworksState _state;

        public GlassworksLedger(GlassworksState? state = null)
        {
            _state = state ?? new GlassworksState();
        }

        public IReadOnlyList<GlassBatchState> Batches => _state.Batches;
        public int BatchCount => _state.Batches.Count;
        public int GritStockPermille => _state.AbrasiveGritStockPermille;

        public GlassworksState CaptureState() => _state.Clone();

        public void RestoreState(GlassworksState? saved)
        {
            if (saved == null) return;
            if (saved.SchemaVersion > _state.SchemaVersion)
                throw new InvalidOperationException(
                    $"glassworks schema {saved.SchemaVersion} is newer than supported {_state.SchemaVersion}.");

            _state.SchemaVersion = saved.SchemaVersion <= 0 ? _state.SchemaVersion : saved.SchemaVersion;
            _state.Batches = saved.Batches != null ? new List<GlassBatchState>(saved.Batches) : new List<GlassBatchState>();
            _state.VisionPrescriptions = saved.VisionPrescriptions != null
                ? new Dictionary<string, int>(saved.VisionPrescriptions, StringComparer.Ordinal)
                : new Dictionary<string, int>(StringComparer.Ordinal);
            _state.AbrasiveGritStockPermille = Math.Clamp(saved.AbrasiveGritStockPermille, 0, 1000);
        }

        /// <summary>Starts a silica batch. Returns false when the id is empty or already present.</summary>
        public bool AddBatch(string batchId, int silicaPurityPermille)
        {
            if (string.IsNullOrWhiteSpace(batchId)) return false;
            if (_state.Batches.Exists(b => string.Equals(b.BatchId, batchId, StringComparison.Ordinal))) return false;

            _state.Batches.Add(new GlassBatchState
            {
                BatchId = batchId,
                SilicaPurityPermille = Math.Clamp(silicaPurityPermille, 0, 1000),
                AnnealingStage = 0,
                ThermalShockRiskPermille = 0,
                IsCracked = false,
                ResultingTier = GlassPurityTier.Crude
            });
            return true;
        }

        public GlassBatchState? FindBatch(string batchId) =>
            string.IsNullOrWhiteSpace(batchId)
                ? null
                : _state.Batches.Find(b => string.Equals(b.BatchId, batchId, StringComparison.Ordinal));

        /// <summary>Advances one annealing stage. Returns the resulting batch, or null if unknown.</summary>
        public GlassBatchState? AdvanceAnnealing(string batchId, int kilnTemperaturePermille)
        {
            var batch = FindBatch(batchId);
            if (batch == null) return null;
            PrecisionGlassworksOpticsEngine.AdvanceAnnealingStage(batch, kilnTemperaturePermille);
            return batch;
        }

        /// <summary>
        /// Grinds a corrective lens from a batch's resulting purity, consuming
        /// abrasive stock through the canonical engine.
        /// </summary>
        public LensGrindingResult GrindCorrectionLens(string batchId, VisionCorrectionBand targetBand, int grinderSkillPermille, int grindSeed)
        {
            var batch = FindBatch(batchId);
            GlassPurityTier purity = batch?.ResultingTier ?? GlassPurityTier.Crude;
            if (batch != null && batch.IsCracked) purity = GlassPurityTier.Crude;

            var result = PrecisionGlassworksOpticsEngine.GrindCorrectionLens(
                purity, targetBand, grinderSkillPermille, _state.AbrasiveGritStockPermille, grindSeed);

            _state.AbrasiveGritStockPermille = Math.Max(0, _state.AbrasiveGritStockPermille - result.GritConsumedPermille);
            return result;
        }

        /// <summary>Calibrates a theodolite using a batch's resulting purity. Never owns survey.</summary>
        public TheodoliteCalibrationResult CalibrateTheodolite(string batchId, int calibratorSkillPermille)
        {
            var batch = FindBatch(batchId);
            GlassPurityTier purity = batch?.ResultingTier ?? GlassPurityTier.Crude;
            if (batch != null && batch.IsCracked) purity = GlassPurityTier.Crude;
            return PrecisionGlassworksOpticsEngine.CalibrateTheodolite(purity, calibratorSkillPermille);
        }

        public void SetVisionPrescription(string survivorId, VisionCorrectionBand band)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return;
            _state.VisionPrescriptions[survivorId] = (int)band;
        }

        public VisionCorrectionBand GetVisionPrescription(string survivorId) =>
            !string.IsNullOrWhiteSpace(survivorId) && _state.VisionPrescriptions.TryGetValue(survivorId, out int band)
                ? (VisionCorrectionBand)band
                : VisionCorrectionBand.NoCorrectionNeeded;

        public void AddGritStock(int permille) =>
            _state.AbrasiveGritStockPermille = Math.Clamp(_state.AbrasiveGritStockPermille + permille, 0, 1000);

        public GlassworksCensus GetCensus()
        {
            int annealed = 0, cracked = 0, precision = 0;
            foreach (var b in _state.Batches)
            {
                if (b.IsCracked) cracked++;
                if (b.AnnealingStage >= PrecisionGlassworksOpticsEngine.MaxAnnealingStages) annealed++;
                if (b.ResultingTier == GlassPurityTier.PrecisionOptic) precision++;
            }
            return new GlassworksCensus(
                _state.Batches.Count, annealed, cracked, precision,
                _state.VisionPrescriptions.Count, _state.AbrasiveGritStockPermille);
        }

        public void Clear()
        {
            _state.Batches.Clear();
            _state.VisionPrescriptions.Clear();
            _state.AbrasiveGritStockPermille = 1000;
        }
    }
}
