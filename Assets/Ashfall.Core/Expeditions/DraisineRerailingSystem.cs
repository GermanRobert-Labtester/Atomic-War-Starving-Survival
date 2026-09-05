using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Expeditions
{
    [Serializable]
    public sealed class RerailingEquipmentDefinition
    {
        public string equipment_id = string.Empty;
        public string display_name = string.Empty;
        public string required_item_id = string.Empty;
        public int required_item_amount = 1;
        public float required_power_watts;
        public int duration_days = 1;
        public float success_chance01 = 0.8f;
        public float train_condition_restored = 15f;
        public float track_integrity_restored = 0.05f;
        public bool supports_armored_draisine = true;
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public sealed class RerailingEquipmentCatalog
    {
        public int schema_version = 1;
        public List<RerailingEquipmentDefinition> equipment = new List<RerailingEquipmentDefinition>();
    }

    public enum DraisineRecoveryStatus
    {
        Idle,
        Assessing,
        Rerailing,
        Recovered,
        Failed,
        Abandoned
    }

    [Serializable]
    public sealed class DraisineRecoveryState
    {
        public const int CurrentVersion = 1;
        public int version = CurrentVersion;
        public string system_id = DraisineRerailingSystem.SystemId;
        public DraisineRecoveryStatus status = DraisineRecoveryStatus.Idle;
        public string train_id = string.Empty;
        public string segment_id = string.Empty;
        public string equipment_id = string.Empty;
        public int started_day = -1;
        public int last_tick_day = -1;
        public int duration_days;
        public int days_elapsed;
        public int attempts;
        public float track_integrity_at_start;
        public float train_condition_restored;
        public float track_integrity_restored;
        public string last_result_code = string.Empty;
    }

    public static class RerailingEquipmentCatalogLoader
    {
        public const string FileName = "rerailing_equipment_catalog.json";

        public static RerailingEquipmentCatalog Load(string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null)
        {
            if (fileIO == null || json == null || string.IsNullOrWhiteSpace(dataDir))
                return new RerailingEquipmentCatalog();
            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
            {
                log?.Warn($"[DraisineRecovery] catalog not found at {path}");
                return new RerailingEquipmentCatalog();
            }
            try
            {
                return json.Deserialize<RerailingEquipmentCatalog>(fileIO.ReadAllText(path))
                    ?? new RerailingEquipmentCatalog();
            }
            catch (Exception ex)
            {
                log?.Error($"[DraisineRecovery] failed loading catalog: {ex.Message}");
                return new RerailingEquipmentCatalog();
            }
        }
    }

    /// <summary>
    /// Recovery state machine for derailed rail vehicles. RailwaySystem owns
    /// train status, cars, and track; this system owns only the recovery job
    /// and consumes the selected recovery equipment.
    /// </summary>
    public class DraisineRerailingSystem
    {
        public const string SystemId = "draisine_recovery";

        private readonly Inventory.Inventory _inventory;
        private readonly RailwaySystem _railway;
        private readonly ISeededRng _rng;
        private readonly Func<float> _availablePowerWatts;
        private readonly ILog _log;
        private readonly Dictionary<string, RerailingEquipmentDefinition> _equipment =
            new Dictionary<string, RerailingEquipmentDefinition>(StringComparer.Ordinal);
        private DraisineRecoveryState _state = new DraisineRecoveryState();

        public DraisineRecoveryState State => _state;
        public IReadOnlyDictionary<string, RerailingEquipmentDefinition> Equipment => _equipment;
        public event Action? OnStateChanged;
        public event Action<DraisineRecoveryState>? OnRecoveryCompleted;

        public DraisineRerailingSystem(Inventory.Inventory inventory, RailwaySystem railway,
            ISeededRng? rng = null, Func<float>? availablePowerWatts = null, ILog? log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _railway = railway ?? throw new ArgumentNullException(nameof(railway));
            _rng = rng ?? new SeededRng(133);
            _availablePowerWatts = availablePowerWatts ?? (() => float.MaxValue);
            _log = log ?? NullLog.Instance;
        }

        public void LoadCatalog(RerailingEquipmentCatalog catalog)
        {
            _equipment.Clear();
            foreach (var item in catalog?.equipment ?? new List<RerailingEquipmentDefinition>())
            {
                if (item == null || string.IsNullOrEmpty(item.equipment_id)) continue;
                _equipment[item.equipment_id] = item;
            }
        }

        public RerailingEquipmentDefinition? GetEquipment(string equipmentId)
            => _equipment.TryGetValue(equipmentId ?? string.Empty, out var item) ? item : null;

        public ActionResult StartRecovery(string trainId, string equipmentId, int day)
        {
            if (_state.status == DraisineRecoveryStatus.Assessing
                || _state.status == DraisineRecoveryStatus.Rerailing)
                return ActionResult.Blocked("already_recovering", "draisine_recovery.already_recovering");
            var train = _railway.GetTrain(trainId);
            if (train == null) return ActionResult.Blocked("train_not_found", "draisine_recovery.train_not_found");
            if (train.status != TrainDispatchStatus.Derailment)
                return ActionResult.Blocked("train_not_derailed", "draisine_recovery.train_not_derailed");
            var tool = GetEquipment(equipmentId);
            if (tool == null) return ActionResult.Failed("unknown_equipment", "draisine_recovery.unknown_equipment");
            if (!tool.supports_armored_draisine)
                return ActionResult.Blocked("equipment_incompatible", "draisine_recovery.equipment_incompatible");
            if (_availablePowerWatts() < Math.Max(0f, tool.required_power_watts))
                return ActionResult.Blocked("insufficient_power", "draisine_recovery.insufficient_power");
            if (_inventory.CountById(tool.required_item_id) < Math.Max(1, tool.required_item_amount))
                return ActionResult.Blocked("missing_equipment", "draisine_recovery.missing_equipment");

            if (!_inventory.TryConsumeBill(new Dictionary<string, int>
                { [tool.required_item_id] = Math.Max(1, tool.required_item_amount) }))
                return ActionResult.Blocked("missing_equipment", "draisine_recovery.missing_equipment");

            var segment = !string.IsNullOrEmpty(train.activeSegmentId)
                ? _railway.EnsureSegmentState(train.activeSegmentId)
                : null;
            _state.status = DraisineRecoveryStatus.Rerailing;
            _state.train_id = trainId;
            _state.segment_id = train.activeSegmentId ?? string.Empty;
            _state.equipment_id = equipmentId;
            _state.started_day = day;
            _state.last_tick_day = day;
            _state.duration_days = Math.Max(1, tool.duration_days);
            _state.days_elapsed = 0;
            _state.attempts++;
            _state.track_integrity_at_start = segment?.integrity ?? 0f;
            _state.train_condition_restored = Math.Max(0f, tool.train_condition_restored);
            _state.track_integrity_restored = Math.Max(0f, tool.track_integrity_restored);
            _state.last_result_code = string.Empty;
            OnStateChanged?.Invoke();
            return ActionResult.Success("draisine_recovery.started");
        }

        public ActionResult TickDay(int day)
        {
            if (_state.status != DraisineRecoveryStatus.Rerailing)
                return ActionResult.Success("draisine_recovery.idle");
            if (_availablePowerWatts() < Math.Max(0f, GetEquipment(_state.equipment_id)?.required_power_watts ?? 0f))
                return ActionResult.Blocked("insufficient_power", "draisine_recovery.power_starved");
            _state.last_tick_day = day;
            _state.days_elapsed++;
            if (_state.days_elapsed < _state.duration_days)
            {
                OnStateChanged?.Invoke();
                return ActionResult.Success("draisine_recovery.progressed");
            }

            var tool = GetEquipment(_state.equipment_id);
            bool success = tool != null && _rng.NextDouble() <= Math.Clamp(tool.success_chance01, 0f, 1f);
            if (success && _railway.RestoreTrainAfterRecovery(
                    _state.train_id,
                    _state.train_condition_restored,
                    _state.segment_id,
                    _state.track_integrity_restored))
            {
                _state.status = DraisineRecoveryStatus.Recovered;
                _state.last_result_code = "recovered";
                _log.Info($"[DraisineRecovery] train {_state.train_id} recovered on day {day}");
                OnRecoveryCompleted?.Invoke(_state);
                OnStateChanged?.Invoke();
                return ActionResult.Success("draisine_recovery.completed");
            }

            _state.status = DraisineRecoveryStatus.Failed;
            _state.last_result_code = "rerail_failed";
            OnStateChanged?.Invoke();
            return ActionResult.Blocked("rerail_failed", "draisine_recovery.failed");
        }

        public ActionResult Abandon()
        {
            if (_state.status != DraisineRecoveryStatus.Rerailing)
                return ActionResult.Blocked("not_recovering", "draisine_recovery.not_recovering");
            _state.status = DraisineRecoveryStatus.Abandoned;
            _state.last_result_code = "abandoned";
            OnStateChanged?.Invoke();
            return ActionResult.Success("draisine_recovery.abandoned");
        }

        public DraisineRecoveryState CaptureState()
        {
            var serializer = new SystemTextJsonSerializer();
            return serializer.Deserialize<DraisineRecoveryState>(serializer.Serialize(_state))
                ?? new DraisineRecoveryState();
        }

        public void RestoreState(DraisineRecoveryState? state)
        {
            if (state == null) return;
            var serializer = new SystemTextJsonSerializer();
            _state = serializer.Deserialize<DraisineRecoveryState>(serializer.Serialize(state))
                ?? new DraisineRecoveryState();
            OnStateChanged?.Invoke();
        }
    }

    public class DraisineRecoverySystem : DraisineRerailingSystem
    {
        public DraisineRecoverySystem(Inventory.Inventory inventory, RailwaySystem railway,
            ISeededRng? rng = null, Func<float>? availablePowerWatts = null, ILog? log = null)
            : base(inventory, railway, rng, availablePowerWatts, log) { }
    }

    public class ArmoredDraisineRecoverySystem : DraisineRerailingSystem
    {
        public ArmoredDraisineRecoverySystem(Inventory.Inventory inventory, RailwaySystem railway,
            ISeededRng? rng = null, Func<float>? availablePowerWatts = null, ILog? log = null)
            : base(inventory, railway, rng, availablePowerWatts, log) { }
    }
}
