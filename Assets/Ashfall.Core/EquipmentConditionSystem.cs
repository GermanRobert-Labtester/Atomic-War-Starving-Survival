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
        public float originalMaxCondition = 100f;
        public float rustLevel = 0f;
        public float repairQuality = 1f;
        public bool temporaryPatch = false;
        public bool isBroken = false;
        public bool isJammed = false;
    }

    public enum EquipmentFamily { Tool, Weapon, Medical, Clothing, Electronics, Container, Watercraft }

    [Serializable]
    public sealed class WearEvent
    {
        public string source = string.Empty;
        public float intensity = 1f;
        public float environmentModifier = 1f;
        public string actionId = string.Empty;
    }

    [Serializable]
    public sealed class DegradationProfileDef
    {
        public string profile_id = string.Empty;
        public string item_family = string.Empty;
        public string display_name = string.Empty;
        public float base_wear_per_use = 1f;
        public float base_wear_per_day_exposed = 0.5f;
        public float corrosion_susceptibility = 1f;
        public float cold_brittleness = 1f;
        public float heat_sensitivity = 1f;
        public float jam_threshold = 20f;
        public float break_threshold = 5f;
        public float max_durability_loss_per_repair = 5f;
        public List<string> maintenance_materials = new List<string>();
        public float repair_efficiency = 0.8f;
        public bool jury_rig_allowed = true;
        public List<string> tags = new List<string>();
    }

    [Serializable]
    public sealed class ItemDegradationCatalog
    {
        public int schema_version = 1;
        public List<DegradationProfileDef> profiles = new List<DegradationProfileDef>();
    }

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
        public event Action<EquipmentInstance>? OnItemConditionChanged;
        public event Action<EquipmentInstance>? OnItemJammed;
        public event Action<EquipmentInstance>? OnItemBroken;
        public event Action<EquipmentInstance>? OnItemRepaired;

        private readonly Dictionary<string, DegradationProfileDef> _profiles = new Dictionary<string, DegradationProfileDef>(StringComparer.Ordinal);

        public EquipmentConditionSystem(
            ISeededRng rng,
            Inventory.Inventory inventory,
            CraftingSystem crafting,
            ILog? log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _crafting = crafting ?? throw new ArgumentNullException(nameof(crafting));
            _log = log ?? NullLog.Instance;
            RegisterDefaultProfiles();
        }

        private void RegisterDefaultProfiles()
        {
            RegisterProfile(new DegradationProfileDef
            {
                profile_id = "degrade_profile_firearm",
                item_family = "Weapon",
                base_wear_per_use = 1.5f,
                corrosion_susceptibility = 1.2f,
                jam_threshold = 25f,
                break_threshold = 5f,
                max_durability_loss_per_repair = 5f,
                jury_rig_allowed = true
            });
            RegisterProfile(new DegradationProfileDef
            {
                profile_id = "degrade_profile_tool",
                item_family = "Tool",
                base_wear_per_use = 0.8f,
                corrosion_susceptibility = 1.3f,
                jam_threshold = 20f,
                break_threshold = 4f,
                max_durability_loss_per_repair = 4f,
                jury_rig_allowed = true
            });
            RegisterProfile(new DegradationProfileDef
            {
                profile_id = "degrade_profile_clothing",
                item_family = "Clothing",
                base_wear_per_use = 0.4f,
                corrosion_susceptibility = 1.5f,
                jam_threshold = 0f,
                break_threshold = 10f,
                max_durability_loss_per_repair = 6f,
                jury_rig_allowed = true
            });
            RegisterProfile(new DegradationProfileDef
            {
                profile_id = "degrade_profile_watercraft",
                item_family = "Watercraft",
                base_wear_per_use = 1.2f,
                corrosion_susceptibility = 2.0f,
                jam_threshold = 15f,
                break_threshold = 10f,
                max_durability_loss_per_repair = 8f,
                jury_rig_allowed = true
            });
        }

        public void RegisterProfile(DegradationProfileDef profile)
        {
            if (profile != null && !string.IsNullOrEmpty(profile.profile_id))
            {
                _profiles[profile.profile_id] = profile;
                if (!string.IsNullOrEmpty(profile.item_family))
                    _profiles[profile.item_family] = profile;
            }
        }

        public void LoadProfiles(string jsonContent)
        {
            if (string.IsNullOrWhiteSpace(jsonContent)) return;
            try
            {
                var serializer = new SystemTextJsonSerializer();
                var catalog = serializer.Deserialize<ItemDegradationCatalog>(jsonContent);
                if (catalog?.profiles != null)
                {
                    foreach (var p in catalog.profiles)
                        RegisterProfile(p);
                }
            }
            catch (Exception ex)
            {
                _log.Warn($"[EquipmentCondition] Failed to load degradation catalog: {ex.Message}");
            }
        }

        public ActionResult RegisterItem(string instanceId, string itemId, string ownerId, EquipmentFamily family, float maxCondition = 100f)
        {
            if (_state.items.Exists(i => i.instanceId == instanceId))
                return ActionResult.Blocked("item_exists", "equip.item_exists");

            _state.items.Add(new EquipmentInstance
            {
                instanceId = instanceId, itemId = itemId, ownerId = ownerId,
                family = family, condition = maxCondition, maxCondition = maxCondition,
                originalMaxCondition = maxCondition
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

            if (item.condition <= 0)
            {
                item.isBroken = true;
                OnItemBroken?.Invoke(item);
            }

            OnConditionChanged?.Invoke(item);
            OnItemConditionChanged?.Invoke(item);
            return ActionResult.Success("equip.item_used",
                new Dictionary<string, double> { { "condition", item.condition } });
        }

        public ActionResult ApplyWear(string instanceId, WearEvent evt)
        {
            var item = _state.items.Find(i => i.instanceId == instanceId);
            if (item == null) return ActionResult.Failed("unknown_item", "equip.unknown_item");

            string familyKey = item.family.ToString();
            _profiles.TryGetValue(familyKey, out var profile);
            float baseWear = profile?.base_wear_per_use ?? 1.0f;
            float intensity = evt?.intensity ?? 1.0f;
            float envMod = evt?.environmentModifier ?? 1.0f;
            float wear = baseWear * intensity * envMod;

            if (item.rustLevel > 0f)
            {
                float rustFactor = 1f + (item.rustLevel / 100f) * 0.5f;
                wear *= rustFactor;
            }

            item.condition = Math.Max(0f, item.condition - wear);

            float breakThreshold = profile?.break_threshold ?? 5f;
            if (item.condition <= breakThreshold && item.condition > 0f)
            {
                if (_rng.NextDouble() < 0.20f)
                {
                    item.condition = 0f;
                    item.isBroken = true;
                    OnItemBroken?.Invoke(item);
                }
            }

            if (item.condition <= 0f)
            {
                item.isBroken = true;
                OnItemBroken?.Invoke(item);
            }

            float jamThreshold = profile?.jam_threshold ?? 20f;
            if (item.condition <= jamThreshold && !item.isJammed && !item.isBroken)
            {
                float jamRisk = GetJamRisk(instanceId);
                if (_rng.NextDouble() < jamRisk)
                {
                    item.isJammed = true;
                    OnItemJammed?.Invoke(item);
                }
            }

            OnConditionChanged?.Invoke(item);
            OnItemConditionChanged?.Invoke(item);
            return ActionResult.Success("equip.wear_applied",
                new Dictionary<string, double> { { "condition", item.condition }, { "isBroken", item.isBroken ? 1 : 0 }, { "isJammed", item.isJammed ? 1 : 0 } });
        }

        public ActionResult ApplyCorrosion(string instanceId, float exposureAmount, string environmentType = "weather")
        {
            var item = _state.items.Find(i => i.instanceId == instanceId);
            if (item == null) return ActionResult.Failed("unknown_item", "equip.unknown_item");

            string familyKey = item.family.ToString();
            _profiles.TryGetValue(familyKey, out var profile);
            float susceptibility = profile?.corrosion_susceptibility ?? 1.0f;

            item.rustLevel = Math.Clamp(item.rustLevel + exposureAmount * susceptibility, 0f, 100f);
            item.condition = Math.Max(0f, item.condition - (exposureAmount * susceptibility * 0.25f));

            if (item.condition <= 0f)
            {
                item.isBroken = true;
                OnItemBroken?.Invoke(item);
            }

            OnConditionChanged?.Invoke(item);
            OnItemConditionChanged?.Invoke(item);
            return ActionResult.Success("equip.corrosion_applied",
                new Dictionary<string, double> { { "rustLevel", item.rustLevel }, { "condition", item.condition } });
        }

        public ActionResult JuryRig(string instanceId, List<string> scrapMaterialIds)
        {
            var item = _state.items.Find(i => i.instanceId == instanceId);
            if (item == null) return ActionResult.Failed("unknown_item", "equip.unknown_item");

            if (scrapMaterialIds != null && scrapMaterialIds.Count > 0)
            {
                if (!_inventory.TryConsumeBill(scrapMaterialIds))
                    return ActionResult.Blocked("missing_scrap", "equip.missing_scrap");
            }

            float recovered = item.maxCondition * 0.40f;
            item.condition = Math.Min(item.maxCondition, item.condition + recovered);
            item.maxCondition = Math.Max(10f, item.maxCondition - 8f);
            item.temporaryPatch = true;
            item.isBroken = false;
            item.isJammed = false;

            OnItemRepaired?.Invoke(item);
            OnConditionChanged?.Invoke(item);
            OnItemConditionChanged?.Invoke(item);
            return ActionResult.Success("equip.jury_rigged",
                new Dictionary<string, double> { { "condition", item.condition }, { "maxCondition", item.maxCondition } });
        }

        public ActionResult RepairItem(string instanceId, MaintenanceType type, List<string> parts, float repairQuality = 1.0f)
        {
            var item = _state.items.Find(i => i.instanceId == instanceId);
            if (item == null) return ActionResult.Failed("unknown_item", "equip.unknown_item");

            if (parts != null && parts.Count > 0)
            {
                if (!_inventory.TryConsumeBill(parts))
                    return ActionResult.Blocked("missing_part", "equip.missing_part");
            }

            float maxLoss = Math.Max(0f, 5f * (1f - Math.Clamp(repairQuality, 0f, 1f) * 0.6f));
            item.maxCondition = Math.Max(10f, item.maxCondition - maxLoss);
            item.condition = item.maxCondition;
            item.repairQuality = repairQuality;
            item.temporaryPatch = false;
            item.isBroken = false;
            item.isJammed = false;
            item.lastMaintainedDay = _currentDay;
            item.maintenanceHistory.Add($"Repaired (quality {repairQuality:F2}) on day {_currentDay}");

            OnItemRepaired?.Invoke(item);
            OnConditionChanged?.Invoke(item);
            OnItemConditionChanged?.Invoke(item);
            return ActionResult.Success("equip.repaired",
                new Dictionary<string, double> { { "condition", item.condition }, { "maxCondition", item.maxCondition } });
        }

        public ActionResult ClearJam(string instanceId)
        {
            var item = _state.items.Find(i => i.instanceId == instanceId);
            if (item == null) return ActionResult.Failed("unknown_item", "equip.unknown_item");

            item.isJammed = false;
            OnConditionChanged?.Invoke(item);
            OnItemConditionChanged?.Invoke(item);
            return ActionResult.Success("equip.jam_cleared");
        }

        public float GetConditionPercent(string key)
        {
            var item = _state.items.Find(i => i.instanceId == key || i.itemId == key);
            if (item == null || item.maxCondition <= 0f) return 0f;
            return Math.Clamp((item.condition / item.maxCondition) * 100f, 0f, 100f);
        }

        public ActionResult StartMaintenance(string instanceId, string stationId, MaintenanceType type, List<string> requiredParts)
        {
            var item = _state.items.Find(i => i.instanceId == instanceId);
            if (item == null) return ActionResult.Failed("unknown_item", "equip.unknown_item");

            var reserved = new List<string>();
            if (requiredParts != null && requiredParts.Count > 0)
            {
                if (!_inventory.TryConsumeBill(requiredParts))
                    return ActionResult.Blocked("missing_part", "equip.missing_part");

                reserved.AddRange(requiredParts);
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

        public EquipmentConditionState CaptureState() => CloneState(_state);

        public void RestoreState(EquipmentConditionState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static EquipmentConditionState CloneState(EquipmentConditionState src)
        {
            if (src == null) return new EquipmentConditionState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<EquipmentConditionState>(json) ?? new EquipmentConditionState();
        }
    }
}
