using System;
using System.Collections.Generic;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>Which existing bunker asset a maintenance order targets.</summary>
    public enum MaintenanceTargetType
    {
        Module,
        PowerSource
    }

    /// <summary>Player-set urgency for the currently assigned repair worker.</summary>
    public enum MaintenanceRepairPriority
    {
        Low,
        Standard,
        Critical
    }

    /// <summary>
    /// Read-only condition reporting plus tightly-scoped repair orders for installed
    /// shelter modules and registered power sources. This system deliberately uses
    /// the existing module FilterHealth and source Durability fields, rather than
    /// creating a competing durability model.
    /// </summary>
    public sealed class BunkerMaintenanceSystem : IDisposable
    {
        public const string SystemId = "bunker_maintenance";
        public const int ModuleMechanicalPartsRequired = 1;
        public const int PoweredModuleElectronicScrapRequired = 1;
        public const int PowerSourceMechanicalPartsRequired = 2;

        private readonly Shelter _shelter;
        private readonly PowerNetwork _powerNetwork;
        private readonly Func<string, int> _countMaterial;
        private readonly Func<string, int, bool> _consumeMaterial;
        private readonly Func<IReadOnlyList<Survivor>> _getSurvivors;
        private BunkerMaintenanceSnapshot _lastSnapshot;

        /// <summary>Survivor currently assigned to the terminal's repair order.</summary>
        public string AssignedSurvivorId { get; private set; }
        /// <summary>Urgency assigned to the terminal's repair order.</summary>
        public MaintenanceRepairPriority RepairPriority { get; private set; } = MaintenanceRepairPriority.Standard;

        /// <summary>Raised whenever displayed maintenance state changes.</summary>
        public event Action OnChanged;
        /// <summary>Raised after a successful material-backed repair.</summary>
        public event Action<BunkerMaintenanceRepairResult> OnRepairCompleted;

        public BunkerMaintenanceSystem(
            Shelter shelter,
            PowerNetwork powerNetwork,
            Func<string, int> countMaterial,
            Func<string, int, bool> consumeMaterial,
            Func<IReadOnlyList<Survivor>> getSurvivors)
        {
            _shelter = shelter;
            _powerNetwork = powerNetwork;
            _countMaterial = countMaterial;
            _consumeMaterial = consumeMaterial;
            _getSurvivors = getSurvivors;

            if (_shelter != null)
            {
                _shelter.OnModuleAdded += HandleModuleChanged;
                _shelter.OnModuleRemoved += HandleModuleRemoved;
                _shelter.OnModuleUpgraded += HandleModuleUpgraded;
            }
            if (_powerNetwork != null)
                _powerNetwork.OnPowerStateChanged += Refresh;

            _lastSnapshot = GetSnapshot();
        }

        /// <summary>Builds a detached snapshot for UI and save-safe consumers.</summary>
        public BunkerMaintenanceSnapshot GetSnapshot()
        {
            var snapshot = new BunkerMaintenanceSnapshot
            {
                RepairsBlockedByPowerFailure = _powerNetwork != null && _powerNetwork.IsBlackout,
                MechanicalPartsOnHand = CountMaterial(ScrapMaterialIds.MechanicalParts),
                ElectronicScrapOnHand = CountMaterial(ScrapMaterialIds.ElectronicScrap),
                AssignedSurvivorId = AssignedSurvivorId,
                RepairPriority = RepairPriority,
                Targets = new List<BunkerMaintenanceTargetSnapshot>()
            };

            var assigned = FindLivingSurvivor(AssignedSurvivorId);
            snapshot.AssignedSurvivorName = assigned != null
                ? DisplaySurvivor(assigned)
                : string.Empty;
            snapshot.HasAssignedLivingSurvivor = assigned != null;

            if (_shelter != null && _shelter.Modules != null)
            {
                for (int i = 0; i < _shelter.Modules.Count; i++)
                {
                    var module = _shelter.Modules[i];
                    if (module == null || string.IsNullOrEmpty(module.ModuleId)) continue;
                    snapshot.Targets.Add(BuildModuleTarget(module));
                }
            }

            if (_powerNetwork != null && _powerNetwork.Sources != null)
            {
                for (int i = 0; i < _powerNetwork.Sources.Count; i++)
                {
                    var source = _powerNetwork.Sources[i];
                    if (source == null || string.IsNullOrEmpty(source.SourceId)) continue;
                    snapshot.Targets.Add(BuildSourceTarget(source));
                }
            }

            return snapshot;
        }

        /// <summary>Assign a living survivor to the repair order.</summary>
        public bool AssignSurvivor(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (FindLivingSurvivor(survivorId) == null) return false;
            if (string.Equals(AssignedSurvivorId, survivorId, StringComparison.Ordinal)) return false;

            AssignedSurvivorId = survivorId;
            Refresh(force: true);
            return true;
        }

        /// <summary>Move the repair order between low, standard, and critical urgency.</summary>
        public bool AdjustPriority(int direction)
        {
            if (direction == 0) return false;
            var next = (MaintenanceRepairPriority)Mathf.Clamp(
                (int)RepairPriority + (direction > 0 ? 1 : -1),
                (int)MaintenanceRepairPriority.Low,
                (int)MaintenanceRepairPriority.Critical);
            if (next == RepairPriority) return false;

            RepairPriority = next;
            Refresh(force: true);
            return true;
        }

        /// <summary>
        /// Verify that a repair task can safely begin without changing inventory or
        /// asset condition. RepairWorkOrderSystem uses this before claiming work and
        /// again at completion, so cancellation is always material-safe.
        /// </summary>
        public bool CanStartRepairWork(
            MaintenanceTargetType targetType,
            string targetId,
            out BunkerMaintenanceRepairResult result)
        {
            result = new BunkerMaintenanceRepairResult
            {
                TargetType = targetType,
                TargetId = targetId,
                AssignedSurvivorId = AssignedSurvivorId
            };

            if (string.IsNullOrEmpty(targetId))
            {
                result.Reason = "No maintenance target selected.";
                return false;
            }
            if (_powerNetwork != null && _powerNetwork.IsBlackout)
            {
                result.Reason = "HELD: repairs are locked during an active grid failure.";
                return false;
            }
            if (FindLivingSurvivor(AssignedSurvivorId) == null)
            {
                result.Reason = "HELD: assign a living survivor before starting repair work.";
                return false;
            }

            var target = FindTarget(targetType, targetId);
            if (target == null)
            {
                result.Reason = "HELD: that bunker asset is no longer installed.";
                return false;
            }
            if (!target.CanRepair)
            {
                result.Reason = target.Condition >= 100f
                    ? "HELD: condition is already at 100%."
                    : "HELD: this asset is destroyed and must be replaced, not repaired.";
                return false;
            }

            for (int i = 0; i < target.Materials.Count; i++)
            {
                var material = target.Materials[i];
                if (CountMaterial(material.ItemId) < material.Amount)
                {
                    result.Reason = "HELD: insufficient " + MaterialLabel(material.ItemId) + ".";
                    return false;
                }
            }

            result.Succeeded = true;
            result.Reason = "READY: repair work may begin on " + target.DisplayName + ".";
            return true;
        }

        /// <summary>
        /// Consume the displayed materials and restore one damaged asset to full condition.
        /// This remains the atomic completion primitive; repair work orders defer calling it
        /// until their assigned survivor has finished the required work time.
        /// </summary>
        public bool TryRepair(
            MaintenanceTargetType targetType,
            string targetId,
            out BunkerMaintenanceRepairResult result)
        {
            if (!CanStartRepairWork(targetType, targetId, out result))
                return false;

            var target = FindTarget(targetType, targetId);
            if (target == null)
            {
                result.Succeeded = false;
                result.Reason = "HELD: that bunker asset is no longer installed.";
                return false;
            }

            // Inventory is single-threaded in the simulation. Pre-validating every
            // requirement keeps this sequence atomic from the player's perspective.
            for (int i = 0; i < target.Materials.Count; i++)
            {
                var material = target.Materials[i];
                if (_consumeMaterial == null || !_consumeMaterial(material.ItemId, material.Amount))
                {
                    result.Succeeded = false;
                    result.Reason = "HELD: maintenance stores could not release the required material.";
                    return false;
                }
            }

            if (targetType == MaintenanceTargetType.Module)
            {
                var module = _shelter != null ? _shelter.GetModule(targetId) : null;
                if (module == null)
                {
                    result.Reason = "HELD: that module is no longer installed.";
                    return false;
                }
                module.FilterHealth = 100f;
                module.IsEnabled = true;
            }
            else
            {
                var source = _powerNetwork != null ? _powerNetwork.GetSource(targetId) : null;
                if (source == null)
                {
                    result.Reason = "HELD: that power source is no longer installed.";
                    return false;
                }
                source.Durability = 100f;
            }

            _powerNetwork?.Rebalance();
            result.Succeeded = true;
            result.Reason = "RESTORED: " + target.DisplayName + " returned to full condition.";
            result.MaterialsConsumed = CopyMaterials(target.Materials);
            OnRepairCompleted?.Invoke(result);
            Refresh(force: true);
            return true;
        }

        public BunkerMaintenanceSave CaptureState()
        {
            return new BunkerMaintenanceSave
            {
                systemId = SystemId,
                assignedSurvivorId = AssignedSurvivorId,
                repairPriority = (int)RepairPriority
            };
        }

        public void RestoreState(BunkerMaintenanceSave state)
        {
            AssignedSurvivorId = state != null ? state.assignedSurvivorId : null;
            RepairPriority = state == null
                ? MaintenanceRepairPriority.Standard
                : (MaintenanceRepairPriority)Mathf.Clamp(
                    state.repairPriority,
                    (int)MaintenanceRepairPriority.Low,
                    (int)MaintenanceRepairPriority.Critical);
            Refresh(force: true);
        }

        /// <summary>Compare live durability and stores after a simulation or inventory update.</summary>
        public void Refresh() => Refresh(force: false);

        public void Dispose()
        {
            if (_shelter != null)
            {
                _shelter.OnModuleAdded -= HandleModuleChanged;
                _shelter.OnModuleRemoved -= HandleModuleRemoved;
                _shelter.OnModuleUpgraded -= HandleModuleUpgraded;
            }
            if (_powerNetwork != null)
                _powerNetwork.OnPowerStateChanged -= Refresh;
        }

        private void Refresh(bool force)
        {
            var snapshot = GetSnapshot();
            if (!force && SnapshotsEqual(_lastSnapshot, snapshot)) return;
            _lastSnapshot = snapshot;
            OnChanged?.Invoke();
        }

        private BunkerMaintenanceTargetSnapshot FindTarget(MaintenanceTargetType targetType, string targetId)
        {
            var snapshot = GetSnapshot();
            for (int i = 0; i < snapshot.Targets.Count; i++)
            {
                var target = snapshot.Targets[i];
                if (target.TargetType == targetType && target.TargetId == targetId)
                    return target;
            }
            return null;
        }

        private BunkerMaintenanceTargetSnapshot BuildModuleTarget(ShelterModuleInstance module)
        {
            float condition = Mathf.Clamp(module.FilterHealth, 0f, 100f);
            bool destroyed = module.Level <= 0 || module.FilterHealth < 0f;
            var materials = BuildMaterials(
                ModuleMechanicalPartsRequired,
                _powerNetwork != null && _powerNetwork.GetConsumer(module.ModuleId) != null
                    ? PoweredModuleElectronicScrapRequired
                    : 0);
            return new BunkerMaintenanceTargetSnapshot
            {
                TargetType = MaintenanceTargetType.Module,
                TargetId = module.ModuleId,
                DisplayName = module.Definition != null && !string.IsNullOrEmpty(module.Definition.DisplayName)
                    ? module.Definition.DisplayName
                    : module.ModuleId,
                Condition = condition,
                IsDestroyed = destroyed,
                CanRepair = !destroyed && condition < 100f,
                Materials = materials,
                HasRequiredMaterials = HasMaterials(materials)
            };
        }

        private BunkerMaintenanceTargetSnapshot BuildSourceTarget(PowerSourceInstance source)
        {
            float condition = Mathf.Clamp(source.Durability, 0f, 100f);
            var materials = BuildMaterials(PowerSourceMechanicalPartsRequired, 0);
            return new BunkerMaintenanceTargetSnapshot
            {
                TargetType = MaintenanceTargetType.PowerSource,
                TargetId = source.SourceId,
                DisplayName = source.Definition != null && !string.IsNullOrEmpty(source.Definition.DisplayName)
                    ? source.Definition.DisplayName
                    : source.SourceId,
                Condition = condition,
                IsDestroyed = false,
                CanRepair = condition < 100f,
                Materials = materials,
                HasRequiredMaterials = HasMaterials(materials)
            };
        }

        private static List<BunkerMaintenanceMaterialRequirement> BuildMaterials(int mechanicalParts, int electronicScrap)
        {
            var materials = new List<BunkerMaintenanceMaterialRequirement>();
            if (mechanicalParts > 0)
                materials.Add(new BunkerMaintenanceMaterialRequirement
                {
                    ItemId = ScrapMaterialIds.MechanicalParts,
                    Amount = mechanicalParts
                });
            if (electronicScrap > 0)
                materials.Add(new BunkerMaintenanceMaterialRequirement
                {
                    ItemId = ScrapMaterialIds.ElectronicScrap,
                    Amount = electronicScrap
                });
            return materials;
        }

        private bool HasMaterials(List<BunkerMaintenanceMaterialRequirement> materials)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                if (CountMaterial(materials[i].ItemId) < materials[i].Amount)
                    return false;
            }
            return true;
        }

        private int CountMaterial(string itemId) => _countMaterial != null ? Mathf.Max(0, _countMaterial(itemId)) : 0;

        private Survivor FindLivingSurvivor(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            var survivors = _getSurvivors != null ? _getSurvivors() : null;
            if (survivors == null) return null;
            for (int i = 0; i < survivors.Count; i++)
            {
                var survivor = survivors[i];
                if (survivor != null && survivor.IsAlive && survivor.Id == survivorId)
                    return survivor;
            }
            return null;
        }

        private void HandleModuleChanged(ShelterModuleInstance _) => Refresh(force: true);
        private void HandleModuleRemoved(string _) => Refresh(force: true);
        private void HandleModuleUpgraded(ShelterModuleInstance _, int __) => Refresh(force: true);

        private static string DisplaySurvivor(Survivor survivor)
        {
            return !string.IsNullOrEmpty(survivor.DisplayName) ? survivor.DisplayName : survivor.Id;
        }

        private static string MaterialLabel(string itemId)
        {
            if (itemId == ScrapMaterialIds.MechanicalParts) return "mechanical parts";
            if (itemId == ScrapMaterialIds.ElectronicScrap) return "electronic scrap";
            return itemId;
        }

        private static List<BunkerMaintenanceMaterialRequirement> CopyMaterials(
            List<BunkerMaintenanceMaterialRequirement> source)
        {
            var copy = new List<BunkerMaintenanceMaterialRequirement>();
            if (source == null) return copy;
            for (int i = 0; i < source.Count; i++)
            {
                var material = source[i];
                if (material == null) continue;
                copy.Add(new BunkerMaintenanceMaterialRequirement
                {
                    ItemId = material.ItemId,
                    Amount = material.Amount
                });
            }
            return copy;
        }

        private static bool SnapshotsEqual(BunkerMaintenanceSnapshot left, BunkerMaintenanceSnapshot right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left == null || right == null) return false;
            if (left.RepairsBlockedByPowerFailure != right.RepairsBlockedByPowerFailure
                || left.MechanicalPartsOnHand != right.MechanicalPartsOnHand
                || left.ElectronicScrapOnHand != right.ElectronicScrapOnHand
                || left.AssignedSurvivorId != right.AssignedSurvivorId
                || left.AssignedSurvivorName != right.AssignedSurvivorName
                || left.HasAssignedLivingSurvivor != right.HasAssignedLivingSurvivor
                || left.RepairPriority != right.RepairPriority
                || left.Targets == null || right.Targets == null
                || left.Targets.Count != right.Targets.Count)
                return false;
            for (int i = 0; i < left.Targets.Count; i++)
            {
                var a = left.Targets[i];
                var b = right.Targets[i];
                if (a == null || b == null) { if (a != b) return false; else continue; }
                if (a.TargetType != b.TargetType || a.TargetId != b.TargetId || a.DisplayName != b.DisplayName
                    || !Mathf.Approximately(a.Condition, b.Condition) || a.IsDestroyed != b.IsDestroyed
                    || a.CanRepair != b.CanRepair || a.HasRequiredMaterials != b.HasRequiredMaterials
                    || !MaterialsEqual(a.Materials, b.Materials)) return false;
            }
            return true;
        }

        private static bool MaterialsEqual(
            List<BunkerMaintenanceMaterialRequirement> left,
            List<BunkerMaintenanceMaterialRequirement> right)
        {
            if (left == null || right == null) return left == right;
            if (left.Count != right.Count) return false;
            for (int i = 0; i < left.Count; i++)
            {
                if (left[i] == null || right[i] == null)
                {
                    if (left[i] != right[i]) return false;
                    continue;
                }
                if (left[i].ItemId != right[i].ItemId || left[i].Amount != right[i].Amount) return false;
            }
            return true;
        }
    }

    [Serializable]
    public sealed class BunkerMaintenanceSave
    {
        public string systemId = BunkerMaintenanceSystem.SystemId;
        public string assignedSurvivorId;
        public int repairPriority = (int)MaintenanceRepairPriority.Standard;
    }

    [Serializable]
    public sealed class BunkerMaintenanceSnapshot
    {
        public bool RepairsBlockedByPowerFailure;
        public int MechanicalPartsOnHand;
        public int ElectronicScrapOnHand;
        public string AssignedSurvivorId;
        public string AssignedSurvivorName;
        public bool HasAssignedLivingSurvivor;
        public MaintenanceRepairPriority RepairPriority;
        public List<BunkerMaintenanceTargetSnapshot> Targets;
    }

    [Serializable]
    public sealed class BunkerMaintenanceTargetSnapshot
    {
        public MaintenanceTargetType TargetType;
        public string TargetId;
        public string DisplayName;
        public float Condition;
        public bool IsDestroyed;
        public bool CanRepair;
        public bool HasRequiredMaterials;
        public List<BunkerMaintenanceMaterialRequirement> Materials;
    }

    [Serializable]
    public sealed class BunkerMaintenanceMaterialRequirement
    {
        public string ItemId;
        public int Amount;
    }

    [Serializable]
    public sealed class BunkerMaintenanceRepairResult
    {
        public bool Succeeded;
        public string Reason;
        public MaintenanceTargetType TargetType;
        public string TargetId;
        public string AssignedSurvivorId;
        public List<BunkerMaintenanceMaterialRequirement> MaterialsConsumed;
    }
}
