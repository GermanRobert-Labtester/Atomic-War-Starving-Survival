// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : PharmaceuticalTabletSaveStore
// Core State : Ashfall.Core.Medical.TabletWorksFullState
// Host Caller: Main.PharmaceuticalTablet
// Purpose    : PLAN-PHARMACEUTICAL-TRUTH-167 — tablet production state under
//              its own checksummed save key. Reagents/tablets stay canonical
//              inventory items (Plan 93); this owner persists press condition,
//              the active batch, and the unclaimed output buffer.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class PharmaceuticalTabletSaveStore
    {
        public const string FileName = "pharmaceutical_tablet_save.json";
        public const string SectionName = "pharmaceutical_tablet";

        private static readonly SaveStore<TabletWorksFullState> s_store =
            SaveStoreHub.Checksummed<TabletWorksFullState>(FileName, nameof(PharmaceuticalTabletSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();
        public static string TryCapturePersisted(TabletWorksFullState state) => s_store.CaptureBare(state);
        public static TabletWorksFullState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(TabletWorksFullState state) => s_store.TrySave(state);
        public static TabletWorksFullState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>Read-only tablet-works projection.</summary>
    public sealed class PharmaceuticalTabletSnapshot
    {
        public bool Constructed { get; set; }
        public string MachineState { get; set; } = string.Empty;
        public float MachineCondition { get; set; }
        public float ToolingCondition { get; set; }
        public float CalibrationState { get; set; }
        public string ActiveBatchId { get; set; } = string.Empty;
        public int ActiveBatchProgress { get; set; }
        public int ActiveBatchTotalDays { get; set; }
        public int BufferedBatches { get; set; }
        public int FormulationCount { get; set; }
    }

    /// <summary>
    /// PLAN-PHARMACEUTICAL-TRUTH-167 host session. Wraps the Core
    /// <see cref="PharmaceuticalTabletEngine"/>: constructs the press, stages
    /// batches from authored formulations, advances the deterministic daily
    /// process, and claims finished outputs into canonical inventory.
    /// </summary>
    public sealed class PharmaceuticalTabletHostSession : HostSessionBase
    {
        private readonly PharmaceuticalTabletEngine _engine;
        private int _formulationCount;

        public PharmaceuticalTabletEngine Engine => _engine;
        public int FormulationCount => _formulationCount;
        public string LastEvent { get; private set; } = string.Empty;

        public PharmaceuticalTabletHostSession(ISeededRng? rng = null)
        {
            _engine = new PharmaceuticalTabletEngine(rng);
            _engine.OnBatchResolved += (result, accepted) =>
            {
                LastEvent = accepted
                    ? $"Tablet batch produced: {result.result_item_id} x{result.quantity} ({result.quality_grade})."
                    : $"Tablet batch rejected: {result.batch_id}.";
                RaiseStateChanged();
            };
            _engine.OnEventRaised += message =>
            {
                LastEvent = message;
                RaiseStateChanged();
            };
        }

        public void LoadCatalog(string dataDir)
        {
            if (string.IsNullOrWhiteSpace(dataDir)) return;
            string path = Path.Combine(dataDir, "tablet_manufacturing_catalog.json");
            if (!File.Exists(path)) return;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var catalog = JsonSerializer.Deserialize<TabletManufacturingCatalog>(File.ReadAllText(path), options);
                if (catalog != null)
                {
                    _engine.BindCatalog(catalog);
                    _formulationCount = catalog.formulations?.Count ?? 0;
                }
            }
            catch (Exception)
            {
                _formulationCount = 0;
            }
        }

        public void BindInventory(
            Func<string, int> getCount,
            Func<string, int, bool> canAdd,
            Action<string, int> addItem,
            Action<string, int> consume)
            => _engine.BindInventory(getCount, canAdd, addItem, consume);

        public ActionResult ConstructPress() { var r = _engine.ConstructPress(); RaiseStateChanged(); return r; }
        public ActionResult MaintainPress() { var r = _engine.MaintainPress(); RaiseStateChanged(); return r; }
        public ActionResult ReplaceTooling() { var r = _engine.ReplaceTooling(); RaiseStateChanged(); return r; }
        public ActionResult Recalibrate() { var r = _engine.Recalibrate(); RaiseStateChanged(); return r; }
        public ActionResult StageBatch(string formulationId) { var r = _engine.StageBatch(formulationId); RaiseStateChanged(); return r; }

        public void TickDay(int day) { _engine.TickDay(day); RaiseStateChanged(); }

        public ClaimedTabletOutputs? ClaimOutputs()
        {
            var claimed = _engine.ClaimOutputs();
            if (claimed != null)
            {
                LastEvent = $"Claimed {claimed.total_units} tablet units across {claimed.batches_claimed} batches.";
                RaiseStateChanged();
            }
            return claimed;
        }

        public PharmaceuticalTabletSnapshot GetSnapshot()
        {
            var snapshot = new PharmaceuticalTabletSnapshot
            {
                Constructed = _engine.IsConstructed,
                MachineState = _engine.State.machine_state,
                MachineCondition = _engine.State.machine_condition,
                ToolingCondition = _engine.State.tooling_condition,
                CalibrationState = _engine.State.calibration_state,
                ActiveBatchId = _engine.ActiveBatch?.batch_id ?? string.Empty,
                ActiveBatchProgress = _engine.ActiveBatch?.progress_days ?? 0,
                ActiveBatchTotalDays = _engine.ActiveBatch?.total_process_days ?? 0,
                BufferedBatches = _engine.OutputBuffer.Count,
                FormulationCount = _formulationCount
            };
            return snapshot;
        }

        public TabletWorksFullState CaptureState() => _engine.CaptureFullState();

        public void RestoreState(TabletWorksFullState? state)
        {
            _engine.RestoreFullState(state);
            LastEvent = "Tablet works state restored.";
            RaiseStateChanged();
        }
    }
}
