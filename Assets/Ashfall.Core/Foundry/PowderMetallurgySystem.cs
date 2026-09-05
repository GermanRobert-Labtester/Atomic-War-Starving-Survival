using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Foundry
{
    /// <summary>
    /// Abstract quality bands for compacted advanced materials. The system
    /// deliberately models production quality and reliability only; it does
    /// not contain real-world propellant, ammunition, or weapon recipes.
    /// </summary>
    [Serializable]
    public sealed class PowderMetallurgyFeedstockCost
    {
        public string item_id = string.Empty;
        public int amount = 1;
    }

    [Serializable]
    public sealed class PowderMetallurgyProcessDefinition
    {
        public string process_id = string.Empty;
        public string display_name = string.Empty;
        public List<PowderMetallurgyFeedstockCost> feedstock_costs = new List<PowderMetallurgyFeedstockCost>();
        public string output_item_id = string.Empty;
        public int output_units = 1;
        public int duration_days = 1;
        public float required_power_watts = 0f;
        public float quality_floor = 0.45f;
        public float quality_ceiling = 0.9f;
        public float wear_multiplier_at_floor = 1.15f;
        public float wear_multiplier_at_ceiling = 0.8f;
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public sealed class PowderMetallurgyCatalog
    {
        public int schema_version = 1;
        public List<PowderMetallurgyProcessDefinition> processes = new List<PowderMetallurgyProcessDefinition>();
    }

    [Serializable]
    public sealed class PowderMetallurgyBatchRecord
    {
        public string batch_id = string.Empty;
        public string process_id = string.Empty;
        public string output_item_id = string.Empty;
        public int output_units;
        public int completed_day;
        public float quality01;
        public float reliability_modifier = 1f;
        public float wear_multiplier = 1f;
    }

    [Serializable]
    public sealed class PowderMetallurgyState
    {
        public const int CurrentVersion = 1;
        public int version = CurrentVersion;
        public string system_id = PowderMetallurgySystem.SystemId;
        public bool installed = true;
        public PowderMetallurgyStatus status = PowderMetallurgyStatus.Ready;
        public string active_process_id = string.Empty;
        public string active_batch_id = string.Empty;
        public int active_day;
        public int days_required;
        public int days_elapsed;
        public int last_completed_day = -1;
        public int completed_batches;
        public int produced_units;
        public List<PowderMetallurgyBatchRecord> batches = new List<PowderMetallurgyBatchRecord>();
    }

    public enum PowderMetallurgyStatus
    {
        Offline,
        Ready,
        Processing,
        PowerStarved,
        MaintenanceRequired
    }

    [Serializable]
    public sealed class MaterialQualityModifier
    {
        public string material_id = string.Empty;
        public float quality01;
        public float reliability01;
        public float wear_multiplier = 1f;

        /// <summary>
        /// A bounded presentation/combat readiness projection. It is a generic
        /// reliability modifier, not a ballistic or manufacturing instruction.
        /// </summary>
        public float ReadinessMultiplier => Math.Clamp(reliability01, 0.5f, 1.1f);
    }

    public static class PowderMetallurgyCatalogLoader
    {
        public const string FileName = "powder_metallurgy_catalog.json";

        public static PowderMetallurgyCatalog Load(
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json,
            ILog? log = null)
        {
            if (fileIO == null || json == null || string.IsNullOrWhiteSpace(dataDir))
                return new PowderMetallurgyCatalog();

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
            {
                log?.Warn($"[PowderMetallurgy] catalog not found at {path}");
                return new PowderMetallurgyCatalog();
            }

            try
            {
                return json.Deserialize<PowderMetallurgyCatalog>(fileIO.ReadAllText(path))
                    ?? new PowderMetallurgyCatalog();
            }
            catch (Exception ex)
            {
                log?.Error($"[PowderMetallurgy] failed loading catalog: {ex.Message}");
                return new PowderMetallurgyCatalog();
            }
        }
    }

    /// <summary>
    /// Deterministic, inventory-backed production authority for abstract
    /// advanced materials.
    /// </summary>
    public class PowderMetallurgySystem
    {
        public const string SystemId = "powder_metallurgy";

        private readonly Inventory.Inventory _inventory;
        private readonly ISeededRng _rng;
        private readonly Func<float> _availablePowerWatts;
        private readonly ILog _log;
        private readonly Dictionary<string, PowderMetallurgyProcessDefinition> _processes =
            new Dictionary<string, PowderMetallurgyProcessDefinition>(StringComparer.Ordinal);
        private PowderMetallurgyState _state = new PowderMetallurgyState();

        public PowderMetallurgyState State => _state;
        public IReadOnlyDictionary<string, PowderMetallurgyProcessDefinition> Processes => _processes;

        public event Action<PowderMetallurgyState>? OnStateChanged;
        public event Action<PowderMetallurgyBatchRecord>? OnBatchCompleted;

        public PowderMetallurgySystem(
            Inventory.Inventory inventory,
            ISeededRng? rng = null,
            Func<float>? availablePowerWatts = null,
            ILog? log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _rng = rng ?? new SeededRng(130);
            _availablePowerWatts = availablePowerWatts ?? (() => float.MaxValue);
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(PowderMetallurgyCatalog catalog)
        {
            _processes.Clear();
            foreach (var process in catalog?.processes ?? new List<PowderMetallurgyProcessDefinition>())
            {
                if (process == null || string.IsNullOrEmpty(process.process_id)) continue;
                _processes[process.process_id] = process;
            }
        }

        public PowderMetallurgyProcessDefinition? GetProcess(string processId)
            => _processes.TryGetValue(processId ?? string.Empty, out var process) ? process : null;

        public ActionResult StartBatch(string processId, int day)
        {
            if (!_state.installed)
                return ActionResult.Blocked("station_not_installed", "powder_metallurgy.station_not_installed");
            if (_state.status == PowderMetallurgyStatus.Processing)
                return ActionResult.Blocked("already_processing", "powder_metallurgy.already_processing");
            if (!_processes.TryGetValue(processId ?? string.Empty, out var process))
                return ActionResult.Failed("unknown_process", "powder_metallurgy.unknown_process");
            if (_availablePowerWatts() < Math.Max(0f, process.required_power_watts))
                return ActionResult.Blocked("insufficient_power", "powder_metallurgy.insufficient_power");

            var bill = new InventoryBill();
            foreach (var cost in process.feedstock_costs ?? new List<PowderMetallurgyFeedstockCost>())
            {
                if (cost == null || cost.amount <= 0) continue;
                bill.AddCost(cost.item_id, cost.amount);
            }

            if (!_inventory.TryExecuteTransaction(bill))
                return ActionResult.Blocked("insufficient_feedstock", "powder_metallurgy.insufficient_feedstock");

            _state.active_process_id = process.process_id;
            _state.active_batch_id = $"pm_{Math.Max(0, day)}_{_state.completed_batches + _state.days_elapsed + 1}";
            _state.active_day = day;
            _state.days_required = Math.Max(1, process.duration_days);
            _state.days_elapsed = 0;
            _state.status = PowderMetallurgyStatus.Processing;
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success("powder_metallurgy.batch_started");
        }

        public ActionResult TickDay(int day)
        {
            if (_state.status != PowderMetallurgyStatus.Processing)
                return ActionResult.Success("powder_metallurgy.idle");

            var process = GetProcess(_state.active_process_id);
            if (process == null)
            {
                _state.status = PowderMetallurgyStatus.MaintenanceRequired;
                OnStateChanged?.Invoke(_state);
                return ActionResult.Failed("invalid_process_state", "powder_metallurgy.invalid_process_state");
            }

            if (_availablePowerWatts() < Math.Max(0f, process.required_power_watts))
            {
                _state.status = PowderMetallurgyStatus.PowerStarved;
                OnStateChanged?.Invoke(_state);
                return ActionResult.Blocked("insufficient_power", "powder_metallurgy.power_starved");
            }

            _state.status = PowderMetallurgyStatus.Processing;
            _state.days_elapsed++;
            if (_state.days_elapsed < _state.days_required)
            {
                OnStateChanged?.Invoke(_state);
                return ActionResult.Success("powder_metallurgy.progressed");
            }

            float floor = Math.Clamp(Math.Min(process.quality_floor, process.quality_ceiling), 0f, 1f);
            float ceiling = Math.Clamp(Math.Max(process.quality_floor, process.quality_ceiling), floor, 1f);
            float quality = floor + _rng.NextFloat() * (ceiling - floor);
            float wearFloor = Math.Max(0.5f, process.wear_multiplier_at_floor);
            float wearCeiling = Math.Max(0.5f, process.wear_multiplier_at_ceiling);
            float wear = wearFloor + ((quality - floor) / Math.Max(0.001f, ceiling - floor))
                * (wearCeiling - wearFloor);

            int amount = Math.Max(1, process.output_units);
            if (!_inventory.TryProduce(process.output_item_id, amount))
            {
                _state.status = PowderMetallurgyStatus.MaintenanceRequired;
                OnStateChanged?.Invoke(_state);
                return ActionResult.Blocked("storage_full", "powder_metallurgy.storage_full");
            }

            var record = new PowderMetallurgyBatchRecord
            {
                batch_id = _state.active_batch_id,
                process_id = process.process_id,
                output_item_id = process.output_item_id,
                output_units = amount,
                completed_day = day,
                quality01 = quality,
                reliability_modifier = Math.Clamp(0.8f + quality * 0.3f, 0.5f, 1.1f),
                wear_multiplier = Math.Clamp(wear, 0.5f, 1.5f)
            };
            _state.batches.Add(record);
            _state.completed_batches++;
            _state.produced_units += amount;
            _state.last_completed_day = day;
            _state.active_process_id = string.Empty;
            _state.active_batch_id = string.Empty;
            _state.days_elapsed = 0;
            _state.days_required = 0;
            _state.status = PowderMetallurgyStatus.Ready;
            _log.Info($"[PowderMetallurgy] completed {record.process_id} at quality {quality:0.00}");
            OnBatchCompleted?.Invoke(record);
            OnStateChanged?.Invoke(_state);
            return ActionResult.Success(
                "powder_metallurgy.batch_completed",
                new Dictionary<string, double>
                {
                    ["quality01"] = quality,
                    ["output_units"] = amount
                });
        }

        public bool TryGetLatestModifier(string outputItemId, out MaterialQualityModifier modifier)
        {
            modifier = new MaterialQualityModifier { material_id = outputItemId ?? string.Empty };
            if (string.IsNullOrEmpty(outputItemId)) return false;
            var record = _state.batches
                .Where(b => b != null && b.output_item_id == outputItemId)
                .OrderByDescending(b => b.completed_day)
                .ThenByDescending(b => b.batch_id, StringComparer.Ordinal)
                .FirstOrDefault();
            if (record == null) return false;
            modifier.quality01 = record.quality01;
            modifier.reliability01 = record.reliability_modifier;
            modifier.wear_multiplier = record.wear_multiplier;
            return true;
        }

        public PowderMetallurgyState CaptureState()
        {
            var serializer = new SystemTextJsonSerializer();
            return serializer.Deserialize<PowderMetallurgyState>(serializer.Serialize(_state))
                ?? new PowderMetallurgyState();
        }

        public void RestoreState(PowderMetallurgyState? state)
        {
            if (state == null) return;
            var serializer = new SystemTextJsonSerializer();
            _state = serializer.Deserialize<PowderMetallurgyState>(serializer.Serialize(state))
                ?? new PowderMetallurgyState();
            _state.batches ??= new List<PowderMetallurgyBatchRecord>();
            OnStateChanged?.Invoke(_state);
        }
    }

    // Compatibility names used by early Plan 130 notes.
    public class PowderMetallurgyEngine : PowderMetallurgySystem
    {
        public PowderMetallurgyEngine(Inventory.Inventory inventory, ISeededRng? rng = null,
            Func<float>? availablePowerWatts = null, ILog? log = null)
            : base(inventory, rng, availablePowerWatts, log) { }
    }
}
