using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Crafting
{
    /// <summary>
    /// Workbench component economy: disassemble non-consumables into scrap,
    /// repair degraded gear / hard-broken devices, recalibrate instruments with
    /// ElectronicScrap. Reads ScrapValue / RepairRecipe from ItemDefinition.
    /// </summary>
    public class WorkbenchSystem
    {
        public const string StationId = "workbench";
        public const float DefaultDisassembleYield = 0.5f;
        public const int ElectronicScrapForGeigerRecal = 1;
        public const int ElectronicScrapForPurifierRepair = 2;
        public const float PurifierCriticalFilterHealth = 25f;

        private readonly Inventory.Inventory _inventory;
        private readonly Func<string, ItemDefinition> _itemLookup;
        private readonly CraftingSystem _crafting;
        private readonly Func<Shelter.Shelter> _getShelter;
        private readonly Func<int> _getDay;

        public event Action OnWorkbenchChanged;
        public event Action<ItemDefinition, List<ScrapYield>> OnDisassembled;
        public event Action<ItemDefinition> OnRepaired;
        public event Action<ItemDefinition> OnRecalibrated;

        public WorkbenchSystem(
            Inventory.Inventory inventory,
            Func<string, ItemDefinition> itemLookup,
            CraftingSystem crafting = null,
            Func<Shelter.Shelter> getShelter = null,
            Func<int> getDay = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _itemLookup = itemLookup ?? throw new ArgumentNullException(nameof(itemLookup));
            _crafting = crafting;
            _getShelter = getShelter;
            _getDay = getDay ?? (() => 0);
        }

        public bool HasOperationalWorkbench()
        {
            if (_crafting == null) return true; // tests without station gate
            var station = _crafting.GetStation(StationId);
            return station != null && station.IsOperational;
        }

        // -----------------------------------------------------------------
        // Scrap / recipe resolution (dynamic from ItemDefinition + defaults)
        // -----------------------------------------------------------------

        public List<ScrapYield> GetScrapValue(ItemDefinition item)
        {
            var list = new List<ScrapYield>();
            if (item == null) return list;

            if (item.scrapValue != null && item.scrapValue.Count > 0)
            {
                for (int i = 0; i < item.scrapValue.Count; i++)
                {
                    var y = item.scrapValue[i];
                    if (y != null && y.amount > 0 && !string.IsNullOrEmpty(y.materialId))
                        list.Add(y.Clone());
                }
                return list;
            }

            // Type defaults so items.json can omit scrapValue and UI still works
            switch (item.type)
            {
                case ItemType.Device:
                    list.Add(new ScrapYield(ScrapMaterialIds.ElectronicScrap, 2));
                    list.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
                    break;
                case ItemType.Tool:
                    list.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
                    list.Add(new ScrapYield(ScrapMaterialIds.ElectronicScrap, 1));
                    break;
                case ItemType.Protective:
                    list.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
                    list.Add(new ScrapYield(ScrapMaterialIds.Chemicals, 1));
                    break;
                case ItemType.Filter:
                case ItemType.Fuel:
                    list.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
                    break;
                case ItemType.Material:
                    if (item.id == "scrap_metal")
                        list.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
                    else if (item.id == "battery")
                        list.Add(new ScrapYield(ScrapMaterialIds.ElectronicScrap, 1));
                    break;
                default:
                    list.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
                    break;
            }
            return list;
        }

        public List<ScrapYield> GetDisassembleYield(ItemDefinition item)
        {
            float frac = item != null && item.disassembleYieldFraction > 0f
                ? Mathf.Clamp01(item.disassembleYieldFraction)
                : DefaultDisassembleYield;

            var baseScrap = GetScrapValue(item);
            var yield = new List<ScrapYield>();
            for (int i = 0; i < baseScrap.Count; i++)
            {
                int amount = Mathf.Max(1, Mathf.FloorToInt(baseScrap[i].amount * frac + 0.001f));
                // At least 1 of each material line if base had any
                if (baseScrap[i].amount > 0)
                    amount = Mathf.Max(1, amount);
                if (frac <= 0f) amount = 0;
                if (amount > 0)
                    yield.Add(new ScrapYield(baseScrap[i].materialId, amount));
            }
            return yield;
        }

        public RepairRecipe GetRepairRecipe(ItemDefinition item)
        {
            if (item?.repairRecipe != null && item.repairRecipe.costs != null && item.repairRecipe.costs.Count > 0)
                return item.repairRecipe.Clone();

            var recipe = new RepairRecipe { hours = 0.5f, requiresTools = true };
            // Defaults by type / id
            if (item != null && (item.id == "geiger_counter" || item.id == "dosimeter" || item.type == ItemType.Device))
            {
                recipe.costs.Add(new ScrapYield(ScrapMaterialIds.ElectronicScrap, 2));
                recipe.costs.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
            }
            else if (item != null && item.type == ItemType.Protective)
            {
                recipe.costs.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
                recipe.costs.Add(new ScrapYield(ScrapMaterialIds.Chemicals, 1));
            }
            else
            {
                recipe.costs.Add(new ScrapYield(ScrapMaterialIds.MechanicalParts, 1));
            }
            return recipe;
        }

        // -----------------------------------------------------------------
        // Disassemble
        // -----------------------------------------------------------------

        public bool CanDisassemble(ItemDefinition item)
        {
            if (item == null || !item.CanDisassemble) return false;
            if (!HasOperationalWorkbench()) return false;
            if (_inventory.Count(item) < 1) return false;
            var yield = GetDisassembleYield(item);
            if (yield.Count == 0) return false;
            for (int i = 0; i < yield.Count; i++)
            {
                var mat = ResolveMaterial(yield[i].materialId);
                if (mat == null || !_inventory.CanAdd(mat, yield[i].amount)) return false;
            }
            return true;
        }

        /// <summary>
        /// Destroy one unit of the item and grant a percentage of its ScrapValue.
        /// </summary>
        public bool Disassemble(ItemDefinition item)
        {
            if (!CanDisassemble(item)) return false;

            var yield = GetDisassembleYield(item);
            // Pre-check capacity for all yields
            for (int i = 0; i < yield.Count; i++)
            {
                var mat = ResolveMaterial(yield[i].materialId);
                if (mat == null || !_inventory.CanAdd(mat, yield[i].amount)) return false;
            }

            if (!_inventory.Remove(item, 1)) return false;

            for (int i = 0; i < yield.Count; i++)
            {
                var mat = ResolveMaterial(yield[i].materialId);
                _inventory.Add(mat, yield[i].amount);
            }

            WearStation();
            OnDisassembled?.Invoke(item, yield);
            OnWorkbenchChanged?.Invoke();
            return true;
        }

        /// <summary>Disassemble a specific slot (preserves broken device state for flavor only — item is destroyed).</summary>
        public bool DisassembleSlot(InventorySlot slot)
        {
            if (slot?.Item == null) return false;
            return Disassemble(slot.Item);
        }

        // -----------------------------------------------------------------
        // Repair
        // -----------------------------------------------------------------

        public bool CanRepair(InventorySlot slot)
        {
            if (slot?.Item == null) return false;
            if (!HasOperationalWorkbench()) return false;
            if (!slot.IsBrokenOrDegraded()) return false;

            var recipe = GetRepairRecipe(slot.Item);
            if (recipe.requiresTools && !HasOperationalWorkbench()) return false;
            return HasScrapCosts(recipe.costs);
        }

        public bool Repair(InventorySlot slot)
        {
            if (!CanRepair(slot)) return false;

            var recipe = GetRepairRecipe(slot.Item);
            if (!ConsumeScrapCosts(recipe.costs)) return false;

            if (slot.Item.type == ItemType.Device)
            {
                if (slot.Device == null) slot.Device = DeviceState.CreateDefault();
                InstrumentDevice.RepairHardFailure(slot.Device, _getDay());
            }
            else if (slot.Item.durability > 0f)
            {
                slot.CurrentDurability = slot.Item.durability;
            }

            WearStation();
            OnRepaired?.Invoke(slot.Item);
            OnWorkbenchChanged?.Invoke();
            return true;
        }

        /// <summary>Repair first matching item id in inventory that is broken/degraded.</summary>
        public bool RepairFirst(string itemId)
        {
            var slot = FindBrokenOrDegraded(itemId);
            return slot != null && Repair(slot);
        }

        // -----------------------------------------------------------------
        // Recalibrate (uncalibrated geiger → ElectronicScrap)
        // -----------------------------------------------------------------

        public bool CanRecalibrateGeiger()
        {
            if (!HasOperationalWorkbench()) return false;
            var slot = _inventory.FindSlot("geiger_counter");
            if (slot?.Device == null) return false;
            if (slot.Device.Broken) return false; // hard-broken needs Repair, not recal
            if (slot.Device.Calibration >= InstrumentDevice.ReliableCalibrationThreshold) return false;
            var scrap = ResolveMaterial(ScrapMaterialIds.ElectronicScrap);
            return scrap != null && _inventory.Count(scrap) >= ElectronicScrapForGeigerRecal;
        }

        public bool RecalibrateGeiger()
        {
            if (!CanRecalibrateGeiger()) return false;
            var scrap = ResolveMaterial(ScrapMaterialIds.ElectronicScrap);
            if (!_inventory.Remove(scrap, ElectronicScrapForGeigerRecal)) return false;

            var slot = _inventory.FindSlot("geiger_counter");
            InstrumentDevice.Recalibrate(slot.Device, _getDay());
            WearStation();
            OnRecalibrated?.Invoke(slot.Item);
            OnWorkbenchChanged?.Invoke();
            return true;
        }

        // -----------------------------------------------------------------
        // Critical module repair (water purifier)
        // -----------------------------------------------------------------

        public bool NeedsElectronicScrapForCriticalRepair()
        {
            int need = GetCriticalElectronicScrapDeficit();
            return need > 0;
        }

        /// <summary>
        /// How many ElectronicScrap units short for critical repairs (purifier + broken geiger).
        /// </summary>
        public int GetCriticalElectronicScrapDeficit()
        {
            int needed = 0;
            var shelter = _getShelter?.Invoke();
            var purifier = shelter?.GetModule("water_purifier");
            if (purifier != null && purifier.FilterHealth <= PurifierCriticalFilterHealth)
                needed += ElectronicScrapForPurifierRepair;

            var geiger = _inventory.FindSlot("geiger_counter");
            if (geiger?.Device != null && geiger.Device.Broken)
                needed += 2; // matches default repair recipe

            var scrap = ResolveMaterial(ScrapMaterialIds.ElectronicScrap);
            int have = scrap != null ? _inventory.Count(scrap) : 0;
            return Mathf.Max(0, needed - have);
        }

        public bool CanRepairWaterPurifier()
        {
            if (!HasOperationalWorkbench()) return false;
            var shelter = _getShelter?.Invoke();
            var purifier = shelter?.GetModule("water_purifier");
            if (purifier == null) return false;
            if (purifier.FilterHealth > PurifierCriticalFilterHealth && purifier.FilterHealth >= 99f)
                return false;
            var scrap = ResolveMaterial(ScrapMaterialIds.ElectronicScrap);
            return scrap != null && _inventory.Count(scrap) >= ElectronicScrapForPurifierRepair;
        }

        public bool RepairWaterPurifier()
        {
            if (!CanRepairWaterPurifier()) return false;
            var scrap = ResolveMaterial(ScrapMaterialIds.ElectronicScrap);
            if (!_inventory.Remove(scrap, ElectronicScrapForPurifierRepair)) return false;
            var purifier = _getShelter?.Invoke()?.GetModule("water_purifier");
            if (purifier == null) return false;
            purifier.FilterHealth = 100f;
            purifier.IsEnabled = true;
            WearStation();
            OnWorkbenchChanged?.Invoke();
            return true;
        }

        // -----------------------------------------------------------------
        // UI helpers
        // -----------------------------------------------------------------

        public List<WorkbenchLine> BuildLines()
        {
            var lines = new List<WorkbenchLine>();
            var slots = _inventory.Slots;
            if (slots == null) return lines;

            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                if (slot?.Item == null) continue;
                var item = slot.Item;

                if (item.CanDisassemble)
                {
                    var yield = GetDisassembleYield(item);
                    lines.Add(new WorkbenchLine
                    {
                        Kind = WorkbenchActionKind.Disassemble,
                        Item = item,
                        SlotIndex = i,
                        Label = $"Disassemble {item.displayName}",
                        CostSummary = FormatYield(yield),
                        CanExecute = CanDisassemble(item)
                    });
                }

                if (slot.IsBrokenOrDegraded())
                {
                    var recipe = GetRepairRecipe(item);
                    lines.Add(new WorkbenchLine
                    {
                        Kind = WorkbenchActionKind.Repair,
                        Item = item,
                        SlotIndex = i,
                        Label = $"Repair {item.displayName}",
                        CostSummary = FormatCosts(recipe.costs),
                        CanExecute = CanRepair(slot)
                    });
                }
            }

            var geiger = _inventory.FindSlot("geiger_counter");
            if (geiger?.Device != null
                && !geiger.Device.Broken
                && geiger.Device.Calibration < InstrumentDevice.ReliableCalibrationThreshold)
            {
                lines.Add(new WorkbenchLine
                {
                    Kind = WorkbenchActionKind.Recalibrate,
                    Item = geiger.Item,
                    SlotIndex = -1,
                    Label = "Recalibrate Geiger Counter",
                    CostSummary = $"{ElectronicScrapForGeigerRecal}x electronic_scrap",
                    CanExecute = CanRecalibrateGeiger()
                });
            }

            if (CanRepairWaterPurifier() || NeedsElectronicScrapForCriticalRepair())
            {
                var shelter = _getShelter?.Invoke();
                var purifier = shelter?.GetModule("water_purifier");
                if (purifier != null && purifier.FilterHealth < 100f)
                {
                    lines.Add(new WorkbenchLine
                    {
                        Kind = WorkbenchActionKind.RepairPurifier,
                        Item = null,
                        SlotIndex = -1,
                        Label = "Repair Water Purifier",
                        CostSummary = $"{ElectronicScrapForPurifierRepair}x electronic_scrap",
                        CanExecute = CanRepairWaterPurifier()
                    });
                }
            }

            return lines;
        }

        public bool ExecuteLine(WorkbenchLine line)
        {
            if (line == null) return false;
            switch (line.Kind)
            {
                case WorkbenchActionKind.Disassemble:
                    return Disassemble(line.Item);
                case WorkbenchActionKind.Repair:
                    if (line.SlotIndex >= 0 && line.SlotIndex < _inventory.Slots.Count)
                        return Repair(_inventory.Slots[line.SlotIndex]);
                    return RepairFirst(line.Item?.id);
                case WorkbenchActionKind.Recalibrate:
                    return RecalibrateGeiger();
                case WorkbenchActionKind.RepairPurifier:
                    return RepairWaterPurifier();
                default:
                    return false;
            }
        }

        // -----------------------------------------------------------------
        // Internals
        // -----------------------------------------------------------------

        private ItemDefinition ResolveMaterial(string materialId)
        {
            return _itemLookup?.Invoke(materialId);
        }

        private bool HasScrapCosts(List<ScrapYield> costs)
        {
            if (costs == null) return true;
            for (int i = 0; i < costs.Count; i++)
            {
                var c = costs[i];
                if (c == null || c.amount <= 0) continue;
                var mat = ResolveMaterial(c.materialId);
                if (mat == null || _inventory.Count(mat) < c.amount) return false;
            }
            return true;
        }

        private bool ConsumeScrapCosts(List<ScrapYield> costs)
        {
            if (!HasScrapCosts(costs)) return false;
            for (int i = 0; i < costs.Count; i++)
            {
                var c = costs[i];
                if (c == null || c.amount <= 0) continue;
                var mat = ResolveMaterial(c.materialId);
                if (!_inventory.Remove(mat, c.amount)) return false;
            }
            return true;
        }

        private InventorySlot FindBrokenOrDegraded(string itemId)
        {
            if (string.IsNullOrEmpty(itemId) || _inventory.Slots == null) return null;
            for (int i = 0; i < _inventory.Slots.Count; i++)
            {
                var s = _inventory.Slots[i];
                if (s?.Item != null && s.Item.id == itemId && s.IsBrokenOrDegraded())
                    return s;
            }
            return null;
        }

        private void WearStation()
        {
            var station = _crafting?.GetStation(StationId);
            station?.Degrade(CraftingSystem.StationWearPerCraft * 0.2f);
        }

        private static string FormatYield(List<ScrapYield> yield)
        {
            if (yield == null || yield.Count == 0) return "—";
            var parts = new List<string>();
            for (int i = 0; i < yield.Count; i++)
                parts.Add($"+{yield[i].amount} {yield[i].materialId}");
            return string.Join(", ", parts);
        }

        private static string FormatCosts(List<ScrapYield> costs)
        {
            if (costs == null || costs.Count == 0) return "—";
            var parts = new List<string>();
            for (int i = 0; i < costs.Count; i++)
                parts.Add($"{costs[i].amount}x {costs[i].materialId}");
            return string.Join(", ", parts);
        }
    }

    public enum WorkbenchActionKind
    {
        Disassemble,
        Repair,
        Recalibrate,
        RepairPurifier
    }

    [Serializable]
    public class WorkbenchLine
    {
        public WorkbenchActionKind Kind;
        public ItemDefinition Item;
        public int SlotIndex;
        public string Label;
        public string CostSummary;
        public bool CanExecute;
    }
}
