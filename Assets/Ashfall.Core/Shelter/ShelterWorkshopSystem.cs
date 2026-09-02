// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ashfall.Core.Combat;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Shelter
{
    public enum WorkshopJobKind
    {
        Fabrication = 0,
        AmmunitionReload = 1,
        WeaponService = 2,
        ElectronicsRepair = 3,
        ToolOverhaul = 4,
        HeavyWorkshopService = 5
    }

    public enum WorkshopJobStatus
    {
        Queued = 0,
        Active = 1,
        CompletedPendingCollection = 2,
        Completed = 3,
        Cancelled = 4,
        Failed = 5
    }

    [Serializable]
    public sealed class WorkshopRecipeInput
    {
        [JsonPropertyName("item_id")]
        public string ItemId { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public int Amount { get; set; } = 1;
    }

    [Serializable]
    public sealed class WorkshopRecipeOutput
    {
        [JsonPropertyName("item_id")]
        public string ItemId { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public int Amount { get; set; } = 1;
    }

    [Serializable]
    public sealed class WorkshopRecipeDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = "fabrication";

        [JsonPropertyName("required_room_ids")]
        public List<string> RequiredRoomIds { get; set; } = new List<string>();

        [JsonPropertyName("required_rule_ids")]
        public List<string> RequiredRuleIds { get; set; } = new List<string>();

        [JsonPropertyName("inputs")]
        public List<WorkshopRecipeInput> Inputs { get; set; } = new List<WorkshopRecipeInput>();

        [JsonPropertyName("outputs")]
        public List<WorkshopRecipeOutput> Outputs { get; set; } = new List<WorkshopRecipeOutput>();

        [JsonPropertyName("base_labor_ticks")]
        public int BaseLaborTicks { get; set; } = 60;

        [JsonPropertyName("base_scrap_waste_permille")]
        public int BaseScrapWastePermille { get; set; } = 50;

        [JsonPropertyName("tool_wear_permille")]
        public int ToolWearPermille { get; set; } = 5;

        [JsonPropertyName("calibration_requirement")]
        public float CalibrationRequirement { get; set; } = 0.5f;

        [JsonPropertyName("skill_weights")]
        public Dictionary<string, float> SkillWeights { get; set; } = new Dictionary<string, float>(StringComparer.Ordinal);

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonIgnore]
        public WorkshopJobKind Kind
        {
            get
            {
                if (string.Equals(Category, "ammunition_reload", StringComparison.OrdinalIgnoreCase))
                    return WorkshopJobKind.AmmunitionReload;
                if (string.Equals(Category, "weapon_service", StringComparison.OrdinalIgnoreCase))
                    return WorkshopJobKind.WeaponService;
                if (string.Equals(Category, "electronics_refit", StringComparison.OrdinalIgnoreCase))
                    return WorkshopJobKind.ElectronicsRepair;
                if (string.Equals(Category, "tool_overhaul", StringComparison.OrdinalIgnoreCase))
                    return WorkshopJobKind.ToolOverhaul;
                if (string.Equals(Category, "heavy_workshop_service", StringComparison.OrdinalIgnoreCase))
                    return WorkshopJobKind.HeavyWorkshopService;
                return WorkshopJobKind.Fabrication;
            }
        }
    }

    [Serializable]
    public sealed class WorkshopRecipeCatalogData
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("recipes")]
        public List<WorkshopRecipeDefinition> Recipes { get; set; } = new List<WorkshopRecipeDefinition>();
    }

    [Serializable]
    public sealed class WorkshopJobState
    {
        public string JobId { get; set; } = string.Empty;
        public string RecipeId { get; set; } = string.Empty;
        public string RoomId { get; set; } = string.Empty;
        public string TargetEntityId { get; set; } = string.Empty;
        public List<string> WorkerIds { get; set; } = new List<string>();
        public int TotalLaborTicks { get; set; }
        public int RemainingLaborTicks { get; set; }
        public WorkshopJobStatus Status { get; set; } = WorkshopJobStatus.Queued;
        public int YieldProduced { get; set; }
        public int WasteProduced { get; set; }
        public long StartedTick { get; set; }
    }

    [Serializable]
    public sealed class WorkshopMachineState
    {
        public string RoomId { get; set; } = string.Empty;
        public float ToolingHealth { get; set; } = 1.0f; // 0.0 - 1.0
        public float Calibration { get; set; } = 1.0f;   // 0.0 - 1.0
        public int LastOverhaulDay { get; set; } = -1;
    }

    [Serializable]
    public sealed class ShelterWorkshopSave
    {
        public string systemId = ShelterWorkshopSystem.SystemId;
        public int schemaVersion = 1;
        public List<WorkshopJobState> jobs = new List<WorkshopJobState>();
        public Dictionary<string, WorkshopMachineState> machines = new Dictionary<string, WorkshopMachineState>(StringComparer.Ordinal);
        public long currentTick;
        public int currentDay;
    }

    public sealed class ShelterWorkshopSystem
    {
        public const string SystemId = "shelter_workshop";

        private ShelterWorkshopSave _state = new ShelterWorkshopSave();
        private readonly Dictionary<string, WorkshopRecipeDefinition> _recipes = new(StringComparer.Ordinal);
        private readonly InventoryContainer _inventory;
        private readonly EquipmentConditionSystem? _equipment;
        private readonly ExpeditionVehicleSystem? _vehicles;
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        private Func<string, string, float>? _workerSkillProvider; // (workerId, skillId) -> skill level (0.0 to 1.0+)

        public ShelterWorkshopSave State => _state;
        public IReadOnlyDictionary<string, WorkshopRecipeDefinition> Recipes => _recipes;

        public event Action<WorkshopJobState>? OnJobStarted;
        public event Action<WorkshopJobState>? OnJobCompleted;
        public event Action<WorkshopJobState>? OnJobCancelled;
        public event Action<WorkshopMachineState>? OnMachineStateChanged;
        public event Action? OnWorkshopChanged;

        public ShelterWorkshopSystem(
            InventoryContainer inventory,
            ISeededRng rng,
            EquipmentConditionSystem? equipment = null,
            ExpeditionVehicleSystem? vehicles = null,
            ILog? log = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _equipment = equipment;
            _vehicles = vehicles;
            _log = log ?? NullLog.Instance;
        }

        public void BindWorkerSkillProvider(Func<string, string, float> provider)
        {
            _workerSkillProvider = provider;
        }

        public void LoadCatalog(WorkshopRecipeCatalogData? data)
        {
            if (data?.Recipes == null) return;
            _recipes.Clear();
            foreach (var r in data.Recipes)
            {
                if (!string.IsNullOrEmpty(r.Id))
                    _recipes[r.Id] = r;
            }
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            var serializer = new SystemTextJsonSerializer();
            var data = serializer.Deserialize<WorkshopRecipeCatalogData>(json);
            LoadCatalog(data);
        }

        public WorkshopMachineState GetOrCreateMachineState(string roomId)
        {
            if (string.IsNullOrEmpty(roomId)) roomId = "room_workshop";
            if (_state.machines.TryGetValue(roomId, out var machine))
                return machine;

            var newMachine = new WorkshopMachineState
            {
                RoomId = roomId,
                ToolingHealth = 1.0f,
                Calibration = 1.0f,
                LastOverhaulDay = _state.currentDay
            };
            _state.machines[roomId] = newMachine;
            return newMachine;
        }

        public IReadOnlyList<WorkshopRecipeDefinition> GetAvailableRecipes(string roomId)
        {
            var list = new List<WorkshopRecipeDefinition>();
            foreach (var r in _recipes.Values)
            {
                if (r.RequiredRoomIds.Count == 0 || r.RequiredRoomIds.Contains(roomId))
                    list.Add(r);
            }
            return list;
        }

        public bool CanStartJob(
            string recipeId,
            string roomId,
            string? targetEntityId,
            IReadOnlyList<string>? workerIds,
            out string reason)
        {
            reason = string.Empty;
            if (!_recipes.TryGetValue(recipeId, out var recipe))
            {
                reason = "unknown_recipe";
                return false;
            }

            if (recipe.RequiredRoomIds.Count > 0 && !recipe.RequiredRoomIds.Contains(roomId))
            {
                reason = "invalid_room";
                return false;
            }

            var machine = GetOrCreateMachineState(roomId);
            if (machine.ToolingHealth <= 0.10f && recipe.Kind != WorkshopJobKind.ToolOverhaul)
            {
                reason = "tooling_broken";
                return false;
            }

            if (machine.Calibration < recipe.CalibrationRequirement && recipe.Kind != WorkshopJobKind.ToolOverhaul)
            {
                reason = "calibration_too_low";
                return false;
            }

            // Target validations
            if (recipe.Kind == WorkshopJobKind.WeaponService)
            {
                if (string.IsNullOrEmpty(targetEntityId) || _equipment == null)
                {
                    reason = "missing_weapon_target";
                    return false;
                }
                var item = _equipment.State.items.Find(i => i.instanceId == targetEntityId);
                if (item == null)
                {
                    reason = "unknown_weapon_instance";
                    return false;
                }
                if (item.condition >= item.maxCondition)
                {
                    reason = "weapon_already_pristine";
                    return false;
                }
            }
            else if (recipe.Kind == WorkshopJobKind.HeavyWorkshopService)
            {
                if (string.IsNullOrEmpty(targetEntityId) || _vehicles == null)
                {
                    reason = "missing_vehicle_target";
                    return false;
                }
                var vehicle = _vehicles.GetVehicle(targetEntityId);
                if (vehicle == null)
                {
                    reason = "unknown_vehicle";
                    return false;
                }
                if (vehicle.condition >= 100f && !vehicle.isBrokenDown)
                {
                    reason = "vehicle_already_pristine";
                    return false;
                }
            }

            // Inventory cost validation
            var bill = BuildBill(recipe);
            var validation = _inventory.ValidateTransaction(bill);
            if (!validation.IsValid)
            {
                reason = validation.FailureReason;
                return false;
            }

            return true;
        }

        private InventoryBill BuildBill(WorkshopRecipeDefinition recipe)
        {
            var bill = new InventoryBill();
            foreach (var input in recipe.Inputs)
            {
                if (!string.IsNullOrEmpty(input.ItemId) && input.Amount > 0)
                    bill.AddCost(input.ItemId, input.Amount);
            }
            return bill;
        }

        public ActionResult TryStartJob(
            string recipeId,
            string roomId,
            string? targetEntityId,
            IReadOnlyList<string>? workerIds,
            out string jobId)
        {
            jobId = string.Empty;
            if (!CanStartJob(recipeId, roomId, targetEntityId, workerIds, out var reason))
            {
                return ActionResult.Blocked("cannot_start", reason);
            }

            var recipe = _recipes[recipeId];
            var bill = BuildBill(recipe);

            // Atomic material consumption
            if (!_inventory.TryExecuteTransaction(bill))
            {
                return ActionResult.Blocked("transaction_failed", "insufficient_materials");
            }

            // Calculate duration and efficiency based on worker skills
            float laborMultiplier = CalculateWorkerLaborMultiplier(recipe, workerIds);
            int effectiveTicks = Math.Max(10, (int)Math.Round(recipe.BaseLaborTicks * laborMultiplier));

            jobId = $"job_{recipe.Id}_{_state.currentTick}_{_rng.Next(1000, 9999)}";
            var job = new WorkshopJobState
            {
                JobId = jobId,
                RecipeId = recipe.Id,
                RoomId = roomId,
                TargetEntityId = targetEntityId ?? string.Empty,
                WorkerIds = workerIds != null ? new List<string>(workerIds) : new List<string>(),
                TotalLaborTicks = effectiveTicks,
                RemainingLaborTicks = effectiveTicks,
                Status = WorkshopJobStatus.Active,
                StartedTick = _state.currentTick
            };

            _state.jobs.Add(job);
            OnJobStarted?.Invoke(job);
            OnWorkshopChanged?.Invoke();
            return ActionResult.Success("workshop.job_started", new Dictionary<string, double> { { "ticks", effectiveTicks } });
        }

        private float CalculateWorkerLaborMultiplier(WorkshopRecipeDefinition recipe, IReadOnlyList<string>? workerIds)
        {
            if (workerIds == null || workerIds.Count == 0 || _workerSkillProvider == null)
                return 1.0f;

            float skillSum = 0f;
            foreach (var worker in workerIds)
            {
                foreach (var kv in recipe.SkillWeights)
                {
                    float skillLevel = _workerSkillProvider(worker, kv.Key);
                    skillSum += skillLevel * kv.Value;
                }
            }

            // High skill speeds up production, down to 50% base time
            float reduction = Math.Clamp(skillSum * 0.15f, 0f, 0.50f);
            return 1.0f - reduction;
        }

        private float CalculateWasteMultiplier(WorkshopRecipeDefinition recipe, IReadOnlyList<string>? workerIds)
        {
            if (workerIds == null || workerIds.Count == 0 || _workerSkillProvider == null)
                return 1.0f;

            float skillSum = 0f;
            foreach (var worker in workerIds)
            {
                foreach (var kv in recipe.SkillWeights)
                {
                    float skillLevel = _workerSkillProvider(worker, kv.Key);
                    skillSum += skillLevel * kv.Value;
                }
            }

            float reduction = Math.Clamp(skillSum * 0.20f, 0f, 0.70f);
            return 1.0f - reduction;
        }

        public ActionResult TryCancelJob(string jobId)
        {
            var job = _state.jobs.Find(j => j.JobId == jobId);
            if (job == null) return ActionResult.Failed("unknown_job", "workshop.unknown_job");
            if (job.Status != WorkshopJobStatus.Active && job.Status != WorkshopJobStatus.Queued)
                return ActionResult.Blocked("not_active", "workshop.job_not_active");

            job.Status = WorkshopJobStatus.Cancelled;
            OnJobCancelled?.Invoke(job);
            OnWorkshopChanged?.Invoke();
            return ActionResult.Success("workshop.job_cancelled");
        }

        public ActionResult TryCollectCompletedJob(string jobId)
        {
            var job = _state.jobs.Find(j => j.JobId == jobId);
            if (job == null) return ActionResult.Failed("unknown_job", "workshop.unknown_job");
            if (job.Status != WorkshopJobStatus.CompletedPendingCollection)
                return ActionResult.Blocked("not_ready", "workshop.job_not_ready");

            if (!_recipes.TryGetValue(job.RecipeId, out var recipe))
                return ActionResult.Failed("unknown_recipe", "workshop.unknown_recipe");

            // Grant outputs to inventory
            if (recipe.Outputs.Count > 0)
            {
                var grantBill = new InventoryBill();
                foreach (var output in recipe.Outputs)
                {
                    if (!string.IsNullOrEmpty(output.ItemId) && output.Amount > 0)
                        grantBill.AddGrant(output.ItemId, output.Amount);
                }
                if (!_inventory.TryExecuteTransaction(grantBill))
                {
                    return ActionResult.Blocked("inventory_full", "inventory_capacity_exceeded");
                }
            }

            job.Status = WorkshopJobStatus.Completed;
            OnWorkshopChanged?.Invoke();
            return ActionResult.Success("workshop.job_collected");
        }

        public ActionResult TryOverhaulTooling(string roomId, string? workerId = null)
        {
            var machine = GetOrCreateMachineState(roomId);
            var bill = new InventoryBill();
            bill.AddCost("scrap_metal", 6);
            bill.AddCost("mechanical_parts", 2);
            bill.AddCost("machine_oil", 1);

            if (!_inventory.TryExecuteTransaction(bill))
                return ActionResult.Blocked("missing_materials", "insufficient_overhaul_materials");

            machine.ToolingHealth = 1.0f;
            machine.Calibration = 1.0f;
            machine.LastOverhaulDay = _state.currentDay;

            OnMachineStateChanged?.Invoke(machine);
            OnWorkshopChanged?.Invoke();
            return ActionResult.Success("workshop.tooling_overhauled");
        }

        public void AdvanceLaborTicks(int ticks, int currentDay)
        {
            if (ticks <= 0) return;
            _state.currentTick += ticks;
            _state.currentDay = currentDay;

            for (int i = _state.jobs.Count - 1; i >= 0; i--)
            {
                var job = _state.jobs[i];
                if (job.Status != WorkshopJobStatus.Active) continue;

                job.RemainingLaborTicks -= ticks;
                if (job.RemainingLaborTicks <= 0)
                {
                    CompleteJob(job);
                }
            }
        }

        private void CompleteJob(WorkshopJobState job)
        {
            if (!_recipes.TryGetValue(job.RecipeId, out var recipe))
            {
                job.Status = WorkshopJobStatus.Failed;
                return;
            }

            var machine = GetOrCreateMachineState(job.RoomId);

            // Apply machine wear & calibration drift
            float wear = (recipe.ToolWearPermille / 1000f);
            machine.ToolingHealth = Math.Max(0f, machine.ToolingHealth - wear);
            machine.Calibration = Math.Max(0f, machine.Calibration - (wear * 0.5f));
            OnMachineStateChanged?.Invoke(machine);

            // Execute job-kind specific completion logic
            if (recipe.Kind == WorkshopJobKind.WeaponService)
            {
                if (_equipment != null && !string.IsNullOrEmpty(job.TargetEntityId))
                {
                    var item = _equipment.State.items.Find(i => i.instanceId == job.TargetEntityId);
                    if (item != null)
                    {
                        float restoreAmount = recipe.Id.Contains("refurbish", StringComparison.OrdinalIgnoreCase) ? 40f : 20f;
                        item.condition = Math.Min(item.maxCondition, item.condition + restoreAmount);
                        item.lastMaintainedDay = _state.currentDay;
                        item.maintenanceHistory.Add($"serviced_in_workshop on day {_state.currentDay}");
                    }
                }
                job.Status = WorkshopJobStatus.Completed;
            }
            else if (recipe.Kind == WorkshopJobKind.HeavyWorkshopService)
            {
                if (_vehicles != null && !string.IsNullOrEmpty(job.TargetEntityId))
                {
                    _vehicles.Repair(job.TargetEntityId, 35f);
                }
                job.Status = WorkshopJobStatus.Completed;
            }
            else if (recipe.Kind == WorkshopJobKind.ToolOverhaul)
            {
                machine.ToolingHealth = 1.0f;
                machine.Calibration = 1.0f;
                machine.LastOverhaulDay = _state.currentDay;
                OnMachineStateChanged?.Invoke(machine);
                job.Status = WorkshopJobStatus.Completed;
            }
            else
            {
                // Ammunition reload or precision fabrication
                float wasteMult = CalculateWasteMultiplier(recipe, job.WorkerIds);
                int waste = (int)Math.Round((recipe.BaseScrapWastePermille / 1000f) * wasteMult * 10f);
                job.WasteProduced = waste;

                if (recipe.Outputs.Count > 0)
                {
                    var grantBill = new InventoryBill();
                    foreach (var output in recipe.Outputs)
                    {
                        if (!string.IsNullOrEmpty(output.ItemId) && output.Amount > 0)
                            grantBill.AddGrant(output.ItemId, output.Amount);
                    }

                    if (_inventory.TryExecuteTransaction(grantBill))
                    {
                        job.Status = WorkshopJobStatus.Completed;
                    }
                    else
                    {
                        // Stash in pending collection if storage momentarily restricted
                        job.Status = WorkshopJobStatus.CompletedPendingCollection;
                    }
                }
                else
                {
                    job.Status = WorkshopJobStatus.Completed;
                }
            }

            OnJobCompleted?.Invoke(job);
            OnWorkshopChanged?.Invoke();
        }

        public void TickDay(int day)
        {
            _state.currentDay = day;
            // Advance by default shift of 480 labor ticks (8 hours) per day
            AdvanceLaborTicks(480, day);
        }

        public ShelterWorkshopSave CaptureState()
        {
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(_state);
            return s.Deserialize<ShelterWorkshopSave>(json) ?? new ShelterWorkshopSave();
        }

        public void RestoreState(ShelterWorkshopSave? saved)
        {
            if (saved == null)
            {
                _state = new ShelterWorkshopSave();
                return;
            }

            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(saved);
            _state = s.Deserialize<ShelterWorkshopSave>(json) ?? new ShelterWorkshopSave();

            // Ensure healthy default machines if legacy save loaded with empty machines
            if (_state.machines.Count == 0)
            {
                GetOrCreateMachineState("room_workshop_precision");
                GetOrCreateMachineState("room_armory_munitions");
                GetOrCreateMachineState("room_workshop_heavy");
            }
            OnWorkshopChanged?.Invoke();
        }
    }
}
