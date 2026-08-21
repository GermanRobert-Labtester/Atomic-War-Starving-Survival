using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.Crafting;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class EquipmentConditionState
    {
        public string systemId = EquipmentConditionSystem.SystemId;
        public List<EquipmentInstance> items = new List<EquipmentInstance>();
        public List<MaintenanceJob> pendingJobs = new List<MaintenanceJob>();
    }

    [Serializable]
    public sealed class EquipmentInstance
    {
        public string instanceId = string.Empty;
        public string itemId = string.Empty;
        public string ownerId = string.Empty;
        public float condition = 100f;
        public float maxCondition = 100f;
        public EquipmentFamily family;
        public string material = string.Empty;
        public int usesRemaining = -1;      // -1 = unlimited
        public float lastMaintainedDay = -1;
        public List<string> maintenanceHistory = new List<string>();
    }

    public enum EquipmentFamily { Tool, Weapon, Medical, Clothing, Electronics, Container }

    [Serializable]
    public sealed class MaintenanceJob
    {
        public string jobId = string.Empty;
        public string instanceId = string.Empty;
        public string stationId = string.Empty;
        public MaintenanceType type;
        public float progress;
        public float totalRequired = 1f;
        public bool isComplete;
        public List<string> reservedParts = new List<string>();
    }

    public enum MaintenanceType { Sharpen, Repair, Calibrate, Clean, ReplacePart }

    public sealed class EquipmentConditionSystem
    {
        public const string SystemId = "equipment_condition";
        private EquipmentConditionState _state = new EquipmentConditionState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly Inventory.Inventory _inventory;
        private readonly CraftingSystem _crafting;
        private int _currentDay;

        public EquipmentConditionState State => _state;
        public event Action<EquipmentInstance> OnConditionChanged;
        public event Action<MaintenanceJob> OnMaintenanceCompleted;
        public event Action OnEquipmentChanged;

        public EquipmentConditionSystem(
            ISeededRng rng,
            Inventory.Inventory inventory,
            CraftingSystem crafting,
            ILog log = null!)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _crafting = crafting ?? throw new ArgumentNullException(nameof(crafting));
            _log = log ?? NullLog.Instance;
        }

        public ActionResult RegisterItem(string instanceId, string itemId, string ownerId, EquipmentFamily family, float maxCondition = 100f)
        {
            if (_state.items.Exists(i => i.instanceId == instanceId))
                return ActionResult.Blocked("item_exists", "equip.item_exists");

            _state.items.Add(new EquipmentInstance
            {
                instanceId = instanceId, itemId = itemId, ownerId = ownerId,
                family = family, condition = maxCondition, maxCondition = maxCondition
            });
            OnEquipmentChanged?.Invoke();
            return ActionResult.Success("equip.item_registered");
        }

        public ActionResult UseItem(string instanceId, float wearAmount = 1f)
        {
            var item = _state.items.Find(i => i.instanceId == instanceId);
            if (item == null) return ActionResult.Failed("unknown_item", "equip.unknown_item");

            item.condition = Math.Max(0, item.condition - wearAmount);
            if (item.usesRemaining > 0)
                item.usesRemaining--;

            OnConditionChanged?.Invoke(item);
            return ActionResult.Success("equip.item_used",
                new Dictionary<string, double> { { "condition", item.condition } });
        }

        public ActionResult StartMaintenance(string instanceId, string stationId, MaintenanceType type, List<string> requiredParts)
        {
            var item = _state.items.Find(i => i.instanceId == instanceId);
            if (item == null) return ActionResult.Failed("unknown_item", "equip.unknown_item");

            // CR3-03: was a single-pass loop that called _inventory.RemoveById
            // before checking the next iteration's CountById. Make this atomic:
            // pre-check every required part's availability first; only consume
            // when every required part resolves. Earlier parts are not drained
            // when a later part is missing.
            if (requiredParts != null)
            {
                foreach (var part in requiredParts)
                {
                    if (_inventory.CountById(part) < 1)
                        return ActionResult.Blocked("missing_part", "equip.missing_part");
                }
            }

            var reserved = new List<string>();
            if (requiredParts != null)
            {
                foreach (var part in requiredParts)
                {
                    _inventory.RemoveById(part, 1);
                    reserved.Add(part);
                }
            }

            var job = new MaintenanceJob
            {
                jobId = $"maint_{_currentDay}_{instanceId}_{type}",
                instanceId = instanceId, stationId = stationId, type = type,
                reservedParts = reserved
            };
            _state.pendingJobs.Add(job);
            OnEquipmentChanged?.Invoke();
            return ActionResult.Success("equip.maintenance_started");
        }

        public void TickDay(int day)
        {
            _currentDay = day;

            foreach (var job in _state.pendingJobs)
            {
                if (job.isComplete) continue;
                job.progress += 1f; // 1 day of work
                if (job.progress >= job.totalRequired)
                {
                    job.isComplete = true;
                    var item = _state.items.Find(i => i.instanceId == job.instanceId);
                    if (item != null)
                    {
                        item.condition = Math.Min(item.maxCondition, item.condition + 20f);
                        item.lastMaintainedDay = day;
                        item.maintenanceHistory.Add($"{job.type} on day {day}");
                    }
                    _log.Info($"[Equipment] maintenance complete: {job.jobId}");
                    OnMaintenanceCompleted?.Invoke(job);
                }
            }
        }

        public float GetSlipRisk(string instanceId)
        {
            var item = _state.items.Find(i => i.instanceId == instanceId);
            if (item == null) return 0f;
            return item.condition < 30f ? (30f - item.condition) / 30f : 0f;
        }

        public float GetJamRisk(string instanceId)
        {
            var item = _state.items.Find(i => i.instanceId == instanceId);
            if (item == null) return 0f;
            return item.condition < 20f ? (20f - item.condition) / 20f : 0f;
        }

        public bool IsUsable(string instanceId)
        {
            var item = _state.items.Find(i => i.instanceId == instanceId);
            if (item == null) return false;
            return item.condition > 0 && (item.usesRemaining == -1 || item.usesRemaining > 0);
        }

        public EquipmentConditionState CaptureState() => _state;
        public void RestoreState(EquipmentConditionState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnEquipmentChanged?.Invoke();
        }
    }
}
