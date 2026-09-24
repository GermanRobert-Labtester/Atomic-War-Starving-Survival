// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : KilnworksSaveStore
// Core State : Ashfall.Core.Shelter.KilnFiringState
// Host Caller: Main.Kilnworks
// Purpose    : Expansion 31 — queued kiln batches, the kiln fuel reserve, lining
//              wear, and drawn-output tallies. Metallurgy stays with
//              CupolaFoundryEngine; shelter upgrades stay with the canonical
//              infrastructure owners.
// ============================================================================

using Ashfall.Core.Shelter;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class KilnworksSaveStore
    {
        public const string FileName = "kilnworks_save.json";
        public const string SectionName = "kilnworks";

        private static readonly SaveStore<KilnFiringState> s_store =
            SaveStoreHub.Checksummed<KilnFiringState>(FileName, nameof(KilnworksSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(KilnFiringState state) => s_store.CaptureBare(state);
        public static KilnFiringState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(KilnFiringState state) => s_store.TrySave(state);
        public static KilnFiringState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Expansion 31 host session. Wraps the stateful <see cref="KilnFiringLedger"/>
    /// over the signed pure <see cref="KilnFiringEngine"/>. Firing is deterministic:
    /// every stage advance charges the kiln's own fuel reserve and accrues lining
    /// wear through the engine. No metallurgy or upgrade decisions live here.
    /// </summary>
    public sealed class KilnworksHostSession : HostSessionBase
    {
        private readonly KilnFiringLedger _ledger;

        public KilnFiringLedger Ledger => _ledger;
        public KilnFiringCensus Census => _ledger.GetCensus();
        public int BatchCount => _ledger.BatchCount;
        public string LastEvent { get; private set; } = string.Empty;

        public KilnworksHostSession(KilnFiringState? state = null)
        {
            _ledger = new KilnFiringLedger(state);
        }

        public static KilnworksHostSession Create(KilnFiringState? state = null) =>
            new KilnworksHostSession(state);

        /// <summary>Queues a batch for firing. Returns false for an empty or duplicate id.</summary>
        public bool AddBatch(string batchId, KilnLoadKind loadKind, int rawMaterialQualityPermille)
        {
            bool added = _ledger.AddBatch(batchId, loadKind, rawMaterialQualityPermille);
            if (added)
            {
                LastEvent = $"Queued kiln batch '{batchId}' ({loadKind}).";
                RaiseStateChanged();
            }
            return added;
        }

        /// <summary>Advances one firing stage, charging fuel and accruing lining wear on draw.</summary>
        public KilnBatchState? AdvanceFiring(string batchId, int kilnTemperaturePermille = KilnFiringLedger.OptimalFiringTemperaturePermille)
        {
            var batch = _ledger.AdvanceFiring(batchId, kilnTemperaturePermille);
            if (batch == null) return null;

            LastEvent = batch.IsWaster
                ? $"Batch '{batch.BatchId}' drew as a waster."
                : $"Batch '{batch.BatchId}' at stage {batch.FiringStage}/{KilnFiringEngine.MaxFiringStages}" +
                  (batch.FiringStage >= KilnFiringEngine.MaxFiringStages
                      ? $" — draw grade {batch.ResultingGrade}."
                      : ".");
            RaiseStateChanged();
            return batch;
        }

        /// <summary>Advances the oldest active batch one stage (the daily kiln owner path).</summary>
        public KilnBatchState? AdvanceOldestBatch(int kilnTemperaturePermille = KilnFiringLedger.OptimalFiringTemperaturePermille)
        {
            var oldest = _ledger.FindOldestActiveBatch();
            return oldest == null ? null : AdvanceFiring(oldest.BatchId, kilnTemperaturePermille);
        }

        /// <summary>Runs a lime calcination pass and tallies the quicklime yield.</summary>
        public LimeCalcinationResult CalcinateLimestone(
            int limestoneKg,
            int kilnTemperaturePermille = KilnFiringLedger.OptimalFiringTemperaturePermille,
            int soakHours = 24)
        {
            var result = _ledger.CalcinateLimestone(limestoneKg, kilnTemperaturePermille, soakHours);
            LastEvent = result.IsFullyCalcined
                ? $"Calcination yielded {result.QuicklimeYieldKg} kg quicklime (fully calcined)."
                : $"Calcination incomplete: {result.ResidualCarbonatePermille}‰ residual carbonate, " +
                  $"{result.QuicklimeYieldKg} kg yield.";
            RaiseStateChanged();
            return result;
        }

        public void Refuel(int fuelPermille)
        {
            _ledger.Refuel(fuelPermille);
            LastEvent = $"Kiln fuel reserve now {_ledger.FuelReservePermille}‰.";
            RaiseStateChanged();
        }

        public void Reline(int relinePermille = 1000)
        {
            _ledger.Reline(relinePermille);
            LastEvent = $"Kiln lining relined; wear now {_ledger.LiningWearPermille}‰.";
            RaiseStateChanged();
        }

        public KilnBatchState? FindBatch(string batchId) => _ledger.FindBatch(batchId);

        public KilnFiringState CaptureState() => _ledger.CaptureState();
        public void RestoreState(KilnFiringState? state) => _ledger.RestoreState(state);
        public void Clear() => _ledger.Clear();
    }
}
