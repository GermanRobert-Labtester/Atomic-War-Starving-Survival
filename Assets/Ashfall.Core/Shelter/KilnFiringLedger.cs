// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core : Expansion 31 — The Kiln
// Subsystem    : Kiln Firing Ledger (stateful owner over the signed pure
//                KilnFiringEngine)
// ============================================================================
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// Persisted kiln state: queued batches, the kiln's fuel reserve, lining wear,
    /// and the tallies of what has been drawn. This is the kiln's own layer only;
    /// metallurgy stays with <c>CupolaFoundryEngine</c> and shelter upgrades stay
    /// with the canonical infrastructure owners.
    /// </summary>
    [Serializable]
    public sealed class KilnFiringState
    {
        public int SchemaVersion { get; set; } = 1;
        public List<KilnBatchState> Batches { get; set; } = new List<KilnBatchState>();
        /// <summary>Kiln fuel reserve (0..1000 permille). No canonical fuel authority exists.</summary>
        public int FuelReservePermille { get; set; } = 1000;
        /// <summary>Accumulated refractory lining wear (0..1000 permille).</summary>
        public int LiningWearPermille { get; set; }
        public int CumulativeQuicklimeKg { get; set; }
        public int DrawnPottery { get; set; }
        public int DrawnBricks { get; set; }
        public int DrawnTiles { get; set; }
        public int DrawnRefractoryTile { get; set; }
        public int WasterBatches { get; set; }
        public int CalcinationRuns { get; set; }

        public KilnFiringState Clone() => new KilnFiringState
        {
            SchemaVersion = SchemaVersion,
            Batches = Batches != null
                ? Batches.ConvertAll(b => b?.Clone() ?? new KilnBatchState())
                : new List<KilnBatchState>(),
            FuelReservePermille = FuelReservePermille,
            LiningWearPermille = LiningWearPermille,
            CumulativeQuicklimeKg = CumulativeQuicklimeKg,
            DrawnPottery = DrawnPottery,
            DrawnBricks = DrawnBricks,
            DrawnTiles = DrawnTiles,
            DrawnRefractoryTile = DrawnRefractoryTile,
            WasterBatches = WasterBatches,
            CalcinationRuns = CalcinationRuns
        };
    }

    /// <summary>Bounded read model of the kiln ledger.</summary>
    public struct KilnFiringCensus
    {
        public int BatchCount { get; }
        public int ActiveBatches { get; }
        public int DrawReadyBatches { get; }
        public int WasterBatches { get; }
        public int FuelReservePermille { get; }
        public int LiningWearPermille { get; }
        public bool IsLiningReplacementDue { get; }
        public int CumulativeQuicklimeKg { get; }
        public int DrawnRefractoryTile { get; }

        public KilnFiringCensus(
            int batchCount, int activeBatches, int drawReadyBatches, int wasterBatches,
            int fuelReservePermille, int liningWearPermille, bool isLiningReplacementDue,
            int cumulativeQuicklimeKg, int drawnRefractoryTile)
        {
            BatchCount = batchCount;
            ActiveBatches = activeBatches;
            DrawReadyBatches = drawReadyBatches;
            WasterBatches = wasterBatches;
            FuelReservePermille = fuelReservePermille;
            LiningWearPermille = liningWearPermille;
            IsLiningReplacementDue = isLiningReplacementDue;
            CumulativeQuicklimeKg = cumulativeQuicklimeKg;
            DrawnRefractoryTile = drawnRefractoryTile;
        }
    }

    /// <summary>
    /// Owns the mutable kiln state and delegates every firing, calcination, and
    /// lining calculation to the signed pure <see cref="KilnFiringEngine"/>.
    /// It never owns metallurgy, shelter upgrades, or the ceramics narrative catalogs.
    /// </summary>
    public sealed class KilnFiringLedger
    {
        /// <summary>
        /// Deterministic daily firing temperature. Chosen below the engine's thermal
        /// shock threshold (800) and high enough for strong heat work.
        /// </summary>
        public const int OptimalFiringTemperaturePermille = 780;
        /// <summary>Fuel charged per firing stage, drawn from the kiln's own reserve.</summary>
        public const int FuelCostPerStagePermille = 60;
        /// <summary>Accumulated lining wear at which a reline is due.</summary>
        public const int LiningReplacementThresholdPermille = 800;
        /// <summary>Upper bound on tracked batches, so a long campaign cannot grow without limit.</summary>
        public const int MaxTrackedBatches = 100;

        private readonly KilnFiringState _state;

        public KilnFiringLedger(KilnFiringState? state = null)
        {
            _state = state ?? new KilnFiringState();
            if (_state.Batches == null) _state.Batches = new List<KilnBatchState>();
        }

        public IReadOnlyList<KilnBatchState> Batches => _state.Batches;
        public int BatchCount => _state.Batches.Count;
        public int FuelReservePermille => _state.FuelReservePermille;
        public int LiningWearPermille => _state.LiningWearPermille;

        public KilnFiringState CaptureState() => _state.Clone();

        public void RestoreState(KilnFiringState? saved)
        {
            if (saved == null) return;
            if (saved.SchemaVersion > _state.SchemaVersion)
                throw new InvalidOperationException(
                    $"kilnworks schema {saved.SchemaVersion} is newer than supported {_state.SchemaVersion}.");

            _state.SchemaVersion = saved.SchemaVersion <= 0 ? _state.SchemaVersion : saved.SchemaVersion;
            _state.Batches = saved.Batches != null
                ? saved.Batches.ConvertAll(b => b?.Clone() ?? new KilnBatchState())
                : new List<KilnBatchState>();
            _state.FuelReservePermille = Math.Clamp(saved.FuelReservePermille, 0, 1000);
            _state.LiningWearPermille = Math.Clamp(saved.LiningWearPermille, 0, 1000);
            _state.CumulativeQuicklimeKg = Math.Max(0, saved.CumulativeQuicklimeKg);
            _state.DrawnPottery = Math.Max(0, saved.DrawnPottery);
            _state.DrawnBricks = Math.Max(0, saved.DrawnBricks);
            _state.DrawnTiles = Math.Max(0, saved.DrawnTiles);
            _state.DrawnRefractoryTile = Math.Max(0, saved.DrawnRefractoryTile);
            _state.WasterBatches = Math.Max(0, saved.WasterBatches);
            _state.CalcinationRuns = Math.Max(0, saved.CalcinationRuns);
        }

        /// <summary>Queues a batch. Returns false when the id is empty or already tracked.</summary>
        public bool AddBatch(string batchId, KilnLoadKind loadKind, int rawMaterialQualityPermille)
        {
            if (string.IsNullOrWhiteSpace(batchId)) return false;
            if (_state.Batches.Exists(b => string.Equals(b.BatchId, batchId, StringComparison.Ordinal))) return false;

            if (_state.Batches.Count >= MaxTrackedBatches)
                _state.Batches.RemoveRange(0, _state.Batches.Count - (MaxTrackedBatches - 1));

            _state.Batches.Add(new KilnBatchState
            {
                BatchId = batchId.Trim(),
                LoadKind = loadKind,
                RawMaterialQualityPermille = Math.Clamp(rawMaterialQualityPermille, 0, 1000),
                FiringStage = 0,
                HeatWorkPermille = 0,
                IsWaster = false,
                ResultingGrade = DrawGrade.SubStandard
            });
            return true;
        }

        public KilnBatchState? FindBatch(string batchId) =>
            string.IsNullOrWhiteSpace(batchId)
                ? null
                : _state.Batches.Find(b => string.Equals(b.BatchId, batchId, StringComparison.Ordinal));

        /// <summary>The oldest batch that is neither a waster nor draw-ready, or null when the kiln is idle.</summary>
        public KilnBatchState? FindOldestActiveBatch() =>
            _state.Batches.Find(b => !b.IsWaster && b.FiringStage < KilnFiringEngine.MaxFiringStages);

        /// <summary>
        /// Advances one firing stage of a batch, charging the kiln's fuel reserve and
        /// accruing lining wear when the batch reaches draw. Returns null for unknown
        /// batches; returns the batch unchanged when it cannot advance (waster,
        /// draw-ready, or insufficient fuel).
        /// </summary>
        public KilnBatchState? AdvanceFiring(string batchId, int kilnTemperaturePermille = OptimalFiringTemperaturePermille)
        {
            var batch = FindBatch(batchId);
            if (batch == null) return null;
            if (batch.IsWaster || batch.FiringStage >= KilnFiringEngine.MaxFiringStages) return batch;
            if (_state.FuelReservePermille < KilnFiringEngine.MinimumFuelPermille) return batch;

            int temperature = Math.Clamp(kilnTemperaturePermille, 0, 1000);
            int previousStage = batch.FiringStage;

            KilnFiringEngine.AdvanceFiringStage(batch, temperature, _state.FuelReservePermille);

            // A thermal-shocked batch burned its fuel too.
            _state.FuelReservePermille = Math.Max(
                0, _state.FuelReservePermille - FuelCostPerStagePermille);

            if (batch.IsWaster && !batch.ResultingGrade.Equals(DrawGrade.Waster))
                batch.ResultingGrade = DrawGrade.Waster;

            bool newlyDrawn = previousStage < KilnFiringEngine.MaxFiringStages
                && batch.FiringStage >= KilnFiringEngine.MaxFiringStages;

            if (batch.IsWaster)
            {
                _state.WasterBatches++;
            }
            else if (newlyDrawn)
            {
                AccrueLiningWear(batch.LoadKind, temperature);
                TallyDraw(batch.LoadKind);
            }

            return batch;
        }

        /// <summary>
        /// Runs a lime calcination pass, charging the engine's computed fuel draw
        /// against the kiln reserve and tallying the quicklime yield.
        /// </summary>
        public LimeCalcinationResult CalcinateLimestone(
            int limestoneKg,
            int kilnTemperaturePermille = OptimalFiringTemperaturePermille,
            int soakHours = 24)
        {
            var result = KilnFiringEngine.CalcinateLimestone(
                limestoneKg, kilnTemperaturePermille, soakHours, _state.FuelReservePermille);

            _state.FuelReservePermille = Math.Max(0, _state.FuelReservePermille - result.FuelConsumedPermille);
            _state.CumulativeQuicklimeKg += result.QuicklimeYieldKg;
            _state.CalcinationRuns++;
            AccrueLiningWear(KilnLoadKind.LimestoneCalc, kilnTemperaturePermille);
            return result;
        }

        /// <summary>Adds fuel to the kiln reserve from shelter stores.</summary>
        public void Refuel(int fuelPermille) =>
            _state.FuelReservePermille = Math.Clamp(_state.FuelReservePermille + Math.Max(0, fuelPermille), 0, 1000);

        /// <summary>Replaces the refractory lining, clearing accrued wear.</summary>
        public void Reline(int relinePermille = 1000) =>
            _state.LiningWearPermille = Math.Clamp(
                _state.LiningWearPermille - Math.Clamp(relinePermille, 0, 1000), 0, 1000);

        private void AccrueLiningWear(KilnLoadKind loadKind, int kilnTemperaturePermille) =>
            _state.LiningWearPermille = Math.Clamp(
                _state.LiningWearPermille
                    + KilnFiringEngine.CalculateRefractoryLiningWear(loadKind, kilnTemperaturePermille),
                0, 1000);

        private void TallyDraw(KilnLoadKind loadKind)
        {
            switch (loadKind)
            {
                case KilnLoadKind.ClayPottery: _state.DrawnPottery++; break;
                case KilnLoadKind.FiredBrick: _state.DrawnBricks++; break;
                case KilnLoadKind.RefractoryTile: _state.DrawnRefractoryTile++; break;
                case KilnLoadKind.CeramicTile: _state.DrawnTiles++; break;
                // Limestone is calcined directly; a fired limestone batch yields no counted ware.
            }
        }

        public KilnFiringCensus GetCensus()
        {
            int active = 0, drawReady = 0, waster = 0;
            foreach (var b in _state.Batches)
            {
                if (b.IsWaster) waster++;
                else if (b.FiringStage >= KilnFiringEngine.MaxFiringStages) drawReady++;
                else active++;
            }

            return new KilnFiringCensus(
                _state.Batches.Count, active, drawReady, waster,
                _state.FuelReservePermille, _state.LiningWearPermille,
                _state.LiningWearPermille >= LiningReplacementThresholdPermille,
                _state.CumulativeQuicklimeKg, _state.DrawnRefractoryTile);
        }

        public void Clear()
        {
            _state.Batches.Clear();
            _state.FuelReservePermille = 1000;
            _state.LiningWearPermille = 0;
            _state.CumulativeQuicklimeKg = 0;
            _state.DrawnPottery = 0;
            _state.DrawnBricks = 0;
            _state.DrawnTiles = 0;
            _state.DrawnRefractoryTile = 0;
            _state.WasterBatches = 0;
            _state.CalcinationRuns = 0;
        }
    }
}
